﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityStandardAssets.Utility;
using TMPro;

public class MoveCtrl : MonoBehaviourPunCallbacks
{
    private float h, v;
    private Rigidbody rb;
    private Transform tr;

    public float speed = 10.0f;
    public float rotSpeed = 60.0f;
    public TextMeshPro nickName;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0, -1.5f, 0); // 무게 중심점을 변경

        tr = GetComponent<Transform>();

        if (photonView.IsMine)
        {
            Camera.main.GetComponent<SmoothFollow>().target = tr.Find("CamPivot").transform;
        }

        nickName.text = photonView.Owner.NickName;
    }

    void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        v = Input.GetAxis("Vertical");
        h = Input.GetAxis("Horizontal");

        tr.Translate(Vector3.forward * v * speed * Time.deltaTime);
        tr.Rotate(Vector3.up * h * rotSpeed * Time.deltaTime);
    }
}


