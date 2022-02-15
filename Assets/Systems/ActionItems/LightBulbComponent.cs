using System;
using SystemBase;
using SystemBase.Core;

namespace Systems.ActionItems
{
    public class LightBulbComponent : GameComponent
    {
        public float repairStatusPercent = 0;
        public float repairSpeedPerSecond = 0.3f;
        public IDisposable repairDisposable;
    }
}