using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class Inventory : MonoBehaviourPun
{
    [SerializeField] private int _maxCountItemsInInventory;
    [SerializeField] private float _takeDistance;
    [SerializeField] private Transform _takeInHandPoint;
    [SerializeField] private Transform _rightHand;
    private List<int> _myInventory;
    private PlayerMovement _playerMovement;
    private bool _isHandEmpty = true;

    public List<int> MyInventory => _myInventory;

    void Start()
    {
        _myInventory = new List<int>();
        _playerMovement = GetComponent<PlayerMovement>();
    }


    void Update()
    {
        TakeInHand();
    }

    private void TakeInHand()
    {
        if(Input.GetKeyDown(KeyCode.E) && photonView.IsMine && _myInventory.Count < _maxCountItemsInInventory)
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, _takeDistance))
            {
                if(hit.collider.TryGetComponent(out ITakeble prop))
                {
                    if(prop.IsTake && hit.collider.TryGetComponent(out ITool tool))
                    {
                        GameObject newTool = PhotonNetwork.Instantiate(tool.Name(), _rightHand.position, _rightHand.rotation);
                        photonView.RPC("PickUp", RpcTarget.AllBuffered, tool.Name(), newTool.GetPhotonView().ViewID, photonView.ViewID);
                    }
                    prop.Use(_takeInHandPoint);
                    if(prop.IsTake)
                    {
                        _playerMovement.enabled = false;
                    }
                    else
                    {
                        _playerMovement.enabled = true;
                    }
                }
            }
        }
    }

    [PunRPC]
    private void PickUp(string itemName, int itemViewID, int playerPhotonID)
    {
        Transform toolTransform = PhotonView.Find(itemViewID).transform;
        toolTransform.SetParent(_rightHand);
        if(_isHandEmpty)
        {
            _isHandEmpty = !_isHandEmpty;
        }
        else
        {
            toolTransform.gameObject.SetActive(false);
        }
        _myInventory.Add(itemViewID);
    }
}
