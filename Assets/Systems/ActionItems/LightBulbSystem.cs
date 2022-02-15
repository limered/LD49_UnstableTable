using System;
using SystemBase;
using SystemBase.Core;
using Systems.Player;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Systems.ActionItems
{
    [GameSystem(typeof(MovementSystem))]
    public class LightBulbSystem : GameSystem<LightBulbComponent>
    {
        public override void Register(LightBulbComponent component)
        {
            component.OnTriggerEnter2DAsObservable()
                .Where(collider2D => collider2D.GetComponent<PlayerComponent>())
                .Subscribe(coll => StartRepair(coll, component))
                .AddTo(component);
            
            component.OnTriggerExit2DAsObservable()
                .Where(collider2D => collider2D.GetComponent<PlayerComponent>())
                .Subscribe(coll => StopRepair(coll, component))
                .AddTo(component);
            
            component.GetComponent<SpriteRenderer>().color = Color.black;
        }

        private static void StopRepair(Collider2D playerCollider, LightBulbComponent lightBulbComponent)
        {
            lightBulbComponent.repairDisposable?.Dispose();
        }

        private static void StartRepair(Collider2D playerCollider, LightBulbComponent lightBulbComponent)
        {
            lightBulbComponent.repairDisposable = Observable.Interval(TimeSpan.FromSeconds(0.2f))
                .Subscribe(_ => Repair(lightBulbComponent));
        }

        private static void Repair(LightBulbComponent lightBulbComponent)
        {
            if (lightBulbComponent.repairStatusPercent > 1f) return;
            
            lightBulbComponent.repairStatusPercent += lightBulbComponent.repairSpeedPerSecond * 0.2f;
            lightBulbComponent.GetComponent<SpriteRenderer>().color =
                Color.Lerp(Color.black, Color.yellow, lightBulbComponent.repairStatusPercent);
        }
    }
}