using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // TextMeshPro namespace for better text rendering

// This MonoBehaviour handles the display of rotating instructional tips with fade effects.
public class InstructionManager : MonoBehaviour
{
    [Header("Instruction UI")]
    public GameObject InstructionPanel;         // UI panel that holds the instruction text
    public TMP_Text InstructionText;            // TextMeshPro UI component to display instructions

    [Header("Instruction Settings")]
    public float TotalDisplayTime = 4.5f;       // Total time each instruction is displayed
    public float FadeDuration = 0.5f;           // Time taken for fade-in and fade-out animations

    // Predefined list of instructional strings/tips to be shown to the player
    private readonly string[] instructions = new string[]
    {
        // A variety of gameplay tips, strategic advice, and category-specific guidance
        "Tip: The correct answer is hidden in the question!",
        "Tip: \"SOMETIMES\", the correct answer is hiding in the question!",
        "Tip: You have only 7 seconds to think of a the correct response!",
        "Tip: “YES” can be your secret weapon!",
        "Tip: Animals category can be tricky — some answers depend on species, not facts!",
        "Tip: Stuck? Switch to a new category to refresh your thinking!",
        "Tip: Solo mode for practice. Multiplayer for party chaos!",
        "Tip: Don’t underestimate “Sometimes.” It’s not a dodge — it’s data!",
        "Tip: Animals category can be tricky — some answers depend on species, not facts!",
        "Tip: Master categories – “Mystery Mix” is for pros. Start with Food or Animals to warm up.",
        "Tip: Don’t rush your turns – Be clever, not quick.",
        "Tip: If you're thinking in straight lines, you're already behind.",
        "Tip: “Sometimes” is basically the Animal category in one word!",
        "Tip: Real or fictional? That’s your first wall to break!",
        "Tip: Mystery Mix breaks all the rules. And maybe your brain..",
        "Tip: Sometimes “MAYBE” is the honest answer. Sometimes it’s psychological warfare.",
        "Tip: Your opponent's emotes might hint at their object… or be a bluff.",
        "Tip: Chain logic works. If it’s not alive, not electronic, not edible… it's not a “YES”",
        "Tip: The best players ask what something isn’t!",
        "Tip: Some players believe the 'Mystery Mix' is haunted code.",
        "Tip: “Sometimes” is the trickster answer — it’s both true and false. Use it to your advantage.",
        "Tip: In the Animal category, “Sometimes” is practically a default. Lean into it.",
        "Tip: Learn the difference between “Maybe” and “Sometimes.” One is uncertainty. The other is inconsistency.",
        "Tip: If you’ve ruled out “Yes” and “No,” the weird truth probably lives in “Maybe.”",
        "Tip: Some objects live in the gray zone — “Sometimes” is your spotlight.",
    };

    private List<string> shuffledInstructions;  // List of instructions shuffled for random display order
    private int currentIndex = 0;               // Tracks current index in the shuffled list

    // Called when the script instance is first enabled
    void Start()
    {
        // Ensure the instruction panel is active
        if (InstructionPanel != null)
        {
            InstructionPanel.SetActive(true);
        }

        // Copy the original instructions array into a list for shuffling
        shuffledInstructions = new List<string>(instructions);
        ShuffleInstructions(); // Randomize the order

        // Start the coroutine that continuously shows instructions
        StartCoroutine(ShowRandomInstructions());
    }

    // Shuffles the instruction list using Fisher-Yates algorithm
    void ShuffleInstructions()
    {
        for (int i = 0; i < shuffledInstructions.Count; i++)
        {
            // Pick a random index between i and the end of the list
            int randIndex = Random.Range(i, shuffledInstructions.Count);

            // Swap current element with the randomly chosen one
            string temp = shuffledInstructions[i];
            shuffledInstructions[i] = shuffledInstructions[randIndex];
            shuffledInstructions[randIndex] = temp;
        }

        // Reset index to start of the new shuffle
        currentIndex = 0;
    }

    // Coroutine to loop through the instructions and display them with fade animations
    IEnumerator ShowRandomInstructions()
    {
        // Keep showing instructions while the panel exists and is active
        while (InstructionPanel != null && InstructionPanel.activeSelf)
        {
            // Reshuffle once all instructions have been displayed
            if (currentIndex >= shuffledInstructions.Count)
            {
                ShuffleInstructions();
            }

            // Get the next instruction from the list
            string newInstruction = shuffledInstructions[currentIndex];
            currentIndex++;

            // Fade out current text before showing new one
            yield return StartCoroutine(FadeTextOut());

            // Set new instruction text
            InstructionText.text = newInstruction;

            // Fade in new instruction
            yield return StartCoroutine(FadeTextIn());

            // Hold the text visible for remaining display time (excluding fade times)
            float holdTime = TotalDisplayTime - (2 * FadeDuration);
            yield return new WaitForSeconds(holdTime);
        }
    }

    // Coroutine to fade the instruction text out to transparent
    IEnumerator FadeTextOut()
    {
        float elapsed = 0f;
        Color originalColor = InstructionText.color;

        while (elapsed < FadeDuration)
        {
            // Gradually reduce alpha from 1 to 0 over FadeDuration
            float alpha = Mathf.Lerp(1f, 0f, elapsed / FadeDuration);
            InstructionText.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            elapsed += Time.deltaTime;
            yield return null; // Wait for next frame
        }

        // Ensure it's fully transparent at the end
        InstructionText.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
    }

    // Coroutine to fade the instruction text in from transparent
    IEnumerator FadeTextIn()
    {
        float elapsed = 0f;
        Color originalColor = InstructionText.color;

        while (elapsed < FadeDuration)
        {
            // Gradually increase alpha from 0 to 1 over FadeDuration
            float alpha = Mathf.Lerp(0f, 1f, elapsed / FadeDuration);
            InstructionText.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            elapsed += Time.deltaTime;
            yield return null; // Wait for next frame
        }

        // Ensure it's fully opaque at the end
        InstructionText.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1f);
    }
}
