// Updated code base

//using UnityEngine.SocialPlatforms.Impl;

//Score tracking

//Progress bar or timer UI

//Sound effects or animations

//Data persistence or question loading from external files

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Handles quiz question setup, answer validation, timer, and UI control.
/// Manages the flow of the quiz game including question selection, answer display,
/// timing, and user feedback.
/// </summary>
public class QuestionSetup : MonoBehaviour
{
    [Header("Questions")]
    public List<QuestionData> questions;                  // List of all questions (loaded or assigned)
    private List<QuestionData> unansweredQuestions;       // Tracks questions not yet asked in the current cycle

    [Header("UI")]
    public TextMeshProUGUI questionText;                   // UI text component to show the question string
    public TextMeshProUGUI categoryText;                   // UI text component to show question category (optional)
    public AnswerButton[] answerButtons;                    // Array of buttons to display possible answers visually

    [Header("Settings")]
    public float timeToAnswer = 5f;                         // Time limit for each question before auto-timing out

    [Header("Answer Sprites")]
    public Sprite yesSprite;                                // Sprite for "Yes" answer visual
    public Sprite noSprite;                                 // Sprite for "No" answer visual
    public Sprite maybeSprite;                              // Sprite for "Maybe" answer visual
    public Sprite sometimesSprite;                          // Sprite for "Sometimes" answer visual

    private QuestionData currentQuestion;                   // Currently active question
    private Coroutine timerCoroutine;                        // Coroutine reference for the countdown timer
    private bool answered;                                   // Flag to track if current question has been answered

    private Dictionary<string, Sprite> spriteMap;           // Maps answer text strings to corresponding sprites

    /// <summary>
    /// Unity Start method: initializes mappings, loads questions, and starts the quiz.
    /// </summary>
    private void Start()
    {
        BuildSpriteMap();          // Create dictionary mapping answer strings to sprites
        GetQuestionAssets();       // Load question data if not assigned manually
        RestartGame();             // Begin quiz cycle with fresh questions
    }

    /// <summary>
    /// Creates a dictionary mapping answer strings to their corresponding visual sprites.
    /// This allows easy lookup of sprites based on the answer text.
    /// </summary>
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

    /// <summary>
    /// Loads question assets from the Resources folder if the questions list is empty or null.
    /// This method assumes questions are ScriptableObjects stored in "Resources/Questions".
    /// </summary>
    private void GetQuestionAssets()
    {
        if (questions == null || questions.Count == 0)
        {
            questions = new List<QuestionData>(Resources.LoadAll<QuestionData>("AnimalQuestion"));
        }
    }

    /// <summary>
    /// Starts or restarts the quiz game.
    /// Resets the list of unanswered questions and picks the first question.
    /// </summary>
    public void RestartGame()
    {
        unansweredQuestions = new List<QuestionData>(questions);  // Reset unanswered questions
        ShuffleList(unansweredQuestions);                         // Shuffle to randomize question order
        NextQuestion();                                           // Load the first question
    }

    /// <summary>
    /// Loads the next question in the quiz.
    /// Updates UI, randomizes answer order, initializes answer buttons,
    /// and starts the answer countdown timer.
    /// </summary>
    void NextQuestion()
    {
        answered = false;  // Reset answer state

        // Check if all questions have been answered
        if (unansweredQuestions.Count == 0)
        {
            Debug.Log("All questions answered! Restarting...");
            RestartGame();
            return;
        }

        // Take the next question from the list and remove it
        currentQuestion = unansweredQuestions[0];
        unansweredQuestions.RemoveAt(0);

        // Update UI with question and category text
        questionText.text = currentQuestion.question;
        if (categoryText != null)
            categoryText.text = currentQuestion.category;

        // Prepare a randomized list of answer indices to shuffle answer order on UI
        List<int> answerIndices = new List<int>();
        for (int i = 0; i < currentQuestion.answers.Length; i++)
            answerIndices.Add(i);
        ShuffleList(answerIndices);

        // Assign each shuffled answer to an AnswerButton, including the correct flag and sprite
        for (int i = 0; i < answerButtons.Length; i++)
        {
            if (i < answerIndices.Count)
            {
                int answerIndex = answerIndices[i];
                bool isCorrect = answerIndex == currentQuestion.correctAnswerIndex;

                string answerKey = currentQuestion.answers[answerIndex];

                // Retrieve sprite mapped to this answer's text (may be null if not mapped)
                Sprite answerSprite = spriteMap.ContainsKey(answerKey) ? spriteMap[answerKey] : null;

                if (answerSprite == null)
                {
                    Debug.LogWarning($"No sprite mapped for answer: {answerKey}");
                }

                // Activate and initialize the button with the sprite and correctness flag
                answerButtons[i].gameObject.SetActive(true);
                answerButtons[i].Initialize(answerSprite, isCorrect, this);
            }
            else
            {
                // Hide buttons that don't have corresponding answers for this question
                answerButtons[i].gameObject.SetActive(false);
            }
        }

        // Stop any existing timer coroutine before starting a new countdown
        if (timerCoroutine != null)
            StopCoroutine(timerCoroutine);

        // Start the countdown timer coroutine for answer timeout
        timerCoroutine = StartCoroutine(AnswerTimeout());
    }

