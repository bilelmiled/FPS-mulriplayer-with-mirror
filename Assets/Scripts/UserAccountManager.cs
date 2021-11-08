using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class UserAccountManager : MonoBehaviour
{
    public static UserAccountManager instance;

    public static string loggedInUsername;
    public string lobbyName = "Lobby";

    void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this);
    }

    public void LogIn(Text username)
    {
        loggedInUsername = username.text;
        Debug.Log(loggedInUsername);
        SceneManager.LoadScene(lobbyName);
    }
}
