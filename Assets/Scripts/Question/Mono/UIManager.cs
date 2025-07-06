//using System;
//using System.Collections;
//using System.Collections.Generic;
//using TMPro; // For using TextMeshPro components
//using Unity.VisualScripting;
//using UnityEngine;
//using UnityEngine.UI;

//#region Struct Definitions

///// <summary>
///// A container for UI configuration parameters like margins and colors.
///// </summary>
//[Serializable]
//public struct UIManagerParameters
//{
//    [Header("Answers Options")]
//    [SerializeField] float margins; // Vertical space between answer options
//    public float Margins { get { return margins; } }

//    [Header("Resolution Screen Options")]
//    [SerializeField] Color correctBGColor; // Background color when answer is correct
//    public Color CorrectBGColor { get { return correctBGColor; } }

//    [SerializeField] Color incorrectBGColor; // Background color when answer is incorrect
//    public Color IncorrectBGColor { get { return incorrectBGColor; } }

//    [SerializeField] Color finalBGColor; // Background color when quiz finishes
//    public Color FinalBGColor { get { return finalBGColor; } }

//    [SerializeField] Color timeoutBGColor; // Background color when time runs out
//    public Color TimeoutBGColor { get { return timeoutBGColor; } } // NEW COLOR FOR TIMEOUT
//}

///// <summary>
///// Holds references to all UI elements that will be modified by the UIManager.
///// </summary>
//[Serializable]
//public struct UIElements
//{
//    [SerializeField] RectTransform answersContentArea; // Parent container for all answer buttons
//    public RectTransform AnswersContentArea { get { return answersContentArea; } }

//    [SerializeField] TextMeshProUGUI questionInfoTextObject; // Displays the current question
//    public TextMeshProUGUI QuestionInfoTextObject { get { return questionInfoTextObject; } }

//    [SerializeField] TextMeshProUGUI scoreText; // Displays the running score
//    public TextMeshProUGUI ScoreText { get { return scoreText; } }

//    [Space]
//    [SerializeField] Animator resolutionScreenAnimator; // Controls resolution screen transitions
//    public Animator ResolutionScreenAnimator { get { return resolutionScreenAnimator; } }

//    [SerializeField] Image resolutionBG; // Background of resolution screen
//    public Image ResolutionBG { get { return resolutionBG; } }

//    [SerializeField] TextMeshProUGUI resolutionStateInfoText; // Displays message like CORRECT or WRONG
//    public TextMeshProUGUI ResolutionStateInfoText { get { return resolutionStateInfoText; } }

//    [SerializeField] TextMeshProUGUI resolutionScoreText; // Displays score change or total
//    public TextMeshProUGUI ResolutionScoreText { get { return resolutionScoreText; } }

//    [Space]
//    [SerializeField] TextMeshProUGUI highScoreText; // Final screen highscore display
//    public TextMeshProUGUI HighScoreText { get { return highScoreText; } }

//    [SerializeField] CanvasGroup mainCanvasGroup; // Main canvas group for enabling/disabling interaction
//    public CanvasGroup MainCanvasGroup { get { return mainCanvasGroup; } }

//    [SerializeField] RectTransform finishUIElements; // UI elements shown at quiz end
//    public RectTransform FinishUIElements { get { return finishUIElements; } }
//}

//#endregion

///// <summary>
///// Manages the updating and transitions of UI elements in the quiz app.
///// </summary>
//public class UIManager : MonoBehaviour
//{
//    // Enum that defines different states for the resolution screen
//    public enum ResolutionScreenType { Correct, Incorrect, Finish, Timeout }

//    [Header("References")]
//    [SerializeField] GameEvents events = null; // Central event system for game communication

//    [Header("UI Elements (Prefabs)")]
//    [SerializeField] AnswerData answerPrefab = null; // Prefab for answer buttons

//    [SerializeField] UIElements uIElements = new UIElements(); // All configurable UI elements

//    [Space]
//    [SerializeField] UIManagerParameters parameters = new UIManagerParameters(); // Configuration parameters

//    private List<AnswerData> currentAnswers = new List<AnswerData>(); // List of current answer UI instances
//    private int resStateParaHash = 0; // Cached hash for animator parameter
//    private IEnumerator IE_DisplayTimedResolution = null; // Coroutine reference for timed resolution

//    #region Unity Methods

//    void OnEnable()
//    {
//        // Subscribe to relevant game events
//        events.UpdateQuestionUI += UpdateQuestionUI;
//        events.DisplayResolutionScreen += DisplayResolution;
//        events.ScoreUpdated += UpdateScoreUI;
//    }

//    void OnDisable()
//    {
//        // Unsubscribe when object is disabled
//        events.UpdateQuestionUI -= UpdateQuestionUI;
//        events.DisplayResolutionScreen -= DisplayResolution;
//        events.ScoreUpdated -= UpdateScoreUI;
//    }

//    void Start()
//    {
//        UpdateScoreUI(); // Initialize score display
//        resStateParaHash = Animator.StringToHash("ScreenState"); // Cache animator parameter
//    }

//    #endregion

//    #region Question UI

//    /// <summary>
//    /// Updates the question text and populates new answers.
//    /// </summary>
//    void UpdateQuestionUI(Question question)
//    {
//        uIElements.QuestionInfoTextObject.text = question.Info;
//        CreateAnswers(question);
//    }

//    /// <summary>
//    /// Instantiates answer UI objects and positions them.
//    /// </summary>
//    void CreateAnswers(Question question)
//    {
//        EraseAnswers(); // Clear previous answers

