using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameEvents events = null;

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timerText; // Assign this in Inspector

    void OnEnable()
    {
        events.UpdateQuestionUI += UpdateQuestionUI;
        events.ScoreUpdated += UpdateScoreUI;
        events.UpdateTimerUI += UpdateTimerUI; // Subscribe to timer updates
    }

    void OnDisable()
    {
        events.UpdateQuestionUI -= UpdateQuestionUI;
        events.ScoreUpdated -= UpdateScoreUI;
        events.UpdateTimerUI -= UpdateTimerUI; // Unsubscribe
    }

    void Start()
    {
        UpdateScoreUI();
        UpdateTimerUI(0); // Initialize timer text
    }

    void UpdateQuestionUI(Question question)
    {
        questionText.text = question.Info;
    }

    void UpdateTimerUI(float timeLeft)
    {
        if (timerText != null)
        {
            timerText.text = Mathf.CeilToInt(timeLeft).ToString();
        }
    }

    void UpdateScoreUI()
    {
        scoreText.text = "Score: " + events.CurrentFinalScore;
    }
}
