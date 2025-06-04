using UnityEngine;

public class ContainerLootItem : MonoBehaviour
{
    private ContainerHandler actualItemContainer;
    private ContainerHandler hoveringContainer = null;

    private Vector3 lastItemPosition;

    private AudioClip grabClip;
    private AudioClip releaseClip;

    [HideInInspector] public LootItemModel itemModel;

    /// <summary>
    /// Called by Lean Drag events, don't call outside this events
    /// </summary>
    public void LootItemGrabbed()
    {
        FindFirstObjectByType<AudioSource>().PlayOneShot(grabClip);
    }

    /// <summary>
    /// Called by Lean Drag events, don't call outside this events
    /// </summary>
    public void LootItemDropped()
    {
        if(hoveringContainer != null)
        {
            actualItemContainer.RemoveLootItemFromContainer(this);
            SetActualItemContainer(hoveringContainer);

        }
        else
        {
            //Item dropped on empty space
            transform.position = lastItemPosition; //Go back to last position
        }

        FindFirstObjectByType<AudioSource>().PlayOneShot(releaseClip);
    }

    /// <summary>
    /// Sets the current container where the item will be stored via drop or initialization
    /// </summary>
    /// <param name="container">The container where it will be assigned</param>
    public void SetActualItemContainer(ContainerHandler container)
    {
        actualItemContainer = container;
        lastItemPosition = transform.position;
        container.AddLootItemToContainer(this);
    }

    /// <summary>
    /// Sets the hovering container where the item is floating
    /// </summary>
    public void SetHoveringContainer(ContainerHandler container)
    {
        hoveringContainer = container;
    }

    public void SetAudioClips(AudioClip grabClip, AudioClip releaseClip)
    {
        this.grabClip = grabClip;
        this.releaseClip = releaseClip;
    }
}
