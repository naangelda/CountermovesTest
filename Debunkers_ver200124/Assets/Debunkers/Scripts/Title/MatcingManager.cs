using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;

public class MatcingManager : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject menuObj;
    [SerializeField] GameObject MatchGameTimeCounter;
    [SerializeField] public GameObject canelMatching;

    [SerializeField] Image matchGameImage;
    [SerializeField] bool flip = false;
    [SerializeField] float imageFillSpeed;

    [SerializeField] Text timer;

    [SerializeField] float matchTimerS;
    [SerializeField] float matchTimerM;
    [SerializeField] float matchTimerH;

    [SerializeField] public bool Matching = false;
    [SerializeField] public static bool matching = false;

    [SerializeField] public bool matchingAsDefender = false;
    [SerializeField] public bool matchingAsAttacker = false;


    [SerializeField] string[] textForMatching;
    [SerializeField] Text textWhenMatching;

    [SerializeField] bool isJoined = false;
    [SerializeField] Text playerCountInCurRoom;
    [SerializeField] GameObject playerCountInCurRoomObj;
    [SerializeField] float maxPlayerInCurRoom;
    [SerializeField] float nowPlayerInCurRoom;

    // Start is called before the first frame update
    void Start()
    {
        matchGameImage.fillAmount = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        matching = Matching;
        if (Matching)
        {
            matchTimerS += Time.deltaTime;
            if (matchTimerS >= 60)
            {
                matchTimerS = 0f;
                matchTimerM += 1f;
            }
            if (matchTimerM >= 60)
            {
                matchTimerM = 0f;
                matchTimerH += 1f;
            }
            var S = "";
            var M = "";
            var H = "";
            if (matchTimerS < 10)
            {
                S = "0" + (int)matchTimerS;
            }
            else
            {
                S = "" + (int)matchTimerS;
            }
            if (matchTimerM < 10)
            {
                M = "0" + (int)matchTimerM;
            }
            else
            {
                M = "" + (int)matchTimerM;
            }
            if (matchTimerH < 10)
            {
                H = "0" + (int)matchTimerH;
            }
            else
            {
                H = "" + (int)matchTimerH;
            }
            timer.text = H + ":" + M + ":" + S;


            if (!flip)
            {
                matchGameImage.fillAmount += Time.deltaTime * imageFillSpeed;
                if (matchGameImage.fillAmount >= 1)
                {
                    flip = true;
                    matchGameImage.transform.localScale = new Vector3(-1f, 1f, 1f);
                }
            }
            else
            {
                matchGameImage.fillAmount -= Time.deltaTime * imageFillSpeed;
                if (matchGameImage.fillAmount <= 0)
                {
                    flip = false;
                    matchGameImage.transform.localScale = new Vector3(1f, 1f, 1f);
                }
            }

            if (matchingAsDefender && !PhotonNetwork.InRoom && PhotonNetwork.InLobby)
            {
                if (PhotonNetwork.IsConnected)
                {

                    PhotonNetwork.CreateRoom(PhotonNetwork.LocalPlayer.NickName + "'s Room", new RoomOptions { MaxPlayers = 5 } );
                }
                else
                {

                    PhotonNetwork.OfflineMode = false;

                    PhotonNetwork.ConnectUsingSettings();
                }
            }
            if (matchingAsAttacker&&!PhotonNetwork.InRoom&&PhotonNetwork.InLobby)
            {
                if (PhotonNetwork.IsConnected)
                {

                    PhotonNetwork.JoinRandomRoom();
                }
                else
                {

                    PhotonNetwork.OfflineMode = false;

                    PhotonNetwork.ConnectUsingSettings();
                }
            }
            if (isJoined)
            {
                nowPlayerInCurRoom = PhotonNetwork.CurrentRoom.PlayerCount;
                maxPlayerInCurRoom = PhotonNetwork.CurrentRoom.MaxPlayers;
                playerCountInCurRoomObj.SetActive(true);
                playerCountInCurRoom.text = "(" + nowPlayerInCurRoom + "/" + maxPlayerInCurRoom + ")";
                if (nowPlayerInCurRoom == maxPlayerInCurRoom)
                {

                    if (loadWaiteTimer < loadWaitTimeLimit)
                    {
                        textWhenMatching.text = "正在开始游戏";
                        loadWaiteTimer += Time.deltaTime;
                    }
                    else
                    {
                        loadWaiteTimer = 0f;
                        PhotonNetwork.LoadLevel(1);
                    }
                }
            }
            else
            {

                playerCountInCurRoomObj.SetActive(false);
            }
        }

    }

    float loadWaitTimeLimit = 3f;
    float loadWaiteTimer = 0f;

    public void OnClickMatchGameAsDefender()
    {
        if (PhotonNetwork.InLobby)
        {
            Matching = true;
            menuObj.SetActive(false);
            MatchGameTimeCounter.SetActive(true);
            matchingAsDefender = true;
            textWhenMatching.text = textForMatching[1];
            canelMatching.SetActive(false);
        }
        else if(PhotonNetwork.InRoom)
        {

            PhotonNetwork.LeaveRoom();
            PhotonNetwork.JoinLobby();
        }
        else
        {

            PhotonNetwork.JoinLobby();
        }
    }
    public void OnClickMatchGameAsAttacker()
    {
        if (PhotonNetwork.InLobby)
        {
            Matching = true;
            menuObj.SetActive(false);
            MatchGameTimeCounter.SetActive(true);
            matchingAsAttacker = true;
            textWhenMatching.text = textForMatching[0];
            canelMatching.SetActive(false);
        }
        else if (PhotonNetwork.InRoom)
        {

            PhotonNetwork.LeaveRoom();
            PhotonNetwork.JoinLobby();
        }
        else
        {

            PhotonNetwork.JoinLobby();
        }
    }

    public void OnClickCancelMatching()
    {

        canelMatching.SetActive(false);
        matchTimerS = 0f;
        matchTimerM = 0f;
        matchTimerH = 0f;
        matchingAsDefender = false;
        matchingAsAttacker = false;
        Matching = false;
        MatchGameTimeCounter.SetActive(false);
        menuObj.SetActive(true);
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.JoinLobby();
        }

        isJoined = false;
    }


    public override void OnJoinRandomFailed(short returnCode, string message)
    {

        base.OnJoinRandomFailed(returnCode, message);
        print("Try to connect a random room");
        if (Matching)
        {
            PhotonNetwork.JoinRandomRoom();
            canelMatching.SetActive(true);
        }
        else if(!Matching)
        {
            if (!PhotonNetwork.InLobby)
            {
                PhotonNetwork.JoinLobby();
            }
        }
    }

    public override void OnCreatedRoom()
    { 
        base.OnJoinedRoom();
        canelMatching.SetActive(true);
        print("创建了房间：" + PhotonNetwork.CurrentRoom.Name);


    }


    public override void OnJoinedRoom()
    {
        print("加入了房间：" + PhotonNetwork.CurrentRoom.Name);
        object value=null;
        bool started = false;
        textWhenMatching.text = textForMatching[1];
        isJoined = true;
        canelMatching.SetActive(true);
        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("GameStarted", out value))
        {
            started = (bool)value;
        }
        if (started)
        {
            PlayerDataScript.Player.isBuwei = true;
            loadWaiteTimer = 0f;
            PhotonNetwork.LoadLevel(1);
            return;
        }
        PlayerDataScript.Player.playerIdInRoom = PhotonNetwork.CurrentRoom.PlayerCount;
    }

}
