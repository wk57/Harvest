using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LootManagerController : MonoBehaviour
{
    [SerializeField] private LootManagerModel lootModel;
    [SerializeField] private Transform containerSpawnPoint;
    [SerializeField] private GameObject inventoryCanvas;

    public UnityEvent OnInventoryOpened;
    public UnityEvent OnInventoryClosed;

    public LootItemModel GetItemByID(int itemId) => lootModel.GetItemByID(itemId);
    public ContainerModel GetContainerByEnum(ContainerModel.ContainersEnum containerEnum) => lootModel.GetContainerByEnum(containerEnum);


    public void OpenInventoryTab()
    {
        inventoryCanvas.SetActive(true);
        OnInventoryOpened?.Invoke();
    }

    public void CloseInventoryTab()
    {
        inventoryCanvas.SetActive(false);
        OnInventoryClosed?.Invoke();
    }

    public ContainerHandler GenerateContainer(ContainerModel containerModel)
    {
        ContainerHandler containerCreated = null;

        if(containerModel != null)
        {
            containerCreated = Instantiate(containerModel.containerPrefab, containerSpawnPoint);
            containerCreated.transform.position = containerSpawnPoint.position;
            containerCreated.SetAudioClips(containerModel.GetOpenSound(), containerModel.GetCloseSound());
            containerCreated.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("Container model is null for creation");
        }

        return containerCreated;
    }

    /// <summary>
    /// Generates a UI instance of the loot item (used for containers)
    /// </summary>
    /// <param name="itemModelToCreate"> The model that will be used to create the UI item</param>
    /// <returns></returns>
    public GameObject GenerateLootItem(LootItemModel itemModelToCreate)
    {
        GameObject lootItem = null;

        if (itemModelToCreate != null)
        {
            Image itemImageInstantiated = Instantiate(lootModel.lootContainerItemPrefab);
            itemImageInstantiated.sprite = itemModelToCreate.itemUISprite;

            ContainerLootItem containerLootItem = itemImageInstantiated.transform.GetComponent<ContainerLootItem>();
            containerLootItem.itemModel = itemModelToCreate;
            containerLootItem.SetAudioClips(itemModelToCreate.GetGrabSound(), itemModelToCreate.GetReleaseSound());

            BoxCollider2D imageCollider = itemImageInstantiated.transform.GetComponent<BoxCollider2D>();
            imageCollider.size = itemModelToCreate.itemUISprite.bounds.size;

            lootItem = itemImageInstantiated.gameObject;
        }
        else
        {
            Debug.LogError("Cannot generate loot item with null model");
        }

        return lootItem;
    }

    public GameObject GetRandomLootItem()
    {
        int randomItemId = Random.Range(1, lootModel.itemsDictionary.Count);
        return GenerateLootItem(lootModel.GetItemByID(randomItemId));
    }
}
