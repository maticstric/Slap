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

    [SyncVar(hook = nameof(OnIsAliveChanged))]
    public bool IsAlive;
    [SyncVar]
    public CharacterSelect.Characters Character = CharacterSelect.Characters.Sphere;
    [SyncVar(hook = nameof(OnColorChanged))]
    public int Color = (int)CharacterSelect.Colors.Red; // Bug in mirror, so sadly this has to be an int instead of CharacterSelect.Colors

    private PlayerDeath _playerDeath;

    private void Awake() {
        _playerDeath = GetComponent<PlayerDeath>();

        Rigidbody = GetComponent<Rigidbody>();
        InitialRotationForward = Mathf.Atan2(transform.forward.x, transform.forward.z);
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

    private void OnColorChanged(int oldValue, int newValue) {
        GetComponentInChildren<SkinnedMeshRenderer>().material.SetTexture("_MainTex", CharacterSelect.Instance.GetTexture((CharacterSelect.Colors)newValue, Character));
    }
}
