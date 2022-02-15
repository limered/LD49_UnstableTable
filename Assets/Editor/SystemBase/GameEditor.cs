using SystemBaseEditorCommon;
using UniRx;
using UnityEditor;
using UnityEngine;

namespace Editor.SystemBase
{
    [InitializeOnLoad]
    public class GameEditor
    {
        // ReSharper disable once MemberCanBePrivate.Global
        public static readonly ReactiveProperty<MouseData> MouseData = new ReactiveProperty<MouseData>();
        public static readonly SystemCollection Systems = new SystemCollection();

        static GameEditor(){
            Debug.Log("Game Editor started");

            Systems.Initialize();
            
            SceneView.duringSceneGui += OnSceneGUI;
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
                editorPosition = mousePos,
                mouseWorldPosition = worldPos,
                mouseButtonClicked = cur.clickCount > 0 ? cur.button : -1,
            });
        }
    }
}