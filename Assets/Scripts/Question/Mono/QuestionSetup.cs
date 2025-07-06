//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using TMPro;

///// <summary>
///// Manages question display, answer setup, validation, and game flow.
///// </summary>
//public class QuestionSetup : MonoBehaviour
//{
//    [Header("Questions")]
//    public List<QuestionData> questions;                  // All available questions
//    private List<QuestionData> unansweredQuestions;       // Tracks remaining questions

//    [Header("UI References")]
//    public TextMeshProUGUI questionText;
//    public TextMeshProUGUI categoryText;
//    public AnswerButton[] answerButtons;

//    [Header("Answer Sprites")]
//    public Sprite yesSprite;
//    public Sprite noSprite;
//    public Sprite maybeSprite;
//    public Sprite sometimesSprite;

//    [Header("Settings")]
//    public float timeToAnswer = 10f;

//    private QuestionData currentQuestion;
//    private Coroutine timerCoroutine;
//    private bool answered;

//    private Dictionary<string, Sprite> spriteMap;

//    private void Start()
//    {
//        BuildSpriteMap();
//        GetQuestionAssets();
//        RestartGame();
//    }

//    /// <summary>
//    /// Maps answer text (e.g., "Yes") to corresponding sprite.
//    /// </summary>
//    private void BuildSpriteMap()
//    {
//        spriteMap = new Dictionary<string, Sprite>
//        {
//            { "Yes", yesSprite },
//            { "No", noSprite },
//            { "Maybe", maybeSprite },
//            { "Sometimes", sometimesSprite }
//        };
//    }

//    /// <summary>
//    /// Loads questions from Resources if not assigned manually.
//    /// </summary>
//    private void GetQuestionAssets()
//    {
//        if (questions == null || questions.Count == 0)
//        {
//            questions = new List<QuestionData>(Resources.LoadAll<QuestionData>("AnimalQuestion"));
//        }
//    }

//    /// <summary>
//    /// Resets quiz and starts first question.
//    /// </summary>
//    public void RestartGame()
//    {
//        unansweredQuestions = new List<QuestionData>(questions);
//        ShuffleList(unansweredQuestions);
//        NextQuestion();
//    }

//    /// <summary>
//    /// Sets up the next question and initializes answer buttons.
//    /// </summary>
//    private void NextQuestion()
//    {
//        answered = false;

//        if (unansweredQuestions.Count == 0)
//        {
//            Debug.Log("All questions answered. Restarting quiz.");
//            RestartGame();
//            return;
//        }

//        currentQuestion = unansweredQuestions[0];
//        unansweredQuestions.RemoveAt(0);

//        questionText.text = currentQuestion.question;
//        if (categoryText != null)
//            categoryText.text = currentQuestion.category;

//        // Shuffle answers
//        List<int> answerIndices = new List<int>();
//        for (int i = 0; i < currentQuestion.answers.Length; i++)
//            answerIndices.Add(i);
//        ShuffleList(answerIndices);

//        // Assign answers to buttons
//        for (int i = 0; i < answerButtons.Length; i++)
//        {
//            if (i < answerIndices.Count)
//            {
//                int answerIndex = answerIndices[i];
//                bool isCorrect = answerIndex == currentQuestion.correctAnswerIndex;
//                string answerKey = currentQuestion.answers[answerIndex];

//                Sprite answerSprite = spriteMap.ContainsKey(answerKey) ? spriteMap[answerKey] : null;
//                if (answerSprite == null)
//                    Debug.LogWarning($"No sprite mapped for answer: {answerKey}");

//                answerButtons[i].gameObject.SetActive(true);
//                answerButtons[i].Initialize(answerSprite, isCorrect, this);
//            }
//            else
//            {
//                answerButtons[i].gameObject.SetActive(false);
//            }
//        }

//        // Restart timer
//        if (timerCoroutine != null)
//            StopCoroutine(timerCoroutine);

//        timerCoroutine = StartCoroutine(AnswerTimeout());
//    }

//    /// <summary>
//    /// Called when a player selects an answer.
//    /// Handles feedback and moves to the next question.
//    /// </summary>
//    public void OnAnswerSelected(bool isCorrect, AnswerButton selectedButton)
//    {
//        if (answered) return;
//        answered = true;

//        if (timerCoroutine != null)
//            StopCoroutine(timerCoroutine);

//        Debug.Log(isCorrect ? "CORRECT ANSWER" : "WRONG ANSWER");

//        if (isCorrect)
//        {
//            selectedButton.MarkCorrect();
//        }
//        else
//        {
//            selectedButton.MarkWrong();
//            ShowCorrectAnswerHighlight(selectedButton);
//        }

//        DisableAllButtons();
//        StartCoroutine(WaitAndNext(2.5f));
//    }

//    /// <summary>
//    /// Highlights the correct answer (green border only).
//    /// </summary>
//    private void ShowCorrectAnswerHighlight(AnswerButton selectedButton)
//    {
//        foreach (var btn in answerButtons)
//        {
//            if (btn != null && btn.IsCorrectAnswer())
//            {
//                if (btn != selectedButton)
//                    btn.HighlightAsCorrectBorderOnly();
//                else
//                    btn.MarkCorrect(); // In case the wrong button is also the correct one, rare but safe
//            }
//        }
//    }

//    /// <summary>
//    /// Coroutine that runs if player takes too long to answer.
//    /// </summary>
//    IEnumerator AnswerTimeout()
//    {
//        yield return new WaitForSeconds(timeToAnswer);

//        if (!answered)
//        {
//            answered = true;
//            Debug.Log("Time expired.");
//            ShowCorrectAnswerHighlight(null);
//            DisableAllButtons();
//            yield return new WaitForSeconds(1.5f);
//            NextQuestion();
//        }
//    }

