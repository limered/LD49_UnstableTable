using SystemBase;
using SystemBase.Core;
using UnityEngine;

namespace Systems.Player
{
    public class PlayerComponent : GameComponent
    {
        public float speed;
        public bool isOnGround;
        public bool canClimb;
        public Collider2D groundCollider;
        public Collider2D climbCollider;
        public bool canPlace;
    }
}