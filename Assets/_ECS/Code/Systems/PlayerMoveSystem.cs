using Leopotam.Ecs;
using UnityEngine;

public class PlayerMoveSystem : IEcsRunSystem
{
    private const string WallTag = "Wall";
    private EcsFilter<Player, PlayerInputData> _filter;

    private readonly RaycastHit[] _hits = new RaycastHit[3];

    private float _velocity;

    public void Run()
    {
        foreach (var i in _filter)
        {
            ref var player = ref _filter.Get1(i);
            ref var input = ref _filter.Get2(i);


            PuddleMove(input, player);
        }
    }

    private void PuddleMove(PlayerInputData input, Player player)
    {
        if (Mathf.Abs(input.moveInput.x) > 0)
        {
            _velocity += input.moveInput.x * 0.1f;
        }

        if (Mathf.Abs(_velocity) < 0.01f)
        {
            _velocity = 0;
        }
        else
        {
            _velocity *= 0.8f;
        }

        // Debug.Log(_velocity);


        var newPos = player.transform.position;
        newPos.x += _velocity;

        if (CheckAllowedMove(player, newPos))
        {
            player.transform.position = newPos;
        }
        else
        {
            _velocity *= 0.5f;
        }
    }

    private bool CheckAllowedMove(Player player, Vector3 pos)
    {
        var height = player.collider.height;
        var p1 = pos + Vector3.left * (height * 0.5f);
        var p2 = p1 + Vector3.right * height;

        var radius = player.collider.radius;
        var c1 = p1 + Vector3.right * radius;
        var c2 = p2 - Vector3.right * radius;

        return CheckCollision(Vector3.zero);

        bool CheckCollision(Vector3 direction)
        {
            var hitNumber = Physics.CapsuleCastNonAlloc(c1, c2, radius, Vector3.left, _hits, 0.02f);
            if (hitNumber <= 0) return true;
            for (var i = 0; i < hitNumber; i++)
            {
                if (_hits[i].collider.CompareTag(WallTag)) return false;
            }

            return true;
        }
    }
}