using System.Collections.Generic;
using UnityEngine;

public class InputStorage : MonoBehaviour, IStorage
{
    [Header("Elements")]
    [SerializeField]
    private ResourceHolder _storedResourceType;
    [SerializeField]
    private Transform _elementsPlace;
    [SerializeField]
    private Transform _elementZone;
    [SerializeField]
    private CapacityVisualizer _capacityProgressVisualizer;
    [SerializeField]
    private ProgressVisualizer _neededResourcesVisualizer;
    private Transform[] _elementsPlaces;
    private List<IResource> _storedResources = new List<IResource>();
    [Header("Settings")]
    [SerializeField]
    private int _maxCapacity;
    [SerializeField]
    private int _neededResourceCount = 4;
    private bool _hasNeededResources = false;

    public bool IsFull => _storedResources.Count >= _maxCapacity;
    public bool IsEmpty => _storedResources.Count == 0;
    public IResource StorageResourceType =>  _storedResourceType.GetResource();
    public int ElementsCount => _storedResources.Count;
    public bool HasNeededResources => _hasNeededResources;

    private void OnEnable()
    {
        InitializePlaces();
    }

    private void VisualizeCapacity()
    {
        var progress = (float)_storedResources.Count / _maxCapacity;
        _capacityProgressVisualizer.SetProgress(progress);
    }

    private void VisualizeNeededRsources()
    {
        var progress = Mathf.Min((float)_storedResources.Count/_neededResourceCount,1f);
        _neededResourcesVisualizer.SetProgress(progress);
        _hasNeededResources = progress == 1 ? true : false;
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
        if (!IsFull)
        {
            _storedResources.Add(resource);
            VisualizeCapacity();
            VisualizeNeededRsources();
        }
    }

    public bool HasResource(IResource resource)
    {
        if (_storedResourceType.GetResource().ResourceName == resource.ResourceName)
        {
            return true;
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

    public List<Transform> GetTransferResourcesPositions()
    {
        var retrievedResource = new List<Transform>();
        if (!IsEmpty && _hasNeededResources)
        {
            for (int i = 0; i < _neededResourceCount; i++)
            {
                retrievedResource.Add(_elementZone.GetChild(_elementZone.childCount - 1 - i));
            }
        }
        return retrievedResource;

    }

    public ResourceHolder TransferRecourse()
    {
        ResourceHolder retrievedResource = null;
        if (!IsEmpty)
        {
            retrievedResource = _storedResourceType;
            _storedResources.RemoveRange(0, _neededResourceCount);
            VisualizeCapacity();
            VisualizeNeededRsources();
        }
        return retrievedResource;
    }
}

