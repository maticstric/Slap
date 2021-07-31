using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour {
    [SerializeField] private float shakeDuration;
    [SerializeField] private float shakeMagnitude;

    public IEnumerator Shake() {
        float currentShakeMagnitude = shakeMagnitude;
        float time = 0;

        while (time < shakeDuration) {
            float x = Random.Range(-1f, 1f) * currentShakeMagnitude;
            float y = Random.Range(-1f, 1f) * currentShakeMagnitude;

            transform.position += new Vector3(x, y, 0);
            currentShakeMagnitude = Mathf.Lerp(shakeMagnitude, 0, time / shakeDuration);

            time += Time.deltaTime;

            yield return null;
        }
    }
}
