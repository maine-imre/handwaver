using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enableIfHost : MonoBehaviour
{
    #if PHOTON_UNITY_NETWORKING
    void Start()
    {
        if (!Photon.Pun.PhotonNetwork.IsMasterClient)
        {
            gameObject.SetActive(false);
        }
    }
    #endif
}
