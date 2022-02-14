using SystemBase;
using UnityEngine;

namespace Systems.RopeSimulation
{
    public class RopeComponent : GameComponent
    {
        public GameObject PointPrefab;
        public GameObject StickPrefab;
        
        public RopePoint[] points;
        public RopeStick[] sticks;
        
    }
}