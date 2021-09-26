using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IDCardScript : MonoBehaviour
{
    Animator myAnim;

    [SerializeField] public bool canTakeIDCard = false;

    [SerializeField] float waitTimeLimit;
    float waitTimer = 0f;
    [SerializeField] Quaternion myRotation;
    [SerializeField] Vector2 myPosForShow;
    [SerializeField] Vector2 myPosForHide;
    [SerializeField] float ShowSpeed;

    [SerializeField] public bool took = false;

    [SerializeField] public bool item = false;

    [SerializeField] public bool showIDCard = false;
    [SerializeField] public bool hideIDCard = true;
    [SerializeField] GameObject frontWhenItemTrue;

    // Start is called before the first frame update
    void Start()
    {

        myAnim = GetComponent<Animator>();
        myRotation = transform.rotation;
        myPosForShow = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (canTakeIDCard)
        {
            if (waitTimer < waitTimeLimit)
            {
                waitTimer += Time.deltaTime;
            }
            else
            {
                canTakeIDCard = false;
                if (myAnim != null)
                {
                    myAnim.SetTrigger("Move");
                }
            }
        }

        if (item)
        {
            frontWhenItemTrue.SetActive(true);
            if (Input.GetKeyDown(KeyCode.I)&&!MatcingManager.matching)
            {
                if (showIDCard)
                {
                    showIDCard = false;
                    hideIDCard = true;
                }
                else
                {
                    showIDCard = true;
                    hideIDCard = false;
                }
            }
            
            
            if (showIDCard)
            {
                if (transform.localPosition.y < myPosForShow.y)
                {
                    transform.Translate(Vector2.up * Time.deltaTime * ShowSpeed);
                }
                else
                {
                    transform.localPosition = myPosForShow;
                }
            }
            if (hideIDCard)
            {
                
                if (transform.localPosition.y > myPosForHide.y)
                {
                    transform.Translate(Vector2.down * Time.deltaTime * ShowSpeed);
                }
                else
                {
                    transform.localPosition = myPosForHide; 
                }
            }
        }
        else
        {
            frontWhenItemTrue.SetActive(false);
        }

    }

    public void disableThis()
    {
        this.transform.rotation = myRotation;
        myPosForHide = transform.localPosition;
        took = true;
        hideIDCard = true;
        Destroy(myAnim);
        myAnim = null;
        //this.gameObject.SetActive(true);
    }
}
