using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;

public class InputFieldScript : MonoBehaviourPun
{
    [SerializeField] public InputField playerName;
    [SerializeField] InputField playerBrithYear, playerBrithMonth, playerBrithDay;

    [SerializeField] public titleDropDown playerIcon;
    [SerializeField] public string playerIDString;
    [SerializeField] public int playerID;
    [SerializeField] Text playerIDText;

    int playerNum;
    string playerYear;
    string playerMonth;
    string playerDay;
    string playerSex;

    [SerializeField] public bool playerdataLoaded = false;

    // Start is called before the first frame update
    void Start()
    {
        resetInput();
    }

    // Update is called once per frame
    void Update()
    {
        playerYear = playerBrithYear.text;
        playerMonth = playerBrithMonth.text;
        playerDay = playerBrithDay.text;
        playerSex = playerIcon.playerSex.captionText.text;
    }


    public void resetInput()
    {
        playerName.text = "";
        playerBrithYear.text = "";
        playerBrithMonth.text = "";
        playerBrithDay.text = "";
        playerIcon.my_dropDown.value = 0;
        playerIcon.playerSex.value = 0;
    }

    public void getIDCard()
    {
        
        checkIDCard();
    }

    void checkIDCard()
    {
        if (playerBrithYear.text == "")
        {
            var year = Random.Range(0, 100000);
            playerBrithYear.text = "" + year;
        }
        if (playerBrithMonth.text == "")
        {
            var month = Random.Range(1, 13);
            playerBrithMonth.text = "" + month;
            if (playerBrithDay.text == "")
            {
                int day = 0;
                if (month == 2)
                {

                    day = Random.Range(1, 29);
                }
                else if (month % 2 == 0 && month <= 7)
                {
                    day = Random.Range(1, 32);
                }
                else if(month % 2 == 0 && month >= 8)
                {
                    day = Random.Range(1, 31);
                }
                else if (month % 2 != 0 && month <= 7)
                {
                    day = Random.Range(1, 31);
                }
                else
                {
                    day = Random.Range(1, 32);
                }
                playerBrithDay.text = "" + day;
            }
        }
        if (playerSex == "")
        {
            var r = Random.Range(1, 4);
            playerIcon.playerSex.value = r;
        }
        if(playerIcon.my_dropDown.value <= 0)
        {
            var r = Random.Range(1, playerIcon.my_dropDown.options.Count);
            playerIcon.my_dropDown.value = r;
        }

        playerYear = playerBrithYear.text;
        playerMonth = playerBrithMonth.text;
        playerDay = playerBrithDay.text;
        playerSex = playerIcon.playerSex.captionText.text;

        createID();
        playerIDText.text = playerIDString;

        if (playerName.text == "")
        {
            playerName.text = "游客" + playerYear+ playerMonth+ playerDay;
        }
        playerdataLoaded = true;
    }

    void createID()
    {
        string pID = "" + playerID;
        for (int i = 0; i < 9 - pID.Length; i++)
        {
            playerIDString += "0";
        }
        playerIDString += pID;

        for (int i = 0; i < 5 - playerYear.Length; i++)
        {
            playerIDString += "0";
        }
        playerIDString += playerYear;
        for(int i = 0; i < 2 - playerMonth.Length; i++)
        {
            playerIDString += "0";
        }
        playerIDString += playerMonth;
        for (int i = 0; i < 2 - playerDay.Length; i++)
        {
            playerIDString += "0";
        }
        playerIDString += playerDay;
    }
}
