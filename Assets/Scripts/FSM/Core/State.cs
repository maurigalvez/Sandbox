using UnityEngine;
namespace Gameplay.FSM
{
    public abstract class State
    {
        public abstract void Enter();
        public abstract void Exit();

        public virtual void LogicUpdate() { }
        public virtual void PhysicsUpdate() { }
    }
}
