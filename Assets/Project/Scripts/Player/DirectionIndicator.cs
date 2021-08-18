using UnityEngine;

public class DirectionIndicator : MonoBehaviour {
    [Header("Stats")]
    [SerializeField] private float movementRadius;
    [SerializeField] private Color32 enabledColor;
    [SerializeField] private Color32 disabledColor;

    private LevelManager _levelManager;
    private Joystick _movementJoystick;
    private SpriteRenderer _spriteRenderer;

    public bool Enabled = false;

    private void Awake() {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start() {
        _levelManager = FindObjectOfType<LevelManager>();
        _movementJoystick = _levelManager.MovementJoystick;
    }

    private void Update() {
        if (Enabled) {
            Color32 spriteColor = _movementJoystick.Magnitude == 0 ? disabledColor : enabledColor;

            _spriteRenderer.color = spriteColor;

            Vector3 newPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, _movementJoystick.Magnitude * movementRadius);

            transform.localPosition = newPosition;
        }
    }
}
