using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileJoystick : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField]
    private RectTransform _joystickOutline;
    [SerializeField]
    private RectTransform _joystickKnob;

    [Header("Sttings")]
    [SerializeField]
    private float _moveFactor = 1f;
    private Vector3 _move;
    private bool _canControl = false;
    private Vector3 _clickedPosition;
    public Vector3 MoveVector => _move;
    public bool IsActive => _canControl;

    private void Start()
    {
        HideJoystick();
    }

    private void Update()
    {
        if (!_canControl) return;
        ControlJoystick();
    }

    public void ClickedOnJoystickZoneCallback()
    {
        _clickedPosition = Input.mousePosition;
        _joystickOutline.position = _clickedPosition;

        ShowJoystick();
    }

    private void ShowJoystick()
    {
        _joystickOutline.gameObject.SetActive(true);
        _canControl = true;
    }

    private void HideJoystick()
    {
        _joystickOutline.gameObject.SetActive(false);
        _canControl = false;
        _move = Vector3.zero;
    }

    private void ControlJoystick()
    {
        var currentPosition = Input.mousePosition;
        var direction = currentPosition - _clickedPosition;
        var moveMagnitude = direction.magnitude * _moveFactor / Screen.width;
        moveMagnitude = Mathf.Min(moveMagnitude,_joystickOutline.rect.width/2);
        _move = direction.normalized * moveMagnitude;
        var targetPosition = _clickedPosition + _move;
        _joystickKnob.position = targetPosition;
        if (Input.GetMouseButtonUp(0))
            HideJoystick();
    }
}
