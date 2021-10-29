using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    private GameObject pausePanel;

    [SerializeField]
    private GameObject scoreBoardPanel;
    void Start()
    {
        PauseMenu.isOn = false;
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            scoreBoardPanel.SetActive(true);
        }
        else if(Input.GetKeyUp(KeyCode.Tab))
        {
            scoreBoardPanel.SetActive(false);
        }
    }
    public void TogglePauseMenu()
    {
        //active.self l'etat actuelle du gameobject
        pausePanel.SetActive(!pausePanel.activeSelf);
        PauseMenu.isOn = pausePanel.activeSelf;
    }
}
