using System;
using EditorSystemBase;
using EditorSystems;
using Systems.RopeSimulation;
using UniRx;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Editor
{
    [EditorSystem]
    public class RopeSimulationCreationSystem : IEditorSystem
    {
        private IDisposable _mouseSub;
        public RopeCreationMode Mode = RopeCreationMode.Idle;
        public RopeComponent CurrentClickedComponent { get; set; }

        public void GoToMode(RopeCreationMode nextMode)
        {
            Mode = nextMode;

            switch (Mode)
            {
                case RopeCreationMode.CreatePoint:
                    _mouseSub = GameEditor.MouseData
                        .Where(data => data.MouseButtonClicked == 0)
                        .Subscribe(AddPoint);
                    break;
                case RopeCreationMode.Idle:
                    _mouseSub.Dispose();
                    break;
            }
        }

        private void AddPoint(MouseData data)
        {
            if (!CurrentClickedComponent) return;
            var position = data.MouseWorldPosition;
            var worldPos = new Vector3(position.x, position.y, CurrentClickedComponent.transform.position.z);
            Object.Instantiate(CurrentClickedComponent.PointPrefab, 
                worldPos, 
                Quaternion.identity,
                CurrentClickedComponent.gameObject.transform);
        }
    }
}