using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public class PlayerDataScript : MonoBehaviourPun,IPunObservable
{
    public struct player
    {
        public string playerName;
        public int iconId;
        public string playerSex;
        public string playerId;
        public int playerIdInRoom;
        public bool isBuwei;

    }

    public bool playerdataloaded = false;

    public static player Player = new player();

    public static player[] PlayersInRoom = new player[5];

    public player[] playersinRoomForReseive = new player[5];
    /*
    public PlayerDataScript(string name, string id,int iconid)
    {

    }
    */
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            PlayersInRoom = playersinRoomForReseive;
        }
    }

    public void initPlayerData(string name, string id, int iconid)
    {

       //if (!photonView.IsMine) return;

        Player.playerId = id;
        Player.playerName = name;
        Player.iconId = iconid;
        Player.isBuwei = false;
        PhotonNetwork.LocalPlayer.NickName = name;
        playerdataloaded = true;
    }

    public string getPlayerID()
    {
        return Player.playerId;
    }
    
    public static int getPlayerIDInRoom(string name)
    {
        foreach(var p in PlayersInRoom)
        {
            if(p.playerName == name)
            {
                return p.playerIdInRoom;
            }
        }
        return -1;
    }

    // MonoBehaviourPunCallbacks
    
    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

        if (stream.IsWriting)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                foreach (var p in PlayersInRoom)
                {
                    stream.SendNext(p.playerIdInRoom);
                    stream.SendNext(p.playerName);
                }
            }
        }
        else
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                for (int i = 0; i < playersinRoomForReseive.Length; i++)
                {
                    playersinRoomForReseive[i].playerIdInRoom = (int)stream.ReceiveNext();
                    playersinRoomForReseive[i].playerName = (string)stream.ReceiveNext();
                }
            }
        }

    }
}
