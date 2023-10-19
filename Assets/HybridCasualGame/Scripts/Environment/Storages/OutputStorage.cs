using System.Collections.Generic;
using UnityEngine;

public class OutputStorage : MonoBehaviour, IStorage
{
    [Header("Elements")]
    [SerializeField]
    private Transform _elementsPlace;
    [SerializeField]
    private Transform _elementZone;
    [SerializeField]
    private ResourceHolder _storedResourceType;
    [SerializeField]
    private CapacityVisualizer _capacityProgressVisualizer;
    private Transform[] _elementsPlaces;
    private List<IResource> _storedResources = new List<IResource>();

    [Header("Settings")]
    [SerializeField]
    private int _maxCapacity;

    public bool IsFull => _storedResources.Count >= _maxCapacity;
    public bool IsEmpty =>  _storedResources.Count == 0;
    public IResource StorageResourceType => _storedResourceType.GetResource();

    private void OnEnable()
    {
        InitializePlaces();
    }

    private void VisualizeCapacity()
    {
        var progress = (float)_storedResources.Count / _maxCapacity;
        _capacityProgressVisualizer.SetProgress(progress);
    }

    private void InitializePlaces()
    {
        var placesCount = _elementsPlace.childCount;
        _elementsPlaces = new Transform[placesCount];
        for (int i = 0; i < placesCount; i++)
        {
            _elementsPlaces[i] = _elementsPlace.GetChild(i);
        }
    }

    public void AddResource(IResource resource)
    {
        if (!IsFull && resource.ResourceName == StorageResourceType.ResourceName)
        {
            _storedResources.Add(resource);
            VisualizeCapacity();
        }
    }

    public bool HasResource(IResource resource)
    {
        foreach (var storedResource in _storedResources)
        {
            if (storedResource.ResourceName == resource.ResourceName)
            {
                return true;
            }
        }
        return false;
    }

    public Transform[] GetElementPlaces()
    {
        return _elementsPlaces;
    }

    public Transform GetElementZone()
    {
        return _elementZone;
    }

    public ResourceHolder TransferRecourse()
    {
        ResourceHolder retrievedResource = null;
        if (!IsEmpty)
        {
            retrievedResource = _storedResourceType;
            _storedResources.RemoveAt(0);
            Destroy(_elementZone.GetChild(_elementZone.childCount - 1).gameObject);
            VisualizeCapacity();
        }
        return retrievedResource;
    }
}

