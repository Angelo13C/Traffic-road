using Unity.Burst;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UIElements;

public partial struct DisplayCurrentSuperpowerIconUISystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        foreach(var (heldSuperpowers, currentSuperpowerUI) in SystemAPI.Query<DynamicBuffer<HeldSuperpower>, CurrentSuperpowerUI>())
        {
            Texture2D texture = null;
            if (heldSuperpowers.Length > 0)
                texture = state.EntityManager.GetComponentObject<Icon>(heldSuperpowers[0].Prefab).Texture;
            
            if(currentSuperpowerUI.SuperpowerIcon == null)
                currentSuperpowerUI.SuperpowerIcon = UISingleton.Q<VisualElement>("superpower-icon");
            if (currentSuperpowerUI.SuperpowerIcon != null)
            {
                currentSuperpowerUI.SuperpowerIcon.style.backgroundImage = texture;
            }
        }
    }
}