using System;
using UnityEngine;

#region Answer Option Enum
public enum AnswerOption
{
    Yes,
    No,
    Sometimes,
    Maybe
}
#endregion

#region Answer Struct
[Serializable]
public struct Answer
{
    [SerializeField] public AnswerOption _option;
    public AnswerOption Option => _option;

    [SerializeField] public string _info;
    public string Info => _info;
}
#endregion

#region Question ScriptableObject
[CreateAssetMenu(fileName = "New Question", menuName = "Quiz/New Question")]
public class Question : ScriptableObject
{
    [Header("Question Info")]
    [SerializeField] private string _info = string.Empty;
    public string Info => _info;

    [Header("Answer Options")]
    [SerializeField] public Answer[] _answers = new Answer[4];
    public Answer[] Answers => _answers;

    [Header("Correct Answer")]
    [SerializeField] public AnswerOption _correctOption;
    public AnswerOption CorrectOption => _correctOption;

    [Header("Timer Settings")]
    [SerializeField] private bool _useTimer = false;
    public bool UseTimer => _useTimer;

    [SerializeField] private int _timer = 0;
    public int Timer => _timer;

    [Header("Score Settings")]
    [SerializeField] private int _addScore = 10;
    public int AddScore => _addScore;

    /// <summary>
    /// Checks if the player's selected answer is correct.
    /// </summary>
    public bool IsCorrect(AnswerOption playerOption)
    {
        return playerOption == _correctOption;
    }

    /// <summary>
    /// Initialize default answers (Yes, No, Sometimes, Maybe).
    /// Call this in editor or setup script if you want to pre-fill.
    /// </summary>
    public void InitializeDefaultAnswers()
    {
        _answers = new Answer[]
        {
            new Answer { _option = AnswerOption.Yes, _info = "Yes" },
            new Answer { _option = AnswerOption.No, _info = "No" },
            new Answer { _option = AnswerOption.Sometimes, _info = "Sometimes" },
            new Answer { _option = AnswerOption.Maybe, _info = "Maybe" }
        };
    }
}
#endregion
