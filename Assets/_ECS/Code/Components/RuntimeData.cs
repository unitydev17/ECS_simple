using System;
using System.Collections.Generic;
using UnityEngine;

public class RuntimeData
{
    public bool spawnBall;
    public Vector2 boardExtents;
    public bool goal;
    public bool restartRequired;
    public Vector2Int score;

    public readonly Queue<Delay> delays = new Queue<Delay>();

    public void DelayedCall(float delay, Action action)
    {
        delays.Enqueue(new Delay
        {
            delay = delay,
            delayedAction = action
        });
    }
}