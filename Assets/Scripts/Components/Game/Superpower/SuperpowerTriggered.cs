using Unity.Entities;
using UnityEngine;

public struct SuperpowerTriggering : IComponentData, IEnableableComponent
{

}

public struct SuperpowerJustFinishedTriggering : IComponentData, IEnableableComponent
{

}

public struct SuperpowerInputTrigger : IComponentData
{
    public KeyCode TriggeringKey;
    public KeyCode TriggerKey;
}

public struct TriggeredBy : IComponentData
{
    public Entity By;
}