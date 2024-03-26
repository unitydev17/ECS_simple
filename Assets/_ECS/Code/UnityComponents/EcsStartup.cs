using Leopotam.Ecs;
using UnityEngine;

public class EcsStartup : MonoBehaviour
{
    public StaticData configuration;
    public SceneData sceneData;

    private EcsWorld _ecsWorld;
    private EcsSystems _systems;
    private RuntimeData _runtimeData;

    private void Start()
    {
        Application.targetFrameRate = 60;


        _ecsWorld = new EcsWorld();
        _systems = new EcsSystems(_ecsWorld);
        _runtimeData = new RuntimeData();

        _systems.Add(new PlayerInitSystem())
            .Add(new PlayerInputSystem())
            .Add(new PlayerMoveSystem())
            .Add(new BallSpawnSystem())
            .Add(new BallMovePhysicsSystem())
            .Add(new BotInitSystem())
            .Add(new BotMoveSystem())
            .Add(new GoalSystem())
            .Add(new RestartSystem())
            .Inject(configuration)
            .Inject(sceneData)
            .Inject(sceneData.camera)
            .Inject(_systems)
            .Inject(_runtimeData)
            .Inject(sceneData.ui)
            .Init();

        _runtimeData.boardExtents = CountBoardExtents();
        SetBoardColliders(_runtimeData.boardExtents);
    }

    private void SetBoardColliders(Vector2 extents)
    {
        sceneData.leftBorder.SetX(-extents.x);
        sceneData.rightBorder.SetX(extents.x);
        sceneData.topBorder.SetY(extents.y);
        sceneData.bottomBorder.SetY(-extents.y);
        
        sceneData.botGoal.SetY(extents.y);
        sceneData.playerGoal.SetY(-extents.y);
    }

    private Vector2 CountBoardExtents()
    {
        var topRightPos = sceneData.camera.ViewportToWorldPoint(new Vector2(1, 1));
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