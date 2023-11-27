using PathCreation;
using System.Collections;
using UnityEngine;

public class PathFolower : MonoBehaviour
{
    public bool IsFollowing {  get; private set; }

    [SerializeField] private float moveSpeed;
    [SerializeField] private float acceleration;
    
    private PathCreator pathCreator;
    private float distanceTraveled = 0;

    private Coroutine followRoutine;
    private Coroutine accelerateRoutine;

    public void SetPathCreator(PathCreator pathCreator)
    {
        this.pathCreator = pathCreator;
        transform.position = pathCreator.path.GetPointAtDistance(0);
    }

    public void SetSpeed(float moveSpeed)
    {
        this.moveSpeed = moveSpeed;
    }

    public void SetAcceleration(float acceleration)
    {
        this.acceleration = acceleration;
    }

    public void StartFolowing()
    {
        if (pathCreator == null) { Debug.LogWarning("Path creator is null"); return; }
        followRoutine = StartCoroutine(FollowPath());
        IsFollowing = true;
    }

    public void PauseFollowing()
    {
        StopCoroutine(followRoutine);
        IsFollowing = false;
    }

    public void StopFolowing() 
    {
        StopCoroutine(followRoutine);
        StopAccelerate();
        distanceTraveled = 0;
        IsFollowing = false;
    }

    public void StartAccelerate()
    {
        accelerateRoutine = StartCoroutine(Accelerate());
    }

    public void StopAccelerate()
    {
        StopCoroutine(accelerateRoutine);
    }

    private IEnumerator FollowPath()
    {
        while (true)
        {
            distanceTraveled += Time.deltaTime * moveSpeed;

            if (distanceTraveled > pathCreator.path.length) 
            {
                StopFolowing();
                break; 
            }

            transform.position = pathCreator.path.GetPointAtDistance(distanceTraveled);

            yield return null;
        }
    }

    private IEnumerator Accelerate()
    {
        while (true)
        {
             moveSpeed += acceleration * Time.deltaTime;

            yield return null;
        }
    }
}
