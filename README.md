# ECS simple test game

### Тестовое приложение с использованием ECS архитектуры и фреймворка [LEO-ECS](https://github.com/Leopotam/ecs)

![Onion architecture](/Assets/_Project/Images/readme/ecs_game.png)

## ECSStartup.cs

Содержит список систем игры

```C#
    private void Start()
    {
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
            .Add(new ViewPortSystem())
            .Add(new DelaySpawnSystem())
            .Add(new DelayActionSystem())
```
а также объектов для инъекции в эти же системы
```C#
            .Inject(configuration)
            .Inject(sceneData)
            .Inject(sceneData.camera)
            .Inject(_systems)
            .Inject(_runtimeData)
            .Inject(sceneData.ui)
            .Init();
    }
```




## PlayerInitSystem.cs

Пример системы инициализации - инициализация игрока. Выполняется один раз при старте игры.

```C#
public class PlayerInitSystem : IEcsInitSystem
{
    private EcsWorld _ecsWorld;
    private StaticData _staticData;
    private SceneData _sceneData;
    private RuntimeData _runtimeData;

    public void Init()
    {
        var playerEntity = _ecsWorld.NewEntity();
        ref var player = ref playerEntity.Get<Player>();
        playerEntity.Get<PlayerInputData>();

        var playerGo = Object.Instantiate(_staticData.puddlePrefab);
        playerGo.transform.position = _sceneData.puddleSpawnPoint.position;

        player.transform = playerGo.transform;
        player.rigidBody = playerGo.GetComponent<Rigidbody2D>();

    }
}
```

## PlayerInitSystem.cs
Пример системы выполнения - считывание данных ввода игрока (мышь для unity editor, Touch для android)
```C#
public class PlayerInputSystem : IEcsRunSystem
{
    private RuntimeData _runtimeData;
    private EcsFilter<PlayerInputData> _filter;
    private EcsWorld _ecsWorld;
    private StaticData _staticData;

    private Vector3 _currPos;

    public void Run()
    {
        if (_runtimeData.goal) return;

        foreach (var i in _filter)
        {
            ref var input = ref _filter.Get1(i);

#if UNITY_EDITOR
            input.moveInput = GetMousePos();
#else
            input.moveInput = GetTouchPos();
#endif
        }
    }

    private (bool, Vector3) GetMousePos()
    {
        var mouse = Mouse.current;
        if (mouse == null || !mouse.leftButton.IsPressed()) return (false, Vector2.zero);
        return (true, mouse.delta.ReadValue() * _staticData.mouseSpeedMagnifier);
    }

    private static (bool, Vector3) GetTouchPos()
    {
        var touch = Touchscreen.current;
        return touch == null || touch.touches.Count == 0 ? (false, Vector2.zero) : (true, touch.delta.ReadValue());
    }
}
```
