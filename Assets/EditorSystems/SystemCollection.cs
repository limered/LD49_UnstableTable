using System;
using System.Collections.Generic;

namespace EditorSystems
{
    public class SystemCollection
    {
        private Dictionary<Type, IEditorSystem> _systems = new Dictionary<Type, IEditorSystem>();

        public void AddSystem(IEditorSystem system)
        {
            _systems[system.GetType()] = system;
        }

        public TSystem GetSystem<TSystem>() where TSystem : IEditorSystem, new()
        {
            if (_systems.TryGetValue(typeof(TSystem), out var system))
            {
                return (TSystem) system;
            }

            system = new TSystem();
            AddSystem(system);
            return (TSystem)system;
        }
    }
}