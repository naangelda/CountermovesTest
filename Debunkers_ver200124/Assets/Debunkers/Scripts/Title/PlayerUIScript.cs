using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIScript : MonoBehaviour
{
   // [SerializeField] PlayerDataScript playerData;
    [SerializeField] Image playerIcon;
    [SerializeField] Text playerName;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        playerIcon.sprite = PlayerIconData.playerIconForAll[PlayerDataScript.Player.iconId];
        playerName.text = PlayerDataScript.Player.playerName;
    }
}
