using UnityEngine;
using UnityEngine.EventSystems;

public class StaticJoystick : Joystick {
    public override void OnDrag(PointerEventData eventData) {
        _pointerCurrentPosition = eventData.position;

        Vector2 direction = (_centerPosition - _pointerCurrentPosition) * -1;

        float maxMagnitude = size / 2 * canvas.scaleFactor;

        // Update postion of handle
        if (direction.magnitude < maxMagnitude) {
            handle.position = _pointerCurrentPosition;
        } else {
            handle.position = _centerPosition + direction.normalized * maxMagnitude;
        }

        _direction = direction.normalized;
    }
}