using SystemBase;
using Systems.Climbable.Events;
using Systems.Player;
using UniRx;
using UnityEngine;
using Utils;

namespace Systems.Climbable
{
    [GameSystem(typeof(ClimbableSystem), typeof(MovementSystem))]
    public class PlaceableSystem : GameSystem<StatsComponent>
    {
        private GameObject[] _climbablePrefabs;
        private readonly ReactiveProperty<StatsComponent> _stats = new ReactiveProperty<StatsComponent>();
        
        public override void Register(StatsComponent component)
        {
            MessageBroker.Default.Receive<PlaceObjectEvent>()
                .Subscribe(PlaceObject)
                .AddTo(component);

            _stats.Value = component;
            
            _climbablePrefabs = IoC.Game.GetComponent<PrefabComponent>().Placeables;

            component.nextPlaceablePointer.Subscribe(nextPointer => UpdateNextObjectView(component, nextPointer)).AddTo(component);
        }

        private void UpdateNextObjectView(StatsComponent component, int nextPointer)
        {
            if(component.currentNextObject)
            {
                Object.Destroy(component.currentNextObject);
            } 
            component.currentNextObject = Object.Instantiate(
                _climbablePrefabs[nextPointer], 
                Vector3.zero, 
                Quaternion.identity, 
                component.transform);

            component.currentNextObject.transform.position = component.transform.position;

            component.currentNextObject.GetComponent<Rigidbody2D>().gravityScale = 0;
        }

        private void PlaceObject(PlaceObjectEvent evt)
        {
            var objectToSpawn = _climbablePrefabs[_stats.Value.nextPlaceablePointer.Value];
            
            var spawnPosition = evt.PlayerPosition;
            spawnPosition.y += evt.PlayerHeight;

            Object.Instantiate(objectToSpawn, new Vector3(spawnPosition.x, spawnPosition.y, 0.0f), Quaternion.identity);
            
            _stats.Value.nextPlaceablePointer.Value = (int)(Random.value * _climbablePrefabs.Length);
        }
    }
}