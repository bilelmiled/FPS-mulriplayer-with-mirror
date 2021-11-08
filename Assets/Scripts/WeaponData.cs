using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData" , menuName ="My Game/Weapon Data")]
public class WeaponData : ScriptableObject
{
    public string name="machine gun";
    public float damage = 10f;
    public float range = 100f;

    public int magazineSize = 10;
    public float fireRate = 0f;
    public GameObject graphics;
    public float timeToReload;

    public AudioClip shootSound;
    public AudioClip reloadSound;
}
