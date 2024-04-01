using Leopotam.Ecs;
using UnityEngine;

public class BallMovePhysicsSystem : IEcsRunSystem
{
    private EcsFilter<Ball> _filter;
    private RuntimeData _runtimeData;
    private StaticData _staticData;

    public void Run()
    {
        foreach (var i in _filter)
        {
            ref var ball = ref _filter.Get1(i);

            RestrictVelocity(ref ball);
            CheckBorders(ref ball);
            CheckStopGame(ref ball);
        }
    }

    private void CheckStopGame(ref Ball ball)
    {
        if (_runtimeData.goal) ball.rigidBody.velocity = Vector2.zero;
    }

    private void RestrictVelocity(ref Ball ball)
    {
        var velocity = ball.rigidBody.velocity;
        if (velocity.magnitude > _staticData.maxBallVelocity)
        {
            ball.rigidBody.velocity = velocity.normalized * _staticData.maxBallVelocity;
        }
    }


    private void CheckBorders(ref Ball ball)
    {
        Vector2 ballPos = ball.transform.position;
        

        if (ballPos.x - ball.half < -_runtimeData.boardExtents.x)
        {
            ballPos.x = ball.half - _runtimeData.boardExtents.x;
            ball.rigidBody.MovePositionForced(ballPos);
        }
        else if (ballPos.x + ball.half > _runtimeData.boardExtents.x)
        {
            ballPos.x = _runtimeData.boardExtents.x - ball.half;
            ball.rigidBody.MovePositionForced(ballPos);
        }

        if (ballPos.y + ball.half > _runtimeData.boardExtents.y)
        {
            ballPos.y = _runtimeData.boardExtents.y - ball.half;
            ball.rigidBody.MovePositionForced(ballPos);
        }
        else if (ballPos.y - ball.half < -_runtimeData.boardExtents.y)
        {
            ballPos.y = -_runtimeData.boardExtents.y + ball.half;
            ball.rigidBody.MovePositionForced(ballPos);
        }
    }
}