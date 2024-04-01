using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.InputSystem;

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