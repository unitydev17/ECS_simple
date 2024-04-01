using UnityEngine;

[CreateAssetMenu]
public class StaticData : ScriptableObject
{
    public GameObject puddlePrefab;
    public GameObject ballPrefab;
    public float maxBallVelocity;
    public float botMoveVelocity;
    public double botAttackDistance;
    public float mouseSpeedMagnifier;
}