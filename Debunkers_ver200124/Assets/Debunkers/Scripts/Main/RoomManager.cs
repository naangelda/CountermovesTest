using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public class RoomManager : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField] public bool[] playerActive = new bool[5];
    [SerializeField] public bool isSomeoneLeave = false;
    bool isSomeOneLeaveForReseive;


    // Start is called before the first frame update
    void Start()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            isSomeoneLeave = isSomeOneLeaveForReseive;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            isSomeoneLeave = isSomeOneLeaveForReseive;
        }

    }

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

        if (stream.IsWriting)
        {
            stream.SendNext(isSomeoneLeave);
        }
        else
        {
            isSomeOneLeaveForReseive = (bool)stream.ReceiveNext();
        }


    }
}
