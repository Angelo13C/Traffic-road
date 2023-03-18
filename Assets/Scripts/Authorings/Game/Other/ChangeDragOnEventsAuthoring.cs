using Unity.Entities;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ChangeDragOnEventsAuthoring : MonoBehaviour
{
	[System.Serializable]
	public class Event
	{
		public bool Change;
		public float NewValue;
	}
	[HideInInspector] public Event CollisionEvent;
	[HideInInspector] public Event HitByExplosionEvent;
	[HideInInspector] public Event BlackHoleEvent;

	class Baker : Baker<ChangeDragOnEventsAuthoring>
	{
		public override void Bake(ChangeDragOnEventsAuthoring authoring)
		{
			if(authoring.CollisionEvent.Change)
				AddComponent(new ChangeDragOnCollision { NewDrag = authoring.CollisionEvent.NewValue });
			if(authoring.HitByExplosionEvent.Change)
				AddComponent(new ChangeDragOnHitByExplosion { NewDrag = authoring.HitByExplosionEvent.NewValue });
			if(authoring.BlackHoleEvent.Change)
				AddComponent(new ChangeDragOnBlackHole { NewDrag = authoring.BlackHoleEvent.NewValue });
		}
	}
}

#if UNITY_EDITOR
[CustomEditor(typeof(ChangeDragOnEventsAuthoring))]
public class ChangeDragOnEventsAuthoringEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var authoring = target as ChangeDragOnEventsAuthoring;
		DrawEvent("Change on collision", authoring.CollisionEvent);
		DrawEvent("Change on hit by explosion", authoring.HitByExplosionEvent);
		DrawEvent("Change on black hole", authoring.BlackHoleEvent);
    }

	private void DrawEvent(string changeName, ChangeDragOnEventsAuthoring.Event eventToDraw)
	{		
        eventToDraw.Change = EditorGUILayout.Toggle(changeName, eventToDraw.Change);

        if (eventToDraw.Change)
            eventToDraw.NewValue = EditorGUILayout.FloatField("New linear drag", eventToDraw.NewValue);
	}
}
#endif