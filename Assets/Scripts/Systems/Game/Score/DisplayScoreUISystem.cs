using Unity.Entities;
using UnityEngine.UIElements;

public partial struct DisplayScoreUISystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        foreach(var (score, scoreUI) in SystemAPI.Query<Score, ScoreUI>())
        {
            if(scoreUI.ScoreLabel == null)
                scoreUI.ScoreLabel = UISingleton.Q<Label>("score");
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