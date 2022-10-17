using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private Room _roomPrefab;
    [SerializeField] private Transform _content;
    [SerializeField] private TMP_InputField _roomName;
    [SerializeField] private GameObject _loadingScreen;

    private string _roomNameToConnect;

    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    #region Photon Callbacks
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel(1);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        _loadingScreen.SetActive(false);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        _loadingScreen.SetActive(false);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Disconnected");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (var room in roomList)
        {
            if (room.PlayerCount > 0)
            {
                Room newRoom = Instantiate(_roomPrefab, _content);
                newRoom.UpdateRoom(room, this);
            }
        }
    }

    #endregion

    public void ChangeRoomName()
    {
        _roomNameToConnect = _roomName.text;
    }

    public void ChooseRoom(string name)
    {
        _roomNameToConnect = name;
    }

    public void ConnectToRoom()
    {
        if (PhotonNetwork.IsConnected)
        {
            if (_roomNameToConnect != string.Empty)
            {
                _loadingScreen.SetActive(true);
                PhotonNetwork.JoinRoom(_roomNameToConnect);
            }
        }
    }

    public void CreateRoom()
    {
        if (PhotonNetwork.IsConnected)
        {
            if (_roomNameToConnect != string.Empty)
            {
                _loadingScreen.SetActive(true);
                PhotonNetwork.CreateRoom(_roomNameToConnect, new RoomOptions { MaxPlayers = 2, IsVisible = true }, TypedLobby.Default);
            }
        }

    }
}
