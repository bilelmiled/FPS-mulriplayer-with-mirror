using UnityEngine;
using UnityEngine.UI;

public class PlayerItem : MonoBehaviour
{
    [SerializeField]
    private Text usernameText;

    [SerializeField]
    private Text killsText;

    [SerializeField]
    private Text deathsKills;


    public void setup(Player player)
    {
        usernameText.text = player.username;
        killsText.text = "Kills : " + player.kills;
        deathsKills.text = "deaths : " + player.deaths;
    }
}
