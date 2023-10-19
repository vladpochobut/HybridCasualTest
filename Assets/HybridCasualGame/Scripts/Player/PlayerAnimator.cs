using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator
{
    private Animator _animator;
    private float _moveSpeedMultiplier = 40f;
    private const string SpeedParamKey = "moveSpeed";

    public PlayerAnimator(Animator animator)
    {
        _animator = animator;
    }

    public void ManageAnimations(Vector3 moveVector)
    {
        if(moveVector.magnitude > 0)
            _animator.transform.forward = moveVector.normalized;
        _animator.SetFloat(SpeedParamKey, moveVector.magnitude * _moveSpeedMultiplier);
       
    }
}