//    /// <summary>
//    /// Disables all answer buttons to prevent interaction.
//    /// </summary>
//    private void DisableAllButtons()
//    {
//        foreach (var btn in answerButtons)
//        {
//            if (btn != null)
//                btn.DisableButton();
//        }
//    }

//    /// <summary>
//    /// Waits for delay before showing next question.
//    /// </summary>
//    IEnumerator WaitAndNext(float delay)
//    {
//        yield return new WaitForSeconds(delay);
//        NextQuestion();
//    }

//    /// <summary>
//    /// Randomizes order of list items using Fisher-Yates shuffle.
//    /// </summary>
//    private void ShuffleList<T>(List<T> list)
//    {
//        for (int i = 0; i < list.Count; i++)
//        {
//            T temp = list[i];
//            int randomIndex = Random.Range(i, list.Count);
//            list[i] = list[randomIndex];
//            list[randomIndex] = temp;
//        }
//    }
//}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Manages question display, answer setup, validation, and game flow.
/// </summary>
public class QuestionSetup : MonoBehaviour
{
    [Header("Questions")]
    public List<QuestionData> questions;
    private List<QuestionData> unansweredQuestions;

    [Header("UI References")]
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI categoryText;
    public AnswerButton[] answerButtons;

    [Header("Answer Sprites")]
    public Sprite yesSprite;
    public Sprite noSprite;
    public Sprite maybeSprite;
    public Sprite sometimesSprite;

    [Header("Settings")]
    public float timeToAnswer = 10f; // 10 seconds delay
    public bool isDebugMode = false; // Toggle for faster testing

    private QuestionData currentQuestion;
    private Coroutine timerCoroutine;
    private bool answered;

    private Dictionary<string, Sprite> spriteMap;

    private void Start()
    {
        BuildSpriteMap();
        GetQuestionAssets();

        if (isDebugMode)
            timeToAnswer = 3f; // Faster delay during debugging

        RestartGame();
    }

    private void BuildSpriteMap()
    {
        spriteMap = new Dictionary<string, Sprite>
        {
            { "Yes", yesSprite },
            { "No", noSprite },
            { "Maybe", maybeSprite },
            { "Sometimes", sometimesSprite }
        };
    }

    private void GetQuestionAssets()
    {
        if (questions == null || questions.Count == 0)
        {
            questions = new List<QuestionData>(Resources.LoadAll<QuestionData>("AnimalQuestion"));
        }
    }

    public void RestartGame()
    {
        unansweredQuestions = new List<QuestionData>(questions);
        ShuffleList(unansweredQuestions);
        NextQuestion();
    }

    private void NextQuestion()
    {
        answered = false;

        if (unansweredQuestions.Count == 0)
        {
            Debug.Log("All questions answered. Restarting quiz.");
            RestartGame();
            return;
        }

        currentQuestion = unansweredQuestions[0];
        unansweredQuestions.RemoveAt(0);

        questionText.text = currentQuestion.question;
        if (categoryText != null)
            categoryText.text = currentQuestion.category;

        List<int> answerIndices = new List<int>();
        for (int i = 0; i < currentQuestion.answers.Length; i++)
            answerIndices.Add(i);
        ShuffleList(answerIndices);

        for (int i = 0; i < answerButtons.Length; i++)
        {
            if (i < answerIndices.Count)
            {
                int answerIndex = answerIndices[i];
                bool isCorrect = answerIndex == currentQuestion.correctAnswerIndex;
                string answerKey = currentQuestion.answers[answerIndex];

                Sprite answerSprite = spriteMap.ContainsKey(answerKey) ? spriteMap[answerKey] : null;
                if (answerSprite == null)
                    Debug.LogWarning($"No sprite mapped for answer: {answerKey}");

                answerButtons[i].gameObject.SetActive(true);
                answerButtons[i].Initialize(answerSprite, isCorrect, this);
            }
            else
            {
                answerButtons[i].gameObject.SetActive(false);
            }
        }

        if (timerCoroutine != null)
            StopCoroutine(timerCoroutine);

        timerCoroutine = StartCoroutine(AnswerTimeout());
    }

    public void OnAnswerSelected(bool isCorrect, AnswerButton selectedButton)
    {
        if (answered) return;
        answered = true;

        if (timerCoroutine != null)
            StopCoroutine(timerCoroutine);

        Debug.Log(isCorrect ? "CORRECT ANSWER" : "WRONG ANSWER");

        if (isCorrect)
        {
            selectedButton.MarkCorrect();
        }
        else
        {
            selectedButton.MarkWrong();
            ShowCorrectAnswerHighlight(selectedButton);
        }

        DisableAllButtons();
        StartCoroutine(WaitAndNext(2.5f));
    }

    private void ShowCorrectAnswerHighlight(AnswerButton selectedButton)
    {
        foreach (var btn in answerButtons)
        {
            if (btn != null && btn.IsCorrectAnswer())
            {
                if (btn != selectedButton)
                    btn.HighlightAsCorrectBorderOnly();
                else
                    btn.MarkCorrect();
            }
        }
    }

    IEnumerator AnswerTimeout()
    {
        yield return new WaitForSeconds(timeToAnswer);

        if (!answered)
        {
            answered = true;
            Debug.Log("Time expired.");
            ShowCorrectAnswerHighlight(null);
            DisableAllButtons();
            yield return new WaitForSeconds(1.5f);
            NextQuestion();
        }
    }

    private void DisableAllButtons()
    {
        foreach (var btn in answerButtons)
        {
            if (btn != null)
                btn.DisableButton();
        }
    }

    IEnumerator WaitAndNext(float delay)
    {
        yield return new WaitForSeconds(delay);
        NextQuestion();
    }

    private void ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}
