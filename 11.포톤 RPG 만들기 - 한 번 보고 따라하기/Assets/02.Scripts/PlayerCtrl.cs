using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityStandardAssets.Utility;

public class PlayerCtrl : MonoBehaviourPunCallbacks, IPunObservable {

    private float h, v, r;
    private Transform tr;
    private Animator anim;
    public float speed = 10.0f;
    public float rotSpeed = 60.0f;

    private bool isAttack = false;

    void Start()
    {
        tr = GetComponent<Transform>();
        anim = GetComponent<Animator>();

        if (photonView.IsMine)
        {
            Camera.main.GetComponent<SmoothFollow>().target = tr;
        }
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            v = Input.GetAxis("Vertical");
            h = Input.GetAxis("Horizontal");
            Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h);
            tr.Translate(moveDir * Time.deltaTime * speed);

            r = Input.GetAxis("Mouse X");
            tr.Rotate(Vector3.up * Time.deltaTime * r * rotSpeed);

            if (Input.GetKeyDown(KeyCode.K))
            {
                anim.SetTrigger("Attack");
                isAttack = true;
            }
        }
        else
        {
            tr.position = Vector3.Lerp(tr.position, currPos, Time.deltaTime * 10.0f);
            tr.rotation = Quaternion.Slerp(tr.rotation, currRot, Time.deltaTime * 10.0f);
            bool attack = currAttack;
            if (currAttack)
            {
                anim.SetTrigger("Attack");
                currAttack = false;
            }
        }
    }

    private Vector3 currPos;
    private Quaternion currRot;
    private bool currAttack = true;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // Sending Datas ...
            stream.SendNext(tr.position);
            stream.SendNext(tr.rotation);
            stream.SendNext(isAttack);
            isAttack = false;
        }
        else
        {
            currPos = (Vector3)stream.ReceiveNext();
            currRot = (Quaternion)stream.ReceiveNext();
            currAttack = (bool)stream.ReceiveNext();
        }

    }
}
