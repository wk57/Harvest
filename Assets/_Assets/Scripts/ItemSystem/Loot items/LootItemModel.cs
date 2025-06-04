using UnityEngine;

[CreateAssetMenu(fileName = "Loot item data", menuName = "Inventory system/Loot item data")]
public class LootItemModel : ScriptableObject
{
    [Header("Item data")]
    public int id;
    public string itemName;
    public Sprite itemUISprite;
    [SerializeField] private AudioClip grabSound;
    [SerializeField] private bool useSameGrabSoundAsRelease;
    [SerializeField] private AudioClip releaseSound;


    public AudioClip GetGrabSound()
    {
        if(grabSound == null)
        {
            Debug.LogError("No grab sound assigned to " + itemName + " with ID: " + id);
            return null;
        }

        return grabSound;
    }

    public AudioClip GetReleaseSound()
    {
        if (useSameGrabSoundAsRelease || releaseSound == null)
            return grabSound;

        return releaseSound;
    }
}
