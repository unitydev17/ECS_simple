using Leopotam.Ecs;
using UnityEngine;

public class EcsStartup : MonoBehaviour
{
    public StaticData configuration;
    public SceneData sceneData;

    private EcsWorld _ecsWorld;
    private EcsSystems _systems;
    private EcsSystems _fixedUpdateSystems;

    private void Start()
    {
        _ecsWorld = new EcsWorld();
        _systems = new EcsSystems(_ecsWorld);
        _fixedUpdateSystems = new EcsSystems(_ecsWorld);
        var runtimeData = new RuntimeData();

        _systems.Add(new PlayerInitSystem())
            .Add(new PlayerInputSystem())
            .Add(new BallSpawnSystem())
            .Add(new BallMoveSystem())
            .Inject(configuration)
            .Inject(sceneData)
            .Inject(runtimeData)
            .Init();

        _fixedUpdateSystems.Add(new PlayerMoveSystem())
            .Init();
    }

    private void Update()
    {
        _systems?.Run();
    }

    private void FixedUpdate()
    {
        _fixedUpdateSystems?.Run();
    }

    private void OnDestroy()
    {
        _systems?.Destroy();
        _systems = null;
        _fixedUpdateSystems?.Destroy();
        _fixedUpdateSystems = null;
        _ecsWorld?.Destroy();
        _ecsWorld = null;
    }
}