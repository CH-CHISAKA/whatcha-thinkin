using UnityEngine;

[CreateAssetMenu(fileName = "GameEvents", menuName = "Quiz/new GameEvents")]
public class GameEvents : ScriptableObject
{
    public delegate void UpdateQuestionUICallback(Question question);
    public UpdateQuestionUICallback UpdateQuestionUI = null;

    public delegate void ScoreUpdatedCallback();
    public ScoreUpdatedCallback ScoreUpdated = null;

    public delegate void UpdateTimerUICallback(float timeLeft);
    public UpdateTimerUICallback UpdateTimerUI = null;

    [HideInInspector]
    public int CurrentFinalScore = 0;

    [HideInInspector]
    public int StartupHighscore = 0;
}