//        float offset = 0 - parameters.Margins;

//        for (int i = 0; i < question.Answers.Length; i++)
//        {
//            // Instantiate and initialize each answer button
//            AnswerData newAnswer = Instantiate(answerPrefab, uIElements.AnswersContentArea);
//            newAnswer.UpdateData(question.Answers[i].Info, i);

//            // Positioning answer buttons vertically
//            newAnswer.Rect.anchoredPosition = new Vector2(0, offset);
//            offset -= (newAnswer.Rect.sizeDelta.y + parameters.Margins);

//            // Adjust content area height to fit all answers
//            uIElements.AnswersContentArea.sizeDelta = new Vector2(
//                uIElements.AnswersContentArea.sizeDelta.x,
//                offset * -1
//            );

//            currentAnswers.Add(newAnswer);
//        }
//    }

//    /// <summary>
//    /// Removes all currently displayed answer buttons.
//    /// </summary>
//    void EraseAnswers()
//    {
//        foreach (var answer in currentAnswers)
//        {
//            Destroy(answer.gameObject);
//        }
//        currentAnswers.Clear();
//    }

//    #endregion

//    #region Resolution Screen

//    /// <summary>
//    /// Displays resolution screen depending on answer result (correct, incorrect, finish, etc).
//    /// </summary>
//    void DisplayResolution(ResolutionScreenType type, int score)
//    {
//        UpdateResUI(type, score); // Update resolution screen visuals

//        // Show resolution UI screen via animator
//        uIElements.ResolutionScreenAnimator.SetInteger(resStateParaHash, 2);
//        uIElements.MainCanvasGroup.blocksRaycasts = false;

//        // Start coroutine for timed screen if not the final screen
//        if (type != ResolutionScreenType.Finish)
//        {
//            if (IE_DisplayTimedResolution != null)
//                StopCoroutine(IE_DisplayTimedResolution);

//            IE_DisplayTimedResolution = DisplayTimedResolution();
//            StartCoroutine(IE_DisplayTimedResolution);
//        }

//        // Optional audio feedback
//        if (AudioManager.Instance != null)
//        {
//            string sound = type switch
//            {
//                ResolutionScreenType.Correct => "CorrectSFX",
//                ResolutionScreenType.Incorrect => "IncorrectSFX",
//                ResolutionScreenType.Timeout => "TimeoutSFX",
//                ResolutionScreenType.Finish => "FinishSFX",
//                _ => ""
//            };

//            if (!string.IsNullOrEmpty(sound))
//                AudioManager.Instance.PlaySound(sound);
//        }
//    }

//    /// <summary>
//    /// Automatically hides the resolution screen after a delay.
//    /// </summary>
//    IEnumerator DisplayTimedResolution()
//    {
//        yield return new WaitForSeconds(GameUtility.ResolutionDelayTime);

//        uIElements.ResolutionScreenAnimator.SetInteger(resStateParaHash, 1); // Hide screen
//        uIElements.MainCanvasGroup.blocksRaycasts = true; // Re-enable interaction
//    }

//    /// <summary>
//    /// Updates the visuals of the resolution screen based on result type.
//    /// </summary>
//    void UpdateResUI(ResolutionScreenType type, int score)
//    {
//        int highscore = PlayerPrefs.GetInt(GameUtility.SavePrefKey); // Load saved highscore

//        switch (type)
//        {
//            case ResolutionScreenType.Correct:
//                uIElements.ResolutionBG.color = parameters.CorrectBGColor;
//                uIElements.ResolutionStateInfoText.text = "CORRECT!";
//                uIElements.ResolutionScoreText.text = "+" + score;
//                break;

//            case ResolutionScreenType.Incorrect:
//                uIElements.ResolutionBG.color = parameters.IncorrectBGColor;
//                uIElements.ResolutionStateInfoText.text = "WRONG!";
//                uIElements.ResolutionScoreText.text = "-" + score;
//                break;

//            case ResolutionScreenType.Timeout:
//                uIElements.ResolutionBG.color = parameters.TimeoutBGColor;
//                uIElements.ResolutionStateInfoText.text = "TIME'S UP!";
//                uIElements.ResolutionScoreText.text = "-0";
//                break;

//            case ResolutionScreenType.Finish:
//                uIElements.ResolutionBG.color = parameters.FinalBGColor;
//                uIElements.ResolutionStateInfoText.text = "FINAL SCORE";

//                StartCoroutine(CalculateScore()); // Count-up score animation

//                uIElements.FinishUIElements.gameObject.SetActive(true);
//                uIElements.HighScoreText.gameObject.SetActive(true);

//                // If new highscore achieved, highlight it
//                string label = (highscore > events.StartupHighscore) ? "<color=yellow>new </color>" : "";
//                uIElements.HighScoreText.text = label + "Highscore: " + highscore;
//                break;
//        }
//    }

//    /// <summary>
//    /// Coroutine that counts up the final score visually.
//    /// </summary>
//    IEnumerator CalculateScore()
//    {
//        int scoreValue = 0;

//        while (scoreValue < events.CurrentFinalScore)
//        {
//            scoreValue++;
//            uIElements.ResolutionScoreText.text = scoreValue.ToString();
//            yield return null; // Wait for next frame
//        }
//    }

//    #endregion

//    #region Score

//    /// <summary>
//    /// Updates the in-game score text.
//    /// </summary>
//    void UpdateScoreUI()
//    {
//        uIElements.ScoreText.text = "Score: " + events.CurrentFinalScore;
//    }

//    #endregion
//}
