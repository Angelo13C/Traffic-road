using Unity.Entities;
using UnityEngine;

public partial struct SuperpowersInputSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        foreach(var (superpowersInput, superpowers) in SystemAPI.Query<SuperpowersInput, RefRW<Superpowers>>())
        {
            if(Input.GetMouseButtonDown(0))
            {
                var screenPosition = Input.mousePosition;
                var ray = superpowersInput.Camera.ScreenPointToRay(screenPosition);
                superpowers.ValueRW.Ray = ray;
            }
        }
    }
}