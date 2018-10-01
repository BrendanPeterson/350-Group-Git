using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(WeaponManager))]
public class ShootingController : NetworkBehaviour {

    private const string PLAYER_TAG = "Player";

    private PlayerWeapon currentWeapon;
    private WeaponManager weaponManager;
    private WeaponGraphics currentGraphics;


    [SerializeField]
    private Camera cam;

    [SerializeField]
    private LayerMask mask;

    

    //Called on Start
    void Start()
    {
        if (cam == null)
        {
            Debug.LogError("No Camera Referenced");
            this.enabled = false;
        }

        weaponManager = GetComponent<WeaponManager>();

    }

    //Called every Frame
    void Update()
    {
        currentWeapon = weaponManager.GetCurrentWeapon();

        if (PauseMenu.IsOn)
            return;

        if (currentWeapon.ammo < currentWeapon.maxAmmo)
        {

            if (Input.GetButtonDown("Reload"))
            {
                weaponManager.Reload();
                return;
            }

        }

        if(currentWeapon.fireRate <= 0f)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }
        }else
        {
            if (Input.GetButtonDown("Fire1"))
            {
                InvokeRepeating("Shoot", 0f, 1f/currentWeapon.fireRate);

            }else if (Input.GetButtonUp("Fire1"))
            {
                CancelInvoke("Shoot");
            }
        }

       
    }

    //Called on server when player shoots
    [Command]
    void CmdOnShoot()
    {
        RpcDoShootEffect();
        
    }

    

    //Is called on all clients when we need to add shooting effects
    [ClientRpc]
    void RpcDoShootEffect()
    {
        weaponManager.GetCurrentGraphics().muzzleFlash.Play();
    }

    //called on server when we hit something
    //takes in hit point and normal "Direction the surface is facing" of surface hit
    [Command]
    void CmdOnHit(Vector3 _pos, Vector3 _normal)
    {
        RpcDoHitEffect(_pos, _normal);
    }

    //Called on all clients
    //Spawn effects
    [ClientRpc]
    void RpcDoHitEffect(Vector3 _pos, Vector3 _normal)
    {
        //Look into object pooling
        //Instantiating an effect everytime takes a lot of power
        //May edit to create paint splots that dont dissapear by getting rid of destroy and changing the effect
       GameObject _hitEffect = (GameObject)Instantiate(weaponManager.GetCurrentGraphics().hitEffect, _pos, Quaternion.LookRotation(_normal));
        Destroy(_hitEffect, 1f);
    }

    //onlycalled on client
    [Client]
    void Shoot()
    {
        //Debug.Log("SHOOT!");
        if(!isLocalPlayer && !weaponManager.isReloading)
        {
            return;
        }

        if(currentWeapon.ammo <= 0)
        {
            Debug.Log("Out of Ammo");
            weaponManager.Reload();
            return;
        }

        currentWeapon.ammo--;
        Debug.Log("You have " + currentWeapon.ammo + " bullets left.");

        //Local player is shooting call OnShoot function on server
        CmdOnShoot();

        RaycastHit hit;
        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, currentWeapon.range, mask))
        {
            //We hit something
            if ( hit.collider.tag == PLAYER_TAG)
            {
                CmdPlayerShot(hit.collider.name, currentWeapon.damage);
            }
            else
            {
                Debug.Log("We hit" + hit.collider.name);
            }

            //We jit something, call the onhit method on server
            CmdOnHit(hit.point, hit.normal);
            
        }
    }

    //only called on server
    [Command]
    void CmdPlayerShot (string _playerID, int _damage)
    {
        Debug.Log(_playerID + " has been shot. ");

        Player _player = GameManager.GetPlayer(_playerID);
        _player.RpcTakeDamage( _damage);
    }
}
