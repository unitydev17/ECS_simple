using Leopotam.Ecs;
using UnityEngine;

public class EcsStartup : MonoBehaviour
{
    public StaticData configuration;
    public SceneData sceneData;
    public new Camera camera;

    public Transform _leftBorder;
    public Transform _rightBorder;
    public Transform _topBorder;
    public Transform _bottomBorder;

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
            .Add(new PlayerMoveByPhysicsSystem())
            .Add(new BallSpawnSystem())
            .Add(new BallMovePhysicsSystem())
            .Add(new BotInitSystem())
            .Add(new BotMoveSystem())
            .Inject(configuration)
            .Inject(sceneData)
            .Inject(camera)
            .Inject(runtimeData)
            .Init();

        runtimeData.boardExtents = CountBoardExtents();
        AlignBoardColliders(runtimeData.boardExtents);
    }

    private void AlignBoardColliders(Vector2 extents)
    {
        _leftBorder.SetX(-extents.x);
        _rightBorder.SetX(extents.x);
        _topBorder.SetY(extents.y);
        _bottomBorder.SetY(-extents.y);
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