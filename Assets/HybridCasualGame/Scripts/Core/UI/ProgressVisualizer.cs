using System;
using UnityEngine;
using UnityEngine.UI;

public class ProgressVisualizer : MonoBehaviour, IProgressVisualizer
{
    [Header("Elements")]
    [SerializeField]
    private Image radialImage;
    private Camera _mainCamera;
    public void SetProgress(float progress)
    {
        radialImage.fillAmount = progress;
    }

    private void OnEnable()
    {
        _mainCamera = Camera.main;
    }

    private void FixedUpdate()
    {
        CanvasFaceCamera();
    }

    private void CanvasFaceCamera()
    {
        transform.LookAt(_mainCamera.transform);
    }
}
