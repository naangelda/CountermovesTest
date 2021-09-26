using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIconData : MonoBehaviour
{
    [SerializeField] Sprite[] playerIcons;
    public static Sprite[] playerIconForAll;
    public static bool playerIconDataLoaded = false;

    [SerializeField] Sprite[] playerCards;
    public static Sprite[] playerCardsForAll;

    [SerializeField] Sprite[] playerChara;
    public static Sprite[] playerCharaForAll;

    // Start is called before the first frame update
    void Start()
    {

        playerIconForAll = playerIcons;
        playerCardsForAll = playerCards;
        playerCharaForAll = playerChara;
        playerIconDataLoaded = true;

    }

    // Update is called once per frame
    void Update()
    {
        playerIconForAll = playerIcons;
    }
}
