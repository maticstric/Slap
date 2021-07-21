using Mirror;
using UnityEngine;

public class Player : NetworkBehaviour {
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

    [HideInInspector][SyncVar(hook = nameof(OnIsAliveChanged))]
    public bool IsAlive;

    private void Awake() {
        Rigidbody = GetComponent<Rigidbody>();
        InitialRotationForward = Mathf.Atan2(transform.forward.x, transform.forward.z);
        CameraFollow = Camera.main.GetComponent<CameraFollow>();
        MovementJoystick = GameObject.Find("MovementJoystick").GetComponent<Joystick>();
        SlapJoystick = GameObject.Find("SlapJoystick").GetComponent<Joystick>();
    }

    public override void OnStartAuthority() {
        CmdSetIsAlive(true);
    }

    [Command]
    public void CmdSetIsAlive(bool isAlive) {
        IsAlive = isAlive;
    }

    private void OnIsAliveChanged(bool oldValue, bool newValue) { }
}
