using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Assassinate : CardScript
{

    [SerializeField] int targetPlayerID;
    // Start is called before the first frame update
    void Start()
    {
        targetPlayerID = 0;
    }

    // Update is called once per frame
    void Update()
    {

        myParent = RoundManager.myHandObjForAll;
        if (RoundManager.getRoundStatuts() >= 4)
        {
            checkMouseAction();
        }
        if (isDroping)
        {
            if (Input.GetMouseButtonUp(0)&&!isThrowing&&!isUsing)
            {
                isDroping = false;
                transform.SetParent(myParent.transform);
            }
            else if(Input.GetMouseButtonUp(0) && isThrowing)
            {
                isThrowing = false;
                CardDeckScript.threwCardsForAll[CardDeckScript.throwCardDeckSum] = Instantiate(this);
                CardDeckScript.threwCardsForAll[CardDeckScript.throwCardDeckSum].transform.localScale = new Vector2(0.25f, 0.25f);
                CardDeckScript.threwCardsForAll[CardDeckScript.throwCardDeckSum].transform.SetParent(RoundManager.myThrewFileldObjForAll.transform);
                RoundManager.setThrowCardDeckSum(CardDeckScript.throwCardDeckSum + 1);
                Destroy(this.gameObject);
            }else if (Input.GetMouseButtonUp(0) && isUsing)
            {

            }
        }
    }


    void checkMouseAction()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        if (hit.collider != null)
        {
            if (Input.GetMouseButtonDown(0)&&!isDroping)
            {
                if (hit.collider.tag == "card" && hit.collider.transform.gameObject == transform.gameObject)
                {
                    isDroping = true;
                    transform.SetParent(GameManager.myMouse.transform);
                    transform.localPosition = new Vector3(0f, 0f, 0f);
                }
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "ThrowArea")
        {
            isThrowing = true;
        }
        else
        {
            isThrowing = false;
        }
        if (collision.tag == "player")
        {
            isUsing = true;
            targetPlayerID = collision.GetComponent<Player>().playerIdInRoom;
        }
        else
        {
            isUsing = false;
        }
    }
    

}
