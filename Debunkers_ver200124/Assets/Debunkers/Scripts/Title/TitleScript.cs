using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;

public class TitleScript : MonoBehaviourPunCallbacks, IPunObservable
{

    [SerializeField] InputFieldScript titleInputField;


    [SerializeField] GameObject LoginTitle;


    [SerializeField] GameObject ConfirmForInputWindow;
    [SerializeField] bool ConfirmIdCard = false;
    [SerializeField] bool canCreateIDCard = false;


    [SerializeField] GameObject CreateIDCardWindow;
    [SerializeField] bool CreatingIDCard = false;
    [SerializeField] bool CraetedIDCard = false;
    [SerializeField] Image CreateGaugeImage;
    [SerializeField] float CreateGaugeValue;
    [SerializeField] float CreateSpeed;
    [SerializeField] string[] theTextWhileCreating;
    [SerializeField] Text CreateIDCardText;

    [SerializeField] GameObject gaiZhangAnim;
    [SerializeField] IDCardScript myIDCard;


    [SerializeField] GameObject LoginLayout;

    [SerializeField] bool canJoinToLobby = false;

    [SerializeField] GameObject loadGaugeObj;
    [SerializeField] LoadGauge loadGauge;


    [SerializeField] public PlayerDataScript playerdata = new PlayerDataScript();

    [SerializeField] GameObject gameMenu;

    // Start is called before the first frame update
    void Start()
    {
        LoginTitle.SetActive(true);
        gameMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        aboutIDCard();
    }

    void aboutIDCard()
    {

        if (canCreateIDCard)
        {
            CreatingIDCard = true;
            createIDCrad();
        }
        if (myIDCard.took)
        {
            loadGaugeObj.SetActive(true);
            loadGauge.startLoad = true;
            
            if (loadGauge.loaded)
            {

                loadGauge.loaded = false;
                if (gaiZhangAnim != null)
                {
                    gaiZhangAnim.SetActive(false);
                    Destroy(gaiZhangAnim);
                }
                loadGaugeObj.SetActive(false);
                gameMenu.SetActive(true);
                myIDCard.took = false;
                myIDCard.item = true;
            }
        }

        if (!playerdata.playerdataloaded)
        {
            if (titleInputField.playerdataLoaded)
            {
                playerdata.initPlayerData(titleInputField.playerName.text, titleInputField.playerIDString, titleInputField.playerIcon.my_dropDown.value);
                
            }
        }
    }

    public void OnConfirmInput()
    {
        ConfirmForInputWindow.SetActive(true);
    }

    public void OnCancelConfirmInput()
    {
        ConfirmForInputWindow.SetActive(false);
    }

    public void OnResetInputField()
    {
        titleInputField.resetInput();
    }

    public void OnConfirmInputForSure()
    {
        ConfirmForInputWindow.SetActive(false);
        CreateIDCardWindow.SetActive(true);
        canCreateIDCard = true;
    }

    void createIDCrad()
    {
        if (CreatingIDCard)
        {
            if (CreateGaugeValue < theTextWhileCreating.Length+4)
            {
                CreateGaugeValue += Time.deltaTime * CreateSpeed/10f;
                CreateGaugeImage.fillAmount = CreateGaugeValue / 10f;

                if (CreateGaugeValue < theTextWhileCreating.Length-1)
                {
                    CreateIDCardText.text = theTextWhileCreating[(int)CreateGaugeValue];
                }
                else if(CreateGaugeValue > theTextWhileCreating.Length+2)
                {
                    CreateIDCardText.text = theTextWhileCreating[theTextWhileCreating.Length-1];
                }
            }
            else
            {
                canJoinToLobby = true;
                if (PhotonNetwork.IsConnected)
                {

                }
                else
                {
                    PhotonNetwork.OfflineMode = false;

                    PhotonNetwork.ConnectUsingSettings();
                }
            }
        }
    }
    




    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

        if (stream.IsWriting)
        {
        }
        else
        {

        }


    }

    [SerializeField] MatcingManager matchManager;


    public override void OnConnectedToMaster()
    {
        Debug.Log("PUN Basics Tutorial/Launcher: OnConnetedToMaster() was called by PUN");
        Debug.Log("-----接続できました！-----");
        //PhotonNetwork.JoinRandomRoom();

        LoginTitle.SetActive(true);
        if (canJoinToLobby)
        {

            CreatingIDCard = false;
            canJoinToLobby = false;
            PhotonNetwork.JoinLobby();
        }

        if (matchManager.matchingAsAttacker)
        {
            PhotonNetwork.JoinRandomRoom();
        }
        else if(matchManager.matchingAsDefender)
        {
            PhotonNetwork.CreateRoom(PhotonNetwork.LocalPlayer.NickName + "'s Room", new RoomOptions { MaxPlayers = 5 });
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat("PUN Basics Tutoral/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
        Debug.Log("-----接続中断しました！-----");
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("-----ロビーに参加できました！-----");
        if (!matchManager.Matching)
        {
            titleInputField.playerID = PhotonNetwork.CountOfPlayers;
            titleInputField.getIDCard();
            canCreateIDCard = false;
            CreateIDCardWindow.SetActive(false);
            LoginLayout.SetActive(false);
            if (gaiZhangAnim != null)
            {
                gaiZhangAnim.SetActive(true);
            }
        }
        else
        {
            matchManager.canelMatching.SetActive(true);
        }

    }
}
