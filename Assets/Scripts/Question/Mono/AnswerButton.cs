using UnityEngine;
using UnityEngine.UI;
using TMPro;

using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages a purely visual answer button that uses sprites to represent answers (e.g., "Yes", "No", "Maybe").
/// This script handles displaying the button’s visual state, user interaction, and feedback (correct/wrong).
/// </summary>
public class AnswerButton : MonoBehaviour
{
    // The Unity UI Button component used for click input.
    public Button button;

    // The Image component which displays the current visual sprite for the answer.
    public Image answerImage;

    // Sprite to overlay when the answer is marked correct (e.g., green checkmark or highlight).
    public Sprite correctOverlay;

    // Sprite to overlay when the answer is marked wrong (e.g., red cross or highlight).
    public Sprite wrongOverlay;

    // The original sprite that represents the answer, stored to reset the button's appearance.
    public Sprite defaultSprite;

    // Reference to the main quiz controller to notify when this button is clicked.
    private QuestionSetup questionSetup;

    // Indicates whether this button represents the correct answer to the current question.
    private bool isCorrectAnswer;

    /// <summary>
    /// Unity Awake lifecycle method.
    /// Automatically finds and assigns Button and Image components if not manually assigned.
    /// Adds OnClick listener to button for handling user input.
    /// Initially disables interaction until initialized.
    /// </summary>
    private void Awake()
    {
        // Auto-assign the Button component if null
        if (button == null)
            button = GetComponent<Button>();

        // Auto-assign the Image component if null
        if (answerImage == null)
            answerImage = GetComponent<Image>();

        if (button != null)
        {
            button.interactable = false;        // Disable interaction until setup is complete
            button.onClick.AddListener(OnClick); // Attach OnClick handler
        }
    }

    /// <summary>
    /// Initializes the button with the visual answer sprite and correctness flag.
    /// Also sets the reference to the main quiz logic to notify when clicked.
    /// Enables the button for interaction.
    /// </summary>
    /// <param name="sprite">Sprite representing this answer visually (e.g., icon or image)</param>
    /// <param name="isCorrect">True if this answer is correct for the current question</param>
    /// <param name="setup">Reference to the QuestionSetup controller to send answer events</param>
    public void Initialize(Sprite sprite, bool isCorrect, QuestionSetup setup)
    {
        questionSetup = setup;
        isCorrectAnswer = isCorrect;

        if (answerImage != null)
        {
            answerImage.sprite = sprite;  // Set the answer image to provided sprite
            defaultSprite = sprite;       // Save sprite to reset later
        }

        if (button != null)
            button.interactable = true;   // Enable button interaction
    }

    /// <summary>
    /// Called automatically when the button is clicked by the user.
    /// Prevents multiple clicks by disabling interaction immediately.
    /// Notifies the quiz controller of the selected answer and whether it was correct.
    /// </summary>
    private void OnClick()
    {
        // Safety check to ensure QuestionSetup is assigned
        if (questionSetup == null)
        {
            Debug.LogError("[AnswerButton] QuestionSetup reference is null.");
            return;
        }

        button.interactable = false; // Disable button to avoid multiple inputs

        // Notify the QuestionSetup that this answer was selected, passing correctness and self reference
        questionSetup.OnAnswerSelected(isCorrectAnswer, this);
    }

    public bool IsCorrectAnswer()
    {
        return isCorrectAnswer;
    }

    /// <summary>
    /// Visually marks this button as the correct answer.
    /// Changes the sprite to the correctOverlay if set.
    /// </summary>
    public void MarkCorrect()
    {
        if (answerImage != null && correctOverlay != null)
            answerImage.sprite = correctOverlay;
    }

    /// <summary>
    /// Visually marks this button as an incorrect answer.
    /// Changes the sprite to the wrongOverlay if set.
    /// </summary>
    public void MarkWrong()
    {
        if (answerImage != null && wrongOverlay != null)
            answerImage.sprite = wrongOverlay;
    }

    /// <summary>
    /// Resets the visual state of the button to its original appearance.
    /// Re-enables interaction.
    /// </summary>
    public void ResetVisual()
    {
        if (answerImage != null && defaultSprite != null)
            answerImage.sprite = defaultSprite;

        if (button != null)
            button.interactable = true;  // Allow interaction again
    }

    /// <summary>
    /// Disables the button interaction, preventing further clicks.
    /// Typically called after an answer has been selected or timeout occurred.
    /// </summary>
    public void DisableButton()
    {
        if (button != null)
            button.interactable = false;
    }
}
