using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


using Photon.Pun;
using Photon.Realtime;

public class PlayerCharactorScript : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField] Player myPlayer;

    [SerializeField] Image defenderHat;
    [SerializeField] Text playerName;
    [SerializeField] public string myPlayerName;
    [SerializeField] public int playerIDinRoom;

    string myPlayerNameForReceive;
    int playerIDinRoomForReceive;
    int mySpriteIDForReceive;

    [SerializeField] public bool isDefender;
    [SerializeField] public int myLevel;

    int myLevelForReceive;
    // Start is called before the first frame update
    void Start()
    {
        defenderHat.gameObject.SetActive(false);
        if (!photonView.IsMine) return;
        myPlayerName = PhotonNetwork.LocalPlayer.NickName;
        playerName.text = myPlayerName;
        playerIDinRoom = PlayerDataScript.Player.playerIdInRoom;
        this.GetComponent<SpriteRenderer>().sprite = PlayerIconData.playerCharaForAll[PlayerDataScript.Player.iconId];
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine)
        {
            myPlayerName = myPlayerNameForReceive;
            playerName.text = myPlayerNameForReceive;
            playerIDinRoom = playerIDinRoomForReceive;
            myLevel = myLevelForReceive;
            GetComponent<SpriteRenderer>().sprite = PlayerIconData.playerCharaForAll[mySpriteIDForReceive];
        }
        checkDefender();
        updateCharaPos();
    }

    void checkDefender()
    { 
        object value = null;
       
        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("defenderId", out value))
        {
            if (playerIDinRoom == (int)value)
            {
                isDefender = true;
            }
            else
            {
                isDefender = false;
            }
        }
        if (isDefender)
        {

            isDefender = true;
            defenderHat.gameObject.SetActive(true);
        }
        else
        {

            isDefender = false;
            defenderHat.gameObject.SetActive(false);
        }

    }

    void updateCharaPos()
    {
        var targetPos = new Vector2(transform.position.x, -1 + (myLevel - 1) * 1.13f);
        transform.position = Vector2.Lerp(transform.position, targetPos, Time.deltaTime);
    }


    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

        if (stream.IsWriting)
        {
            stream.SendNext(myPlayerName);
            stream.SendNext(playerIDinRoom);
            stream.SendNext(myLevel);
            stream.SendNext(PlayerDataScript.Player.iconId);
        }
        else
        {
            myPlayerNameForReceive = (string)stream.ReceiveNext();
            playerIDinRoomForReceive = (int)stream.ReceiveNext();
            myLevelForReceive = (int)stream.ReceiveNext();
            mySpriteIDForReceive = (int)stream.ReceiveNext();

        }


    }
}
