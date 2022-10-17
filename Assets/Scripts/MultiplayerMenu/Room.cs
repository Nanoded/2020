using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class Room : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _roomName;
    [SerializeField] private TextMeshProUGUI _playerCount;

    private NetworkManager _networkManager;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(ChooseRoom);
    }

    public void UpdateRoom(RoomInfo room, NetworkManager networkManager)
    {
        _roomName.text = room.Name;
        _playerCount.text = room.PlayerCount + "/" + room.MaxPlayers;
        _networkManager = networkManager;
    }

    private void ChooseRoom()
    {
        _networkManager.ChooseRoom(_roomName.text);
    }
}
