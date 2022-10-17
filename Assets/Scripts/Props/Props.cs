using UnityEngine;
using DG.Tweening;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]
public class Props : MonoBehaviourPun, ITakeble
{
    [SerializeField] private float _sensivity = 3;
    [SerializeField] private string _objectName;
    [SerializeField] private string _objectDescription;
    [SerializeField] private string _canGetText = "Press E to pick up";
    [SerializeField] private string _cantGetText = "Press E to put back";
    [SerializeField] private bool _canGet;
    private string _doText;
    private Vector3 _startPos;
    private Vector3 _startRot;
    private UseScreen _useScreen;
    private bool _isTake = false;
    public bool IsTake { get => _isTake; set => _isTake = value; }
    private void Start()
    {
        _startPos = transform.position;
        _startRot = transform.eulerAngles;
        _useScreen = FindObjectOfType<UseScreen>(true);
        if(_canGet == true)
        {
            _doText = _canGetText;
        }
        else
        {
            _doText = _cantGetText;
        }
    }

    private void Update()
    {
        if(_isTake == true)
        {
            transform.eulerAngles += new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0) * _sensivity;
        }
    }

    public void Use(Transform usePoint)
    {
        if(_isTake == false)
        {
            if(_useScreen != null)
            {
                _useScreen.gameObject.SetActive(true);
                _useScreen.UpdateScreen(_objectName, _objectDescription, _doText);
            }
            transform.DOLocalMove(Vector3.zero, 1f);
            transform.DOLocalRotate(Vector3.zero, 1f);
            transform.SetParent(usePoint);
        }
        else
        {
            if (_useScreen != null)
            {
                _useScreen.gameObject.SetActive(false);
            }
            if (_canGet == true)
            {
                photonView.RPC("DestroyProp", RpcTarget.AllBuffered);
            }
            else
            {
                transform.DOLocalMove(_startPos, 1f);
                transform.DOLocalRotate(_startRot, 1f);
                transform.SetParent(null);
            }
        }
        _isTake = !_isTake;
    }

    [PunRPC]
    private void DestroyProp()
    {
        Destroy(gameObject);
    }
}
