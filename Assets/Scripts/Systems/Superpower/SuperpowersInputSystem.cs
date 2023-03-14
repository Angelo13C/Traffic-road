using Unity.Entities;
using UnityEngine;

public partial struct SuperpowersInputSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        foreach(var (superpowersInput, superpowers) in SystemAPI.Query<SuperpowersInput, RefRW<Superpowers>>())
        {
            var screenPosition = Input.mousePosition;
            superpowers.ValueRW.Ray = superpowersInput.Camera.ScreenPointToRay(screenPosition);
        }
    }
}