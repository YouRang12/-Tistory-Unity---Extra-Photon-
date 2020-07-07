using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonInit : MonoBehaviourPunCallbacks {

    public string gameVersion = "1.0";
    public string nickName = "YouRang";

    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    void Start()
    {
        OnLogin();
    }

    void OnLogin()
    {
        PhotonNetwork.GameVersion = this.gameVersion;
        PhotonNetwork.NickName = this.nickName;
        PhotonNetwork.ConnectUsingSettings();   // PUN2
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected !!!");
        PhotonNetwork.JoinRandomRoom();
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Failed join romm !!!");
        this.CreateRoom();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room");
        PhotonNetwork.Instantiate("Player", new Vector3(0, 0, 0), Quaternion.identity);
    }

    void CreateRoom()
    {
        // CreateRoom(방이름, 방옵션)
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 4 });
    }
}
