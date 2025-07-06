using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class CancelButtonManager : MonoBehaviour
{
    [Header("UI Components")]
    public Button cancelButton;            // Reference to Cancel button

    [Header("Sprites")]
    public Sprite cancelSprite;            // Sprite to show on Cancel button

    [Header("Settings")]
    public float cancelDelay = 1f;         // Delay before scene change

    void Start()
    {
        if (cancelButton == null)
        {
            Debug.LogError("CancelButtonManager: Cancel Button not assigned!");
            return;
        }

        cancelButton.onClick.AddListener(OnCancelPressed);

        // Set sprite if assigned
        if (cancelSprite != null && cancelButton.image != null)
        {
            cancelButton.image.sprite = cancelSprite;
        }

        ForceButtonVisible(cancelButton);
    }

    private void OnCancelPressed()
    {
        Debug.Log("CancelButtonManager: Cancel pressed, returning to Main Menu after delay...");
        StartCoroutine(LoadMainMenuAfterDelay(cancelDelay));
    }

    private IEnumerator LoadMainMenuAfterDelay(float delay)
    {
        Time.timeScale = 1f;  // Ensure timeScale is reset before scene load
        yield return new WaitForSecondsRealtime(delay);
        SceneManager.LoadScene(2);  // Load MainMenuScene by index
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

        Transform parent = btn.transform;
        while (parent != null)
        {
            parent.gameObject.SetActive(true);
            parent = parent.parent;
        }
    }

    void OnDestroy()
    {
        if (cancelButton != null)
        {
            cancelButton.onClick.RemoveListener(OnCancelPressed);
        }

        Time.timeScale = 1f;  // Reset time on destroy
    }
}
