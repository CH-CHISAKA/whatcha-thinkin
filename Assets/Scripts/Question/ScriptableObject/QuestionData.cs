// Updated code


using UnityEngine; // Import the UnityEngine namespace which contains core Unity functionality

// This attribute allows you to create instances of this ScriptableObject directly from the Unity Editor's "Create" menu.
// - fileName specifies the default file name when creating a new asset.
// - menuName defines the path where this asset option will appear in the Create menu.
// - order sets the position in the Create menu (lower means higher up).
[CreateAssetMenu(fileName = "AnimalQuestion", menuName = "ScriptableObjects/AnimalQuestion", order = 1)]
public class QuestionData : ScriptableObject
{
    // The actual question text that will be shown to the player.
    public string question;

    // Category to which this question belongs, e.g., "Science", "History".
    // Useful for filtering or grouping questions by topic.
    public string category;

    // Array holding all possible answer choices for this question.
    // Tooltip attribute shows helpful text in the Unity Inspector when hovering over this field.
    [Tooltip("List all possible answers")]
    public string[] answers;

    // The index of the correct answer inside the 'answers' array.
    // For example, if the correct answer is the first element, this would be 0.
    // Tooltip helps to clarify this in the Unity Inspector.
    [Tooltip("Index of the correct answer in the answers array")]
    public int correctAnswerIndex;
}
