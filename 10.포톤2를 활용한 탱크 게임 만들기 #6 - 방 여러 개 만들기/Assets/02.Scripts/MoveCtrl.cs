using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityStandardAssets.Utility;
using TMPro;
using UnityEngine.UI;
using System;

public class MoveCtrl : MonoBehaviourPunCallbacks, IPunObservable
{
    private float h, v;
    private Rigidbody rb;
    private Transform tr;

    public float speed = 10.0f;
    public float rotSpeed = 60.0f;
    public TextMeshPro nickName;
    public Image hpBar;

    private float currHp = 100.0f;
    private float initHp = 100.0f;

    private bool isDie = false;
    public float respawnTime = 3.0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0, -1.5f, 0); // 무게 중심점을 변경

        tr = GetComponent<Transform>();

        if (photonView.IsMine)
        {
            Camera.main.GetComponent<SmoothFollow>().target = tr.Find("CamPivot").transform;
            rb.mass = 50000;
        }
        else
        {
            GetComponent<Rigidbody>().isKinematic = true;  //물리충돌 일어나지 않게 isKinematic
        }
        nickName.text = photonView.Owner.NickName;
    }

    void Update()
    {
        // 나거나, 죽지 않았을 때 
        if (photonView.IsMine && !isDie)
        {
            v = Input.GetAxis("Vertical");
            h = Input.GetAxis("Horizontal");
            tr.Translate(Vector3.forward * Time.deltaTime * v * speed);
            tr.Rotate(Vector3.up * Time.deltaTime * h * rotSpeed);
        }
        // 거리에 따른 Lerp 함수구현(상대방에게 나를 보여줄 때)
        else
        {
            if ((tr.position - currPos).sqrMagnitude >= 10.0f * 10.0f)   
            {
                tr.position = currPos;
                tr.rotation = currRot;
            }
            else
            {
                tr.position = Vector3.Lerp(tr.position, currPos, Time.deltaTime * 10.0f);
                tr.rotation = Quaternion.Slerp(tr.rotation, currRot, Time.deltaTime * 10.0f);
            }
        }
    }

    // 포탄에 맞았을 경우
    private void OnCollisionEnter(Collision coll)
    {
        if (coll.collider.CompareTag("CANNON") && !isDie)
        {
            int actorNumber = coll.gameObject.GetComponent<Cannon>().actorNumber;
            string hitter = GetNickNameByActorNumber(actorNumber);

            currHp -= 20.0f;
            hpBar.fillAmount = currHp / initHp;

            if (photonView.IsMine && currHp <= 0.0f)
            {
                isDie = true;
                Debug.Log("Killed by " + hitter);
                StartCoroutine(RespawnPlayer(actorNumber));
            }
        }
    }

    // 플레이어 다시 생성
    IEnumerator RespawnPlayer(int actorNumber)
    {
        Transform followTr = null;

        foreach (GameObject tank in GameObject.FindGameObjectsWithTag("TANK"))
        {
            if (tank.GetComponent<PhotonView>().OwnerActorNr == actorNumber)
            {
                followTr = tank.transform.Find("CamPivot").transform;
                break;
            }
        }
        Camera.main.GetComponent<SmoothFollow>().target = followTr;
        yield return new WaitForSeconds(respawnTime);
        Camera.main.GetComponent<SmoothFollow>().target = tr.Find("CamPivot").transform;
        currHp = 100.0f;
        hpBar.fillAmount = 1.0f;
        isDie = false;
    }

    //닉네임 가져오기
    string GetNickNameByActorNumber(int actorNumber)
    {
        //지금 현재 방에 접속한 사람의 닉네임을 가져온다   -- PlayerListOthers 자기 자신을 뺀 나머지 다 가져옴
        foreach (Player player in PhotonNetwork.PlayerListOthers)
        {
            if (player.ActorNumber == actorNumber)
            {
                return player.NickName;
            }
        }
        return "Ghost";
    }

    private Vector3 currPos;    // 실시간으로 전송하고 받는 변수
    private Quaternion currRot; // 실시간으로 전송하고 받는 변수
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting) //데이터를 계속 전송만
        {
            stream.SendNext(tr.position);   //내 탱크의 위치값을 보낸다
            stream.SendNext(tr.rotation);   //내 탱크의 회전값을 보낸다
        }
        else
        {
            //stream.ReceiveNext()는 오브젝트 타입이라  currPos에 맞게 vector3로 변경해준다.
            currPos = (Vector3)stream.ReceiveNext();  
            currRot = (Quaternion)stream.ReceiveNext();
        }
    }
}
