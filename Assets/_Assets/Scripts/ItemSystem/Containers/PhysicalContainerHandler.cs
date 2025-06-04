using UnityEngine;

public class PhysicalContainerHandler : MonoBehaviour
{
    [SerializeField] private ContainerModel containerModel;
    private ContainerHandler containerHandler;

    private LootManagerController lootManagerController;

    void Awake()
    {
        lootManagerController = FindFirstObjectByType<LootManagerController>();
        if (lootManagerController == null)
            Debug.LogError("Could not find loot manager controller");
    }

    [ContextMenu("Open container")]
    public void OpenContainer()
    {
        if (containerHandler == null)
            containerHandler = lootManagerController.GenerateContainer(containerModel);

        containerHandler.OpenContainer();

        lootManagerController.OpenInventoryTab();

        lootManagerController.OnInventoryClosed.AddListener(CloseContainer);
    }

    public void CloseContainer()
    {
        containerHandler.CloseContainer();

        lootManagerController.OnInventoryClosed.RemoveListener(CloseContainer);
    }
}
