using Unity.Entities;
using UnityEngine;

public struct SuperpowerTriggered : IComponentData, IEnableableComponent
{

}

public struct SuperpowerTriggering : IComponentData, IEnableableComponent
{

}

public struct SuperpowerInputTrigger : IComponentData
{
    public KeyCode TriggeringKey;
    public KeyCode TriggerKey;
}