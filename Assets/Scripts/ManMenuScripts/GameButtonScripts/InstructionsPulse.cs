using UnityEngine;

/// <summary>
/// Applies a pulsing (scaling) animation to a GameObject to visually draw attention.
/// Common use: pulsing instructional text or icons in UI.
/// </summary>
public class InstructionsPulse : MonoBehaviour
{
    [Header("Pulse Settings")]

    // Maximum scale multiplier during pulse (1.1 = 10% larger).
    public float pulseScale = 1.1f;

    // Time it takes to complete one pulse (grow and shrink).
    public float pulseDuration = 0.8f;

    // Original scale of the GameObject.
    private Vector3 originalScale;

    void Start()
    {
        // Store the starting scale of the object.
        originalScale = transform.localScale;

        // Begin the pulsing coroutine.
        StartCoroutine(PulseRoutine());
    }

    /// <summary>
    /// Coroutine that smoothly pulses the GameObject by scaling it up and down over time.
    /// Repeats with a short delay between pulses.
    /// </summary>
    System.Collections.IEnumerator PulseRoutine()
    {
        while (true)
        {
            float t = 0f;

            // Animate pulse using a sine wave shape for smooth ease-in/ease-out effect.
            while (t < pulseDuration)
            {
                // Calculate scale factor using sine curve.
                float scale = Mathf.Lerp(1f, pulseScale, Mathf.Sin(Mathf.PI * t / pulseDuration));

                // Apply the scale relative to the original size.
                transform.localScale = originalScale * scale;

                // Increment time.
                t += Time.deltaTime;

                // Wait for the next frame.
                yield return null;
            }

            // Reset the scale to original after pulsing.
            transform.localScale = originalScale;

            // Wait before next pulse begins.
            yield return new WaitForSeconds(2f);
        }
    }
}
