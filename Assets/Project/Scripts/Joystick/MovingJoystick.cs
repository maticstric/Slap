using UnityEngine;
using UnityEngine.EventSystems;

public class MovingJoystick : Joystick {
    public override void OnDrag(PointerEventData eventData) {
        _pointerCurrentPosition = eventData.position;

        if (_enabled) {
            Vector2 direction = (_centerPosition - _pointerCurrentPosition) * -1;
            float maxMagnitude = size / 2 * canvas.scaleFactor;

            // Update postion of background
            if (direction.magnitude > maxMagnitude) {
                background.position = _pointerCurrentPosition - direction.normalized * maxMagnitude;
                _centerPosition = background.position;
            } else {
                background.position = _centerPosition;
            }

            handle.position = _pointerCurrentPosition;
            _direction = direction.normalized;
        }
    }
}