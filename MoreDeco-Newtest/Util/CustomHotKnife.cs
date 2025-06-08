namespace CustomItems.Util;

using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Crafting;
using Nautilus.Extensions;
using UnityEngine;

using UnityEngine;

public class CustomHotKnife : HeatBlade
{
    public float hitForce = 100f;
    public ForceMode forceMode = ForceMode.Acceleration;

    public override string animToolName => TechType.HeatBlade.AsString(true);

    public void Configure(float force, float damageMultiplier)
    {
        hitForce = force;
        damage *= damageMultiplier;
    }

    public override void OnToolUseAnim(GUIHand hand)
    {
        base.OnToolUseAnim(hand);

        GameObject hitObj = null;
        Vector3 hitPosition = default;
        UWE.Utils.TraceFPSTargetPosition(Player.main.gameObject, attackDist, ref hitObj, ref hitPosition);

        if (!hitObj) return;

        var liveMixin = hitObj.GetComponentInParent<LiveMixin>();
        if (liveMixin && IsValidTarget(liveMixin))
        {
            var rigidbody = hitObj.GetComponentInParent<Rigidbody>();
            if (rigidbody)
            {
                rigidbody.AddForce(MainCamera.camera.transform.forward * hitForce, forceMode);
            }
        }
    }
}