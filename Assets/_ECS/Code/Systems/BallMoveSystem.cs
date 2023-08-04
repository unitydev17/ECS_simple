using Leopotam.Ecs;
using UnityEngine;

public class BallMoveSystem : IEcsRunSystem
{
    private EcsFilter<Ball> _filter;
    private readonly Collider[] _hits = new Collider[3];

    public void Run()
    {
        foreach (var i in _filter)
        {
            ref var ball = ref _filter.Get1(i);

            var pos = ball.transform.position;
            pos += ball.direction * (Time.deltaTime * 10);

            if (CanMove(ref ball, pos))
            {
                ball.transform.position = pos;
            }
        }
    }

    private bool CanMove(ref Ball ball, Vector3 pos)
    {
        var numCollider = Physics.OverlapSphereNonAlloc(pos, ball.collider.radius, _hits);
        if (numCollider <= 0) return true;

        for (var i = 0; i < numCollider; i++)
        {
            var isBallCollision = !_hits[i].gameObject.Equals(ball.transform.gameObject) && _hits[i].CompareTag("Ball");
            
            if (!(_hits[i].CompareTag("Wall") || _hits[i].CompareTag("Puddle") || isBallCollision)) continue;

            var position = ball.transform.position;
            var negVec = position - _hits[i].ClosestPoint(position);
            ball.direction = Vector3.Reflect(ball.direction, -negVec);
            // ball.direction = (ball.direction + 2 * negVec + Random.onUnitSphere * 0.01f).normalized;
            // ball.direction.y = 0;

         

            return false;
        }

        return true;
    }
}