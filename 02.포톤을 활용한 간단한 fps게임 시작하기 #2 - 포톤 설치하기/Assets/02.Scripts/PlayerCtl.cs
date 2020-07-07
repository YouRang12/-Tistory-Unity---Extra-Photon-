using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Utility;

public class PlayerCtl : MonoBehaviour {

    public float speed = 5.0f;
    public float rotSpeed = 120.0f;

    private Transform tr;
    private PhotonView pv;

    public Material[] _material;

    private Vector3 currPos;
    private Quaternion currRot;

    void Start()
    {
        tr = GetComponent<Transform>();
        pv = GetComponent<PhotonView>();

        // 동기화 콜백함수가 발생하려면 반드시 본 스크립트를 연결 시켜준다.
        pv.ObservedComponents[0] = this;

        if (pv.isMine)
        {
            // 자신의 플레이어에게만 카메라 제어권을 연결한다.
            Camera.main.GetComponent<SmoothFollow>().target = tr;
            this.GetComponent<Renderer>().material = _material[0];
        }
        else
        {
            this.GetComponent<Renderer>().material = _material[1];
        }
    }
    void Update()
    {
        if (pv.isMine)
        {
            // 자신의 플레이어만 키보드 조작을 허용한다.
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            tr.Translate(Vector3.forward * v * Time.deltaTime * speed);
            tr.Rotate(Vector3.up * h * Time.deltaTime * rotSpeed);
        }
        else
        {
            // 네트워크로 연결된 다른 유저일 경우에는 실시간 전송 받는 변수를 이용해 이동시켜준다.
            tr.position = Vector3.Lerp(tr.position, currPos, Time.deltaTime * 10.0f);
            tr.rotation = Quaternion.Lerp(tr.rotation, currRot, Time.deltaTime * 10.0f);
        }
    }

    // 동기화 콜백함수
    void OnPhotonSerializeView (PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            // 자신의 플레이 정보는 다른 네트워크 사용자에게 송신한다.
            stream.SendNext(tr.position);
            stream.SendNext(tr.rotation);
        }
        else
        {
            // 타 플레이어의 정보는 수신한다.
            currPos = (Vector3)stream.ReceiveNext();
            currRot = (Quaternion)stream.ReceiveNext();
        }
    }
}
