using UnityEngine;

public class DirectionIndicator : MonoBehaviour {
    [Header("Stats")]
    [SerializeField] private float movementRadius;

    private LevelManager _levelManager;
    private Joystick _movementJoystick;
    private SpriteRenderer _spriteRenderer;

    private void Awake() {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start() {
        _levelManager = FindObjectOfType<LevelManager>();
        _movementJoystick = _levelManager.MovementJoystick;
    }

    private void Update() {
        bool isSpriteRendererEnabled = _movementJoystick.Magnitude == 0 ? false : true;

        _spriteRenderer.enabled = isSpriteRendererEnabled;

        Vector3 newPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, _movementJoystick.Magnitude * movementRadius);

        transform.localPosition = newPosition;
    }
}
