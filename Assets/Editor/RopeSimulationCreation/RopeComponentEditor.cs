using Editor.SystemBase;
using Systems.RopeSimulation;
using UnityEditor;
using UnityEngine;

namespace Editor.RopeSimulationCreation
{
    [CustomEditor(typeof(RopeComponent))]
    public class RopeComponentEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI () {
            //Called whenever the inspector is drawn for this object.
            DrawDefaultInspector();
            
            var ropeSystem = GameEditor.Systems.GetSystem<RopeSimulationCreationSystem>();
            var nodeButtonLabel = ropeSystem.Mode == RopeCreationMode.Idle ? 
                "Add Rope Point" : 
                "Stop Adding Elements";  
            if (GUILayout.Button(nodeButtonLabel))
            {
                if (ropeSystem.Mode == RopeCreationMode.Idle)
                {
                    ropeSystem.CurrentClickedComponent = (RopeComponent) target;
                    ropeSystem.GoToMode(RopeCreationMode.CreatePoint);
                }
                else
                {
                    ropeSystem.CurrentClickedComponent = null;
                    ropeSystem.GoToMode(RopeCreationMode.Idle);
                }
            }
             
            if (GUILayout.Button("Add Rope Stick")) 
            {
            }
        }
    }
}