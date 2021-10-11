﻿using System.Linq;
using Assets.Utils.Math;
using SystemBase;
using Systems.Climbable;
using UniRx;
using UnityEngine;
using Utils.Unity;

namespace Systems.Player
{
    [GameSystem]
    public class MovementSystem : GameSystem<PlayerComponent>
    {
        public override void Register(PlayerComponent component)
        {
            SystemUpdate(component)
                .Subscribe(UpdatePlayer)
                .AddTo(component);
            
            // RegisterCollisionChecks(component);
        }

        private void UpdatePlayer(PlayerComponent player)
        {
            var movement = Move(player);
            var needsReset = CheckForCollisions(player);
            ResetMovementIfNeeded(needsReset, movement, player);
        }

        private static void ResetMovementIfNeeded(bool needsReset, Vector2 movement, PlayerComponent player)
        {
            if (!needsReset) return;
            player.transform.position -= new Vector3(movement.x, movement.y);;
        }

        private static bool CheckForCollisions(PlayerComponent player)
        {
            var overlaps = Physics2D
                .OverlapBoxAll(player.transform.position.XY(), player.climbCollider.bounds.extents.XY(), 0.0f);
            
            if (overlaps.Any())
            {
                player.canClimb = overlaps.Any(ray => ray.GetComponent<ClimbableComponent>());
                player.isOnGround = overlaps.Any(ray => ray.gameObject.CompareTag("Ground"));
            }

            return !(player.canClimb || player.isOnGround);
        }

        private static Vector2 Move(PlayerComponent player)
        {
            var positionDelta = Vector2.zero;
            if ((player.isOnGround ||
                 player.canClimb) &&
                KeyCode.D.IsPressed())
            {
                positionDelta.x = 1;
            }
            else if ((player.isOnGround ||
                      player.canClimb) &&
                     KeyCode.A.IsPressed())
            {
                positionDelta.x = -1;
            }

            if (player.canClimb && 
                KeyCode.W.IsPressed())
            {
                positionDelta.y = 1;
            }
            else if (player.canClimb && 
                     !player.isOnGround && 
                     KeyCode.S.IsPressed())
            {
                positionDelta.y = -1;
            }

            positionDelta = positionDelta * player.speed * Time.deltaTime;
            player.transform.position += new Vector3(positionDelta.x, positionDelta.y);
            return positionDelta;
        }
    }
}