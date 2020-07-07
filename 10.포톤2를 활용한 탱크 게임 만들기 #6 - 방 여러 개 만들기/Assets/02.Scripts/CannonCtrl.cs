using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class CannonCtrl : MonoBehaviourPunCallbacks
{
    private Transform tr;
    public float rotSpeed = 300.0f;

    void Start()
    {
        tr = GetComponent<Transform>();
    }

    void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        tr.Rotate(Vector3.right * Input.GetAxis("Mouse ScrollWheel")
                                * rotSpeed * Time.deltaTime);
    }
}
