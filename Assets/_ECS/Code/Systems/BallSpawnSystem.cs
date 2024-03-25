using Leopotam.Ecs;
using UnityEngine;

public class BallSpawnSystem : IEcsRunSystem
{
    private RuntimeData _runtimeData;

    private StaticData _config;

    // private EcsFilter<Ball> _filter;
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

        _runtimeData.spawnBall = true;
        
        
        // foreach (var i in _filter)
        // {
        //     var ball = _filter.Get1(i);
        //     var ballGo = Object.Instantiate(_config.ballPrefab);
        //     ball.transform = ballGo.transform;
        //     ball.direction = Vector3.up;
        //     ball.collider = ballGo.GetComponent<SphereCollider>();
        // }
    }
}