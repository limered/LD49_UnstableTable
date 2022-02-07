using System;
using UniRx;
using UnityEngine;

namespace EditorSystems.RopeSimulation
{
    [EditorSystem]
    public class RopeSimulationCreationSystem : IEditorSystem
    {
        private IDisposable _mouseSub;

        public RopeSimulationCreationSystem()
        {
            // _mouseSub = GameEditor.MouseData
            //     .Subscribe(data => Debug.Log(data.MouseButtonClicked));
        }
    }
}