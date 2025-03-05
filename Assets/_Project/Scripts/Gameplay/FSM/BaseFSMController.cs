using System.Collections.Generic;
using FTKingdom.Utils;

namespace FTKingdom
{
    public class BaseFSMController
    {
        private readonly Dictionary<CharacterState, IState> stateToBehavior = new()
        {
            { CharacterState.Waiting, new CharacterStateWaiting() },
            { CharacterState.Walking, new CharacterStateWalking() },
            { CharacterState.Attacking, new CharacterStateAttacking() },
            { CharacterState.Dead, new CharacterStateDead() }
        };

        private IState currentState;
        private IState CurrentState
        {
            get => currentState;
            set
            {
                if (CurrentState != value)
                {
                    if (currentState != null)
                    {
                        currentState.End(_entity);
                        LogHandler.StateLog($"Switching from {currentState.StateType} to {value.StateType}.");
                    }
                    else
                    {
                        LogHandler.StateLog($"Initializing with {value.StateType} state.");
                    }

                    currentState = value;
                    currentState.Start(_entity);
                }
            }
        }

        private CharacterBattle _entity;

        public void InitializeStates(CharacterBattle entity, CharacterState initialState)
        {
            _entity = entity;
            ChangeState(initialState);
        }

        public void UpdateCurrentState()
        {
            CurrentState?.Update(_entity);
        }

        public void ChangeState(CharacterState state)
        {
            if (stateToBehavior.TryGetValue(state, out IState entityState))
            {
                CurrentState = entityState;
            }
        }
    }
}