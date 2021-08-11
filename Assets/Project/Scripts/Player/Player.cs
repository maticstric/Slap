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
    [HideInInspector] public CameraShake CameraShake;

    [SyncVar(hook = nameof(OnIsAliveChanged))]
    public bool IsAlive;

    private PlayerDeath _playerDeath;

    private void Awake() {
        _playerDeath = GetComponent<PlayerDeath>();

        Rigidbody = GetComponent<Rigidbody>();
        InitialRotationForward = Mathf.Atan2(transform.forward.x, transform.forward.z);
        CameraFollow = Camera.main.GetComponent<CameraFollow>();
        CameraShake = Camera.main.GetComponent<CameraShake>();
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

            if (isServer && numPlayersAlive <= 1) {
                _playerDeath.SwitchLevel();
            }
        }
    }
}
