using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class GameMgr : MonoBehaviourPunCallbacks
{
    public Text msgList;
    public InputField ifSendMsg;

    void Start()
    {
        CreateTank();
        // photonNetwork의 데이터 통신을 다시 연결시켜준다. 
        PhotonNetwork.IsMessageQueueRunning = true; 
    }

    void CreateTank()
    {
        Transform[] points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();

        int idx = Random.Range(1, points.Length);
        PhotonNetwork.Instantiate("Tank", points[idx].position, Quaternion.identity);

    }

    //msg를 RPC의 버퍼에 보내준다.
    public void OnSendChatMsg()
    {
        string msg = string.Format("[{0}] {1}"
                                  , PhotonNetwork.LocalPlayer.NickName
                                  , ifSendMsg.text);
        photonView.RPC("ReceiveMsg", RpcTarget.OthersBuffered, msg);
        ReceiveMsg(msg);

        //    ifSendMsg.text = "";
        //    ifSendMsg.ActivateInputField();
    }

    //RPC의 버퍼에 있는 msg를 가져와서 
    [PunRPC]
    void ReceiveMsg(string msg)
    {
        msgList.text += "\n" + msg;
    }
}
