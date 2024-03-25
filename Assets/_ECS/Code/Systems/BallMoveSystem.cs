using Leopotam.Ecs;
using UnityEngine;

public class BallMoveSystem : IEcsRunSystem
{
    private EcsFilter<Ball> _filter;
    private readonly Collider2D[] _hits = new Collider2D[3];
    private RuntimeData _runtimeData;

    public void Run()
    {
        foreach (var i in _filter)
        {
            ref var ball = ref _filter.Get1(i);


            CheckCollisions(ref ball);
            UpdatePosition(ref ball);
            CheckBorders(ref ball);
        }
    }

    private void CheckBorders(ref Ball ball)
    {
        Vector2 ballPos = ball.transform.position;

        var half = ball.collider.bounds.extents.x;
        
        if (ballPos.x - half < -_runtimeData.boardExtents.x)
        {
            ballPos.x = half -_runtimeData.boardExtents.x;
            ball.transform.position = ballPos;
            ball.velocity = Vector3.Reflect(ball.velocity, Vector2.right);
        }
        else if (ballPos.x + half > _runtimeData.boardExtents.x)
        {
            ballPos.x = _runtimeData.boardExtents.x - half;
            ball.transform.position = ballPos;
            ball.velocity = Vector3.Reflect(ball.velocity, Vector2.left);
        }
        
        
        if (ballPos.y + half > _runtimeData.boardExtents.y)
        {
            ballPos.y = _runtimeData.boardExtents.y - half;
            ball.transform.position = ballPos;
            ball.velocity = Vector3.Reflect(ball.velocity, Vector2.down);
        }
        else if (ballPos.y - half < -_runtimeData.boardExtents.y)
        {
            ballPos.y = -_runtimeData.boardExtents.y + half;
            ball.transform.position = ballPos;
            ball.velocity = Vector3.Reflect(ball.velocity, Vector2.up);
        }


        Debug.DrawLine(ballPos, (Vector3) ballPos + ball.velocity * 3, Color.white, 5);
    }


    private static void UpdatePosition(ref Ball ball)
    {
        var pos = ball.transform.position;
        pos += ball.velocity * (Time.deltaTime * 10);
        ball.velocity *= 0.99f;
        ball.transform.position = pos;
    }

    private void CheckCollisions(ref Ball ball)
    {
        var numCollider = Physics2D.OverlapCircleNonAlloc(ball.transform.position, ball.collider.radius, _hits);
        if (numCollider < 0) return;
        for (var i = 0;
            i < numCollider;
            i++)
        {
            var isPuddle = _hits[i].CompareTag("Puddle");
            if (isPuddle)
            {
                var puddleTr = _hits[i].transform;
                // ball.velocity = ball.transform.position - puddleTr.position;
                ball.velocity += _runtimeData.playerPuddleVelocity * 3;
                continue;
            }
        }
    }
}