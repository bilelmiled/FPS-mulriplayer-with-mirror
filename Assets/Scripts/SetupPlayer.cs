using UnityEngine;
using Mirror;
using System;

public class SetupPlayer : NetworkBehaviour
{
    [SerializeField]
    Behaviour[] componentsToDisable;

    [SerializeField]
    private string remoteLayerName = "RemotePlayer";

    [SerializeField]
    private GameObject playerUIPrefab;

   
    public GameObject playerUIInstance;

    // Start is called before the first frame update
    void Start()
    {
        if (!isLocalPlayer)
        {
            DisableComponents();
            AssignRemoteLayer();

        }
        else
        {
            //creation de UI du player
            playerUIInstance = Instantiate(playerUIPrefab);

            PlayerUI ui = playerUIInstance.GetComponent<PlayerUI>();
            if(ui == null)
            {
                Debug.LogError("pas de UI");
            }
            else
            {
                ui.SetPlayer(GetComponent<Player>());
            }

            GetComponent<Player>().Setup();

            CmdSetUsername(transform.name, UserAccountManager.loggedInUsername);
        }

    }

    [Command]
    private void CmdSetUsername(string playerID, string _username)
    {
        Player player = GameManager.GetPlayer(playerID);
        if(player!=null)
        {
            Debug.Log("player: " + _username + " has joined !");
        }
        player.username = _username;
    }

    private void OnDisable()
    {
       //Destroy(playerUIInstance);
        if(isLocalPlayer)
        {
            GameManager.instance.SetSceneCameraActive(true);
        }
        GameManager.UnregisterPlayer(gameObject.name);
    }
    public override void OnStartClient()
    {
        
        base.OnStartClient();

        string netId = GetComponent<NetworkIdentity>().netId.ToString();
        Player player = GetComponent<Player>();

        GameManager.RegisterPlayer(netId, player);
    }



    public void AssignRemoteLayer()
    {
        gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
    }
    public void DisableComponents()
    {
        for (int i = 0; i < componentsToDisable.Length; i++)
        {
            componentsToDisable[i].enabled = false;
        }
    }


}
