using System.Collections.Generic;
using SystemBase;
using SystemBase.Core;
using UniRx;
using UnityEngine;

namespace Systems.RopeSimulation
{
    [GameSystem]
    public class RopeSimulationSystem : GameSystem<RopeComponent>
    {
        private const int IterationCount = 5;

        public override void Register(RopeComponent component)
        {
            CollectPointsAndSticks(component);
            FillPointPosition(component.points);
            CalculateStickLength(component.sticks);

            SystemUpdate(component).Subscribe(Simulate).AddTo(component);
            SystemUpdate(component).Subscribe(Render).AddTo(component);
        }
        private static void Simulate(RopeComponent ropeComponent)
        {
            foreach (var point in ropeComponent.points)
            {
                if (point.locked)
                {
                    point.pos = point.gameObject.transform.position;
                    continue;
                }

                var positionBeforeUpdate = point.pos;
                point.pos += (point.pos - point.prevPos) * 0.99f;
                point.pos += Vector2.down * 9.81f * Time.deltaTime * Time.deltaTime;
                point.prevPos = positionBeforeUpdate;
            }

            for (var i = 0; i < IterationCount; i++)
            {
                foreach (var stick in ropeComponent.sticks)
                {
                    var stickCenter = (stick.pointA.pos + stick.pointB.pos) / 2f;
                    var stickDirection = (stick.pointA.pos - stick.pointB.pos).normalized;

                    if (!stick.pointA.locked)
                    {
                        stick.pointA.pos = stickCenter + stickDirection * stick.halflength;
                    }
                    if (!stick.pointB.locked)
                    {
                        stick.pointB.pos = stickCenter - stickDirection * stick.halflength;
                    }
                    stick.transform.position = stickCenter;
                }
            }
        }
        private static void Render(RopeComponent ropeComponent)
        {
            foreach (var point in ropeComponent.points)
            {
                if(point.locked) continue;
                point.gameObject.transform.position = point.pos;
            }
        }

        private static void FillPointPosition(IEnumerable<RopePoint> componentPoints)
        {
            foreach (var point in componentPoints)
            {
                point.pos = point.gameObject.transform.position;
                point.prevPos = point.pos;
            }
        }

        private static void CollectPointsAndSticks(RopeComponent component)
        {
            component.points = component.GetComponentsInChildren<RopePoint>();
            component.sticks = component.GetComponentsInChildren<RopeStick>();
        }

        private static void CalculateStickLength(IEnumerable<RopeStick> sticks)
        {
            foreach (var stick in sticks)
            {
                stick.length = (stick.pointA.pos - stick.pointB.pos).magnitude;
                stick.halflength = stick.length * 0.5f;
            }
        }
    }
}