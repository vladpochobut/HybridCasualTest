using UnityEngine;

public class ResourceHolder : MonoBehaviour
{
    [SerializeField]
    private Resource _resource;

    public Resource GetResource()
    {
        return _resource;
    }
}

