using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


using Photon.Pun;
using Photon.Realtime;


public class RoundManager : MonoBehaviourPunCallbacks, IPunObservable
{

    [SerializeField] GameObject beforeRoundStartObjs;
    [SerializeField] GameObject afterRoundStartObjs;

    [SerializeField] GameObject shuffleCardObj;
    [SerializeField] Text shuffleCardText;
    [SerializeField] float shuffleTimeLimit;
    [SerializeField] float shuffleTimer;

    [SerializeField] GameObject gameSettingObj;
    [SerializeField] GameObject settingWhenDefender;
    [SerializeField] GameObject settingWhenAttacker;
    [SerializeField] Text defenderSettingText;
    [SerializeField] Sprite targetImage;
    [SerializeField] Sprite fakeTargetImage;
    [SerializeField] GameObject confirmSettingButton;
    [SerializeField] Button pos1Button, pos2Button, pos3Button, pos4Button, pos5Button;


    [SerializeField] GameObject handObj;
    [SerializeField] GameObject roundStatutsObj;
    [SerializeField] Text roundStatusTxet;
    [SerializeField] Button drawButton;
    [SerializeField] Button endButton;
    [SerializeField] GameObject levelObj;

    [SerializeField] CardDeckScript cardDeck;
    [SerializeField] HandScript myHand;
    [SerializeField] GameObject myHandObj;
    [SerializeField] public static GameObject myHandObjForAll;

    [SerializeField] GameObject myThrewFieldObj;
    [SerializeField] public static GameObject myThrewFileldObjForAll;
    [SerializeField] GameObject throwCardsWindow;
    [SerializeField] bool isOnThrowCardsWindow = false;

    [SerializeField] GameObject[] playerPosForRoundStartedObjs;
    [SerializeField] Vector2[] playerPosForRoundStarted;
    [SerializeField] public static Vector2[] playerPosForRoundStartedForall;

    public static int roundStatuts = 0; // 0 未开始， 1 玩家移动到相应位置， 2 洗牌， 3 防御者放置目标和陷阱， 4 回合开始， 5 抽牌阶段， 6 行动阶段， 7 弃牌阶段   5 - 7 循环 ， 8 特殊阶段， 9 结算阶段， 10 计分板;

    public static int roundPlayer = 1; // 判断当前回合是 1 - 5 号玩家 的回合;

    public static int playerReadySum = 0; //

    public static int tartgetPos = 0;

    [SerializeField] int nowRoundStatuts,nowRoundPlayer,nowPlayerReadySum, nowTartgetPos;


    [SerializeField] int canDrawCardLimit;
    [SerializeField] int nowDrawedCardCount;

    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            var roundState = new ExitGames.Client.Photon.Hashtable() { { "roundStatuts", 0 } };
            PhotonNetwork.CurrentRoom.SetCustomProperties(roundState);

            var roundPlayer = new ExitGames.Client.Photon.Hashtable() { { "roundPlayer", PlayerDataScript.Player.playerIdInRoom } };
            PhotonNetwork.CurrentRoom.SetCustomProperties(roundPlayer);

            var playerReadySum = new ExitGames.Client.Photon.Hashtable() { { "playerReady", 0 } };
            PhotonNetwork.CurrentRoom.SetCustomProperties(playerReadySum);

            var targetPos = new ExitGames.Client.Photon.Hashtable() { { "targetPos", 0 } };
            PhotonNetwork.CurrentRoom.SetCustomProperties(targetPos);

            var cardDeckSum = new ExitGames.Client.Photon.Hashtable() { { "cardDeckSum", 0 } };
            PhotonNetwork.CurrentRoom.SetCustomProperties(cardDeckSum);

            var throwCardDeckSum = new ExitGames.Client.Photon.Hashtable() { { "throwCardDeckSum", 0 } };
            PhotonNetwork.CurrentRoom.SetCustomProperties(throwCardDeckSum);

