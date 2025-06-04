using UnityEngine;

[CreateAssetMenu(fileName = "Container data", menuName = "Inventory system/Cointainer data")]
public class ContainerModel : ScriptableObject
{
    [Header("Container data")]
    public ContainersEnum containerEnum;
    public ContainerHandler containerPrefab;
    [SerializeField] private AudioClip openSound;
    [SerializeField] private bool useSameOpenSoundAsClose;
    [SerializeField] private AudioClip closeSound;

    public enum ContainersEnum
    {
        Chest,
        Box
    }

    public string GetContainerName => containerEnum.ToString();

    public AudioClip GetOpenSound () => openSound;

    public AudioClip GetCloseSound()
    {
        if (useSameOpenSoundAsClose || closeSound == null)
            return openSound;

        return closeSound;
    }
}
