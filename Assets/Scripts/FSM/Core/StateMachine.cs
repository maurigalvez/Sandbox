using UnityEngine;
namespace Gameplay.FSM
{
    public class StateMachine
    {
        public State CurrentState { get; private set; }

        public bool IsActive => CurrentState != null;

        public void Initialize(State startingState) => ChangeState(startingState);

        public void ChangeState(State newState)
        {
            if(CurrentState != null)
                CurrentState.Exit();

            CurrentState = newState;
            CurrentState.Enter();
        }
    }
}
