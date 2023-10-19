using DG.Tweening;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class TomatoProducerBuilding : ProducerBuilding
{
    private void Start()
    {
        StartCoroutine(ContinuousProduction());
    }

    public override bool CanProduceResource()
    {
        return !_outputStorage.IsFull;
    }

    private IEnumerator ContinuousProduction()
    {
        while (true)
        {
            if (!_isProducing && CanProduceResource())
            {
                yield return ProduceResourceWithVisual();
            }
            else
            {
                yield return null;
            }
        }
    }

    public override void ProduceResource()
    {
        if (CanProduceResource() && !_isProducing)
        {
            if (_outputStorage.IsEmpty) _elementYOffset = 0;
            StartCoroutine(ProduceResourceWithVisual());
        }
    }
}

