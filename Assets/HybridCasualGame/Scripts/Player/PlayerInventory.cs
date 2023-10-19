using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class PlayerInventory : MonoBehaviour, IPlayerInventory
{
    [Header("Elements")]
    [SerializeField]
    private ProgressVisualizer _productionProgressVisualizer;
    [SerializeField]
    private Transform _backpackPlace;
    private List<IResource> _inventory = new List<IResource>();
    private ResourceHolder _currentResource;

    [Header("Settings")]
    [SerializeField]
    private int _maxInventorySize = 20;
    private float _transferTime = 1f; 
    private bool _isTransferringResource = false;
    public bool IsInventoryFull { get { return _inventory.Count >= _maxInventorySize; } }

    public ResourceHolder CurrentResource { get { return _currentResource; } }

    public void AddToInventory(ResourceHolder resource)
    {
        _inventory.Add(resource.GetResource());
        _currentResource = resource;
        _isTransferringResource = true;
        StartCoroutine(AddResourceWithVisual(resource));
    }

    private IEnumerator AddResourceWithVisual(ResourceHolder resource)
    {
        _productionProgressVisualizer.gameObject.SetActive(true);
        _productionProgressVisualizer.SetProgress(0f);
        float elapsedTime = 0f;
        while (elapsedTime < _transferTime)
        {
            float progress = elapsedTime / _transferTime;
            _productionProgressVisualizer.SetProgress(progress);
            yield return null;
            elapsedTime += Time.deltaTime;
        }
        _productionProgressVisualizer.SetProgress(1f);
        var elementYOffset = (_inventory.Count - 1) * resource.GetComponent<BoxCollider>().size.y  * resource.transform.lossyScale.y;
        Instantiate(resource.gameObject,
            new Vector3(_backpackPlace.position.x,
                _backpackPlace.position.y + elementYOffset,
                    _backpackPlace.position.z),
                        quaternion.identity,
                            _backpackPlace);
        _isTransferringResource = false;
        _productionProgressVisualizer.gameObject.SetActive(false);
    }

    public IResource RemoveFromInventory()
    {
        var removedResource = _inventory[0];
        _inventory.RemoveAt(0);
        Destroy(_backpackPlace.GetChild(_backpackPlace.childCount - 1).gameObject);
        return removedResource;
    }

    private bool CanAddToInvntory(IResource resource)
    {
        return !IsInventoryFull && (_inventory.Count == 0 || _inventory.FirstOrDefault().ResourceName == resource.ResourceName);
    }

    //TODO : I should Fix that, change logic to event Manager and call that logic, that was temp logic sorry((
    private void OnTriggerStay(Collider other)
    {
        var storage = other.gameObject.GetComponent<IStorage>();
        if (storage == null) return;
        if (_isTransferringResource) return;

        if (storage is OutputStorage outputStorage)
        {
            if (CanAddToInvntory(outputStorage.StorageResourceType))
            {
                var transferResource = outputStorage.TransferRecourse();
                if (transferResource == null) return;
                AddToInventory(transferResource);
            }
        }
        else if (storage is InputStorage inputStorage)
        {
            if (_inventory.Count() > 0 
                && _inventory.FirstOrDefault().ResourceName == inputStorage.StorageResourceType.ResourceName
                && !inputStorage.IsFull)
            {
                TransferResource(inputStorage);
            }
        }
    }

    private void TransferResource(InputStorage inputStorage)
    {
        _isTransferringResource = true;
        var resource = Instantiate(CurrentResource,transform.position
            ,quaternion.identity,inputStorage.GetElementZone());
        var elementPlaces = inputStorage.GetElementPlaces();
        var _elementYOffset = resource.GetComponent<BoxCollider>().size.y * resource.transform.lossyScale.y;
        resource.transform.DOJump(new Vector3(elementPlaces[0].position.x,
            elementPlaces[0].position.y + (_elementYOffset * inputStorage.ElementsCount ),
                elementPlaces[0].position.z)
                    , 2f, 1, .5f)
                .SetEase(Ease.OutQuad)
                .OnComplete(() =>
                {
                    var resourceToTransfer = RemoveFromInventory();
                    inputStorage.AddResource(resourceToTransfer);
                    _isTransferringResource = false;
                });
    }
}

