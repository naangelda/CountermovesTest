using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


using Photon.Pun;
using Photon.Realtime;

public class HandScript : MonoBehaviourPunCallbacks, IPunObservable
{

    [SerializeField] public CardScript[] myCards = new CardScript[100];
    [SerializeField] public int myCardSum = 0;
    [SerializeField] public static int myCardSumForStatic;
    [SerializeField] public Vector2 cardStartPos = new Vector2(0f, 0f);

    [SerializeField] public int handCardCountMax;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        myCardSumForStatic = myCardSum;
        updateCardPos();
        updateHandObj();
    }

    void updateCardPos()
    {
        if (myCardSum > 0)
        {
            if (myCardSum < 7)
            {
                cardStartPos = new Vector2(0f - (myCardSum - 1) * 50f, 0f);
                for (int i = 0; i < myCards.Length; i++)
                {
                    if (myCards[i] != null&&!myCards[i].isDroping)
                    {
                        Vector2 targetPos = new Vector2(cardStartPos.x +  i* 100f, 0f);
                        myCards[i].transform.localPosition = Vector2.Lerp(myCards[i].transform.localPosition, targetPos, Time.deltaTime * 10f);
                    }
                }
            }
            else
            {
                cardStartPos = new Vector2(-300f, 0f);
                for (int i = 0; i < myCards.Length; i++)
                {
                    if (myCards[i] != null && !myCards[i].isDroping)
                    {
                        Vector2 targetPos = new Vector2(cardStartPos.x + i*(600f/myCardSum), 0f);
                        myCards[i].transform.localPosition = Vector2.Lerp(myCards[i].transform.localPosition, targetPos, Time.deltaTime * 10f);
                    }
                }
            }
        }
    }

    void updateHandObj()
    {
        if (myCardSum >= 7)
        {
            /*
            var leftX = myCards[0].transform.localPosition.x;
            var rightX = myCards[myCardSum - 1].transform.localPosition.x;
            var width = rightX - leftX;
            var nowScaleX = width / 700f;

            for (int i = 0; i < myCards.Length; i++)
            {
                if (myCards[i] != null)
                {
                    myCards[i].transform.SetParent(null);
                }
            }
            transform.localScale = new Vector2(nowScaleX, 1f); for (int i = 0; i < myCards.Length; i++)
            {
                if (myCards[i] != null)
                {
                    myCards[i].transform.SetParent(transform);
                }
            }*/
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
}
