using UnityEngine;
using Mirror;
using System;


public class PlayerShoot : NetworkBehaviour
{
    private WeaponData currentWeapon;

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
        currentWeapon = weaponManager.GetCurrentWeapon();

        if (PauseMenu.isOn)
        {
            return;
        }

        if (isLocalPlayer)
        {

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

            if(Input.GetKeyDown(KeyCode.R) && currentWeapon.magazineSize > weaponManager.currentMagazineSize)
            {
                StartCoroutine(weaponManager.Reload());
                return;
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

        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(currentWeapon.shootSound);
    }

    [Client]
    private void Shoot()
    {
        if(!isLocalPlayer || weaponManager.isReloading)
        {
            return;
        }

        if(weaponManager.currentMagazineSize <= 0)
        {
            
            StartCoroutine(weaponManager.Reload());
            return;
        }
        Debug.Log(weaponManager.currentMagazineSize);
        weaponManager.currentMagazineSize--;
        CmdOnShoot();

        RaycastHit hit;
         
        if(Physics.Raycast(cam.transform.position ,cam.transform.forward ,out hit , currentWeapon.range ,mask))
        {
            if(hit.collider.tag == "Player")
            {
                CmdPlayerShot(hit.collider.name, currentWeapon.damage,transform.name);
            }
            CmdOnHit(hit.point,hit.normal);
        }
        if (weaponManager.currentMagazineSize <= 0)
        {

            StartCoroutine(weaponManager.Reload());
            return;
        }
    }

    [Command]
    private void CmdPlayerShot(string playerId,float damage,string sourceId)
    {
        Debug.Log(playerId + "a ete touche");

        Player player = GameManager.GetPlayer(playerId);

        player.RpcTakeDamage(damage,sourceId);

    }
}
