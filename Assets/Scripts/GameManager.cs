//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;
//using UnityEngine.UI;
//using TMPro;

//public class GameManager : MonoBehaviour
//{
//    [Header("UI Elements")]
//    [SerializeField] private TextMeshProUGUI _questionText;
//    [SerializeField] private List<Button> _answerButtons;

//    [Header("Timer")]
//    [SerializeField] private TextMeshProUGUI _timerText;
//    [SerializeField] private Color _halfTimeColor = Color.yellow;
//    [SerializeField] private Color _almostOutColor = Color.red;
//    private Color _defaultColor;

//    [Header("Questions")]
//    [SerializeField] private string _questionFolderName = "Questions";
//    private List<Question> _questions;
//    private Question _currentQuestion;

//    private Coroutine _timerCoroutine;

//    private int _currentIndex = 0;
//    private int _score = 0;
//    private List<int> _usedIndexes = new List<int>();

//    void Start()
//    {
//        _defaultColor = _timerText.color;
//        LoadQuestions();
//        ShuffleQuestions();
//        LoadQuestion();
//    }

//    /// <summary>
//    /// Loads all question assets from the Resources/Questions folder.
//    /// </summary>
//    private void LoadQuestions()
//    {
//        var loadedObjects = Resources.LoadAll<Question>(_questionFolderName);
//        _questions = loadedObjects.ToList();
//    }

//    /// <summary>
//    /// Shuffles the question list so that they're presented in random order.
//    /// </summary>
//    private void ShuffleQuestions()
//    {
//        for (int i = 0; i < _questions.Count; i++)
//        {
//            Question temp = _questions[i];
//            int randomIndex = Random.Range(i, _questions.Count);
//            _questions[i] = _questions[randomIndex];
//            _questions[randomIndex] = temp;
//        }
//    }

//    /// <summary>
//    /// Loads the current question and starts the timer.
//    /// </summary>
//    private void LoadQuestion()
//    {
//        if (_currentIndex >= _questions.Count)
//        {
//            Debug.Log($"Quiz Finished! Final Score: {_score}");
//            // You can trigger end screen logic here.
//            return;
//        }

//        _currentQuestion = _questions[_currentIndex];
//        _questionText.text = _currentQuestion.Info;

//        // Set up answer buttons
//        for (int i = 0; i < _answerButtons.Count; i++)
//        {
//            if (i < _currentQuestion.Answers.Length)
//            {
//                _answerButtons[i].gameObject.SetActive(true);
//                _answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = _currentQuestion.Answers[i].Info;

//                int index = i; // Capture index for lambda
//                _answerButtons[i].onClick.RemoveAllListeners();
//                _answerButtons[i].onClick.AddListener(() => OnAnswerSelected(index));
//            }
//            else
//            {
//                _answerButtons[i].gameObject.SetActive(false);
//            }
//        }

//        // Start timer if enabled
//        if (_currentQuestion.UseTimer)
//        {
//            if (_timerCoroutine != null)
//                StopCoroutine(_timerCoroutine);

//            _timerCoroutine = StartCoroutine(AnswerTimer(_currentQuestion.Timer));
//        }
//        else
//        {
//            _timerText.text = "";
//        }
//    }

//    /// <summary>
//    /// Called when a player selects an answer.
//    /// </summary>
//    /// <param name="index">Index of the selected answer</param>
//    private void OnAnswerSelected(int index)
//    {
//        if (_timerCoroutine != null)
//            StopCoroutine(_timerCoroutine);

//        bool isCorrect = _currentQuestion.Answers[index].IsCorrect;

//        Debug.Log($"Selected: {_currentQuestion.Answers[index].Info} | Correct: {isCorrect}");

//        if (isCorrect)
//        {
//            _score += _currentQuestion.AddScore;
//            Debug.Log($"Correct! Score: {_score}");
//        }
//        else
//        {
//            Debug.Log("Incorrect!");
//        }

//        _currentIndex++;
//        LoadQuestion();
//    }

//    /// <summary>
//    /// Timer coroutine: waits and updates timer text every second.
//    /// When time runs out, moves to the next question.
//    /// </summary>
//    /// <param name="seconds">Time to wait</param>
//    IEnumerator AnswerTimer(int seconds)
//    {
//        int timeLeft = seconds;
//        _timerText.color = _defaultColor;

//        while (timeLeft > 0)
//        {
//            _timerText.text = timeLeft.ToString();

//            if (timeLeft <= seconds / 2 && timeLeft > seconds / 4)
//            {
//                _timerText.color = _halfTimeColor;
//            }
//            else if (timeLeft <= seconds / 4)
//            {
//                _timerText.color = _almostOutColor;
//            }

//            yield return new WaitForSeconds(1f);
//            timeLeft--;
//        }

//        Debug.Log("Time's up!");
//        _currentIndex++;
//        LoadQuestion();
//    }

//    /// <summary>
//    /// Restarts the quiz with new random order.
//    /// </summary>
//    public void RestartQuiz()
//    {
//        _score = 0;
//        _currentIndex = 0;
//        ShuffleQuestions();
//        LoadQuestion();
//    }
//}
