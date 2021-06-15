using UnityEngine;

public class CameraFollow : MonoBehaviour {
    [Header("Stats")]
    [SerializeField] private float offsetAbove;
    [SerializeField] private float offsetBehind;
    [Space]
    [SerializeField] private float rotationDown;

    private Transform _target;
    private Vector3 _offset;

    private void Update() {
        if (_target) {
            transform.position = _target.position + _offset;
        }
    }

    public void Follow(Transform target) {
        _target = target;

        // Set offset and rotation

        _offset = -_target.forward * offsetBehind + _target.up * offsetAbove;

        Quaternion rotation = Quaternion.LookRotation(_target.forward, Vector3.up);
        rotation *= Quaternion.Euler(rotationDown, 0, 0);
        transform.localRotation = rotation;
    }
}
