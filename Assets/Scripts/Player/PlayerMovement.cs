using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PhotonView))]
public class PlayerMovement : MonoBehaviourPun
{
    [Header("Move settings")]
    [SerializeField] private float _runSpeed;
    [SerializeField] private float _walkSpeed;
    [SerializeField] private Transform _head;
    [Header("Look settings")]
    [SerializeField] private Camera _camera;
    [SerializeField] private float _sensivity;
    [SerializeField] private float _minValueRot;
    [SerializeField] private float _maxValueRot;
    private Rigidbody _rigidbody;
    private Animator _animator;
    private Vector3 _rotation;
    private float _currentSpeed = 0;



    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _rotation = Vector3.zero;
        if (photonView.IsMine == false)
        {
            Destroy(_camera.gameObject);
        }
    }

    private void FixedUpdate()
    {
        if(photonView.IsMine)
        {
            Run();
            Movement();
            MouseLook();
        }
    }

    private void Run()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            _animator.SetBool("Run", true);
            _currentSpeed = _runSpeed;
        }
        else
        {
            _animator.SetBool("Run", false);
            _currentSpeed = _walkSpeed;
        }
    }

    private void Movement()
    {
        Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        _animator.SetFloat("X", direction.x);
        _animator.SetFloat("Y", direction.z);

        transform.eulerAngles += new Vector3(0, Input.GetAxis("Mouse X") * _sensivity, 0);
        _rigidbody.MovePosition(transform.position + (transform.forward * Input.GetAxis("Vertical") * _currentSpeed) + 
            (transform.right * Input.GetAxis("Horizontal") * _currentSpeed));
    }

    private void MouseLook()
    {
        _camera.transform.position = _head.position;
        _rotation += new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0);
        _rotation.x = Mathf.Clamp(_rotation.x, _minValueRot, _maxValueRot);
        _camera.transform.eulerAngles = _rotation * _sensivity;
    }
}
