using UnityEngine;

[System.Serializable]
public class PlayerWeapon {

    public string name = "Tippman";

    public int damage = 10;
    public float range = 100f;
    public float reloadTime = 1f;

    //fire rate zero is single fire anything above is automatic
    public float fireRate = 0f;

    public int maxAmmo = 30;

    [HideInInspector]
    public int ammo; 

    public GameObject graphics;

    public PlayerWeapon()
    {
        ammo = maxAmmo;
    }


}
