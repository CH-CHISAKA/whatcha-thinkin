using UnityEngine;
using UnityEngine.UI; // Import this for Legacy UI Text

public class LoadingTextAnimator : MonoBehaviour
{
    public Text loadingText; // Use UnityEngine.UI.Text instead of TextMeshProUGUI
    private string baseText = "Loading";
    private float dotTimer = 0f;
    private int dotCount = 0;

    void Update()
    {
        dotTimer += Time.deltaTime;

        if (dotTimer >= 0.3f)
        {
            dotCount = (dotCount + 1) % 4; // Cycle through 0 to 3 dots
            loadingText.text = baseText + new string('.', dotCount);
            dotTimer = 0f;
        }
    }
}