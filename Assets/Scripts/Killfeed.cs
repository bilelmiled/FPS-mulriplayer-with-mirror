using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killfeed : MonoBehaviour
{
    [SerializeField]
    private GameObject killfeddItem;
    void Start()
    {
        GameManager.instance.onPlayerKilledCallback += OnKill;
    }


    public void OnKill(string player,string source)
    {
        GameObject killItemInst = Instantiate(killfeddItem, transform);
        killItemInst.GetComponent<KillfeedItem>().Setup(player, source);
        Destroy(killItemInst, 3f);
    }


    void Update()
    {
        
    }
}
