using UnityEngine;
using System.Collections;

public class PlatformMovement02 : MonoBehaviour {
    [Header("Objects")]
    [SerializeField] private Transform smallPlatform;
    [SerializeField] private Transform middlePlatform;

    [Header("Stats")]
    [SerializeField] private float smallPlatformMovementDuration;
    [SerializeField] private float middlePlatformMovementDuration;
    [Space]
    [SerializeField] private float smallPlatformFinalPosition;
    [SerializeField] private float middlePlatformFinalPosition;

    [Header("Curves")]
    [SerializeField] private AnimationCurve animationCurve;

    private float _smallPlatformInitialPosition;
    private float _middlePlatformInitialPosition;

    private void Awake() {
        _smallPlatformInitialPosition = smallPlatform.position.y;
        _middlePlatformInitialPosition = middlePlatform.position.y;
    }

    private void Start() {
        StartCoroutine(MovePlatform(smallPlatform, _smallPlatformInitialPosition, smallPlatformFinalPosition, smallPlatformMovementDuration));
        StartCoroutine(MovePlatform(middlePlatform, _middlePlatformInitialPosition, middlePlatformFinalPosition, middlePlatformMovementDuration));
    }

    private IEnumerator MovePlatform(Transform platform, float initialPosition, float finalPosition, float movementDuration) {
        while (true) {
            float time = 0;

            platform.position = new Vector3(platform.position.x, initialPosition, platform.position.z);

            while (time < movementDuration) {
                float percent = time / movementDuration;

                float position = Mathf.Lerp(initialPosition, finalPosition, animationCurve.Evaluate(percent));

                platform.position = new Vector3(platform.position.x, position, platform.position.z);

                time += Time.deltaTime;

                yield return null;
            }

            platform.position = new Vector3(platform.position.x, finalPosition, platform.position.z);
        }
    }
}
