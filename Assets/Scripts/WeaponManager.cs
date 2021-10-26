using UnityEngine;
using Mirror;

public class WeaponManager : NetworkBehaviour
{
    [SerializeField]
    private PlayerWeapon primaryWeapon;

    private PlayerWeapon currentWeapon;

    private WeaponGraphics currentGraphics;

    [SerializeField]
    private string weaponLayerName = "Weapon";

    [SerializeField]
    private Transform weaponHolder;

    void Start()
    {
        EquipWeapon(primaryWeapon);
    }

    public PlayerWeapon GetCurrentWeapon()
    {
        return currentWeapon;
    }

    public WeaponGraphics GetCurrentGraphics()
    {
        return currentGraphics;
    }

    void EquipWeapon(PlayerWeapon _weapon)
    {
        currentWeapon = _weapon;
        GameObject weaponIns = Instantiate(_weapon.graphics, weaponHolder.position, weaponHolder.rotation);
        weaponIns.transform.SetParent(weaponHolder);

        currentGraphics = weaponIns.GetComponent<WeaponGraphics>();

        if(currentGraphics == null)
        {
            Debug.LogError("pas de scrpit weapons graphics sur l'arme" + weaponIns.name);
        }

        if(isLocalPlayer)
        {
            Util.SetLayerRecursively(weaponIns, LayerMask.NameToLayer(weaponLayerName));
        }
    }

    
}
