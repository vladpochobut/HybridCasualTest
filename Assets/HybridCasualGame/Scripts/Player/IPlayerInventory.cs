public interface IPlayerInventory
{
    bool IsInventoryFull { get; }
    ResourceHolder CurrentResource { get; }
    void AddToInventory(ResourceHolder resource);
    IResource RemoveFromInventory();
}

