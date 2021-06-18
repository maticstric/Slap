using UnityEngine;
using UnityEngine.EventSystems;

public abstract class Joystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler {
    [Header("Objects")]
    [SerializeField] protected RectTransform background;
    [SerializeField] protected RectTransform handle;
    [SerializeField] protected Canvas canvas;

    [Header("Stats")]
    [SerializeField] protected float size = 100;
    [SerializeField] protected bool rightSide = true; // false = on left side

    protected Vector2 _centerPosition;
    protected Vector2 _pointerCurrentPosition;

    protected Vector2 _direction;

    public Vector2 Direction => _direction;
    public float Horizontal => _direction.x;
    public float Vertical => _direction.y;

    private void Start() {
        ResetJoystick();
    }

    public void OnPointerDown(PointerEventData eventData) {
        _pointerCurrentPosition = eventData.position;
        _centerPosition = eventData.position;

        background.position = _centerPosition;
        handle.position = _centerPosition;
    }

    public abstract void OnDrag(PointerEventData eventData);

    public void OnPointerUp(PointerEventData eventData) {
        ResetJoystick();

        _direction = Vector2.zero;
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