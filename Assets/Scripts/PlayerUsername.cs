using UnityEngine;
using UnityEngine.UI;

public class PlayerUsername : MonoBehaviour
{

    [SerializeField]
    private Text usernameText;

    [SerializeField]
    private Player player;

    [SerializeField]
    private RectTransform healthBarFill;


    // Update is called once per frame
    void Update()
    {
        usernameText.text=player.username;
        healthBarFill.localScale = new Vector3(player.GetHealthPct(), 1f, 1f);
    }
}
