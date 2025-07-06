using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a single answer option in a quiz question.
/// </summary>
[Serializable]
public struct Answer
{
    [SerializeField] private string _info;       // The text of the answer (e.g., "yes", "no", "maybe", "sometimes")
    public string Info => _info;

    [SerializeField] private bool _isCorrect;    // Whether this answer is correct
    public bool IsCorrect => _isCorrect;
}

/// <summary>
/// ScriptableObject used to define a quiz question in Unity.
/// Can be created from the Unity Editor.
/// </summary>
[CreateAssetMenu(fileName = "New Question", menuName = "Quiz/New Question")]
public class Question : ScriptableObject
{
    public enum AnswerType { Single } // Only single answer type supported based on requirements

    [SerializeField] private string _info = string.Empty; // The question text
    public string Info => _info;

    [SerializeField] private Answer[] _answers = null;    // List of possible answers
    public Answer[] Answers => _answers;

    [SerializeField] private bool _useTimer = true;       // Whether to use a timer
    public bool UseTimer => _useTimer;

    [SerializeField] private int _timer = 5;              // Timer duration in seconds
    public int Timer => _timer;

    [SerializeField] private AnswerType _answerType = AnswerType.Single;  // Always Single
    public AnswerType GetAnswerType => _answerType;

    [SerializeField] private int _addScore = 10;          // Score added for correct answer
    public int AddScore => _addScore;

    /// <summary>
    /// Returns a list of indexes for all correct answers.
    /// For single-choice, this will have only one index.
    /// </summary>
    public List<int> GetCorrectAnswers()
    {
        List<int> correctAnswers = new List<int>();

        for (int i = 0; i < _answers.Length; i++)
        {
            if (_answers[i].IsCorrect)
            {
                correctAnswers.Add(i);
            }
        }

        return correctAnswers;
    }
}
