using UnityEngine;

public class DeployableStorageHandler : MonoBehaviour, IHandTarget
{
    private StorageContainer storage;
    private Pickupable pickup;

    private void Awake()
    {
        storage = GetComponentInChildren<StorageContainer>();
        pickup = GetComponent<Pickupable>();
    }

    public void OnHandHover(GUIHand hand)
    {
        if (storage != null)
        {
            HandReticle.main.SetText(HandReticle.TextType.Use,"Open Storage",true, GameInput.Button.LeftHand);
        }
    }

    public void OnHandClick(GUIHand hand)
    {
        if (storage != null)
        {
            storage.Open();
            // prevent pickup while interacting
            if (pickup != null)
                pickup.AllowedToPickUp();
        }
    }
}