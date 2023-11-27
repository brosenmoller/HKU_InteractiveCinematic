using UnityEngine;
using System.Collections;

public class Sequence : MonoBehaviour
{
    [SerializeField] private bool playOnAwake = false;
    public SequenceStep[] sequenceSteps;

    private Transform player;

    private void Awake()
    {
        player = FindObjectOfType<PlayerMovement>().transform;
        if (playOnAwake) { Play(); }
    }

    [ContextMenu("Play")]
    public void Play()
    {
        StartCoroutine(ExecuteSequence(this));
    }

    [ContextMenu("Stop")]
    public void Stop()
    {
        StopAllCoroutines();
    }

    public IEnumerator ExecuteSequence(Sequence sequence)
    {
        foreach (SequenceStep step in sequence.sequenceSteps)
        {
            yield return new WaitForSeconds(step.delayBeforeConditions);
            
            if (step.requiredCollider != null)
            {
                yield return new WaitUntil(() => step.requiredCollider.bounds.Contains(player.position));
            }

            yield return new WaitForSeconds(step.delayAfterConditions);

            step.OnExecuted?.Invoke();
        }
    }
}
