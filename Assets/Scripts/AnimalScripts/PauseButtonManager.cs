using UnityEngine;
using UnityEngine.UI;

public class PauseButtonManager : MonoBehaviour
{
    [Header("UI Components")]
    public Button pauseButton;              // Reference to the UI button
    public GameObject pausePanel;           // Optional UI panel shown when paused

    [Header("Sprites")]
    public Sprite pauseSprite;              // Icon shown when game is running
    public Sprite resumeSprite;             // Icon shown when game is paused

    [Header("Settings")]
    public bool muteAudioOnPause = false;   // Optionally mute all audio on pause

    private bool isPaused = false;
    private AudioSource[] allAudioSources;

    void Start()
    {
        if (pauseButton == null)
        {
            Debug.LogError("PauseButtonManager: Pause Button is not assigned.");
            return;
        }

        // Add pause toggle listener
        pauseButton.onClick.AddListener(TogglePause);

        // Set default sprite and ensure button is active
        if (pauseSprite != null && pauseButton.image != null)
        {
            pauseButton.image.sprite = pauseSprite;
        }

        // Ensure pause button is always visible
        ForceButtonVisible(pauseButton);

        // Hide pause panel at start
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }
    }

    void Update()
    {
        // Auto-recover if something disables the pause button
        if (pauseButton != null && !pauseButton.gameObject.activeInHierarchy)
        {
            Debug.LogWarning("PauseButtonManager: Pause button was deactivated. Re-enabling...");
            ForceButtonVisible(pauseButton);
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        // Pause or resume game time
        Time.timeScale = isPaused ? 0f : 1f;

        // Toggle audio if enabled
        if (muteAudioOnPause)
        {
            if (isPaused)
                MuteAllAudio();
            else
                UnmuteAllAudio();
        }

        // Show or hide pause panel
        if (pausePanel != null)
        {
            pausePanel.SetActive(isPaused);
        }

        // Update button sprite
        if (pauseButton.image != null && pauseSprite != null && resumeSprite != null)
        {
            pauseButton.image.sprite = isPaused ? resumeSprite : pauseSprite;
        }
    }

    private void MuteAllAudio()
    {
        allAudioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audio in allAudioSources)
        {
            audio.Pause();
        }
    }

    private void UnmuteAllAudio()
    {
        if (allAudioSources != null)
        {
            foreach (AudioSource audio in allAudioSources)
            {
                audio.UnPause();
            }
        }
    }

    private void ForceButtonVisible(Button btn)
    {
        btn.gameObject.SetActive(true);

        if (btn.image != null)
        {
            btn.image.enabled = true;
        }

        CanvasGroup cg = btn.GetComponentInParent<CanvasGroup>();
        if (cg != null)
        {
            cg.alpha = 1f;
            cg.interactable = true;
            cg.blocksRaycasts = true;
        }

        // Ensure parent objects are also enabled
        Transform parent = btn.transform;
        while (parent != null)
        {
            parent.gameObject.SetActive(true);
            parent = parent.parent;
        }
    }

    void OnDestroy()
    {
        if (pauseButton != null)
        {
            pauseButton.onClick.RemoveListener(TogglePause);
        }

        // Always reset time scale when exiting
        Time.timeScale = 1f;
    }
}
