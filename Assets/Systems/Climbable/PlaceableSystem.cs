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
        }

        private void PlaceObject(PlaceObjectEvent evt)
        {
            Debug.Log(evt);
            var objectToSpawn = _climbablePrefabs[_stats.Value.nextPlaceablePointer];
            var spawnHeight = objectToSpawn.GetComponent<ClimbableComponent>().collider.bounds.extents.y;
            
            var spawnPosition = evt.PlayerPosition;
            spawnPosition.y += evt.PlayerHeight + spawnHeight;

            Object.Instantiate(objectToSpawn, new Vector3(spawnPosition.x, spawnPosition.y, 0.0f), Quaternion.identity);
            
            _stats.Value.nextPlaceablePointer = (int)(Random.value * _climbablePrefabs.Length);
        }
    }
}