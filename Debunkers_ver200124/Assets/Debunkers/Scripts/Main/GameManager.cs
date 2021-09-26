using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

using UnityEngine.UI;

public class GameManager : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField] CardDeckScript gameCardDeck;
    //[SerializeField] public GameObject gcd;
    [SerializeField] Player player;
    
    [SerializeField] public static GameObject[] players = new GameObject[5];
    [SerializeField] GameObject[] playersForShow;

    [SerializeField] RoomManager curRoom;
    [SerializeField] public static RoomManager room = new RoomManager();

    RoomManager roomForReseive = new RoomManager();

    [SerializeField] bool gameStarted = false;

    [SerializeField] bool playerLoaded = false;

    [SerializeField] float loadWaitTimeLimit= 2f;
    [SerializeField] float loadWaitTimer;

    [SerializeField] float GameStartCountDownTimeLimit;
    [SerializeField] float GameStartCountDownTimer;
    [SerializeField] Text needPlayer;
    [SerializeField] Text timerText;
    [SerializeField] public static bool roundStarted = false;

    [SerializeField] public static GameObject myMouse;
    [SerializeField] GameObject mouseObj;
    //GameObject p;
    // Start is called before the first frame update
    void Start()
    {
        myMouse = mouseObj;
        if (PhotonNetwork.IsMasterClient)
        {
            ExitGames.Client.Photon.Hashtable p = new ExitGames.Client.Photon.Hashtable { { "buwei", 0 } };
            PhotonNetwork.CurrentRoom.SetCustomProperties(p);
            GameStartCountDownTimer = GameStartCountDownTimeLimit;
        }
    }

    void updateMousePos()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseObj.transform.position = new Vector3 (mousePos.x,mousePos.y,-20f);
        myMouse = mouseObj;
    }
    // Update is called once per frame
    void Update()
    {
        updateMousePos();
        if (!playerLoaded)
        {
            load();
        }
        playersForShow = players;
        if (!PhotonNetwork.IsMasterClient)
        {
            room.playerActive = roomForReseive.playerActive;
            
        }
        
        curRoom.playerActive = room.playerActive;
        if (playerLoaded)
        {
            beforeRoundStart();
        }
    }

    void beforeRoundStart()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount < PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            needPlayer.color = Color.red;
            needPlayer.text = "人数不足，等待玩家";
            if (PhotonNetwork.IsMasterClient)
            {
                GameStartCountDownTimer = GameStartCountDownTimeLimit;
            }
        }
        else
        {
            needPlayer.color = Color.black;
            needPlayer.text = "正在开始游戏中";
            if (PhotonNetwork.IsMasterClient)
            {
                GameStartCountDownTimer -= Time.deltaTime;
                ExitGames.Client.Photon.Hashtable timer = new ExitGames.Client.Photon.Hashtable { { "roundStartTimer", GameStartCountDownTimer } };
                PhotonNetwork.CurrentRoom.SetCustomProperties(timer);
            }

        }
        if (!roundStarted)
        {
            checkTimer();
        }
    }


    void checkTimer()
    {
        object value = null;
        if(PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("roundStartTimer",out value))
        {
            if ((float)value < 10)
            {
                timerText.color = Color.red;
                timerText.text = "0" + (int)((float)value);
                if ((float)value <= 0)
                {
                    timerText.text = "00";
                    roundStarted = true;

                }
            } else
            {
                timerText.color = Color.black;
                timerText.text = "" + (int)((float)value);
            }
        }
    }

    void load()
    {
        if (loadWaitTimer < loadWaitTimeLimit)
        {
            loadWaitTimer += Time.deltaTime;
        }
        else
        {
            loadWaitTimer = 0f;
            if (PlayerDataScript.Player.isBuwei)
            {
                for (int i = 0; i < room.playerActive.Length; i++)
                {
                    if (!room.playerActive[i])
                    {
                        object value = null;
                        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("buwei", out value))
                        {

                            if (i <= (int)value - 1)
                            {
                                continue;
                            }
                            else
                            {
                                ExitGames.Client.Photon.Hashtable p = new ExitGames.Client.Photon.Hashtable { { "buwei", i + 1 } };
                                PhotonNetwork.CurrentRoom.SetCustomProperties(p);
                                PlayerDataScript.Player.playerIdInRoom = i + 1;
                                setRoom(i + 1, PlayerDataScript.Player.playerName);
                                break;
                            }
                        }
                    }
                }
            }
            if (PhotonNetwork.IsMasterClient)
            {
                gameStarted = true;
                var properties = new ExitGames.Client.Photon.Hashtable();
                properties.Add("GameStarted", gameStarted);
                PhotonNetwork.CurrentRoom.SetCustomProperties(properties);
            }
            if (players[PlayerDataScript.Player.playerIdInRoom - 1] == null)
            {
                players[PlayerDataScript.Player.playerIdInRoom - 1] = PhotonNetwork.Instantiate(player.name, new Vector3(-5f + 2.5f * (PlayerDataScript.Player.playerIdInRoom - 1), 0f, -5f), Quaternion.identity);
            }
            room = curRoom;
            playerLoaded = true;
        }
    }

    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        base.OnRoomPropertiesUpdate(propertiesThatChanged);
        object value = null;
        if (propertiesThatChanged.TryGetValue("GameStarted", out value))
        {
            gameStarted = (bool)value;
        }
    }


    public static void setRoom(int id, string name)
    {
        if (id > 0)
        {
            room.playerActive[id - 1] = true;

            PlayerDataScript.PlayersInRoom[id - 1].playerName = name;
            PlayerDataScript.PlayersInRoom[id - 1].playerIdInRoom = id;

        }


    }




    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        int pid = PlayerDataScript.getPlayerIDInRoom(otherPlayer.NickName);
        if (pid > 0)
        {
            room.playerActive[pid - 1] = false;
        }
        room.isSomeoneLeave = true;
        ExitGames.Client.Photon.Hashtable p = new ExitGames.Client.Photon.Hashtable { { "buwei", 0} };
        PhotonNetwork.CurrentRoom.SetCustomProperties(p);
    }



    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

        if (stream.IsWriting)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                stream.SendNext(room.playerActive);
            }
        }
        else
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                roomForReseive.playerActive = (bool[])stream.ReceiveNext();
            }
        }


    }
}
