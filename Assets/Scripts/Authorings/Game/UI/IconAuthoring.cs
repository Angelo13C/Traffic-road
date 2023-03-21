using UnityEngine;
using Unity.Entities;

public class IconAuthoring : MonoBehaviour
{
    [SerializeField] private Texture2D _texture;
    
    class Baker : Baker<IconAuthoring>
    {
        public override void Bake(IconAuthoring authoring)
        {
            var icon = new Icon
            {
                Texture = authoring._texture
            };

            AddComponentObject(icon);
        }
    }
}