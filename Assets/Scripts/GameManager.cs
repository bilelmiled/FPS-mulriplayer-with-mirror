using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public MatchSettings matchSettings;

    public static GameManager instance;

    [SerializeField]
    private GameObject sceneCamera;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            return;
        }
        Debug.LogError("plus d'un instance");
    }

    public void SetSceneCameraActive(bool isActive)
    {
        if(sceneCamera == null)
        {
            return;
        }
        sceneCamera.SetActive(isActive);
    }

    private const string playerIdPrefix = "Player";
    private static Dictionary<string, Player> players = new Dictionary<string, Player>();

    public static void RegisterPlayer(string netID , Player player)
    {
        string playerId = playerIdPrefix + netID;
        players.Add(playerId, player);
        player.gameObject.name = playerId;
    }

    public static void UnregisterPlayer(string playerId)
    {
        players.Remove(playerId);
    }

    public static Player GetPlayer(string playerId)
    {
        return players[playerId];
    }
   
}
