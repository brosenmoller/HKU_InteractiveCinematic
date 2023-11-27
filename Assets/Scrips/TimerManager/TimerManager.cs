using System;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    public static TimerManager Instance { get; private set; }
    public event Action<float> OnTimerUpdate;

    private void Awake()
    {
        Instance = this;
    }

    public void FixedUpdate()
    {
        OnTimerUpdate?.Invoke(Time.fixedDeltaTime);
    }
}
