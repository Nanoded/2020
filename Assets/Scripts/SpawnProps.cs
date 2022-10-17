using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class SpawnProps : MonoBehaviourPun
{
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private GameObject[] _propsPrefabs;

    void Start()
    {
        if(PhotonNetwork.IsMasterClient)
        {
             photonView.RPC("SpawnProp", RpcTarget.AllBuffered);
        }
  
    }

    [PunRPC]
    private void SpawnProp()
    {
        for(int i = 0; i < _propsPrefabs.Length; i++)
        {
            PhotonNetwork.Instantiate(_propsPrefabs[i].name, _spawnPoints[Random.Range(0, _spawnPoints.Length)].position, Quaternion.identity);
        }
    }
}
