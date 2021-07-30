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

    [SyncVar(hook = nameof(OnIsAliveChanged))]
    public bool IsAlive;

    private PlayerDeath _playerDeath;

    private void Awake() {
        Rigidbody = GetComponent<Rigidbody>();
        InitialRotationForward = Mathf.Atan2(transform.forward.x, transform.forward.z);
        CameraFollow = Camera.main.GetComponent<CameraFollow>();
        MovementJoystick = GameObject.Find("MovementJoystick").GetComponent<Joystick>();
        SlapJoystick = GameObject.Find("SlapJoystick").GetComponent<Joystick>();
        _playerDeath = GetComponent<PlayerDeath>();
    }

    public override void OnStartAuthority() {
        base.OnStartAuthority();

        CmdSetIsAlive(true);
    }

    [Command]
    public void CmdSetIsAlive(bool isAlive) {
        IsAlive = isAlive;
    }

    private void OnIsAliveChanged(bool oldValue, bool newValue) {
        if (oldValue == true && newValue == false) { // Somebody died
            int numPlayersAlive = _playerDeath.CountAlivePlayers();

            if (isServer && numPlayersAlive == 1) {
                _playerDeath.SwitchLevel();
            }
        }
    }
}
