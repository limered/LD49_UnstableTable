using SystemBase;
using UnityEngine;

namespace Systems.RopeSimulation
{
    public class RopePoint : GameComponent
    {
        public Vector2 pos;
        public Vector2 prevPos;
        public bool locked;
    }
}