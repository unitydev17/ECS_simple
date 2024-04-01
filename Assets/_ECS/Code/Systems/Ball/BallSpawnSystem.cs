using Leopotam.Ecs;
using UnityEngine;

public class BallSpawnSystem : IEcsRunSystem
{
    private RuntimeData _runtimeData;

    private StaticData _config;

    private EcsWorld _ecsWorld;

    public void Run()
    {
        if (_runtimeData.spawnBall) return;

        var ballEntity = _ecsWorld.NewEntity();
        ref var ball = ref ballEntity.Get<Ball>();


        var ballGo = Object.Instantiate(_config.ballPrefab);
        ball.transform = ballGo.transform;
        ball.velocity = Vector3.zero;
        ball.collider = ballGo.GetComponent<CircleCollider2D>();
        ball.rigidBody = ballGo.GetComponent<Rigidbody2D>();
        ball.trailRenderer = ballGo.GetComponentInChildren<TrailRenderer>();
        ball.half = ball.collider.bounds.extents.x * 0.5f;

        _runtimeData.spawnBall = true;
    }
}