using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GaiZhangAim : MonoBehaviour
{

    Animator myAnim;
    [SerializeField] GameObject passedObj;
    [SerializeField] IDCardScript myIDCard;
    // Start is called before the first frame update
    void Start()
    {
        myAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void destoryThis()
    {
        myIDCard.canTakeIDCard = true;
        this.gameObject.SetActive(false);
    }

    public void getPassed()
    {

        passedObj.SetActive(true);
    }
}
