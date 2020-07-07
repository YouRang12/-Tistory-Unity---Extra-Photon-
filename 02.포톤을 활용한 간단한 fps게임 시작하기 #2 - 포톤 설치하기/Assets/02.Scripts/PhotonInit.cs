using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

public class PhotonInit : Photon.PunBehaviour {

	void Awake()
    {
        // 포톤네트워크에 버전별로 분리하여 접속한다.
        PhotonNetwork.ConnectUsingSettings("MyFps 1.0");
    }

    // 로비에 입장하였을 때 호출되는 콜백함수
    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby");
        PhotonNetwork.JoinRandomRoom();
    }

    // 랜덤 룸 입장에 실패하였을 때 호출되는 콜백함수
    public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
    {
        Debug.Log("No Room");
        PhotonNetwork.CreateRoom("MyRoom");
    }

    // 룸을 생성완료 하였을 때 호출되는 콜백함수
    public override void OnCreatedRoom()
    {
        Debug.Log("Finish make a room");
    }

    // 룸에 입장되었을 경우 호출되는 콜백함수
    public override void OnJoinedRoom()
    {
        Debug.Log("Joined room");

        // 플레이어를 생성한다.
        StartCoroutine(this.CreatePlayer());
    }

    // 네트워크상에 연결되어 있는 모든 클라이언트에 플레이어를 생성한다.
    IEnumerator CreatePlayer()
    {
        // 유니티의 Instantiate가 아니다. 사용법이 다르다.
        PhotonNetwork.Instantiate("Player",
                                    new Vector3(0, 1, 0),
                                    Quaternion.identity,
                                    0);
        yield return null;
    }

    void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
    }
}
