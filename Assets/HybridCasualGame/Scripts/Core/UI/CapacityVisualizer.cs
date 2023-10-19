using UnityEngine;
using UnityEngine.UI;

public class CapacityVisualizer : MonoBehaviour, IProgressVisualizer
{
    [Header("Elements")]
    [SerializeField]
    private Image radialImage;

    public void SetProgress(float progress)
    {
        radialImage.fillAmount = progress;
    }
}

