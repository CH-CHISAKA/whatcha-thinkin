using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the appearance and behavior of an answer button using only sprite and color feedback (no extra UI visuals).
/// </summary>
public class AnswerButton : MonoBehaviour
{
    [Header("Core Components")]
    public Button button;               // Button for click input
    public Image answerImage;           // Displays the answer sprite and color highlight

    [Header("Colors")]
    public Color correctColor = new Color(0.2f, 1f, 0.2f);  // Green
    public Color wrongColor = new Color(1f, 0.2f, 0.2f);    // Red
    public Color defaultColor = Color.white;               // Neutral white

    // State
    private QuestionSetup questionSetup;                   // Reference to quiz logic
    private bool isCorrectAnswer;                          // Whether this is the correct choice
    private Sprite defaultSprite;                          // Default visual sprite

    private void Awake()
    {
        if (button == null) button = GetComponent<Button>();
        if (answerImage == null) answerImage = GetComponent<Image>();

        button.interactable = false;
        button.onClick.AddListener(OnClick);
    }

    /// <summary>
    /// Initialize button with sprite, answer state, and setup reference.
    /// </summary>
    public void Initialize(Sprite sprite, bool isCorrect, QuestionSetup setup)
    {
        questionSetup = setup;
        isCorrectAnswer = isCorrect;

        if (answerImage != null && sprite != null)
        {
            answerImage.sprite = sprite;
            defaultSprite = sprite;
        }

        ResetVisual();
        button.interactable = true;
    }

    private void OnClick()
    {
        if (questionSetup != null)
        {
            button.interactable = false;
            questionSetup.OnAnswerSelected(isCorrectAnswer, this);
        }
    }

    public bool IsCorrectAnswer()
    {
        return isCorrectAnswer;
    }

    /// <summary>
    /// Highlights this button as correct (green color overlay).
    /// </summary>
    public void MarkCorrect()
    {
        if (answerImage != null)
            answerImage.color = correctColor;
    }

    /// <summary>
    /// Highlights this button as wrong (red color overlay).
    /// </summary>
    public void MarkWrong()
    {
        if (answerImage != null)
            answerImage.color = wrongColor;
    }

    /// <summary>
    /// Highlights correct answer subtly if user selected the wrong one.
    /// </summary>
    public void HighlightAsCorrectBorderOnly()
    {
        // Since we only have one image, use a soft tint instead of full green
        if (answerImage != null)
            answerImage.color = new Color(correctColor.r, correctColor.g, correctColor.b, 0.5f); // semi-transparent green
    }

    /// <summary>
    /// Resets the button's color and sprite.
    /// </summary>
    public void ResetVisual()
    {
        if (answerImage != null)
        {
            answerImage.color = defaultColor;
            if (defaultSprite != null)
                answerImage.sprite = defaultSprite;
        }
    }

    /// <summary>
    /// Prevents further interaction.
    /// </summary>
    public void DisableButton()
    {
        if (button != null)
            button.interactable = false;
    }
}
