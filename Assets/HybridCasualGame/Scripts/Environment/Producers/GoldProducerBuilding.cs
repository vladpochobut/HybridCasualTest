using DG.Tweening;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class GoldProducerBuilding : ProducerBuilding
{
    [Header("Additional Elements")]
    [SerializeField]
    private InputStorage _inputStorage1;
    [SerializeField]
    private InputStorage _inputStorage2;
    [SerializeField]
    private ResourceHolder _neededResource1;
    [SerializeField]
    private ResourceHolder _neededResource2;

    public override bool CanProduceResource()
    {
        return !_outputStorage.IsFull 
            && _inputStorage1.HasNeededResources
            && _inputStorage2.HasNeededResources;
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
        GrabResourcesFromSpecificStorage(_inputStorage1);
        GrabResourcesFromSpecificStorage(_inputStorage2);
    }

    private void GrabResourcesFromSpecificStorage(InputStorage inputStorage)
    {
        var resources = inputStorage.GetTransferResourcesPositions();
        resources.ForEach(element =>
        {
            element.DOJump(transform.position, 2f, 1, 1f).SetEase(Ease.OutQuad).OnComplete(() =>
            {
                Destroy(element.gameObject);
            });
        });
        inputStorage.TransferRecourse();
    }
}

