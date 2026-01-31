using UnityEngine;
using Yash.GameSystem.Core;
using Yash.GameSystem.Utils;

namespace Yash.GameSystem.Services
{
    // --- Events ---
    public struct GameStateChangedEvent
    {
        public BaseGameState NewState;
        public BaseGameState OldState;
    }

    // --- State Base Class ---
    public abstract class BaseGameState
    {
        protected GameStateManager Ctx;
        public BaseGameState(GameStateManager ctx) { Ctx = ctx; }
        public virtual void EnterState() { }
        public virtual void UpdateState() { }
        public virtual void ExitState() { }
    }

    // --- The Manager ---
    public class GameStateManager : BaseManager
    {
        public BaseGameState CurrentState { get; private set; }

        protected override void OnInitialize()
        {
            // Initialize with a default state if you have one, or leave null
            // e.g., SwitchState(new MenuState(this));
        }

        private void Update()
        {
            CurrentState?.UpdateState();
        }

        public void SwitchState(BaseGameState newState)
        {
            if (CurrentState == newState) return;

            var oldState = CurrentState;
            oldState?.ExitState();
            
            CurrentState = newState;
            CurrentState.EnterState();

            EventBus.Raise(new GameStateChangedEvent 
            { 
                NewState = newState, 
                OldState = oldState 
            });
        }
    }
}