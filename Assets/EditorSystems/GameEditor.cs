using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEditor;
using UnityEngine;

namespace EditorSystems
{
    [InitializeOnLoad]
    public class GameEditor
    {
        // ReSharper disable once MemberCanBePrivate.Global
        public static readonly ReactiveProperty<MouseData> MouseData = new ReactiveProperty<MouseData>();

        static GameEditor(){
            Debug.Log("Game Editor started");
            
            SceneView.duringSceneGui += OnSceneGUI;

            InitSystems();
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
        }

        private static IEnumerable<Type> CollectAllSystems()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(ass => ass.GetTypes(), (ass, type) => new { ass, type })
                .Where(assemblyType => Attribute.IsDefined(assemblyType.type, typeof(EditorSystemAttribute)))
                .Select(assemblyType => assemblyType.type);
        }

        private static void OnSceneGUI(SceneView sceneView)
        {
            UpdateMouseData(sceneView);
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

    public struct MouseData
    {
        /// <summary>
        /// 2D Mouse Position only over editor render window (Center is (0,0))
        /// </summary>
        public Vector2 EditorPosition;
        
        /// <summary>
        /// Mouse Position in World (z-coord is camera coord)
        /// </summary>
        public Vector3 MouseWorldPosition;
        
        /// <summary>
        /// Button Numbers: -1 no button, 0 left, 2 right, 3 middle
        /// </summary>
        public int MouseButtonClicked;
    }
}