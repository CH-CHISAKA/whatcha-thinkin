//using UnityEngine;
//using System.Collections;
//using UnityEngine.UI;
//using UnityEngine.SceneManagement;

//public class LoadingScreenManager : MonoBehaviour
//{
//    // Singleton instance so other scripts can easily access this manager
//    public static LoadingScreenManager Instance;

//    [Header("Loading UI")]
//    public GameObject m_LoadingScreenObject; // UI object that contains the loading screen
//    public Slider ProgressBar; // UI slider used to display loading progress

//    // Ensure only one instance of the manager exists (Singleton Pattern)
//    private void Awake()
//    {
//        if (Instance != null && Instance != this)
//        {
//            // If another instance already exists, destroy this one
//            Destroy(this.gameObject);
//        }
//        else
//        {
//            // Set this as the instance and prevent it from being destroyed on scene load
//            Instance = this;
//            DontDestroyOnLoad(this.gameObject);
//        }
//    }

//    // Public method to start switching to a new scene by scene index
//    public void SwitchToScene(int sceneId)
//    {
//        m_LoadingScreenObject.SetActive(true); // Show the loading screen
//        ProgressBar.value = 0; // Reset progress bar to 0
//        StartCoroutine(LoadSceneAsync(sceneId)); // Start asynchronous loading
//    }

//    // Coroutine to handle async loading of a scene
//    private IEnumerator LoadSceneAsync(int sceneId)
//    {
//        // Begin loading the scene asynchronously
//        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);
//        operation.allowSceneActivation = false; // Prevent the scene from activating immediately

//        // Loop until the scene is fully loaded
//        while (!operation.isDone)
//        {
//            // Scene loading progress ranges from 0 to 0.9, so normalize it to 0–1
//            float progress = Mathf.Clamp01(operation.progress / 0.9f);
//            ProgressBar.value = progress; // Update the progress bar UI

//            // When loading is complete (operation.progress >= 0.9), activate the scene
//            if (operation.progress >= 0.9f)
//            {
//                // Optional: add a short delay or wait for player input here
//                yield return new WaitForSeconds(1f);
//                operation.allowSceneActivation = true;
//            }

//            yield return null; // Wait for the next frame
//        }

//        // Hide the loading screen once the scene has loaded
//        m_LoadingScreenObject.SetActive(false);
//    }
//}



//using System.Collections; // Needed for IEnumerator
//using System.Collections.Generic;
//using UnityEngine;                // Core Unity engine namespace
//using UnityEngine.UI;            // Needed to access UI components like Slider
//using UnityEngine.SceneManagement; // Required for scene loading


//public class LoadingScreenManager : MonoBehaviour
//{
//    // Reference to the loading panel UI (used to show/hide during loading)
//    public GameObject LoadingScreenObject;

//    // Reference to the UI slider that visually shows loading progress
//    public Slider ProgressBar;

//    // Public method that initiates loading of a scene by index
//    public void LoadLevel(int sceneIndex)
//    {
//        // Start the coroutine that loads the scene asynchronously
//        StartCoroutine(LoadAsynchronously(sceneIndex));
//    }

//    // Coroutine that handles loading the scene in the background
//    IEnumerator LoadAsynchronously(int sceneIndex)
//    {
//        // Begin loading the scene asynchronously and get the operation handle
//        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

//        // Show the loading UI panel while the scene loads
//        LoadingScreenObject.SetActive(true);

//        // Keep looping until the scene has finished loading
//        while (!operation.isDone)
//        {
//            // Unity loads scenes up to 90% before activation; progress is in [0, 0.9]
//            // We normalize it to [0,1] for slider display
//            float progress = Mathf.Clamp01(operation.progress / 0.9f);

//            // Update the slider value to show loading progress
//            ProgressBar.value = progress;

//            // Wait until the next frame before continuing the loop
//            yield return null;
//        }
//    }
//}



//using System.Collections;
//using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.SceneManagement;

//public class LoadingScreenManager : MonoBehaviour
//{
//    public GameObject LoadingScreenObject;
//    public Slider ProgressBar;

//    // Target scene index to load after simulation
//    public int TargetSceneIndex = 1; // Set this in Inspector or hardcode

//    void Start()
//    {
//        // Start loading immediately when scene starts
//        StartCoroutine(LoadWithFakeProgress());
//    }

//    IEnumerator LoadWithFakeProgress()
//    {
//        LoadingScreenObject.SetActive(true);

//        float fakeProgress = 0f;

//        // Simulate gradual loading
//        while (fakeProgress < 1f)
//        {
//            fakeProgress += Time.deltaTime * 0.2f; // Adjust speed here
//            ProgressBar.value = Mathf.SmoothStep(0f, 1f, fakeProgress);
//            yield return null;
//        }

//        // Optional: wait 0.5s after bar is full for UX polish
//        yield return new WaitForSeconds(0.3f);

//        // Load the next scene
//        SceneManager.LoadScene(TargetSceneIndex);
//    }
//}


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



//using System.Collections;
//using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.SceneManagement;

//public class LoadingScreenManager : MonoBehaviour
//{
//    [Header("Loading Screen UI")]
//    public GameObject LoadingScreenObject;
//    public Slider ProgressBar;
//    public TMPro.TMP_Text TipText;

//    private readonly string[] tips = {
//        "Tip: Start with broad questions like “Is it alive?” before narrowing down!",
//        // ... rest of your tips ...
//        "Tip: Some players believe the 'Mystery Mix' is haunted code."
//    };

//    void Start()
//    {
//        StartCoroutine(LoadWithFakeProgress());
//    }

//    IEnumerator LoadWithFakeProgress()
//    {
//        LoadingScreenObject.SetActive(true);
//        StartCoroutine(ShowRandomTips());

//        float fakeProgress = 0f;

//        while (fakeProgress < 1f)
//        {
//            fakeProgress += Time.deltaTime * 0.2f;
//            ProgressBar.value = Mathf.SmoothStep(0f, 1f, fakeProgress);
//            yield return null;
//        }

//        yield return new WaitForSeconds(0.3f);

//        // Load target scene by name
//        string nextScene = GameSceneManager.NextSceneToLoad;

//        if (!string.IsNullOrEmpty(nextScene))
//        {
//            SceneManager.LoadScene(nextScene);
//        }
//        else
//        {
//            Debug.LogError("Next scene to load is not set!");
//        }
//    }

//    IEnumerator ShowRandomTips()
//    {
//        while (LoadingScreenObject.activeSelf)
//        {
//            TipText.text = tips[Random.Range(0, tips.Length)];
//            yield return new WaitForSeconds(Random.Range(2f, 3f));
//        }
//    }
//}