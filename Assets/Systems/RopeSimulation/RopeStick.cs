using SystemBase;
using SystemBase.Core;

namespace Systems.RopeSimulation
{
    public class RopeStick : GameComponent
    {
        public RopePoint pointA;
        public RopePoint pointB;
        public float length;
        public float halflength;
    }
}