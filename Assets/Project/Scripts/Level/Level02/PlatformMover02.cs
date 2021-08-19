using UnityEngine;
using Mirror;

public class PlatformMover02 : NetworkBehaviour {
    [Header("Stats")]
    [SerializeField] private float movementDuration;
    [SerializeField] private float finalPosition;

    [Header("Curves")]
    [SerializeField] private AnimationCurve animationCurve;

    private Rigidbody _rigidbody;
    private float _initialPosition;
    private float _time;

    private void Awake() {
        _rigidbody = GetComponent<Rigidbody>();
        _initialPosition = transform.position.y;
        _time = 0;
    }

    private void FixedUpdate() {
        if (isServer) {
            MovePlatform();
        }
    }

    [Server]
    private void MovePlatform() {
        float percent = (_time % movementDuration) / movementDuration;
        float position = Mathf.Lerp(_initialPosition, finalPosition, animationCurve.Evaluate(percent));

        _rigidbody.MovePosition(new Vector3(transform.position.x, position, transform.position.z));

        _time += Time.fixedDeltaTime;
    }
}
