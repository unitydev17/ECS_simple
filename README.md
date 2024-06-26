# ECS simple test game

### Тестовое приложение с использованием ECS архитектуры и фреймворка [LEO-ECS](https://github.com/Leopotam/ecs)

![Ecs game](/Assets/_ECS/Images/readme/ecs_game.png) ![Ecs game](/Assets/_ECS/Images/readme/ecs_game2.png)

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
            .Inject(_runtimeData)
            .Inject(sceneData.ui)
            .Init();
    }
```




## PlayerInitSystem.cs

Пример системы инициализации - инициализация игрока. Выполняется один раз при старте игры

```C#
public class PlayerInitSystem : IEcsInitSystem
{
    private EcsWorld _ecsWorld;
```
статические данные StaticData - пример инъекции в систему. Это Scriptable object с общей конфигурацией игры
```C#
    private StaticData _staticData;
    private SceneData _sceneData;

    public void Init()
    {
        var playerEntity = _ecsWorld.NewEntity();
        ref var player = ref playerEntity.Get<Player>();

```
в сущность playerEntity добавляется компонент PlayerInputData, в дальнейшем этот компонент будет использоваться для передачи данных пользовательского ввода в систему передвижения биты игрока
```C#
        playerEntity.Get<PlayerInputData>();

        var playerGo = Object.Instantiate(_staticData.puddlePrefab);
        playerGo.transform.position = _sceneData.puddleSpawnPoint.position;

        player.transform = playerGo.transform;
        player.rigidBody = playerGo.GetComponent<Rigidbody2D>();

    }
}
```



## PlayerInputData.cs
Компонент, который содержит в себе данные о вводе пользователя, используются для передвижения биты, bool значение указывает есть ли вообще движение
```C#
public struct PlayerInputData
{
    public (bool, Vector3) moveInput;
}
```



## PlayerInputSystem.cs
Пример системы выполнения - считывание данных ввода игрока в Update (мышь для unity editor, Touch для android)
```C#
public class PlayerInputSystem : IEcsRunSystem
{
    private RuntimeData _runtimeData;
```
фильтр EcsFilter<PlayerInputData> позволяет получить ссылки на сущности, которые содержат в себе компоненты PlayerInputData
```C#
    private EcsFilter<PlayerInputData> _filter;
    private EcsWorld _ecsWorld;
    private StaticData _staticData;

    private Vector3 _currPos;

    public void Run()
    {
        if (_runtimeData.goal) return;
```
пробегаем по данным фильтра и достаем необходимые для этой системе компоненты, в данном случае - пользовательский ввод (PlayerInputData) и заполняем его данными (mouse, touch)
```C#
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



## PlayerMovetData.cs
Система управления передвижением биты на основании PlayerInputData компонента
```C#

public class PlayerMoveSystem : IEcsRunSystem
{
    private EcsFilter<Player, PlayerInputData> _filter;
    private RuntimeData _runtimeData;
    private Camera _camera;

    public void Run()
    {
```
игнорируем ввод пользователя если был забит гол
```C#
        if (_runtimeData.goal) return;

        foreach (var i in _filter)
        {
            ref var player = ref _filter.Get1(i);
            ref var input = ref _filter.Get2(i);

            PuddleMove(player, input);
        }
    }

    private void PuddleMove(Player player, PlayerInputData input)
    {
        var (tap, deltaMove) = input.moveInput;
        if (!tap) return;

        var currPos = player.transform.position;
        var targetPos = currPos + deltaMove * Time.deltaTime;
        var nextPos = BoundPosition(currPos, targetPos);

        player.rigidBody.MovePosition(nextPos);
    }

    private Vector3 BoundPosition(Vector3 currPos, Vector3 nextPos)
    {
        if (nextPos.x < -_runtimeData.boardExtents.x || nextPos.x > _runtimeData.boardExtents.x)
        {
            nextPos.x = currPos.x;
        }

        if (nextPos.y < -_runtimeData.boardExtents.y || nextPos.y > 0)
        {
            nextPos.y = currPos.y;
        }

        return nextPos;
    }
}
```


