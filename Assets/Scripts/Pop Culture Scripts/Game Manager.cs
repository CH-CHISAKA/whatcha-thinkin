using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private Question[] _questions = null;
    public Question[] Questions => _questions;

    [SerializeField] private GameEvents events = null;

    private List<int> FinishedQuestions = new List<int>();
    private int currentQuestion = 0;

    private AnswerOption pickedOption;
    private bool questionAnswered = false;

    [SerializeField] private int totalQuizTime = 60; // Total quiz time in seconds
    private float quizTimeLeft;
    private bool quizActive = false;

    private bool IsFinished => FinishedQuestions.Count >= Questions.Length;

    void Awake()
    {
        events.CurrentFinalScore = 0;
    }

    void Start()
    {
        events.StartupHighscore = PlayerPrefs.GetInt("Highscore", 0);

        LoadQuestions();

        var seed = Random.Range(int.MinValue, int.MaxValue);
        Random.InitState(seed);

        quizTimeLeft = totalQuizTime;
        quizActive = true;
        StartCoroutine(GlobalTimerCoroutine());

        Display();
    }

    public void OnAnswerSelected(AnswerOption option)
    {
        if (questionAnswered || !quizActive)
            return;

        pickedOption = option;
        Accept();
    }

    public void OnYesSelected() => OnAnswerSelected(AnswerOption.Yes);
    public void OnNoSelected() => OnAnswerSelected(AnswerOption.No);
    public void OnMaybeSelected() => OnAnswerSelected(AnswerOption.Maybe);
    public void OnSometimesSelected() => OnAnswerSelected(AnswerOption.Sometimes);

    void Display()
    {
        var question = GetRandomQuestion();

        if (events.UpdateQuestionUI != null)
            events.UpdateQuestionUI(question);
        else
            Debug.LogWarning("GameEvents.UpdateQuestionUI is null.");

        questionAnswered = false;
    }

    void Accept()
    {
        questionAnswered = true;

        bool isCorrect = Questions[currentQuestion].IsCorrect(pickedOption);
        FinishedQuestions.Add(currentQuestion);

        // Correct = +50, Incorrect = -25
        UpdateScore(isCorrect ? 50 : -25);


        if (!IsFinished && quizActive)
        {
            Invoke(nameof(PrepareNextQuestion), 1.5f);
        }
        else
        {
            Debug.Log("Quiz finished!");
        }
    }

    void PrepareNextQuestion()
    {
        Display();
    }

    IEnumerator GlobalTimerCoroutine()
    {
        while (quizTimeLeft > 0)
        {
            if (events.UpdateTimerUI != null)
                events.UpdateTimerUI(quizTimeLeft);

            yield return new WaitForSeconds(1f);
            quizTimeLeft--;
        }

        quizActive = false;
        Debug.Log("Time's up! Quiz ended.");
    }

    void LoadQuestions()
    {
        Object[] objs = Resources.LoadAll("Questions", typeof(Question));
        _questions = new Question[objs.Length];
        for (int i = 0; i < objs.Length; i++)
            _questions[i] = (Question)objs[i];

        Debug.Log("Questions loaded: " + _questions.Length);
    }

    void UpdateScore(int add)
    {
        events.CurrentFinalScore += add;

        if (events.ScoreUpdated != null)
            events.ScoreUpdated();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    Question GetRandomQuestion()
    {
        int random;
        do
        {
            random = Random.Range(0, Questions.Length);
        }
        while (FinishedQuestions.Contains(random));

        currentQuestion = random;
        return Questions[currentQuestion];
    }
}
