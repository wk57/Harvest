using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContainerHandler : MonoBehaviour
{
    protected List<ContainerLootItem> itemsStored;

    [Header("Initials items")]
    [SerializeField] protected List<LootItemModel> initialStoredItems;
    [SerializeField] protected int randomItemsToGenerate = 0;

    [Header("UI Config")]
    [SerializeField] protected Transform itemsContainerTransform;
    [SerializeField] protected BoxCollider2D colliderContainer;

    private AudioClip openContainerClip;
    private AudioClip closeContainerClip;

    private Image containerImage;

    private LootManagerController lootManagerController;

    void Awake()
    {
        Initialize();
    }

    public void OpenContainer()
    {
        gameObject.SetActive(true);
        FindFirstObjectByType<AudioSource>().PlayOneShot(openContainerClip);
    }

    public void CloseContainer()
    {
        gameObject.SetActive(false);
        FindFirstObjectByType<AudioSource>().PlayOneShot(closeContainerClip);
    }

    public void SetAudioClips(AudioClip openClip, AudioClip closeClip)
    {
        openContainerClip = openClip;
        closeContainerClip = closeClip;
    }


    protected virtual void Initialize()
    {
        itemsStored = new List<ContainerLootItem>();

        containerImage = GetComponentInChildren<Image>();
        if(containerImage != null)
            containerImage.raycastTarget = false;
        else
            Debug.LogWarning("Image isn't assigned to container, check the hierarchy to see if the object order is right");

        lootManagerController = FindFirstObjectByType<LootManagerController>();
        if (lootManagerController != null)
            GenerateItemsInContainer();
        else
            Debug.LogWarning("Loot manager controller couldn't be find in the scene");

    }

    private void GenerateItemsInContainer()
    {
        foreach(LootItemModel item in initialStoredItems)
        {
            GameObject itemGenerated = lootManagerController.GenerateLootItem(item);
            itemGenerated.transform.SetParent(itemsContainerTransform);
            itemGenerated.transform.localPosition = GetRandomPositionWithinCollider();
            itemGenerated.GetComponent<ContainerLootItem>().SetActualItemContainer(this);
        }

        for(int i = 0; i < randomItemsToGenerate; i++)
        {
            GameObject itemGenerated = lootManagerController.GetRandomLootItem();
            itemGenerated.transform.SetParent(itemsContainerTransform);
            itemGenerated.transform.localPosition = GetRandomPositionWithinCollider();
            itemGenerated.GetComponent<ContainerLootItem>().SetActualItemContainer(this);
        }
    }

    private Vector2 GetRandomPositionWithinCollider()
    {
        Vector2 worldCenter = colliderContainer.bounds.center;
        Vector2 worldSize = colliderContainer.size;

        float minX = worldCenter.x - worldSize.x / 2f;
        float maxX = worldCenter.x + worldSize.x / 2f;
        float minY = worldCenter.y - worldSize.y / 2f;
        float maxY = worldCenter.y + worldSize.y / 2f;

        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);
        Vector2 randomWorldPos = new Vector2(randomX, randomY);

        RectTransform parentRectTransform = colliderContainer.GetComponent<RectTransform>();
        Vector2 randomLocalPos = parentRectTransform.InverseTransformPoint(randomWorldPos);

        return randomLocalPos;
    }

    public virtual void AddLootItemToContainer(ContainerLootItem item)
    {
        if (!itemsStored.Contains(item))
        {
            itemsStored.Add(item);
        }
    }

    public virtual void RemoveLootItemFromContainer(ContainerLootItem item)
    {
        if(itemsStored.Contains(item))
        {
            itemsStored.Remove(item);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Item"))
        {
            ContainerLootItem lootItemEntered = other.GetComponent<ContainerLootItem>();
            if (lootItemEntered != null)
            {
                ItemEntered(lootItemEntered);
            }
        }
    }

    /// <summary>
    /// Item has entered the container (doesn't mean it was dropped inside it, it could be in drag state)
    /// </summary>
    /// <param name="lootItem">Item found by the OnTriggerEnter event of the container</param>
    protected virtual void ItemEntered(ContainerLootItem lootItem)
    {
        //TODO Posibles efectos, UI ,cosas extras al entrar el contenedor?
        lootItem.SetHoveringContainer(this);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Item"))
        {
            ContainerLootItem lootItemExit = other.GetComponent<ContainerLootItem>();
            if (lootItemExit != null)
            {
                ItemExit(lootItemExit);
            }
        }
    }

    /// <summary>
    /// Item has exit the container (doesn't mean it was dropped in another container, it's just in the air)
    /// </summary>
    /// <param name="lootItem">Item found by the OnTriggerExit event of the container</param>
    protected virtual void ItemExit(ContainerLootItem lootItem)
    {
        //TODO Posibles efectos, UI ,cosas extras al salir del contenedor?
        lootItem.SetHoveringContainer(null);
    }
}
