using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PhotonInit : MonoBehaviourPunCallbacks {

    public enum ActivePanel
    {
        LOGIN = 0,
        ROOMS = 1
    }
    public ActivePanel activePanel = ActivePanel.LOGIN;

    private string gameVersion = "1.0";
    public string userId = "YouRang";
    public byte maxPlayer = 20;

    public TMP_InputField txtUserId;
    public TMP_InputField txtRoomName;

    public GameObject[] panels;

    public GameObject room;
    public Transform gridTr;

    private void Awake()
    {
        // photon1과 photon2로 바뀌면서 달라진점 (같은방 동기화)
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    void Start()
    {
        // 유저아이디를 작성하지 않으면 랜덤으로 ID 적용
        txtUserId.text = PlayerPrefs.GetString("USER_ID", "USER_" + Random.Range(1, 999));
        txtRoomName.text = PlayerPrefs.GetString("ROOM_NAME", "ROOM_" + Random.Range(1, 999));
        if (PhotonNetwork.IsConnected)
        {
            ChangePanel(ActivePanel.ROOMS);
        }
    }

    #region SELF_CALLBACK_FUNCTIONS
    public void OnLogin()
    {
        PhotonNetwork.GameVersion = this.gameVersion;
        PhotonNetwork.NickName = txtUserId.text;

        PhotonNetwork.ConnectUsingSettings();

        PlayerPrefs.SetString("USER_ID", PhotonNetwork.NickName);
        ChangePanel(ActivePanel.ROOMS);

    }

    public void OnCreateRoomClick()
    {
        PhotonNetwork.CreateRoom(txtRoomName.text
                                , new RoomOptions { MaxPlayers = this.maxPlayer });

    }

    public void OnJoinRandomRoomClick()
    {
        PhotonNetwork.JoinRandomRoom();
    }
    #endregion

    private void ChangePanel(ActivePanel panel)
    {
        foreach (GameObject _panel in panels)
        {
            Debug.Log(panels);
            _panel.SetActive(false);
        }
        panels[(int)panel].SetActive(true);
    }

    #region PHOTON_CALLBACK_FUNCTIONS
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connect To Master");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Failed join room");
        PhotonNetwork.CreateRoom(null
                                , new RoomOptions { MaxPlayers = this.maxPlayer });
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room !!!");
        // photonNetwork의 데이터 통신을 잠깐 정지 시켜준다. 
        // gamemanager에서 creatTank하고 나면 다시 연결시킨다
        PhotonNetwork.IsMessageQueueRunning = false; 
        SceneManager.LoadScene("Level01");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("ROOM"))
        {
            Destroy(obj);
        }
        foreach (RoomInfo roomInfo in roomList)
        {
            GameObject _room = Instantiate(room, gridTr);
            RoomData roomData = _room.GetComponent<RoomData>();
            roomData.roomName = roomInfo.Name;
            roomData.maxPlayer = roomInfo.MaxPlayers;
            roomData.playerCount = roomInfo.PlayerCount;
            roomData.UpdateInfo();
            roomData.GetComponent<Button>().onClick.AddListener
            (
                delegate
                {
                    OnClickRoom(roomData.roomName);
                }
            );

        }
    }
    #endregion

    void OnClickRoom(string roomName)
    {
        PhotonNetwork.NickName = txtUserId.text;
        PhotonNetwork.JoinRoom(roomName, null);
        PlayerPrefs.SetString("USER_ID", PhotonNetwork.NickName);
    }
}
