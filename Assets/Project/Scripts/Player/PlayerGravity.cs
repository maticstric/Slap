using UnityEngine;

public class PlayerGravity : MonoBehaviour {
    [Header("Stats")]
    [SerializeField] private float gravityStrenght;

    private Player _player;
    private Vector3 _gravity;

    private void Awake() {
        _player = GetComponent<Player>();
        _gravity = new Vector3(0, gravityStrenght, 0);
    }

    private void FixedUpdate() {
        _player.Rigidbody.AddForce(_gravity);
    }
}
