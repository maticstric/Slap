using UnityEngine;
using Mirror;

public class PlayerMovement : NetworkBehaviour {
    [Header("Stats")]
    [SerializeField] private float movementSpeed;

    [Header("Object")]
    [SerializeField] private GameObject model;

    private Joystick _movementJoystick;
    private Vector3 _movementDirection;
    private Rigidbody _modelRigidbody;

    private void Awake() {
        _movementJoystick = GameObject.Find("MovementJoystick").GetComponent<Joystick>();
        _modelRigidbody = GetComponent<Rigidbody>();
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

        print(transform.position + _movementDirection * movementSpeed * Time.fixedDeltaTime);

        _modelRigidbody.MovePosition(model.transform.position + _movementDirection * movementSpeed * Time.fixedDeltaTime);
    }

    private void Rotate() {
        float rotation = Mathf.Atan2(_movementDirection.x, _movementDirection.z) * Mathf.Rad2Deg;

        transform.localEulerAngles = new Vector3(0, rotation, 0);
    }
}