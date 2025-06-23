using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Manages UI logic for category selection and Play button validation.
/// Ensures a category is selected before allowing the game to proceed.
/// </summary>
public class CategorySelector : MonoBehaviour
{
    [Header("UI References")]

    // Handle Audio
    AudioManager audioManager;

    // Buttons for selecting categories (e.g. Animals, Food, etc.)
    public List<Button> categoryButtons;

    // Color used to highlight the selected category.
    public Color highlightColor = Color.grey;

    // Color used for unselected (default) buttons.
    public Color defaultColor = Color.white;

    // Play button to proceed with the selected category.
    public Button playButton;

    // Text to show error messages if no category is selected.
    public TMP_Text errorText;

    // Index of the currently selected category. -1 means no selection.
    private int selectedIndex = -1;


    // Audio Function
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Start()
    {
        // Assign a click listener to each category button.
        for (int i = 0; i < categoryButtons.Count; i++)
        {
            int idx = i; // Local copy to avoid closure issue.
            categoryButtons[i].onClick.AddListener(() => OnCategorySelected(idx));
        }

        // Assign click listener to the Play button.
        playButton.onClick.AddListener(ValidatePlay);

        audioManager?.PlaySFX(audioManager.buttonPress);

        // Disable Play button initially.
        playButton.interactable = false;

        // Hide error message at start.
        errorText.gameObject.SetActive(false);
    }

    /// <summary>
    /// Handles logic when a category is selected.
    /// Updates button colors and enables the Play button.
    /// </summary>
    void OnCategorySelected(int index)
    {
        selectedIndex = index;

        for (int i = 0; i < categoryButtons.Count; i++)
        {
            // Update the color for each button based on whether it's selected.
            var colors = categoryButtons[i].colors;
            colors.normalColor = (i == index) ? highlightColor : defaultColor;
            categoryButtons[i].colors = colors;
        }

        // Enable Play button now that a category is selected.
        playButton.interactable = true;

        // Play selection sound
        audioManager?.PlaySFX(audioManager.buttonPress);


        // Hide any previous error messages.
        errorText.gameObject.SetActive(false);
    }

    /// <summary>
    /// Called when Play button is pressed. Checks if a category is selected.
    /// </summary>
    void ValidatePlay() 
    {
        if (selectedIndex == -1)
        {
            // No category selected – show error.
            StartCoroutine(ShowError("Please select a category before playing!"));
            playButton.interactable = false;
        }
        else
        {
            // TODO: Proceed to next scene or start game logic.
        }
    }

    /// <summary>
    /// Displays a blinking error message for 1 second, then hides it.
    /// </summary>
    IEnumerator ShowError(string message)
    {
        errorText.text = message;
        errorText.gameObject.SetActive(true);
        errorText.color = Color.red;

        float t = 0f;
        float blinkDuration = 0.2f;

        // Blink the error text for 1 second.
        while (t < 1f)
        {
            errorText.enabled = !errorText.enabled;
            yield return new WaitForSeconds(blinkDuration);
            t += blinkDuration;
        }

        // Ensure it's visible at the end of blinking.
        errorText.enabled = true;

        // Wait a moment longer before hiding.
        yield return new WaitForSeconds(1f);
        errorText.gameObject.SetActive(false);
    }
}
