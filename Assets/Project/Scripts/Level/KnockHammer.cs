using UnityEngine;
using Mirror;
using System.Collections;

public class KnockHammer : NetworkBehaviour {
    [Header("Stats")]
    [SerializeField] private float force;
    [SerializeField] private float turnDuration;
    [SerializeField] private int turnDegrees;
    [SerializeField] private float waitTime;


    [Header("Animation")]
    [SerializeField] private AnimationCurve angleAnimationCurve;

    private Collider _collider;

    private void Awake() {
        _collider = GetComponent<Collider>();
    }

    private void Start() {
        if (isServer) {
            StartCoroutine("Move");
        }
    }

    private IEnumerator Move() {
        while (true) {
            float time = 0;

            int startingRotation = (int)transform.localEulerAngles.y;
            int finalRotation = (int)transform.localEulerAngles.y + turnDegrees;

            while (time < turnDuration) {
                float percent = time / turnDuration;
                float value = angleAnimationCurve.Evaluate(percent);

                float newRotation = startingRotation + (finalRotation - startingRotation) * value;

                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, newRotation, transform.localEulerAngles.z);

                time += Time.deltaTime;

                yield return null;
            }

            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, finalRotation, transform.localEulerAngles.z);

            _collider.enabled = false;

            yield return new WaitForSeconds(waitTime);

            _collider.enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (isServer) {
            // The collider is on the model, which is a child of the actual parent object with NetworkIdentity
            GameObject parentGameObject = other.transform.parent.gameObject;

            if (parentGameObject.tag == "Player") {
                PlayerSlap playerSlap = parentGameObject.GetComponent<PlayerSlap>();
                Vector3 forceDirection = -transform.up * force;

                playerSlap.CmdSlap(parentGameObject, forceDirection);
            }
        }
    }
}