    /// <summary>
    /// Coroutine managing the countdown timer for answering.
    /// If time runs out without an answer, automatically shows correct answer and moves on.
    /// </summary>
    IEnumerator AnswerTimeout()
    {
        yield return new WaitForSeconds(timeToAnswer);

        if (!answered)
        {
            answered = true;                   // Mark question as answered to prevent further input
            Debug.Log("Time expired! Showing correct answer.");
            ShowCorrectAnswerHighlight();     // Highlight the correct answer button visually
            DisableAllButtons();               // Prevent interaction

            yield return new WaitForSeconds(1.5f); // Pause briefly to let user see feedback
            NextQuestion();                   // Load next question
        }
    }

    /// <summary>
    /// Called by an AnswerButton when user selects an answer.
    /// Validates correctness, gives feedback, and proceeds to next question after delay.
    /// </summary>
    /// <param name="isCorrect">Whether the selected answer is correct</param>
    /// <param name="selectedButton">Reference to the button that was clicked</param>
    public void OnAnswerSelected(bool isCorrect, AnswerButton selectedButton)
    {
        if (answered) return;  // Ignore if question already answered

        answered = true;

        if (timerCoroutine != null)
            StopCoroutine(timerCoroutine);  // Stop countdown timer

        Debug.Log(isCorrect ? "CORRECT ANSWER" : "WRONG ANSWER");

        // Show visual feedback on selected button
        if (isCorrect)
        {
            selectedButton.MarkCorrect();
        }
        else
        {
            selectedButton.MarkWrong();
            ShowCorrectAnswerHighlight(); // Highlight the right answer to guide the player
        }

        DisableAllButtons();               // Disable all buttons to prevent input during feedback

        StartCoroutine(WaitAndNext(1.5f)); // Wait before loading next question
    }

    /// <summary>
    /// Waits a specified delay before loading the next question.
    /// Used to give players time to see answer feedback.
    /// </summary>
    /// <param name="delay">Delay in seconds</param>
    IEnumerator WaitAndNext(float delay)
    {
        yield return new WaitForSeconds(delay);
        NextQuestion();
    }

    /// <summary>
    /// Disables interaction on all answer buttons.
    /// Called when the question is answered or time expires.
    /// </summary>
    private void DisableAllButtons()
    {
        foreach (var btn in answerButtons)
        {
            btn.DisableButton();
        }
    }

    /// <summary>
    /// Visually highlights the correct answer button(s).
    /// Called when time runs out or a wrong answer is selected.
    /// </summary>
    private void ShowCorrectAnswerHighlight()
    {
        foreach (var btn in answerButtons)
        {
            // Ensure button is active and corresponds to correct answer before marking
            if (btn != null && btn.gameObject.activeSelf && btn.IsCorrectAnswer())
            {
                btn.MarkCorrect();
            }
        }
    }

    /// <summary>
    /// Fisher-Yates shuffle algorithm to randomize order of list elements.
    /// Used for shuffling questions and answer choices.
    /// </summary>
    /// <typeparam name="T">Type of list elements</typeparam>
    /// <param name="list">List to shuffle</param>
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

