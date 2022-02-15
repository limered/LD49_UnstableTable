using GameState.States;
using SystemBase;
using SystemBase.StateMachineBase;
using GameState.Messages;
using StrongSystems.Audio.Helper;
using UniRx;
using UnityEngine;
using Utils;

namespace Systems
{
    public class Game : GameBase
    {
        // ReSharper disable once MemberCanBePrivate.Global
        public readonly StateContext<Game> gameStateContext = new StateContext<Game>();

        private void Awake()
        {
            IoC.RegisterSingleton(this);

            gameStateContext.Start(new Loading());

            Init();
            

            MessageBroker.Default.Publish(new GameMsgFinishedLoading());
            MessageBroker.Default.Publish(new GameMsgStart());
            UnityEngine.Cursor.visible = true;
        }

        private void Start()
        {
           // MessageBroker.Default.Publish(new GameMsgStart());
            QualitySettings.vSyncCount = 1;
            Application.targetFrameRate = 60;
        }

        public override void Init()
        {
            base.Init();

            IoC.RegisterSingleton<ISFXComparer>(()=> new SFXComparer());
        }
    }
}