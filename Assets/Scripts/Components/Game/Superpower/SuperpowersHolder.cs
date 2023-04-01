using Unity.Entities;
using UnityEngine.UIElements;

public struct SuperpowerUser : IComponentData
{
    public Entity LastUsedSuperpower;
    public bool DisplayInUI;
}

public class CurrentSuperpowerUI : IComponentData
{
    public VisualElement SuperpowerIcon;
}

public struct HeldSuperpower : IBufferElementData
{
    public Entity Prefab;
}