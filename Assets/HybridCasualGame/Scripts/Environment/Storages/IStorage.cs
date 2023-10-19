using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStorage 
{
    bool IsFull { get; }
    bool IsEmpty { get; }

    IResource StorageResourceType { get; }
    Transform[] GetElementPlaces();
    Transform GetElementZone();
    void AddResource(IResource resource);
    ResourceHolder TransferRecourse();
    bool HasResource(IResource resource);
}
