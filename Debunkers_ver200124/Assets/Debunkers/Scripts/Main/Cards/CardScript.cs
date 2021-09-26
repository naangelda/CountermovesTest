using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardScript : MonoBehaviour
{
    [SerializeField] public string cardName;
    [SerializeField] public string cardClass;
    [SerializeField] public int cardNum;
    [SerializeField] public int cardMax;

    [SerializeField] public bool isDroping = false;
    [SerializeField] public bool isThrowing = false;
    [SerializeField] public bool isUsing = false;
    [SerializeField] public GameObject myParent;
    public enum Class
    {
        ActionCard = 0,
        TrapCard
    }

    public Class CardClass = new Class();

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }



}
