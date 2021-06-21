using UnityEngine;
using Mirror;

public class PlayerMovement : NetworkBehaviour {
    [Header("Stats")]
    [SerializeField] private float movementSpeed;

    [Header("Object")]
    [SerializeField] private GameObject model;

    private Joystick _movementJoystick;
    private Vector3 _movementDirection;
    private Rigidbody _rigidbody;
    private CameraFollow _cameraFollow;
    private Vector3 _initialTransformForward;

    private void Awake() {
        _movementJoystick = GameObject.Find("MovementJoystick").GetComponent<Joystick>();
        _rigidbody = GetComponent<Rigidbody>();
        _cameraFollow = Camera.main.GetComponent<CameraFollow>();
        _initialTransformForward = transform.forward;
    }

    private void Start() {
        if (isLocalPlayer) {
            _cameraFollow.Follow(transform);
        }
    }

    private void Update() {
        if (isLocalPlayer) {
            _movementDirection = new Vector3(_movementJoystick.Horizontal, 0, _movementJoystick.Vertical).normalized;

            Rotate();
        }
    }

    private void FixedUpdate() {
        if (isLocalPlayer) {
            Move();
        }
    }

    private void Move() {
        if (_movementDirection == Vector3.zero) return;

        _rigidbody.MovePosition(model.transform.position + transform.forward * movementSpeed * Time.fixedDeltaTime);
    }

    private void Rotate() {
        if (_movementDirection == Vector3.zero) return;

        float rotation = Mathf.Atan2(_movementDirection.x, _movementDirection.z) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, rotation, 0) * Quaternion.LookRotation(_initialTransformForward, Vector3.up);
    }
}