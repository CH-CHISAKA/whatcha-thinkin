using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScreenManager : MonoBehaviour
{
    [Header("Loading Screen UI")]
    public GameObject LoadingScreenObject;
    public Slider ProgressBar;
    public TMPro.TMP_Text TipText; // UI Text element for displaying tips

    [Header("Scene Settings")]
    public int TargetSceneIndex = 2; // Scene to load

    // Array of tips to randomly display during loading
    private readonly string[] tips = new string[]
    {
        "Tip: SOMETIMES, the correct answer is hiding in the question!",
        "Tip: “YES” can be your secret weapon!",
        "Tip: The fewer questions you use, the higher your score!",
        "Tip: Animals category can be tricky — some answers depend on species, not facts!",
        "Tip: Stuck? Switch to a new category to refresh your thinking!",
        "Tip: Solo mode for practice. Multiplayer for party chaos!",
        "Tip: Don’t underestimate “Sometimes.” It’s not a dodge — it’s data!",
        "Tip: Beat the AI with weird logic. It's not as smart as you think... yet.",
        "Tip: Master categories – “Mystery Mix” is for pros. Start with Food or Animals to warm up.",
        "Tip: Don’t rush your turns – The AI learns from you. Be clever, not quick.",
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

    void Start()
    {
        // Start loading immediately
        StartCoroutine(LoadWithFakeProgress());
    }

    // Coroutine that controls the fake loading bar and triggers the scene load
    IEnumerator LoadWithFakeProgress()
    {
        // Activate the loading screen UI
        LoadingScreenObject.SetActive(true);

        // Start showing random loading tips in parallel
        StartCoroutine(ShowRandomTips());

        float fakeProgress = 0f;              // Current fake progress (0.0 to 1.0)
        float totalDuration = 5.5f;           // Total duration of fake loading in seconds
        float stagnantStart = 3f;             // When to start the pause/stall
        float stagnantDuration = 1f;          // How long to pause the progress
        float timer = 0f;                     // Elapsed time since loading started

        while (timer < totalDuration)
        {
            // Simulate the passage of time
            timer += Time.deltaTime;

            // During the stagnant period (after 3s), hold the progress steady
            if (timer >= stagnantStart && timer < stagnantStart + stagnantDuration)
            {
                // Just wait without increasing progress
                yield return null;
                continue;
            }

            // Calculate progress as a fraction of totalDuration (excluding the stagnant pause)
            float adjustedTime = timer;

            // Subtract the stagnant time from the time used in progress calculation
            if (timer > stagnantStart + stagnantDuration)
                adjustedTime -= stagnantDuration;
            else if (timer > stagnantStart)
                adjustedTime = stagnantStart;

            // Calculate fake progress based on adjusted time
            fakeProgress = Mathf.Clamp01(adjustedTime / (totalDuration - stagnantDuration));

            // Smooth the visual progress bar for a nicer effect
            ProgressBar.value = Mathf.SmoothStep(0f, 1f, fakeProgress);

            yield return null; // Wait for next frame
        }

        // Brief pause at 100% to make transition feel more natural
        yield return new WaitForSeconds(0.3f);

        // Load the target scene by its index
        SceneManager.LoadScene(TargetSceneIndex);
    }

    // Coroutine that randomly changes tips on the loading screen
    IEnumerator ShowRandomTips()
    {
        while (LoadingScreenObject.activeSelf)
        {
            // Select a random tip from the array
            TipText.text = tips[Random.Range(0, tips.Length)];

            // Wait 1 to 2 seconds before changing to the next tip
            yield return new WaitForSeconds(Random.Range(1f, 1.3f));
        }
    }
}



