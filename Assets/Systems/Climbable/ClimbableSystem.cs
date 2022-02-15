using System.Linq;
using SystemBase;
using SystemBase.Core;
using SystemBase.Utils;
using Systems.Climbable.Events;
using Systems.Player;
using UniRx;
using UnityEngine;

namespace Systems.Climbable
{
    [GameSystem(typeof(MovementSystem))]
    public class ClimbableSystem : GameSystem<PlayerComponent>
    {
        public override void Register(PlayerComponent component)
        {
            SystemUpdate(component)
                .Subscribe(CheckForPlace)
                .AddTo(component);
        }

        private static void CheckForPlace(PlayerComponent player)
        {
            if (KeyCode.Space.WasPressed() && 
                ObjectCanBePlaced(player))
            {
                player.canPlace = true;
                PlaceObject(player);
            }
            else
            {
                player.canPlace = false;
            }
        }

        private static void PlaceObject(PlayerComponent player)
        {
            MessageBroker.Default.Publish(new PlaceObjectEvent
            {
                PlayerPosition = player.transform.position.XY(),
                PlayerHeight = player.climbCollider.bounds.extents.y
            });
        }

        private static bool ObjectCanBePlaced(PlayerComponent player)
        {
            var topPos = player.transform.position.XY();
            topPos.y += player.climbCollider.bounds.extents.y;
            
            var bottomPos = player.transform.position.XY();
            bottomPos.y -= player.climbCollider.bounds.extents.y;
            
            
            var overlapsTop = Physics2D
                .OverlapBoxAll(topPos, player.climbCollider.bounds.extents.XY()/2.0f, 0.0f);
            var overlapsBottom = Physics2D
                .OverlapBoxAll(bottomPos, player.climbCollider.bounds.extents.XY()/2.0f, 0.0f);

            var topHit = overlapsTop.Any(overlap => overlap.GetComponent<ClimbableComponent>()); 
            var bottomHit = overlapsBottom.Any(overlap => overlap.GetComponent<ClimbableComponent>()); 
            
            return !topHit && bottomHit;
        }
    }
}