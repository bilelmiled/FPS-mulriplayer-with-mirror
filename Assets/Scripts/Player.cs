using Mirror;
using System.Collections;
using UnityEngine;

public class Player : NetworkBehaviour
{
    [SyncVar]
    private bool _isDead = false;

    public bool isDead
    {
        get { return _isDead; }
        protected set { _isDead = value; }
    }
    [SerializeField]
    private float maxHealth = 100f;

    [SyncVar]
    private float currentHealth;

    [SerializeField]
    private Behaviour[] disableOnDeath;

    [SerializeField]
    private GameObject[] disableGameObjectsOnDeath;

    private bool[] wasEnabledOnStart;

    [SerializeField]
    private GameObject spawnEffect;

    [SerializeField]
    private GameObject deathEffect;

    public void Setup()
    {

        GameManager.instance.SetSceneCameraActive(false);
        GetComponent<SetupPlayer>().playerUIInstance.SetActive(true);

        CmdBrodcastNewPlayerSetup();
    }

    [Command]
    private void CmdBrodcastNewPlayerSetup()
    {
        RpcSetUpPlayerOnAllClients();
    }

    [ClientRpc]
    private void RpcSetUpPlayerOnAllClients()
    {
        wasEnabledOnStart = new bool[disableOnDeath.Length];

        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            wasEnabledOnStart[i] = disableOnDeath[i].enabled;
        }
        SetDefaults();
    }

    private void SetDefaults()
    {
        isDead = false;
        currentHealth = maxHealth;

        //reactive les scripts des joueur
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = wasEnabledOnStart[i];
        }
        //reactive les gameobjects des joueur
        for (int i = 0; i < disableGameObjectsOnDeath.Length; i++)
        {
            disableGameObjectsOnDeath[i].SetActive(true);
        }
        //desactive la camera principal

        GameObject spawnGFX = Instantiate(spawnEffect, transform.position, Quaternion.LookRotation(Vector3.up));
        Destroy(spawnGFX, 3f);
    }

    private void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            RpcTakeDamage(200);
        }
    }
    [ClientRpc]
    public void RpcTakeDamage(float damage)
    {
        if (isDead)
        {
            return;
        }
        currentHealth -= damage;
        Debug.Log(transform.name + " hp: " + currentHealth);
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        isDead = true;
        //desactive les scripts des joueur
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = false;
        }
        //desactive les gameobjects des joueur
        for (int i = 0; i < disableGameObjectsOnDeath.Length; i++)
        {
            disableGameObjectsOnDeath[i].SetActive(false);
        }
        GameObject deathGFX = Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(deathGFX, 3f);
        if (isLocalPlayer)
        {
            GameManager.instance.SetSceneCameraActive(true);
            GetComponent<SetupPlayer>().playerUIInstance.SetActive(false);
        }

        Debug.Log("eliminéé");


        CharacterController con = GetComponent<CharacterController>();
        con.enabled = false;
        StartCoroutine(Respawn());



    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(GameManager.instance.matchSettings.respawTimer);
        Transform spawnPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;
        CharacterController con = GetComponent<CharacterController>();
        con.enabled = true;

        GameManager.instance.SetSceneCameraActive(false);
        GetComponent<SetupPlayer>().playerUIInstance.SetActive(true);

        SetDefaults();
    }
}
