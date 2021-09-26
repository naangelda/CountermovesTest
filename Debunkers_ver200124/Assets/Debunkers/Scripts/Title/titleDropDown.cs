using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class titleDropDown : MonoBehaviour
{

    [SerializeField] public Dropdown my_dropDown;

    [SerializeField] public static bool dropdownLoad = false;

    [SerializeField] public static int nowIconId;


    [SerializeField] public Dropdown playerSex;
    [SerializeField] public string playerSexText;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerIconData.playerIconDataLoaded && !dropdownLoad) 
        {
            for (int i = 0; i < PlayerIconData.playerIconForAll.Length; i++)
            {
                Dropdown.OptionData r = new Dropdown.OptionData();
                r.text = "";
                r.image = PlayerIconData.playerIconForAll[i];
                my_dropDown.options.Add(r);
                dropdownLoad = true;
            }
        }
        nowIconId = my_dropDown.value;
        playerSexText = playerSex.captionText.text;
    }

    public void OnValueChanged()
    {
        nowIconId = my_dropDown.value;
        playerSexText = playerSex.captionText.text;
    }
    
}
