using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Adds a tap effect to a UI button by scaling it when pressed and smoothly returning on release.
/// Attach to any UI element with a Button or Image.
/// </summary>
public class ButtonTapEffect : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header("Tap Effect Settings")]

    // Target scale multiplier when the button is pressed (e.g., 0.92 = 92% of original).
    public float tapScale = 0.92f;

    // Time in seconds for the button to return to original scale after release.
    public float returnDuration = 0.08f;

    // Store the button's original scale.
    private Vector3 originalScale;

    // Tracks if the button is currently pressed.
    private bool isPressed = false;

    void Awake()
    {
        // Cache the initial scale of the button.
        originalScale = transform.localScale;
    }

    /// <summary>
    /// Triggered when the button is pressed down.
    /// </summary>
    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;

        // Instantly scale the button down.
        transform.localScale = originalScale * tapScale;
    }

    /// <summary>
    /// Triggered when the button is released.
    /// </summary>
    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;

        // Begin animation to return the button to its original scale.
        StartCoroutine(ReturnScale());
    }

    /// <summary>
    /// Coroutine that smoothly returns the button to its original size over a short duration.
    /// </summary>
    System.Collections.IEnumerator ReturnScale()
    {
        float t = 0f;
        Vector3 currentScale = transform.localScale;

        while (t < returnDuration)
        {
            // Interpolate the scale back to the original.
            transform.localScale = Vector3.Lerp(currentScale, originalScale, t / returnDuration);
            t += Time.deltaTime;
            yield return null;
        }

        // Snap exactly to the original scale at the end.
        transform.localScale = originalScale;
    }
}
