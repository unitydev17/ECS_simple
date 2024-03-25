using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputSystem : IEcsRunSystem
{
    private RuntimeData _runtimeData;
    private EcsFilter<PlayerInputData> _filter;
    private EcsWorld _ecsWorld;

    private Vector3 _currPos;

    public void Run()
    {
        foreach (var i in _filter)
        {
            ref var input = ref _filter.Get1(i);

#if UNITY_EDITOR
            input.moveInput = GetMousePos(ref input);
#else
            input.moveInput = GetTouchPos(ref input);
#endif
            // _runtimeData.spawnBall = Input.GetKeyDown(KeyCode.Space);
            // _runtimeData.runBall = Input.GetKeyDown(KeyCode.KeypadEnter);
        }
    }

    private static (bool, Vector3) GetMousePos(ref PlayerInputData input)
    {
        var prevPos = input.moveInput.Item2;
        
        var mouse = Mouse.current;
        if (mouse == null) return (false, prevPos);

        if (mouse.leftButton.IsPressed())
        {
            return (true, mouse.position.ReadValue());
        }

        return (false, prevPos);
    }

    private static (bool, Vector3) GetTouchPos(ref PlayerInputData input)
    {
        var prevPos = input.moveInput.Item2;
        
        var touch = Touchscreen.current;
        if (touch == null) return (false, prevPos);

        if (touch.touches.Count > 0)
        {
            return (true, touch.position.ReadValue());
        }

        return (false, prevPos);
    }
}