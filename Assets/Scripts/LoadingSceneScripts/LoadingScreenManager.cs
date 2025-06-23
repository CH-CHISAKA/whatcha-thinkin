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
        "Tip: Start with broad questions like “Is it alive?” before narrowing down!",
        "Tip: “Maybe” can be your secret weapon. Keep the AI guessing!",
        "Tip: The fewer questions you use, the higher your score!",
        "Tip: Use your emotes to flex your victory — or mock defeat!",
        "Tip: Stuck? Switch to a new category to refresh your thinking!",
        "Tip: Solo mode for practice. Multiplayer for party chaos!",
        "Tip: Keep your questions unpredictable — the AI is watching...",
        "Tip: Beat the AI with weird logic. It's not as smart as you think... yet.",
        "Tip: Master categories – “Mystery Mix” is for pros. Start with Food or Animals to warm up.",
        "Tip: Don’t rush your turns – The AI learns from you. Be clever, not quick.",
        "Tip:  Think like a hacker – Outsmart the Grid, one weird question at a time.",
        "Tip: Score synced. Climb the leaderboard and flex it later!",
        "Tip: Use fewer questions in early rounds to secure tiebreaker wins later.",
        "Tip: The AI’s tone subtly changes as it closes in on a correct guess. Listen closely.",
        "Tip: Sometimes “maybe” is the honest answer. Sometimes it’s psychological warfare.",
        "Tip: Your opponent's emotes might hint at their object… or be a bluff.",
        "Tip: Chain logic works. If it’s not alive, not electronic, not edible… it might just be a rock.",
        "Tip: Try baiting the AI into guessing early... it can backfire!",
        "Tip: Some players believe the 'Mystery Mix' is haunted code."
    };

    void Start()
    {
        // Start loading immediately
        StartCoroutine(LoadWithFakeProgress());
    }

    IEnumerator LoadWithFakeProgress()
    {
        LoadingScreenObject.SetActive(true);

        // Start showing random tips while loading
        StartCoroutine(ShowRandomTips());

        float fakeProgress = 0f;

        // Simulate gradual loading progress
        while (fakeProgress < 1f)
        {
            fakeProgress += Time.deltaTime * 0.2f; // Adjust speed as needed
            ProgressBar.value = Mathf.SmoothStep(0f, 1f, fakeProgress);
            yield return null;
        }

        // Optional: short pause at full bar for UX smoothness
        yield return new WaitForSeconds(0.3f);

        // Load the next scene
        SceneManager.LoadScene(TargetSceneIndex);
    }

    IEnumerator ShowRandomTips()
    {
        while (LoadingScreenObject.activeSelf)
        {
            // Randomly select a tip
            TipText.text = tips[Random.Range(0, tips.Length)];

            // Wait 3–5 seconds before changing the tip
            yield return new WaitForSeconds(Random.Range(1f, 2f));
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