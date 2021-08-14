using System.Collections;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    [Header("Objects")]
    [SerializeField] private Transform spectatePosition;

    [Header("Stats")]
    [SerializeField] private float offsetAbove;
    [SerializeField] private float offsetBehind;
    [Space]
    [SerializeField] private float rotationDown;
    [Space]
    [SerializeField] private float spectateMoveDuration;

    private Transform _followTarget;
    private Vector3 _offset;

    private void Update() {
        if (_followTarget) {
            transform.position = _followTarget.position + _offset;
        }
    }

    public void Follow(Transform target) {
        _followTarget = target;

        // Set offset and rotation

        _offset = -_followTarget.forward * offsetBehind + _followTarget.up * offsetAbove;

        Quaternion rotation = Quaternion.LookRotation(_followTarget.forward, Vector3.up);
        rotation *= Quaternion.Euler(rotationDown, 0, 0);
        transform.localRotation = rotation;
    }

    public void SetStaticCamera(Vector3 position, Quaternion rotation) {
        _followTarget = null;

        transform.position = position;
        transform.rotation = rotation;
    }

    public void MoveToSpectate() {
        MoveTo(spectatePosition.position, spectatePosition.rotation, spectateMoveDuration);
    }

    public void MoveTo(Vector3 position, Quaternion rotation, float duration) {
        _followTarget = null;

        StartCoroutine(MoveToEnumerator(position, rotation, duration));
    }

    private IEnumerator MoveToEnumerator(Vector3 position, Quaternion rotation, float duration) {
        float time = 0;

        while (time < duration) {
            float percent = time / duration;

            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, percent);
            transform.position = Vector3.Slerp(transform.position, position, percent);

            time += Time.deltaTime;

            yield return null;
        }

        transform.rotation = rotation;
        transform.position = position;
    }
}
