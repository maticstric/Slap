using UnityEngine;
using Mirror;

public class PlayerMovement : NetworkBehaviour {
    [Header("Stats")]
    [SerializeField] private float standardMovementSpeed;
    [SerializeField] private float chargingMovementSpeed;

    [Header("Objects")]
    [SerializeField] private DirectionIndicator directionIndicator;

    public bool MovementEnabled;

    private float _currentMovementSpeed;
    private Player _player;
    private LevelManager _levelManager;
    private CameraFollow _cameraFollow;

    private float? _slapAngle;

    private void Awake() {
        MovementEnabled = true;
        _player = GetComponent<Player>();
    }

    private void Start() {
        _levelManager = FindObjectOfType<LevelManager>();
        _cameraFollow = Camera.main.GetComponent<CameraFollow>();

        if (isLocalPlayer) {
            directionIndicator.Enabled = true;
            _cameraFollow.Follow(transform);
        }
    }

    private void Update() {
        if (isLocalPlayer && MovementEnabled) {
            if (_player.Animator.GetBool("IsCharging")) {
                _currentMovementSpeed = chargingMovementSpeed;
            } else {
                _currentMovementSpeed = standardMovementSpeed;
            }

            Rotate();
        }
    }

    private void FixedUpdate() {
        if (isLocalPlayer && MovementEnabled) {
            Move();
        }
    }

    private void Move() {
        Vector3 movementDirection;

        if (_levelManager.MovementJoystick.Direction == Vector2.zero) {
            movementDirection = Vector3.zero;
        } else {
            float movementAngle = Mathf.Atan2(_levelManager.MovementJoystick.Horizontal, _levelManager.MovementJoystick.Vertical);
            movementAngle += _player.InitialRotationForward;

            movementDirection = new Vector3(Mathf.Sin(movementAngle), 0, Mathf.Cos(movementAngle));

            _player.Rigidbody.MovePosition(_player.Model.transform.position + movementDirection * _currentMovementSpeed * Time.fixedDeltaTime);
        }

        // Animate

        if (movementDirection == Vector3.zero) {
            _player.Animator.SetBool("IsRunning", false);
        } else {
            _player.Animator.SetBool("IsRunning", true);
        }
    }

    private void Rotate() {
        float angle;

        if (!_player.Animator.GetCurrentAnimatorStateInfo(0).IsName(_player.SlapAnimation.name)) {

            if (_slapAngle.HasValue) { _slapAngle = null; }

            angle = Mathf.Atan2(_levelManager.MovementJoystick.Horizontal, _levelManager.MovementJoystick.Vertical);
        } else {
            if (!_slapAngle.HasValue) {
                _slapAngle = Mathf.Atan2(_levelManager.SlapJoystick.HorizontalBeforeRelease, _levelManager.SlapJoystick.VerticalBeforeRelease);
            }

            angle = (float)_slapAngle;
        }

        if (angle != 0) {
            Vector3 initialVectorForward = new Vector3(Mathf.Sin(_player.InitialRotationForward), 0, Mathf.Cos(_player.InitialRotationForward));

            _player.Rigidbody.MoveRotation(Quaternion.Euler(0, angle * Mathf.Rad2Deg, 0) * Quaternion.LookRotation(initialVectorForward, Vector3.up));
        }
    }
}