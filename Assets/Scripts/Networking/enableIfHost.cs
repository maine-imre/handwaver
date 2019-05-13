using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enableIfHost : MonoBehaviour
{
    
    void Start()
    {
        if (!Photon.Pun.PhotonNetwork.IsMasterClient)
        {
            gameObject.SetActive(false);
        }
    }
}
