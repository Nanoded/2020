using UnityEngine;
using TMPro;
using Photon.Pun;
using ExitGames.Client.Photon;

public class PlayerInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _playerName;
    [SerializeField] private TextMeshProUGUI _ping;
    private Hashtable _customProps;
    private float _ratePingUpdate = 3f;
    private float _nextPingUpdate = 0f;

    public string GetName()
    {
        return _playerName.text;
    }

    public void SetPlayerInfo(string name)
    {
        _playerName.text = name;
        if(_playerName.text == PhotonNetwork.NickName)
        {
            _customProps = new Hashtable();
            _customProps["Ping"] = PhotonNetwork.GetPing();
            _customProps["Ready"] = false;
            PhotonNetwork.LocalPlayer.SetCustomProperties(_customProps);
        }
    }

    private void Update()
    {
        if (Time.time > _nextPingUpdate && PhotonNetwork.IsConnected)
        {
            foreach (var player in PhotonNetwork.PlayerList)
            {
                if (player.NickName == _playerName.text && player.CustomProperties.TryGetValue("Ping", out object ping))
                {
                    _ping.text = ping.ToString();
                    break;
                }
            }
            _nextPingUpdate = Time.time + _ratePingUpdate;
        }
        
    }
}
