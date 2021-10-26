using UnityEngine;
using Mirror;


public class SetupPlayer : NetworkBehaviour
{
    [SerializeField]
    Behaviour[] componentsToDisable;

    [SerializeField]
    private string remoteLayerName = "RemotePlayer";

    [SerializeField]
    private GameObject playerUIPrefab;


    private GameObject playerUIInstance;

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
           GetComponent<Player>().Setup();

        }
    }
    private void OnDisable()
    {     
        Destroy(playerUIInstance);
        GameManager.instance.SetSceneCameraActive(true);
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
