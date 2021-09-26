using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class CardDeckScript : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField] public Assassinate assaCard;
    [SerializeField] public Unlock unlockCard;
    [SerializeField] public Relocate relocateCard;
    [SerializeField] public Investigate investCard;

    [SerializeField] public CardScript[] cards;
    [SerializeField] public CardScript[] cardsForAll;
    [SerializeField] int[] cardsSortInt;
    [SerializeField] int cardSum;

    [SerializeField] bool cardShuffled = false;


    [SerializeField] GameObject cardDeckObj;
    [SerializeField] Text cardDeckSumText;
    public static GameObject cardDeck;
    public static int cardDeckSum;

    [SerializeField] GameObject throwCardDeckObj;
    [SerializeField] Text throwCardDeckSumText;
    public static GameObject throwCardDeck;
    public static int throwCardDeckSum;

    [SerializeField] public CardScript[] threwCards;
    public static CardScript[] threwCardsForAll;
    // Start is called before the first frame update
    void Start()
    {
        cardSum = assaCard.cardMax + unlockCard.cardMax + relocateCard.cardMax + investCard.cardMax;
        cards = new CardScript[cardSum];
        threwCardsForAll = new CardScript[cardSum];
        cardsSortInt = new int[cardSum];
        cardDeckSum = cardSum;
        throwCardDeckSum = 0;
        RoundManager.setCardDeckSum(cardDeckSum);
        RoundManager.setThrowCardDeckSum(throwCardDeckSum);

        for (int i =0;i<cardsSortInt.Length;i++)
        {
            cardsSortInt[i] = -1;
        }
        initCardDeck();

        if (!PhotonNetwork.IsMasterClient) return;
        shuffleCards();
        cardsForAll = cards;
        cardShuffled = true;
    }

    void initCardDeck()
    {
        for(int i = 0; i < cards.Length; i++)
        {
            if (i < assaCard.cardMax)
            {
                cards[i] = assaCard;
            }
            else if(i < assaCard.cardMax + unlockCard.cardMax)
            {
                cards[i] = unlockCard;
            }else if(i <assaCard.cardMax + unlockCard.cardMax + relocateCard.cardMax)
            {
                cards[i] = relocateCard;
            }
            else
            {
                cards[i] = investCard;
            }
        }
    }

    public void shuffleCards()
    {
        CardScript[] newCards = new CardScript[cardSum];
        for(int i = 0;i<cards.Length;i++)
        {
            int r = Random.Range(0, cardSum);
            do
            {
                r = Random.Range(0, cardSum);

            } while (newCards[r] != null);

            cardsSortInt[i] = r;
            newCards[r] = cards[i];
        }
        cards = newCards;
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            cardDeckSumText.text = "(" + RoundManager.getCardDeckSum() + ")";
            throwCardDeckSumText.text = "(" + RoundManager.getThrowCardDeckSum() + ")";
        }
        else
        {
            cardDeckSumText.text = "(" + RoundManager.getCardDeckSum() + ")";
            throwCardDeckSumText.text = "(" + RoundManager.getThrowCardDeckSum() + ")";
        }
        throwCardDeckSum = RoundManager.getThrowCardDeckSum();

        threwCards = threwCardsForAll;
        if (!PhotonNetwork.IsMasterClient && !cardShuffled)
        {

            CardScript[] newCards = new CardScript[cardSum];
            for (int i = 0; i < cards.Length; i++)
            {
                if (cardsSortInt[i] != -1)
                {
                    newCards[cardsSortInt[i]] = cards[i];
                }
                else
                {
                    return;
                }
            }
            cards = newCards;

            for(int i = 0; i < cards.Length; i++)
            { 
                if(cards[i] == null)
                {
                    return;
                }
                else
                {
                    if(i == cards.Length - 1)
                    {
                        cardShuffled = true;
                    }
                }
            }
        }
        updateThrewCards();
    }


    void updateThrewCards()
    {
        int i = 0;
        for (int j = 0; j < threwCardsForAll.Length; j++)
        {
            if (j % 18 == 0)
            {
                i++;
            }
            if (threwCardsForAll[j] != null)
            {
                threwCardsForAll[j].transform.position = new Vector2(-450f + j * 50f, 300f - 50f * i);
            }
        }
    }


    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

        if (stream.IsWriting)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                stream.SendNext(cardsSortInt);
            }
        }
        else
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                cardsSortInt = (int[])stream.ReceiveNext();
            }
        }


    }


}
