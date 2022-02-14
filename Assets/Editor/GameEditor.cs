using System;
using System.Collections.Generic;
using System.Linq;
using EditorSystemBase;
using EditorSystems;
using Systems;
using UniRx;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [InitializeOnLoad]
    public class GameEditor
    {
        // ReSharper disable once MemberCanBePrivate.Global
        public static readonly ReactiveProperty<MouseData> MouseData = new ReactiveProperty<MouseData>();
        public static readonly SystemCollection Systems = new SystemCollection();

        static GameEditor(){
            Debug.Log("Game Editor started");

            InitSystems();
            
            SceneView.duringSceneGui += OnSceneGUI;
        }

        private static void InitSystems()
        {
            foreach (var systemType in CollectAllSystems())
            {
                RegisterSystem(Activator.CreateInstance(systemType) as IEditorSystem);
            }
        }

        private static void RegisterSystem(IEditorSystem systemInstance)
        {
            Debug.Log($"System created {systemInstance.GetType()}");
            Systems.AddSystem(systemInstance);
        }

        private static IEnumerable<Type> CollectAllSystems()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(ass => ass.GetTypes(), (ass, type) => new { ass, type })
                .Where(assemblyType => Attribute.IsDefined(assemblyType.type, typeof(EditorSystemAttribute)))
                .Select(assemblyType => assemblyType.type);
        }

        private static void OnSceneGUI(SceneView view)
        {
            UpdateMouseData(view);
        }

        private static void UpdateMouseData(SceneView sceneView)
        {
            
            if (!Event.current.isMouse) return;
            
            var cur = Event.current;
            var mousePos = (Vector3) cur.mousePosition;
            mousePos.y = Camera.current.pixelHeight - mousePos.y;
            var worldPos = sceneView.camera.ScreenToWorldPoint(mousePos);
            
            MouseData.SetValueAndForceNotify(new MouseData
            {
                EditorPosition = mousePos,
                MouseWorldPosition = worldPos,
                MouseButtonClicked = cur.clickCount > 0 ? cur.button : -1,
            });
        }
    }
}