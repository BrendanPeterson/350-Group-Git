using UnityEngine.Networking;
using UnityEngine;
using System.Collections;

public class WeaponManager : NetworkBehaviour {

    [SerializeField]
    private string weaponLayerName = "Weapon";

    [SerializeField]
    private Transform weaponHolder;

    [SerializeField]
    private PlayerWeapon primaryWeapon;

    private PlayerWeapon currentWeapon;
    private WeaponGraphics currentGraphics;

    public bool isReloading = false;
    public Transform lookPos;
    private GameObject weaponIns;
   


	void Start ()
    {
        EquipWeapon(primaryWeapon);
	}

    public PlayerWeapon GetCurrentWeapon ()
    {
        return currentWeapon;    
    }

    public WeaponGraphics GetCurrentGraphics()
    {
        return currentGraphics;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            CheckWeaponGraphicsLocation();
        }
    }
    void CheckWeaponGraphicsLocation()
    {
        Debug.Log(weaponIns.transform.position);
    }

    void EquipWeapon (PlayerWeapon _weapon)
    {

        currentWeapon = _weapon;
        Vector3 _initialTransform = (weaponHolder.transform.position + _weapon.graphics.transform.position);
         weaponIns = (GameObject)Instantiate(_weapon.graphics, _initialTransform, transform.rotation);
        //Debug.Log(_weapon.graphics.transform.position);
       weaponIns.transform.SetParent(weaponHolder);
        
        Debug.Log(weaponIns.transform.position);

        


        currentGraphics = weaponIns.GetComponent<WeaponGraphics>();
        if (currentGraphics == null)
            Debug.LogError("Current weapon has no graphics!" + weaponIns.name);

        if (isLocalPlayer)
            Util.SetLayerRecursively(weaponIns, LayerMask.NameToLayer(weaponLayerName));
    }

    public void Reload()
    {
        if (isReloading)
               return;
        StartCoroutine(Reload_Coroutine());
    }

    private IEnumerator Reload_Coroutine ()
    {
        Debug.Log("Reloading");
        isReloading = true;
        CmdOnReload();

        yield return new WaitForSeconds(currentWeapon.reloadTime);

        currentWeapon.ammo = currentWeapon.maxAmmo;

        isReloading = false;
    }

    [Command]
    void CmdOnReload()
    {
        RpcOnReload();
    }

    [ClientRpc]
    void RpcOnReload()
    {
        Animator anim = currentGraphics.GetComponent<Animator>();
        if(anim != null)
        {
            anim.SetTrigger("Reload");
        }
    }

}