            var defenderId = new ExitGames.Client.Photon.Hashtable() { { "defenderId", PlayerDataScript.Player.playerIdInRoom } };
            PhotonNetwork.CurrentRoom.SetCustomProperties(defenderId);
        }
        shuffleCardObj.SetActive(false);
        gameSettingObj.SetActive(false);
        settingWhenDefender.SetActive(false);
        settingWhenAttacker.SetActive(false);
        confirmSettingButton.SetActive(false);
        levelObj.SetActive(false);
        handObj.SetActive(false);
        roundStatutsObj.SetActive(false);
        roundStatusTxet.gameObject.SetActive(false);
        throwCardsWindow.SetActive(false);
        moveToLevelObj.SetActive(false);
        moveToLevelObjForAll = moveToLevelObj;
    }

    // Update is called once per frame
    void Update()
    {
        nowRoundStatuts = roundStatuts;
        nowRoundPlayer = roundPlayer;
        nowPlayerReadySum = playerReadySum;
        nowTartgetPos = tartgetPos;
        myHandObjForAll = myHandObj;
        moveToLevelObj = moveToLevelObjForAll;
        for (int i = 0; i < playerPosForRoundStartedObjs.Length; i++)
        {
            playerPosForRoundStarted[i] = playerPosForRoundStartedObjs[i].transform.position;
        }
        playerPosForRoundStartedForall = playerPosForRoundStarted;

        if (GameManager.roundStarted)
        {
            beforeRoundStartObjs.SetActive(false);
            afterRoundStartObjs.SetActive(true);

        }
        else
        {
            beforeRoundStartObjs.SetActive(true);
            afterRoundStartObjs.SetActive(false);

        }

        if (!PhotonNetwork.IsMasterClient)
        {
            roundStatuts = getRoundStatuts();
            roundPlayer = getRoundPlayer();
        }
        
        roundStatuts = getRoundStatuts();
        roundPlayer = getRoundPlayer();
        if (roundStatuts == 0)
        {
            if (GameManager.roundStarted)
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    roundStatuts = 1;
                    setRoundStatuts(roundStatuts);
                }
            }
        }
        if (roundStatuts == 1)
        {
            playerReadySum = getPlayerReadySum();
            if (playerReadySum == 1)
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    roundStatuts = 2;
                    setRoundStatuts(roundStatuts);
                }
                levelObj.SetActive(true);
                handObj.SetActive(true);
                roundStatutsObj.SetActive(true);
                roundStatusTxet.gameObject.SetActive(true);
                roundStatusTxet.text = "准备中";
            }
        }
        if (roundStatuts == 2)
        {
            shuffleCardObj.SetActive(true);
            levelObj.SetActive(true);
            handObj.SetActive(true);
            roundStatutsObj.SetActive(true);
            roundStatusTxet.gameObject.SetActive(true);

            if (shuffleTimer < shuffleTimeLimit)
            {
                shuffleTimer += Time.deltaTime;
                shuffleCardText.text = "洗牌中……";
            }
            else
            {
                shuffleTimer = 0f;
                shuffleCardObj.SetActive(false);
                if (PhotonNetwork.IsMasterClient)
                {
                    roundStatuts = 3;
                    setRoundStatuts(roundStatuts);
                }
            }
        }
        if (roundStatuts == 3)
        {
            shuffleTimer = 0f;
            shuffleCardObj.SetActive(false);
            gameSettingObj.SetActive(true);
            levelObj.SetActive(true);
            handObj.SetActive(true);
            roundStatutsObj.SetActive(true);
            roundStatusTxet.gameObject.SetActive(true);
            if (PhotonNetwork.IsMasterClient)
            {
                settingWhenDefender.SetActive(true);
                settingWhenAttacker.SetActive(false);
            }
            else
            {
                settingWhenDefender.SetActive(false);
                settingWhenAttacker.SetActive(true);
            }
        }
        if (roundStatuts == 4)
        {
            gameSettingObj.SetActive(false);
            levelObj.SetActive(true);
            handObj.SetActive(true);
            roundStatutsObj.SetActive(true);
            roundStatusTxet.gameObject.SetActive(true);

            if (roundPlayer == PlayerDataScript.Player.playerIdInRoom)
            {
                roundStatusTxet.text = "你的回合" +",请抽牌";
                drawButton.enabled = true;
                endButton.enabled = true;
            }
            else
            {
                roundStatusTxet.text = "玩家"+getRoundPlayer()+"的回合";
                drawButton.enabled = false;
                endButton.enabled = false;
            }

        }

        if(roundStatuts == 5)
        {
            if (roundPlayer == PlayerDataScript.Player.playerIdInRoom)
            {
                roundStatusTxet.text = "你的回合" + ",请打牌";
                drawButton.enabled = true;
                endButton.enabled = true;
            }
            else
            {
                roundStatusTxet.text = "玩家" + getRoundPlayer() + "的回合";
                drawButton.enabled = false;
                endButton.enabled = false;
            }
        }
    }




    //Button///

    ////setting//
    public void setTargetPosTo1()
    {
        tartgetPos = 1;
        setTargetPos(tartgetPos);
        pos1Button.GetComponent<Image>().sprite = targetImage;
        pos2Button.GetComponent<Image>().sprite = fakeTargetImage;
        pos3Button.GetComponent<Image>().sprite = fakeTargetImage;
        pos4Button.GetComponent<Image>().sprite = fakeTargetImage;
        pos5Button.GetComponent<Image>().sprite = fakeTargetImage;
        confirmSettingButton.SetActive(true);
    }

    public void setTargetPosTo2()
    {
        tartgetPos =2;
        setTargetPos(tartgetPos);
        pos2Button.GetComponent<Image>().sprite = targetImage;
        pos1Button.GetComponent<Image>().sprite = fakeTargetImage;
        pos3Button.GetComponent<Image>().sprite = fakeTargetImage;
        pos4Button.GetComponent<Image>().sprite = fakeTargetImage;
        pos5Button.GetComponent<Image>().sprite = fakeTargetImage;
        confirmSettingButton.SetActive(true);
    }

    public void setTargetPosTo3()
    {

        tartgetPos = 3;
        setTargetPos(tartgetPos);
        pos3Button.GetComponent<Image>().sprite = targetImage;
        pos1Button.GetComponent<Image>().sprite = fakeTargetImage;
        pos2Button.GetComponent<Image>().sprite = fakeTargetImage;
        pos4Button.GetComponent<Image>().sprite = fakeTargetImage;
        pos5Button.GetComponent<Image>().sprite = fakeTargetImage;
        confirmSettingButton.SetActive(true);
    }

    public void setTargetPosTo4()
    {

        tartgetPos = 4;
        setTargetPos(tartgetPos);
        pos4Button.GetComponent<Image>().sprite = targetImage;
        pos1Button.GetComponent<Image>().sprite = fakeTargetImage;
        pos2Button.GetComponent<Image>().sprite = fakeTargetImage;
        pos3Button.GetComponent<Image>().sprite = fakeTargetImage;
        pos5Button.GetComponent<Image>().sprite = fakeTargetImage;
        confirmSettingButton.SetActive(true);
    }

    public void setTargetPosTo5()
    {

        tartgetPos = 5;
        setTargetPos(tartgetPos);
        pos5Button.GetComponent<Image>().sprite = targetImage;
        pos1Button.GetComponent<Image>().sprite = fakeTargetImage;
        pos2Button.GetComponent<Image>().sprite = fakeTargetImage;
        pos3Button.GetComponent<Image>().sprite = fakeTargetImage;
        pos4Button.GetComponent<Image>().sprite = fakeTargetImage;
        confirmSettingButton.SetActive(true);
    }

    public void confirmTargetPos()
    {

        confirmSettingButton.SetActive(false);
        settingWhenDefender.SetActive(false);
        settingWhenAttacker.SetActive(false);
        roundStatuts += 1;
        setRoundStatuts(roundStatuts);

    }
    ////GameUIButton//
    
    public void DrawCard()
    {
        if (getCardDeckSum() > 0&&nowDrawedCardCount<canDrawCardLimit)
        {

            var newCard = Instantiate(cardDeck.cards[getCardDeckSum() - 1]);
            //var newCard =PhotonNetwork.Instantiate(cardDeck.cards[getCardDeckSum() - 1].name, new Vector2(0f, 0f), Quaternion.identity);
            newCard.transform.SetParent(myHandObj.transform);
            newCard.transform.localPosition = new Vector3(0f, 0f,0f);
            newCard.myParent = myHandObj;
            myHand.myCards[myHand.myCardSum] = newCard;
            myHand.myCardSum += 1;
            setCardDeckSum(getCardDeckSum() - 1);
            nowDrawedCardCount += 1;
            if(nowDrawedCardCount == canDrawCardLimit)
            {
                roundStatuts = 5;
                setRoundStatuts(roundStatuts);
            }
        }
    }

    public void checkThrewCards()
    {
        if (!isOnThrowCardsWindow)
        {
            isOnThrowCardsWindow = true;
            throwCardsWindow.SetActive(true);
        }
    }

    public void closeThrewCardsWindo()
    {
        if (isOnThrowCardsWindow)
        {
            isOnThrowCardsWindow = false;
            throwCardsWindow.SetActive(false);
        }
    }
    [SerializeField] public int nowHandCardCountMax;
    

    public void EndRound()
    {
            if (myHand.myCardSum <= myHand.handCardCountMax)
            {
                nowDrawedCardCount = 0;
                if (roundPlayer < 5)
                {
                    roundPlayer = getRoundPlayer() + 1;
                }
                else
                {
                    roundPlayer = 1;

                }
                roundStatuts = 4;
                setRoundStatuts(roundStatuts);
                setRoundPlayer(roundPlayer);
            }
            else
            {
                roundStatusTxet.text = "请丢弃手牌至" + myHand.handCardCountMax + "以下";
            }
    }

    [SerializeField] GameObject moveToLevelObj;
    [SerializeField] public static GameObject moveToLevelObjForAll;

    public void moveUp()
    {
        moveToLevelObjForAll.SetActive(false);
        Player.myPC.myLevel += 1;
    }

    public void moveDown()
    {
        moveToLevelObjForAll.SetActive(false);
        Player.myPC.myLevel -= 1;
    }


    //Online////
    public static int getRoundStatuts()
    {
        object value = null;
        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("roundStatuts", out value)) {
            return (int)value;
        }
        return -1;
    }

    public static void setRoundStatuts(int rs)
    {

        var roundState = new ExitGames.Client.Photon.Hashtable() { { "roundStatuts", rs } };
        PhotonNetwork.CurrentRoom.SetCustomProperties(roundState);
    }

    public static int getRoundPlayer()
    {
        object value = null;
        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("roundPlayer", out value))
        {
            return (int)value;
        }
        return 0;
    }

    public static void setRoundPlayer(int rp)
    {

        var roundPlayer = new ExitGames.Client.Photon.Hashtable() { { "roundPlayer", rp } };
        PhotonNetwork.CurrentRoom.SetCustomProperties(roundPlayer);
    }

    public static int getPlayerReadySum()
    {
        object value = null;
        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("playerReady", out value))
        {
            return (int)value;
        }
        return 0;
    }

    public static void setPlayerReadySum(int pr)
    {
        var playerReadySum = new ExitGames.Client.Photon.Hashtable() { { "playerReady", pr } };
        PhotonNetwork.CurrentRoom.SetCustomProperties(playerReadySum);
    }

    public static int getTargetPos()
    {
        object value = null;
        if(PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("targetPos",out value))
        {
            return (int)value;
        }
        return 0;
    }

    public static void setTargetPos(int tp)
    {

        var targetPos = new ExitGames.Client.Photon.Hashtable() { { "targetPos", tp } };
        PhotonNetwork.CurrentRoom.SetCustomProperties(targetPos);
    }

    public static int getCardDeckSum()
    {
        object value = null;
        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("cardDeckSum", out value))
        {
            return (int)value;
        }
        return 0;
    }

    public static int getThrowCardDeckSum()
    {

        object value = null;
        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("throwCardDeckSum", out value))
        {
            return (int)value;
        }
        return 0;
    }

    public static void setCardDeckSum(int sum)
    {

        var cardDeckSum = new ExitGames.Client.Photon.Hashtable() { { "cardDeckSum", sum } };
        PhotonNetwork.CurrentRoom.SetCustomProperties(cardDeckSum);
    }

    public static void setThrowCardDeckSum(int sum)
    {

        var throwCardDeckSum = new ExitGames.Client.Photon.Hashtable() { { "throwCardDeckSum", sum } };
        PhotonNetwork.CurrentRoom.SetCustomProperties(throwCardDeckSum);
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
}
