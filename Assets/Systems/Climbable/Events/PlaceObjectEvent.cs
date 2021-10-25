using UnityEngine;

namespace Systems.Climbable.Events
{
    public class PlaceObjectEvent
    {
        public Vector2 PlayerPosition { get; set; }
        public float PlayerHeight { get; set; }
    }
}