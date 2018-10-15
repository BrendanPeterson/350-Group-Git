using UnityEngine;
using UnityEngine.Networking;
using System.Collections;


[RequireComponent(typeof(PlayerNetworking))]
public class Player : NetworkBehaviour {

    [SyncVar]
    private bool _isDead = false;


    public bool isDead
    {
        get { return _isDead; }

        //protected set makes it so only classes that derive from player can change the value
        protected set { _isDead = value;  }
    }

    [SerializeField]
    private int maxHealth = 100;

    //syncvar is used so that every time a variable changes it is synced to all the clients
    [SyncVar]
    private int currentHealth;

    [SerializeField]
    private Behaviour[] disableOnDeath;
    private bool[] wasEnabled;

    [SerializeField]
    private GameObject[] disableGameObjectsOnDeath;

    [SerializeField]
    private GameObject deathEffect;

    [SerializeField]
    private GameObject spawnEffect;

    private bool firstSetup = true;

    /*public override void OnStartLocalPlayer()
    {
        Renderer[] rens = GetComponentsInChildren<Renderer>();
        foreach(Renderer ren in rens)
        {
           ren.enabled = false;
        }
        GetComponent<NetworkAnimator>().SetParameterAutoSend(0, true);
       
    }*/

    public override void PreStartClient()
    {
        GetComponent<NetworkAnimator>().SetParameterAutoSend(0, true);
        Debug.Log("SendAnimation");
    }
    //called when player setup is ready
    public void SetupPlayer()
    {
        if(isLocalPlayer)
        {
            //Switch cameras
            GameManager.instance.SetSceneCameraActive(false);
            GetComponent<PlayerNetworking>().playerUIInstance.SetActive(true);
        }

        CmdBroadCastNewPlayerSetup();
    }

    //sends player setup instructions to server
    [Command]
    private void CmdBroadCastNewPlayerSetup()
    {
        RpcSetupPlayerOnAllClientes();
    }

    //allows server to setup a new player on all clients
    [ClientRpc]
    private void RpcSetupPlayerOnAllClientes()
    {
        //only want to do this when player first logs in
        //dont do it everytime player respawns
        if(firstSetup)
        {
            wasEnabled = new bool[disableOnDeath.Length];

            //loops through components and stores in was enabled array
            for (int i = 0; i < wasEnabled.Length; i++)
            {
                wasEnabled[i] = disableOnDeath[i].enabled;
            }

            firstSetup = false;
        }
        

        SetDefaults();
    }

    //Update to test death code without 2 players kills player instantly when you hit K
     /*void Update()
    {
        if (!isLocalPlayer)
            return;

        if (Input.GetKeyDown(KeyCode.K))
        {
            RpcTakeDamage(9999999);
        }
     }*/

    //called when player is hit 
    //clientRpc sends to client rom server
    //originally called in shooting controller
    [ClientRpc]
    public void RpcTakeDamage(int _amount)
    {
        if (isDead)
            return;

        currentHealth -= _amount;

        Debug.Log(transform.name + " now has " + currentHealth + " health.");

        if(currentHealth <=0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;

        //when someone dies disable everything in the disableOnDeath array
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = false;
        }

        //Disable GameObjects 
        for (int i = 0; i < disableGameObjectsOnDeath.Length; i++)
        {
            disableGameObjectsOnDeath[i].SetActive(false);
        }

        //Disable collider
        Collider _col = GetComponent<Collider>();
        if (_col != null)
            _col.enabled = false;

        //spawn Death Effect
       GameObject _gfxIns = (GameObject)Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(_gfxIns, 3f);
        Debug.Log(transform.name + " is DEAD!");

        //Switch Cameras
        if(isLocalPlayer)
        {
            GameManager.instance.SetSceneCameraActive(true);
            GetComponent<PlayerNetworking>().playerUIInstance.SetActive(false);
        }

        StartCoroutine(Respawn());
    }
    
    //calls respawn method after a set time
    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(GameManager.instance.matchSettings.respawnTime);

        //Must come before setup player or will spawn particles in wrong location
        Transform _spawnPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = _spawnPoint.position;
        transform.rotation = _spawnPoint.rotation;

        yield return new WaitForSeconds(0.1f);

        SetupPlayer();

        Debug.Log(transform.name + " respawned.");
    }



    //Sets the default health and anything else we put in
    public void SetDefaults()
    {

        isDead = false;

        currentHealth = maxHealth;
        
        //Enable the Components
        for (int i = 0; i < disableOnDeath.Length; i++)
        {

            disableOnDeath[i].enabled = wasEnabled[i];

        }

        //Enabele GameObjects
        for (int i = 0; i < disableGameObjectsOnDeath.Length; i++)
        {
            disableGameObjectsOnDeath[i].SetActive(true);
        }

        //Enable Collider
        Collider _col = GetComponent<Collider>();
        if (_col != null)
            _col.enabled = true;

        //Create Spawn Effects
        GameObject _gfxIns = (GameObject)Instantiate(spawnEffect, transform.position, Quaternion.Euler(-90f,0f,0f));
        Destroy(_gfxIns, 7f);
    }

}
