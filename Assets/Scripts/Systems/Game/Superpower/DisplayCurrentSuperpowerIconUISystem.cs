using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UIElements;

public partial struct DisplayCurrentSuperpowerIconUISystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        var entityCommandBuffer = new EntityCommandBuffer(Allocator.Temp);
        foreach(var (superpowerUser, entity) in SystemAPI.Query<SuperpowerUser>().WithNone<CurrentSuperpowerUI>().WithEntityAccess())
        {
            if (superpowerUser.DisplayInUI)
            {
                entityCommandBuffer.AddComponent(entity, new CurrentSuperpowerUI
                {
                    SuperpowerIcon = UISingleton.Q<VisualElement>("superpower-icon")
                });
            }
        }
        entityCommandBuffer.Playback(state.EntityManager);
        
        foreach(var (heldSuperpowers, currentSuperpowerUI) in SystemAPI.Query<DynamicBuffer<HeldSuperpower>, CurrentSuperpowerUI>())
        {
            Texture2D texture = null;
            if (heldSuperpowers.Length > 0)
                texture = state.EntityManager.GetComponentObject<Icon>(heldSuperpowers[0].Prefab).Texture;
            
            if (currentSuperpowerUI.SuperpowerIcon != null)
            {
                currentSuperpowerUI.SuperpowerIcon.style.backgroundImage = texture;
            }
        }
    }
}