using UnityEngine;
using Mirror;
using System;


public class PlayerShoot : NetworkBehaviour
{
    private PlayerWeapon currentWeapon;

    [SerializeField]
    private LayerMask mask;

    [SerializeField]
    private Camera cam;

    private WeaponManager weaponManager;

    void Start()
    {
        if(cam == null)
        {
            Debug.LogError("Pas de camera !");
            this.enabled = false;
        }
       
        weaponManager = GetComponent<WeaponManager>();
    }

    private void Update()
    {
        if (isLocalPlayer)
        {
            currentWeapon = weaponManager.GetCurrentWeapon();

            if (currentWeapon.fireRate <= 0f)
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    Shoot();
                }
            }
            else
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    InvokeRepeating("Shoot", 0f, 1 / currentWeapon.fireRate);
                }
                else if (Input.GetButtonUp("Fire1"))
                {
                    CancelInvoke("Shoot");
                }
            }
        }
    }

    [Command]
    void CmdOnHit(Vector3 pos,Vector3 normal)
    {
        RpcDoHitEffect(pos,normal);
    }

    [ClientRpc]
    void RpcDoHitEffect(Vector3 pos, Vector3 normal)
    {
        GameObject hitEffect =  Instantiate(weaponManager.GetCurrentGraphics().hitEffectPrefab, pos, Quaternion.LookRotation(normal));
        Destroy(hitEffect, 1f);
    }

    // prevenir le serveur du tir (pour que tout le monde voit les particule)
    [Command]
    void CmdOnShoot()
    {
        RpcDoShooteEffect();
    }

    //faire apparaitre les effets
    [ClientRpc]
    void RpcDoShooteEffect()
    {
        weaponManager.GetCurrentGraphics().muzzleFlash.Play();
    }

    [Client]
    private void Shoot()
    {
        if(!isLocalPlayer)
        {
            return;
        }

        CmdOnShoot();

        RaycastHit hit;
         
        if(Physics.Raycast(cam.transform.position ,cam.transform.forward ,out hit , currentWeapon.range ,mask))
        {
            if(hit.collider.tag == "Player")
            {
                CmdPlayerShot(hit.collider.name, currentWeapon.damage);
            }
            CmdOnHit(hit.point,hit.normal);
        }
    }

    [Command]
    private void CmdPlayerShot(string playerId,float damage)
    {
        Debug.Log(playerId + "a ete touche");

        Player player = GameManager.GetPlayer(playerId);

        player.RpcTakeDamage(damage);

    }
}
