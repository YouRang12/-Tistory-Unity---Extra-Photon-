using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonInit : MonoBehaviourPunCallbacks {

    private string gameVersion = "1.0";
    public string userId = "YouRang";
    public byte maxPlayer = 20;


    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    void Start()
    {
        PhotonNetwork.GameVersion = this.gameVersion;
        PhotonNetwork.NickName = userId;

        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connect To Master");
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Failed Join room !!!");
        PhotonNetwork.CreateRoom(null
                                , new RoomOptions { MaxPlayers = this.maxPlayer });
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room !!!");
        PhotonNetwork.Instantiate("Tank", new Vector3(0, 3.0f, 0), Quaternion.identity);
    }
}
