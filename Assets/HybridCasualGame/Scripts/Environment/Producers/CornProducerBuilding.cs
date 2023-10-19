using DG.Tweening;
using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class CornProducerBuilding : ProducerBuilding
{
    [Header("Additional Elements")]
    [SerializeField]
    private InputStorage _inputStorage;
    [SerializeField]
    private ResourceHolder _neededResource;

    public override bool CanProduceResource()
    {
        return !_outputStorage.IsFull && _inputStorage.HasNeededResources && _inputStorage.HasResource(_neededResource.GetResource());
    }

    private void FixedUpdate()
    {
        if (_isProducing) return;
        if (!CanProduceResource()) return;
        ProduceResource();
    }

    public override void ProduceResource()
    {
        if (_outputStorage.IsEmpty) _elementYOffset = 0;
        GrabInputStorageResources();
        StartCoroutine(ProduceResourceWithVisual());
    }

    private void GrabInputStorageResources()
    {
        var resources = _inputStorage.GetTransferResourcesPositions();
        resources.ForEach(element =>
        {
            element.DOJump(transform.position, 2f, 1, 1f).SetEase(Ease.OutQuad).OnComplete(() =>
            {
                Destroy(element.gameObject);
            });
        });
        _inputStorage.TransferRecourse();
    }
}

