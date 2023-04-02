using Unity.Entities;
using UnityEngine;

public class ChangeDragOnEventsAuthoring : MonoBehaviour
{
	[System.Serializable]
	public class Event
	{
		public bool Change;
		public float NewValue;
	}
	[SerializeField] private Event _collisionEvent;
	[SerializeField] private Event _hitByExplosionEvent;
	[SerializeField] private Event _blackHoleEvent;

	class Baker : Baker<ChangeDragOnEventsAuthoring>
	{
		public override void Bake(ChangeDragOnEventsAuthoring authoring)
		{
			if(authoring._collisionEvent.Change)
				AddComponent(new ChangeDragOnCollision { NewDrag = authoring._collisionEvent.NewValue });
			if(authoring._hitByExplosionEvent.Change)
				AddComponent(new ChangeDragOnHitByExplosion { NewDrag = authoring._hitByExplosionEvent.NewValue });
			if(authoring._blackHoleEvent.Change)
				AddComponent(new ChangeDragOnBlackHole { NewDrag = authoring._blackHoleEvent.NewValue });
		}
	}
}