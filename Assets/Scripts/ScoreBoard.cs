using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreBoard : MonoBehaviour
{
    [SerializeField]
    private GameObject playerScoreBoardItem;

    [SerializeField]
    private Transform playerScoreboardList;
    private void OnEnable()
    {
        //recuperer la liste des joueurs
        Player[] players = GameManager.GetAllPlayers();
        foreach (Player player in players)
        {
            GameObject itemScore = Instantiate(playerScoreBoardItem, playerScoreboardList);
            PlayerItem item = itemScore.GetComponent<PlayerItem>();
            if(item!= null)
            {
                item.setup(player);
            }
        }
    }

    private void OnDisable()
    {
        foreach (Transform item in playerScoreboardList)
        {
            Destroy(item.gameObject);
        }
    }
}
