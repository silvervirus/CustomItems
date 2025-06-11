using UnityEngine;

public class OpenStorageOnClick : HandTarget, IHandTarget
{
    private StorageContainer container;

    public override void Awake()
    {
        base.Awake();
        container = GetComponentInChildren<StorageContainer>();
    }

    public void OnHandClick(GUIHand hand)
    {
        // Only allow middle click to open storage
        if (!enabled || container == null)
            return;

        if (GameInput.GetButtonDown(GameInput.Button.AltTool)) // Middle mouse
        {
            container.OnHandClick(hand);
        }
    }

    public void OnHandHover(GUIHand hand)
    {
        // Optional — omit this if you don’t want visible hint
        HandReticle.main.SetIcon(HandReticle.IconType.Hand, 1f);
        HandReticle.main.SetText(HandReticle.TextType.Hand, "", false);
        HandReticle.main.SetTextRaw(HandReticle.TextType.HandSubscript, "[Mouse3] Open Storage");
    }
}