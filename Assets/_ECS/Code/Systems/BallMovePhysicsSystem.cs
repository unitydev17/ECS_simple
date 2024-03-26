using Leopotam.Ecs;
using UnityEngine;

public class BallMovePhysicsSystem : IEcsRunSystem
{
    private EcsFilter<Ball> _filter;
    private readonly Collider2D[] _hits = new Collider2D[3];
    private RuntimeData _runtimeData;

    public void Run()
    {
        foreach (var i in _filter)
        {
            ref var ball = ref _filter.Get1(i);

            RestrictVelocity(ref ball);
            // CheckCollisions(ref ball);
            CheckBorders(ref ball);
        }
    }

    private static void RestrictVelocity(ref Ball ball)
    {
        var velocity = ball.rigidBody.velocity;
        if (velocity.magnitude > 50)
        {
            ball.rigidBody.velocity = velocity.normalized * 50;
        }
    }


    private void CheckBorders(ref Ball ball)
    {
        Vector2 ballPos = ball.transform.position;
        var half = 0;

        if (ballPos.x - half < -_runtimeData.boardExtents.x)
        {
            ballPos.x = half - _runtimeData.boardExtents.x;
            ball.rigidBody.MovePosition(ballPos);
        }
        else if (ballPos.x + half > _runtimeData.boardExtents.x)
        {
            ballPos.x = _runtimeData.boardExtents.x - half;
            ball.rigidBody.MovePosition(ballPos);
        }


        if (ballPos.y + half > _runtimeData.boardExtents.y)
        {
            ballPos.y = _runtimeData.boardExtents.y - half;
            ball.rigidBody.MovePosition(ballPos);
        }
        else if (ballPos.y - half < -_runtimeData.boardExtents.y)
        {
            ballPos.y = -_runtimeData.boardExtents.y + half;
            ball.rigidBody.MovePosition(ballPos);
        }
    }


    // private void CheckCollisions(ref Ball ball)
    // {
    //     Debug.DrawRay(ball.transform.position, ball.velocity, Color.cyan, 0.2f);
    //
    //     
    //     var numCollider = Physics2D.OverlapCircleNonAlloc(ball.transform.position, ball.collider.radius, _hits);
    //     if (numCollider < 0) return;
    //     for (var i = 0; i < numCollider; i++)
    //     {
    //         var isPuddle = _hits[i].CompareTag("Puddle");
    //         if (isPuddle)
    //         {
    //             
    //             
    //             // set to safe distance
    //             
    //             var direction = (ball.transform.position - _hits[i].transform.position).normalized;
    //             ball.transform.position = _hits[i].transform.position + direction * (ball.collider.bounds.extents.x + _hits[i].bounds.extents.x);
    //             // Debug.DrawRay(ball.transform.position, direction * 10, Color.red, 10);
    //             
    //
    //             // if (Vector3.Magnitude(ball.velocity) > Vector3.Magnitude(_runtimeData.playerPuddleVelocity))
    //             // {
    //                 // bounce from puddle
    //                 
    //                 // ball.velocity += Vector3.Reflect(ball.velocity, ball.transform.position - _hits[i].transform.position) * Vector3.Magnitude(_runtimeData.playerPuddleVelocity)*10;
    //                 //
    //                 // // ball.velocity += Vector3.Reflect(ball.velocity, ball.transform.position - _hits[i].transform.position) * Vector3.Magnitude(_runtimeData.playerPuddleVelocity)*10;
    //                 // Debug.DrawRay(ball.transform.position, ball.velocity * 10, Color.red, 10);
    //                 
    //             // }
    //             // else
    //             // {
    //             //     // hit ball with puddle
    //             //
    //             ball.velocity += _runtimeData.playerPuddleVelocity;
    //             //     
    //             // }
    //             
    //             
    //         }
    //     }
    // }
}