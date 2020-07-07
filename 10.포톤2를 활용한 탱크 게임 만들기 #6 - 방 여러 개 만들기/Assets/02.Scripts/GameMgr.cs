using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMgr : MonoBehaviourPunCallbacks
{
    public Text msgList;
    public InputField ifSendMsg;
    public Text playerCount;

    void Start()
    {
        CreateTank();
        // photonNetwork의 데이터 통신을 다시 연결시켜준다. 
        PhotonNetwork.IsMessageQueueRunning = true;
        Invoke("CheckPlayerCount", 0.5f);
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

    public void OnExitClick()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("Lobby");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        string msg = string.Format("\n<color=#00ff00>[{0}]님이 입장했습니다.</color>"
                                    , newPlayer.NickName);

        //photonView.RPC("ReceiveMsg", RpcTarget.Others, msg);
        ReceiveMsg(msg);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        CheckPlayerCount();

        string msg = string.Format("\n<color=#ff0000>[{0}]님이 퇴장했습니다.</color>"
                                    , otherPlayer.NickName);

        //photonView.RPC("ReceiveMsg", RpcTarget.Others, msg);
        ReceiveMsg(msg);
    }

    void CheckPlayerCount()
    {
        int currPlayer = PhotonNetwork.PlayerList.Length;
        int maxPlayer = PhotonNetwork.CurrentRoom.MaxPlayers;
        playerCount.text = string.Format("[{0}/{1}]", currPlayer, maxPlayer);
    }
}
