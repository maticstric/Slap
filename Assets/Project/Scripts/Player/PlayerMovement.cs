using UnityEngine;
using Mirror;

public class PlayerMovement : NetworkBehaviour {
    [Header("Stats")]
    [SerializeField] private float movementSpeed;

    private Joystick _movementJoystick;

    private void Awake() {
        _movementJoystick = GameObject.Find("MovementJoystick").GetComponent<Joystick>();
    }

    private void Update() {
        Move();
    }

    private void Move() {
        if (isLocalPlayer) {
            Vector3 movementDirection = new Vector3(_movementJoystick.Horizontal, 0, _movementJoystick.Vertical).normalized;
            movementDirection *= movementSpeed * Time.deltaTime;

            transform.position += movementDirection;
        }
    }
}