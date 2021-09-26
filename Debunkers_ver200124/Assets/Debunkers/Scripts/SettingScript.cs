using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingScript : MonoBehaviour
{

    [SerializeField] GameObject settingObj;
    [SerializeField] public bool isInSetting = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        checkSetting();
        getKey();
    }

    void getKey()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isInSetting)
            {
                isInSetting = false;
            }
            else
            {
                
                isInSetting = true;
            }
        }
    }

    void checkSetting()
    {
        if (isInSetting)
        {
            settingObj.SetActive(true);
        }
        else
        {

            settingObj.SetActive(false);
        }
    }

    public void OnClickSettingButton()
    {
        if (isInSetting)
        {
            isInSetting = false;
        }
        else
        {
        
            isInSetting = true;
        }
    }

    [SerializeField] Dropdown changeScreenMode;
    

    public void OnScreenModeChange()
    {
        if(changeScreenMode.value == 0)
        {
            Screen.SetResolution(1280, 720, true);
            Screen.fullScreen = false;
        }else if (changeScreenMode.value == 1)
        {
            Screen.SetResolution(1920, 1080, true);
        }
        else
        {
            Screen.SetResolution(1280, 720, true);
            Screen.fullScreen = false;
        }
    }

    public void OnClickQuitButton()
    {

        Application.Quit();
    }
}
