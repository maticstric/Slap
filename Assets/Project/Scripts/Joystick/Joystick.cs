using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class Joystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler {
    [Header("Objects")]
    [SerializeField] protected RectTransform background;
    [SerializeField] protected RectTransform handle;
    [SerializeField] protected Canvas canvas;

    [Header("Stats")]
    [SerializeField] protected float size = 100;
    [SerializeField] protected bool rightSide = true; // false = on left side
    [SerializeField] protected Color32 enabledColor = new Color32(255, 255, 255, 255);
    [SerializeField] protected Color32 disabledColor = new Color32(255, 255, 255, 100);

    protected Vector2 _centerPosition;
    protected Vector2 _pointerCurrentPosition;

    protected Vector2 _direction;
    protected Vector2 _directionBeforeRelease;

    protected bool _enabled = true;

    public Vector2 Direction => _direction;
    public float Horizontal => _direction.x;
    public float Vertical => _direction.y;

    public Vector2 DirectionBeforeRelease => _directionBeforeRelease;
    public float HorizontalBeforeRelease => _directionBeforeRelease.x;
    public float VerticalBeforeRelease => _directionBeforeRelease.y;

    public delegate void OnPointerUpEvent();
    public event OnPointerUpEvent onPointerUpEvent;
    public delegate void OnPointerDownEvent();
    public event OnPointerDownEvent onPointerDownEvent;

    private void Start() {
        ResetJoystick();
    }

    public virtual void OnPointerDown(PointerEventData eventData) {
        _pointerCurrentPosition = eventData.position;
        _centerPosition = eventData.position;

        if (_enabled) {
            onPointerDownEvent?.Invoke();

            background.position = _centerPosition;
            handle.position = _centerPosition;
        }
    }

    public abstract void OnDrag(PointerEventData eventData);

    public virtual void OnPointerUp(PointerEventData eventData) {
        if (_enabled) {
            onPointerUpEvent?.Invoke();

            ResetJoystick();

            _directionBeforeRelease = _direction;
            _direction = Vector2.zero;
        }
    }

    public void SetEnabled(bool enabled) {
        _enabled = enabled;

        Image backgroundImage = background.GetComponent<Image>();
        Image handleImage = handle.GetComponent<Image>();

        if (backgroundImage && handleImage) {
            backgroundImage.color = enabled ? enabledColor : disabledColor;
            handleImage.color = enabled ? enabledColor : disabledColor;
        }
    }

    private void ResetJoystick() {
        background.sizeDelta = new Vector2(size, size);
        handle.sizeDelta = new Vector2(size * 0.5f, size * 0.5f); // Handle is 50% of background

        // Good position for joystick based on which side its on
        if (rightSide) {
            background.anchoredPosition = new Vector3(size * 0.75f, size * 0.75f, 0); 
        } else {
            background.anchoredPosition = new Vector3(-size * 0.75f, size * 0.75f, 0);
        }

        handle.anchoredPosition = new Vector3(0, 0, 0); // Center handle
    }
}