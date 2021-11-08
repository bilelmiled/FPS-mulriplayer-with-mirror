using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    private GameObject pausePanel;

    [SerializeField]
    private RectTransform healthBarFill;

    private Player player;
    private WeaponManager weaponManager;

    [SerializeField]
    private Text ammoUI;

    [SerializeField]
    private GameObject scoreBoardPanel;
    void Start()
    {
        PauseMenu.isOn = false;
    }

    public void SetPlayer(Player _player)
    {
        player = _player;
        weaponManager = player.GetComponent<WeaponManager>();
    }
    void Update()
    {

        SetHealthAmount(player.GetHealthPct());
        SetAmmoAmount(weaponManager.currentMagazineSize);
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

    private void SetAmmoAmount(int _ammount)
    {
        ammoUI.text = _ammount.ToString();
    }

    private void SetHealthAmount(float _damage)
    {
        healthBarFill.localScale = new Vector3(1f, _damage, 1f);
    }

    public void TogglePauseMenu()
    {
        //active.self l'etat actuelle du gameobject
        pausePanel.SetActive(!pausePanel.activeSelf);
        PauseMenu.isOn = pausePanel.activeSelf;
    }
}
