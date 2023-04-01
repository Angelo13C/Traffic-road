using Unity.Collections;
using Unity.Entities;
using UnityEngine.UIElements;

public partial struct DisplayScoreUISystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        var entityCommandBuffer = new EntityCommandBuffer(Allocator.Temp);
        foreach(var (score, entity) in SystemAPI.Query<Score>().WithNone<ScoreUI>().WithEntityAccess())
        {
            if (score.DisplayInUI)
            {
                entityCommandBuffer.AddComponent(entity, new ScoreUI
                {
                    LastScore = score.Current,
                    ScoreLabel = UISingleton.Q<Label>("score")
                });
            }
        }
        entityCommandBuffer.Playback(state.EntityManager);
        
        foreach(var (score, scoreUI) in SystemAPI.Query<Score, ScoreUI>())
        {
            if(scoreUI.ScoreLabel != null)
            {
                if(scoreUI.LastScore != score.Current)
                {
                    scoreUI.LastScore = score.Current;
                    scoreUI.ScoreLabel.text = score.Current.ToString();   
                }
            }
        }
    }
}