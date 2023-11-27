using UnityEngine.Events;
using UnityEngine;

[System.Serializable]
public class SequenceStep
{
    public float delayBeforeConditions;
    public BoxCollider requiredCollider;
    public float delayAfterConditions;
    public UnityEvent OnExecuted;
}