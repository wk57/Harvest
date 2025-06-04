using UnityEngine;
using AYellowpaper.SerializedCollections;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Loot manager", menuName = "Inventory system/Loot manager")]
public class LootManagerModel : ScriptableObject
{
    [Header("UI Prefabs")]
    public Image lootContainerItemPrefab;

    [Header("Dictionaries")]

    [SerializedDictionary("ID", "Loot items")]
    public SerializedDictionary<int, LootItemModel> itemsDictionary;

    [SerializedDictionary("ID", "Loot items")]
    public SerializedDictionary<ContainerModel.ContainersEnum, ContainerModel> containersDictionary;

    public LootItemModel GetItemByID(int id)
    {
        if(itemsDictionary.ContainsKey(id))
        {
            return itemsDictionary[id];
        }
        else
        {
            Debug.LogError("Item ID not found");
            return null;
        }
    }

    public ContainerModel GetContainerByEnum(ContainerModel.ContainersEnum containerEnum)
    {
        if(containersDictionary.ContainsKey(containerEnum))
        {
            return containersDictionary[containerEnum];
        }
        else
        {
            Debug.LogError("Container not assigned");
            return null;
        }
    }
}
