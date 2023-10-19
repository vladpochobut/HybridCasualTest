
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public abstract class ProducerBuilding : MonoBehaviour, IBuilding
{
    [Header("Elements")]
    [SerializeField]
    protected OutputStorage _outputStorage;
    [SerializeField]
    private ResourceHolder _productionResource;
    [SerializeField]
    private ProgressVisualizer _productionProgressVisualizer;
    [Header("Settings")]
    [SerializeField]
    protected float _productionTime = 2f;
    protected bool _isProducing = false;
    protected int _resourceIndex = 0;
    protected float _elementYOffset = 0f;

    protected float ElementYOffset => _productionResource.gameObject.GetComponent<BoxCollider>().size.y * _productionResource.transform.lossyScale.y;

    public abstract bool CanProduceResource();
    public abstract void ProduceResource();
    protected IEnumerator ProduceResourceWithVisual()
    {
        _isProducing = true;
        _productionProgressVisualizer.SetProgress(0f);

        float elapsedTime = 0f;
        while (elapsedTime < _productionTime)
        {
            float progress = elapsedTime / _productionTime;
            _productionProgressVisualizer.SetProgress(progress);
            yield return null;
            elapsedTime += Time.deltaTime;
        }

        _productionProgressVisualizer.SetProgress(1f);
        yield return new WaitForSeconds(0.2f);

        var newResource = Instantiate(_productionResource.gameObject
            , transform.position,
                quaternion.identity, _outputStorage.GetElementZone());
        var elementPlaces = _outputStorage.GetElementPlaces();
        newResource.transform.DOJump(new Vector3(elementPlaces[_resourceIndex].position.x,
            elementPlaces[_resourceIndex].position.y + _elementYOffset,
                elementPlaces[_resourceIndex].position.z)
                    , 2f, 1, .5f)
                .SetEase(Ease.OutQuad)
                .OnComplete(() =>
                {
                    if (_resourceIndex < elementPlaces.Length - 1)
                        _resourceIndex++;
                    else
                    {
                        _resourceIndex = 0;
                        _elementYOffset += ElementYOffset;
                    }
                    IResource producedResource = _productionResource.GetResource();
                    _outputStorage.AddResource(producedResource);
                    _isProducing = false;
                    _productionProgressVisualizer.SetProgress(0f);
                });
    }
}

