using Unity.Entities;
using UnityEngine;

public class SuperpowerTriggererAuthoring : MonoBehaviour
{
	[SerializeField] private KeyCode _triggeringKey;
	[SerializeField] private KeyCode _triggerKey = KeyCode.Mouse0;

	class Baker : Baker<SuperpowerTriggererAuthoring>
	{
		public override void Bake(SuperpowerTriggererAuthoring authoring)
		{
			if (authoring._triggeringKey != KeyCode.None)
			{
				AddComponent<SuperpowerTriggering>();
				SetComponentEnabled<SuperpowerTriggering>(true);
				AddComponent<SuperpowerJustFinishedTriggering>();
				SetComponentEnabled<SuperpowerJustFinishedTriggering>(false);
			}

			var inputTrigger = new SuperpowerInputTrigger
			{
				TriggeringKey = authoring._triggeringKey,
				TriggerKey = authoring._triggerKey
			};
			AddComponent(inputTrigger);

			var triggeredBy = new TriggeredBy
			{
				By = Entity.Null
			};
			AddComponent(triggeredBy);
		}
	}
}