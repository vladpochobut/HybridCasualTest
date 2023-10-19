using UnityEngine;

[CreateAssetMenu(menuName = "Resource")]
public class Resource : ScriptableObject, IResource
{
    [SerializeField]
    private RESOURCE_TYPES resourceType;

    public RESOURCE_TYPES ResourceName { get { return resourceType; } }
}

