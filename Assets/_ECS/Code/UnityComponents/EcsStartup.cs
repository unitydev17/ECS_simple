using Leopotam.Ecs;
using UnityEngine;

public class EcsStartup : MonoBehaviour
{
    public StaticData configuration;
    public SceneData sceneData;
    public new Camera camera;

    private EcsWorld _ecsWorld;
    private EcsSystems _systems;

    private void Start()
    {
        Application.targetFrameRate = 60;


        _ecsWorld = new EcsWorld();
        _systems = new EcsSystems(_ecsWorld);
        var runtimeData = new RuntimeData();

        _systems.Add(new PlayerInitSystem())
            .Add(new PlayerInputSystem())
            .Add(new BallSpawnSystem())
            .Add(new BallMoveSystem())
            .Add(new PlayerMoveSystem())
            .Inject(configuration)
            .Inject(sceneData)
            .Inject(camera)
            .Inject(runtimeData)
            .Init();

        runtimeData.boardExtents = CountBoardExtents();
    }

    private Vector2 CountBoardExtents()
    {
        var topRightPos = camera.ViewportToWorldPoint(new Vector2(1, 1));
        return new Vector2(topRightPos.x, topRightPos.y);
    }

    private void Update()
    {
        _systems?.Run();
    }

    private void OnDestroy()
    {
        _systems?.Destroy();
        _systems = null;
        _ecsWorld?.Destroy();
        _ecsWorld = null;
    }
}