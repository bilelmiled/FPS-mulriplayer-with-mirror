using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpWeapon : MonoBehaviour
{
    [SerializeField]
    private WeaponData weapon;

    private GameObject pickUpGraphics;

    bool canPickUp;
    void Start()
    {
        ResetWeapon();
    }

    void ResetWeapon()
    {
        pickUpGraphics = Instantiate(weapon.graphics, transform);
        pickUpGraphics.transform.position = transform.position;
        canPickUp = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && canPickUp)
        {
            WeaponManager weaponManager = other.GetComponent<WeaponManager>();
            EquipNewWeapon(weaponManager);
        }
    }

    void EquipNewWeapon(WeaponManager weaponManager)
    {
        Destroy(weaponManager.GetCurrentGraphics().gameObject);
        weaponManager.EquipWeapon(weapon);
        canPickUp = false;
        Destroy(pickUpGraphics);
        StartCoroutine(DelayResetWeapon());
    }

    IEnumerator DelayResetWeapon()
    {
        yield return new WaitForSeconds(5f);
        ResetWeapon();
    }
}

    
