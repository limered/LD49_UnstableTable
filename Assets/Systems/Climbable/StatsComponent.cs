using SystemBase;
using SystemBase.Core;
using UniRx;
using UnityEngine;

namespace Systems.Climbable
{
    public class StatsComponent : GameComponent
    {
        public IntReactiveProperty nextPlaceablePointer = new IntReactiveProperty(0);
        public GameObject currentNextObject;
    }
}