using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine.UI;

[RequireComponent(typeof(PhotonView))]
public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private Button _playButton;
    [SerializeField] private TextMeshProUGUI _playersReady;
    [SerializeField] private TextMeshProUGUI _textPlayButton;
    [SerializeField] private GameObject _loadScreen;
    [SerializeField] private TextMeshProUGUI _countPlayers;
    [SerializeField] private PlayerInfo _playerInfoPrefab;
    [SerializeField] private Transform _content;
    private List<Player> _playersInRoom = new List<Player>();
    private List<PlayerInfo> _playersInfo = new List<PlayerInfo>();
    private int _playersReadyCount = 0;
    private PhotonView _photonView;

    void Start()
    {
        _photonView = GetComponent<PhotonView>();
        _playersReady.text = _playersReadyCount.ToString();
        _loadScreen.SetActive(false);
        UpdatePlayers();
        foreach(var player in PhotonNetwork.PlayerList)
        {
            PlayerInfo newPlayer = Instantiate(_playerInfoPrefab, _content);
            newPlayer.SetPlayerInfo(player.NickName);
            _playersInRoom.Add(player);
            _playersInfo.Add(newPlayer);
        }
        
        _textPlayButton.text = "Ready";
    }

    public void StartGame()
    {
        _photonView.RPC("UpdateReadyPlayersCount", RpcTarget.AllBuffered, PhotonNetwork.NickName);
        _playButton.interactable = false;
        return;
    }

    [PunRPC]
    private void UpdateReadyPlayersCount(string NickName)
    {
        foreach(var player in _playersInfo)
        {
            if(player.GetName() == NickName)
            {
                player.GetComponent<Image>().color = Color.green;
                break;
            }
        }
        _playersReadyCount++;
        _playersReady.text = _playersReadyCount.ToString();
        if(_playersReadyCount == PhotonNetwork.PlayerList.Length)
        {
            _loadScreen.SetActive(true);
            PhotonNetwork.LoadLevel(2);
        }
    }

    public void Exit()
    {
        PhotonNetwork.LoadLevel(0);
        PhotonNetwork.LeaveRoom();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayers();
        PlayerInfo player = Instantiate(_playerInfoPrefab, _content);
        player.SetPlayerInfo(newPlayer.NickName);
        _playersInRoom.Add(newPlayer);
        _playersInfo.Add(player);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayers();
    }

    private void UpdatePlayers()
    {
        _countPlayers.text = PhotonNetwork.CurrentRoom.PlayerCount.ToString();
        int count = _playersInRoom.Count;
        for(int i = 0; i < count; i++)
        {
            if (!Array.Exists(PhotonNetwork.PlayerList, item => item == _playersInRoom[i]))
            {
                foreach(var player in _playersInfo)
                {
                    if(player.GetName() == _playersInRoom[i].NickName)
                    {
                        Destroy(player.gameObject);
                        _playersInfo.Remove(player);
                        break;
                    }
                }
                _playersInRoom.Remove(_playersInRoom[i]);
                count--;
                i--;
            }
        }
    }
}
