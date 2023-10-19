public interface IResource
{
    RESOURCE_TYPES ResourceName { get; }
}

public enum RESOURCE_TYPES
{
    Corn,
    Tomato,
    Gold
}
