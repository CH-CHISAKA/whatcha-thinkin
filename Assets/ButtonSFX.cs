using UnityEngine;
using UnityEngine.UI;

public class ButtonSFX : MonoBehaviour
{
    private Button button;
    private AudioManager audioManager;

    void Start()
    {
        button = GetComponent<Button>();
        audioManager = FindFirstObjectByType<AudioManager>();


        // Play selection sound
        //audioManager?.PlaySFX(audioManager.buttonPress);

        if (button != null && audioManager != null)
        {
            button.onClick.AddListener(PlaySound);
        }
    }

    void PlaySound()
    {
        audioManager.PlaySFX(audioManager.buttonPress);
    }
}