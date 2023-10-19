using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Mover : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField]
    private MobileJoystick _joystick;
    [SerializeField]
    private PlayerAnimator _playerAnimator;
    [SerializeField]
    private Animator _animator;
    private CharacterController _characterController;

    [Header("Settings")]
    [SerializeField]
    private float _moveSpeed = 3f;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _playerAnimator = new PlayerAnimator(_animator);
    }

    private void Update()
    {
        ManageMovement();
    }

    private void ManageMovement()
    {
        var moveVector = _joystick.MoveVector * _moveSpeed * Time.deltaTime / Screen.width;
        moveVector.z = moveVector.y;
        moveVector.y = 0;
        _characterController.Move(moveVector);
        _playerAnimator.ManageAnimations(moveVector);
    }
}
