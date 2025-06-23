using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Handles the shuffling animation of category buttons around an AI image in a circular layout.
/// Attach this script to the Shuffle Button GameObject and assign references in the Inspector.
/// </summary>
public class ShuffleButtonHandler : MonoBehaviour
{
    [Header("Assign your AI image transform here")]
    // Reference to the central rotating image (e.g., AI face or icon).
    public Transform aiImage;

    [Header("Assign ALL your category button RectTransforms here")]
    // List of UI category buttons to be shuffled around the AI.
    public List<RectTransform> categoryButtons;

    [Header("Radius for the rotation (pixels)")]
    // Radius of the circular layout.
    public float rotateRadius = 220f;

    [Header("Duration of the animation")]
    // Time in seconds to complete one shuffle animation.
    public float rotateDuration = 0.6f;

    // State flag to prevent multiple shuffles at once.
    private bool isRotating = false;

    /// <summary>
    /// This method should be hooked up to the Shuffle button's OnClick event in the Unity Inspector.
    /// </summary>
    public void ShuffleCategories()
    {
        if (!isRotating)
            StartCoroutine(RotateCategories());
    }

    /// <summary>
    /// Coroutine to animate the shuffle of category buttons and rotate the AI image.
    /// </summary>
    private IEnumerator<YieldInstruction> RotateCategories()
    {
        isRotating = true;

        // Step 1: Calculate randomized target positions in a circular pattern.
        float angleStep = 360f / categoryButtons.Count;
        List<Vector3> targetPositions = new List<Vector3>();

        // Apply a random offset to make the shuffle visually varied.
        float randomOffset = Random.Range(0f, 360f);

        for (int i = 0; i < categoryButtons.Count; i++)
        {
            float angle = Mathf.Deg2Rad * (angleStep * i + randomOffset);
            Vector3 pos = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * rotateRadius;
            targetPositions.Add(pos);
        }

        // Step 2: Cache current button positions and AI rotation for smooth interpolation.
        float t = 0f;
        List<Vector3> startPos = new List<Vector3>();
        foreach (RectTransform btn in categoryButtons)
            startPos.Add(btn.localPosition);


        // Step 3: Animate buttons and AI image over rotateDuration.
        while (t < rotateDuration)
        {
            float progress = t / rotateDuration;

            for (int i = 0; i < categoryButtons.Count; i++)
                categoryButtons[i].localPosition = Vector3.Lerp(startPos[i], targetPositions[i], progress);

            //aiImage.rotation = Quaternion.Lerp(aiStart, aiTarget, progress);

            t += Time.deltaTime;
            yield return null;
        }

        // Step 4: Snap to final values to ensure perfect alignment at the end.
        for (int i = 0; i < categoryButtons.Count; i++)
            categoryButtons[i].localPosition = targetPositions[i];

        //aiImage.rotation = aiTarget;

        isRotating = false;
    }
}
