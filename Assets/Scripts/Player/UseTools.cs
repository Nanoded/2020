using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Inventory))]
public class UseTools : MonoBehaviourPun
{
    [SerializeField] private float _minFieldViewValue;
    [SerializeField] private float _maxFieldViewValue;
    [SerializeField] private float _speedZoom;
    private List<int> _myInventory;
    private int _idItemInHand = 0;
    private int _idNextItemInHand = 0;
    private Camera _mainCamera;

    void Start()
    {
        _myInventory = GetComponent<Inventory>().MyInventory;
        _mainCamera = Camera.main;
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            Zoom();
            SelectTool();
            Use();
        }
    }

    private void Zoom()
    {
        if (Input.GetMouseButton(1) && Camera.main.fieldOfView > _minFieldViewValue)
        {
            _mainCamera.fieldOfView -= _speedZoom;
        }
        else if (!Input.GetMouseButton(1) && Camera.main.fieldOfView < _maxFieldViewValue)
        {
            _mainCamera.fieldOfView += _speedZoom;
        }
    }

    private void SelectTool()
    {
        if(Input.mouseScrollDelta.y != 0 && _myInventory.Count > 0)
        {
            if(Input.mouseScrollDelta.y > 0)
            {
                _idNextItemInHand++;
                if(_idNextItemInHand >= _myInventory.Count)
                {
                    _idNextItemInHand = 0;
                }
            }
            else if(Input.mouseScrollDelta.y < 0)
            {
                _idNextItemInHand--;
                if (_idNextItemInHand < 0)
                {
                    _idNextItemInHand = _myInventory.Count - 1;
                }
            }
            photonView.RPC("ChangeTool", RpcTarget.AllBuffered, _myInventory[_idItemInHand], _myInventory[_idNextItemInHand]);
            _idItemInHand = _idNextItemInHand;
        }
    }

    private void Use()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (_myInventory.Count > 0)
            {
                photonView.RPC("UseTool", RpcTarget.AllBuffered, _myInventory[_idItemInHand]);
            }
        }
    }

    [PunRPC]
    private void ChangeTool(int idCurrentTool, int idNextTool)
    {
        PhotonView.Find(idNextTool).gameObject.SetActive(true);
        PhotonView.Find(idCurrentTool).gameObject.SetActive(false);
    }

    [PunRPC]
    private void UseTool(int id)
    {
        PhotonView.Find(id).GetComponent<ITool>().Use();
    }
}
