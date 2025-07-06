////using TMPro;
////using UnityEngine;
////using UnityEngine.UI;

////public class AnswerData : MonoBehaviour
////{

////    #region Variables

////    [Header("UI Elements")]
////    [SerializeField] TextMeshProUGUI infoTextObject = null;
////    [SerializeField] Image toggle = null;

////    [Header("Textures")]
////    [SerializeField] Sprite uncheckedToggle = null;
////    [SerializeField] Sprite checkedToggle = null;

////    [Header("References")]
////    [SerializeField] GameEvents events = null;

////    private RectTransform _rect = null;
////    public RectTransform Rect
////    {
////        get
////        {
////            if (_rect == null)
////            {
////                _rect = GetComponent<RectTransform>() ?? gameObject.AddComponent<RectTransform>();
////            }
////            return _rect;
////        }
////    }

////    private int _answerIndex = -1;
////    public int AnswerIndex { get { return _answerIndex; } }

////    private bool Checked = false;

////    #endregion

////    /// <summary>
////    /// Function that is called to update the answer data.
////    /// </summary>
////    public void UpdateData(string info, int index)
////    {
////        infoTextObject.text = info;
////        _answerIndex = index;
////    }
////    /// <summary>
////    /// Function that is called to reset values back to default.
////    /// </summary>
////    public void Reset()
////    {
////        Checked = false;
////        UpdateUI();
////    }
////    /// <summary>
////    /// Function that is called to switch the state.
////    /// </summary>
////    public void SwitchState()
////    {
////        Checked = !Checked;
////        UpdateUI();

////        if (events.UpdateQuestionAnswer != null)
////        {
////            events.UpdateQuestionAnswer(this);
////        }
////    }
////    /// <summary>
////    /// Function that is called to update UI.
////    /// </summary>
////    void UpdateUI()
////    {
////        if (toggle == null) return;

////        toggle.sprite = (Checked) ? checkedToggle : uncheckedToggle;
////    }
////}



//using TMPro;
//using UnityEngine;
//using UnityEngine.UI;

///// <summary>
///// Handles individual answer UI behavior, including selection state, UI updates,
///// and communication with external game event systems.
///// </summary>
//public class AnswerData : MonoBehaviour
//{
//    #region Variables

//    [Header("UI Elements")]
//    [Tooltip("Reference to the UI text element displaying the answer info.")]
//    [SerializeField] private TextMeshProUGUI infoTextObject = null;

//    [Tooltip("UI Image component representing the toggle checkbox.")]
//    [SerializeField] private Image toggle = null;

//    [Header("Textures")]
//    [Tooltip("Sprite used when the toggle is in the unchecked state.")]
//    [SerializeField] private Sprite uncheckedToggle = null;

//    [Tooltip("Sprite used when the toggle is in the checked state.")]
//    [SerializeField] private Sprite checkedToggle = null;

//    [Header("References")]
//    [Tooltip("Reference to the GameEvents system to trigger callbacks.")]
//    [SerializeField] private GameEvents events = null;

//    // Cached reference to this object's RectTransform
//    private RectTransform _rect = null;

//    /// <summary>
//    /// Public property to access the RectTransform component. If not found,
//    /// attempts to retrieve or add one dynamically.
//    /// </summary>
//    public RectTransform Rect
//    {
//        get
//        {
//            if (_rect == null)
//            {
//                _rect = GetComponent<RectTransform>() ?? gameObject.AddComponent<RectTransform>();
//            }
//            return _rect;
//        }
//    }

//    // The index of this answer in the answer list (e.g. 0, 1, 2...)
//    private int _answerIndex = -1;

//    /// <summary>
//    /// Public getter for the assigned answer index.
//    /// </summary>
//    public int AnswerIndex => _answerIndex;

//    // Internal flag to track whether this answer is currently selected.
//    private bool Checked = false;

//    #endregion

//    #region Public Methods

//    /// <summary>
//    /// Initializes the UI text and stores the associated index of this answer.
//    /// </summary>
//    /// <param name="info">Answer text to be displayed.</param>
//    /// <param name="index">The index of this answer in the list.</param>
//    public void UpdateData(string info, int index)
//    {
//        infoTextObject.text = info;
//        _answerIndex = index;
//    }

//    /// <summary>
//    /// Resets the selection state and updates the visual toggle accordingly.
//    /// </summary>
//    public void Reset()
//    {
//        Checked = false;
//        UpdateUI();
//    }

//    /// <summary>
//    /// Toggles the selection state of the answer, updates its visual representation,
//    /// and notifies any external listeners (e.g. quiz manager) of the change.
//    /// </summary>
//    public void SwitchState()
//    {
//        // Toggle the current state (true becomes false, false becomes true)
//        Checked = !Checked;

//        // Update the visual feedback on the toggle (checkbox icon)
//        UpdateUI();

//        // If any method is subscribed to UpdateQuestionAnswer, invoke it
//        if (events.UpdateQuestionAnswer != null)
//        {
//            // Notify the external system about this answer's new state.
//            // This allows other systems (e.g. quiz manager) to validate selections or progress.
//            events.UpdateQuestionAnswer(this);
//        }
//    }


//    #endregion

//    #region Private Methods

//    /// <summary>
//    /// Updates the sprite of the toggle image based on the current Checked state.
//    /// Shows checkedToggle if selected, uncheckedToggle if not.
//    /// </summary>
//    private void UpdateUI()
//    {
//        if (toggle == null) return;

//        toggle.sprite = Checked ? checkedToggle : uncheckedToggle;
//    }

//    #endregion
//}
