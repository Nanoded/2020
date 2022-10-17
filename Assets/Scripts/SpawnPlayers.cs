using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class SpawnPlayers : MonoBehaviourPun
{
    [SerializeField] private GameObject _previewCamera;
    [SerializeField] private GameObject _previewScreen;
    [SerializeField] private Transform _spawnThief;
    [SerializeField] private Transform _spawnSecurity;
    [SerializeField] private GameObject _playerPrefab;
    private PhotonView _photonView;
    private int _countThiefs = 0;
    private int _countSecurity = 0;

    void Start()
    {
        _photonView = GetComponent<PhotonView>();
    }

    public void SelectTeam(bool isThief)
    {
        if(isThief)
        {
            if(_countSecurity >= _countThiefs)
            {
                _photonView.RPC("AddPlayerToTeam", RpcTarget.AllBuffered, "Thief");
                PhotonNetwork.Instantiate(_playerPrefab.name, _spawnThief.position, Quaternion.identity);
                Destroy(_previewCamera);
                _previewScreen.SetActive(false);
            }    
        }
        else
        {
            if (_countSecurity <= _countThiefs)
            {
                _photonView.RPC("AddPlayerToTeam", RpcTarget.AllBuffered, "Security");
                PhotonNetwork.Instantiate(_playerPrefab.name, _spawnSecurity.position, Quaternion.identity);
                Destroy(_previewCamera);
                _previewScreen.SetActive(false);
            }
        }    
    }

    [PunRPC]
    private void AddPlayerToTeam(string team)
    {
        if(team == "Thief")
        {
            _countThiefs++;
        }
        else
        {
            _countSecurity++;
        }
    }

    void Update()
    {

    }
}
