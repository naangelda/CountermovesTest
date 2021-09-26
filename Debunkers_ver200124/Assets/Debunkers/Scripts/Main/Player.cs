using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;

public class Player : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField] SpriteRenderer playerCardImage;
    [SerializeField] SpriteRenderer playerIcon;
    [SerializeField] Text playerName;
    [SerializeField] Text playerCardCountText;
    [SerializeField] Image roundMarkTop, roundMarkBottom;
    [SerializeField] public int playerCardCount;
    [SerializeField] public int playerIdInRoom;

    int iconIDForReseive;
    string nameForReseive;
    int idForReseive;
    int cardCountForReseive;


    [SerializeField] bool isDefender = false;
    [SerializeField] bool isAttacker = false;
    [SerializeField] GameObject defenderMark;
    [SerializeField] GameObject attackerMark;
    [SerializeField] bool isBetrayer = false;

    bool isdefenderForResrive;
    bool isattackerForResrive;

    bool isSetActive = false;


    [SerializeField] public static int myPlayerID;

    [SerializeField] bool isReady = false;

    [SerializeField] PlayerCharactorScript myPlayerChara;
    [SerializeField] public static PlayerCharactorScript myPC;
    // Start is called before the first frame update
    void Start()
    {

        playerCardCountText.gameObject.SetActive(false);
        roundMarkTop.gameObject.SetActive(false);
        roundMarkBottom.gameObject.SetActive(false);
        if (photonView.IsMine)
        {
            myPlayerID = PlayerDataScript.Player.playerIdInRoom;
        }
    }
    [SerializeField] bool isCharachored = false;

    // Update is called once per frame
    void Update()
    {
        if (GameManager.roundStarted)
        {
            transform.localScale = new Vector3(0.5f, 0.5f, 1f);
        }
        else
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        if (RoundManager.roundStatuts == 0)
        {
            initPlayer();

        }
        if(RoundManager.roundStatuts == 1)
        {
            playerName.gameObject.SetActive(false);
            if (!isReady)
            {
                if (photonView.IsMine)
                {

                    if (transform.position.y > RoundManager.playerPosForRoundStartedForall[0].y -0.05f &&  transform.position.y < RoundManager.playerPosForRoundStartedForall[0].y + 0.05f)
                    {
                        transform.position = RoundManager.playerPosForRoundStartedForall[0];
                        isReady = true;
                        RoundManager.setPlayerReadySum(RoundManager.playerReadySum + 1);
                    }
                    else
                    {
                        transform.position = Vector2.Lerp(transform.position, RoundManager.playerPosForRoundStartedForall[0], Time.deltaTime * 5f);
                    }
                }
                else
                {
                    if (playerIdInRoom > myPlayerID)
                    {
                        if (transform.position.y > RoundManager.playerPosForRoundStartedForall[playerIdInRoom - myPlayerID].y - 0.05f
                            &&  transform.position.y < RoundManager.playerPosForRoundStartedForall[playerIdInRoom - myPlayerID].y +0.05f)
                        {
                            transform.position = RoundManager.playerPosForRoundStartedForall[playerIdInRoom - myPlayerID];
                            isReady = true;
                        }
                        else
                        {

                            transform.position = Vector2.Lerp(transform.position, RoundManager.playerPosForRoundStartedForall[playerIdInRoom - myPlayerID], Time.deltaTime * 5f);
                        }
                    }
                    else if (playerIdInRoom < myPlayerID)
                    {
                        if (transform.position.y > RoundManager.playerPosForRoundStartedForall[playerIdInRoom + 5 - myPlayerID].y - 0.05f
                               && transform.position.y < RoundManager.playerPosForRoundStartedForall[playerIdInRoom + 5 - myPlayerID].y +0.05f)
                        {
                            transform.position = RoundManager.playerPosForRoundStartedForall[playerIdInRoom + 5 - myPlayerID];
                            isReady = true;
                        }
                        else
                        {
                            transform.position = Vector2.Lerp(transform.position, RoundManager.playerPosForRoundStartedForall[playerIdInRoom + 5 - myPlayerID], Time.deltaTime * 5f);
                        }

                    }
                }
            }

        }
        if (RoundManager.roundStatuts == 2)
        {
            playerName.gameObject.SetActive(true);
            if (photonView.IsMine)
            {
                playerCardCountText.gameObject.SetActive(false);
            }
            else
            {
                playerCardCountText.gameObject.SetActive(true);

            }
        }
        if (RoundManager.roundStatuts == 3)
        {
            if (photonView.IsMine)
            {
                if (!isCharachored)
                {
                    var myChara = PhotonNetwork.Instantiate(myPlayerChara.name, new Vector3(-8f + PlayerDataScript.Player.playerIdInRoom - 1, -1f, -10f), Quaternion.identity);
                    myChara.GetComponent<PlayerCharactorScript>().myLevel = 1;
                    myChara.GetComponent<SpriteRenderer>().sprite = PlayerIconData.playerCharaForAll[PlayerDataScript.Player.iconId];
                    myPC = myChara.GetComponent<PlayerCharactorScript>();
                    isCharachored = true;
                }
            }
        }
        if (RoundManager.roundStatuts == 4)
        {
            if (photonView.IsMine)
            {

                playerCardCount = HandScript.myCardSumForStatic;
            }
            else
            {

                playerCardCount = cardCountForReseive;
            }

            if (RoundManager.roundPlayer == playerIdInRoom)
            {
                if (photonView.IsMine)
                {
                    roundMarkTop.gameObject.SetActive(true);
                    roundMarkBottom.gameObject.SetActive(false);
                }
                else
                {
                    roundMarkBottom.gameObject.SetActive(true);
                    roundMarkTop.gameObject.SetActive(false);
                }
            }
            else
            {
                roundMarkTop.gameObject.SetActive(false);
                roundMarkBottom.gameObject.SetActive(false);
            }
            playerCardCountText.text = "" + playerCardCount;
        }

    

        if (!PhotonNetwork.IsMasterClient) return;
        
        GameManager.setRoom(playerIdInRoom,playerName.text);
    }

    void initPlayer()
    {
        if (photonView.IsMine)
        {
            playerCardImage.sprite = PlayerIconData.playerCardsForAll[PlayerDataScript.Player.iconId];
            playerIcon.sprite = PlayerIconData.playerIconForAll[PlayerDataScript.Player.iconId];
            playerName.text = PhotonNetwork.LocalPlayer.NickName;
            playerIdInRoom = PlayerDataScript.Player.playerIdInRoom;
            playerCardCountText.text = "" + playerCardCount;
            if (PhotonNetwork.IsMasterClient)
            {
                var defender = new ExitGames.Client.Photon.Hashtable() { { "Defender", playerIdInRoom } };

                isDefender = true;
                isAttacker = false;
            }
            else
            {
                isAttacker = true;
                isDefender = false;
            }
        }
        else
        {
            playerCardImage.sprite = PlayerIconData.playerCardsForAll[iconIDForReseive];
            playerIcon.sprite = PlayerIconData.playerIconForAll[iconIDForReseive];
            playerName.text = nameForReseive;
            playerIdInRoom = idForReseive;
            playerCardCountText.text = "" + cardCountForReseive;
            if (isdefenderForResrive)
            {
                isDefender = true;
                isAttacker = false;
            }
            else
            {
                isAttacker = true;
                isDefender = false;
            }

        }


        if (isAttacker)
        {
            attackerMark.SetActive(true);
            defenderMark.SetActive(false);
        }
        else if (isDefender)
        {

            attackerMark.SetActive(false);
            defenderMark.SetActive(true);
        }
        else
        {
            attackerMark.SetActive(false);
            defenderMark.SetActive(false);
        }


    }


    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

        if (stream.IsWriting)
        {
            stream.SendNext(PlayerDataScript.Player.iconId);
            stream.SendNext(PlayerDataScript.Player.playerName);
            stream.SendNext(PlayerDataScript.Player.playerIdInRoom);
            stream.SendNext(isDefender);
            stream.SendNext(isAttacker);
            stream.SendNext(playerCardCount);
        }
        else
        {
            iconIDForReseive = (int)stream.ReceiveNext();
            nameForReseive = (string)stream.ReceiveNext();
            idForReseive = (int)stream.ReceiveNext();
            isdefenderForResrive = (bool)stream.ReceiveNext();
            isattackerForResrive = (bool)stream.ReceiveNext();
            cardCountForReseive = (int)stream.ReceiveNext();
        }


    }

}
