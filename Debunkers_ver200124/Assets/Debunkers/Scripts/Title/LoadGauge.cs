using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadGauge : MonoBehaviour
{
    [SerializeField] Image gauge;
    [SerializeField] float loadValue;
    [SerializeField] public float loadSpeed;
    [SerializeField] public float timeLimit;
    float timer;

    [SerializeField] public bool startLoad = false;
    [SerializeField] public bool loaded = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (startLoad)
        {
            loadValue += Time.deltaTime * loadSpeed / 10f;
            gauge.fillAmount = loadValue;
            if (gauge.fillAmount >= 1)
            {
                startLoad = false;
                loaded = true;
                loadValue = 0f;
                
            }
        }

    }
}
