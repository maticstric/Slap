using UnityEngine;

public class Player : MonoBehaviour {
    [Header("Objects")]
    public GameObject Model;

    [Header("Animation")]
    public Animator Animator;
    public AnimationClip SlapAnimation;

    [HideInInspector] public Rigidbody Rigidbody;
    [HideInInspector] public float InitialRotationForward;
    [HideInInspector] public CameraFollow CameraFollow;
    [HideInInspector] public Joystick MovementJoystick;
    [HideInInspector] public Joystick SlapJoystick;

    private void Awake() {
        Rigidbody = GetComponent<Rigidbody>();
        InitialRotationForward = Mathf.Atan2(transform.forward.x, transform.forward.z);
        CameraFollow = Camera.main.GetComponent<CameraFollow>();
        MovementJoystick = GameObject.Find("MovementJoystick").GetComponent<Joystick>();
        SlapJoystick = GameObject.Find("SlapJoystick").GetComponent<Joystick>();
    }
}
