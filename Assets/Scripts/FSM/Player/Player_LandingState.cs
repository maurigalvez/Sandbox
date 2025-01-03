using UnityEngine;
namespace Gameplay.FSM
{
    public class Player_LandingState : PlayerState
    {
        float m_TimePassed;
        const float LANDING_TIME = 0.5f;

        public Player_LandingState(Player_Character _character, StateMachine _stateMachine) : base(_character, _stateMachine) { }

        public override void Enter()
        {
            base.Enter();
            m_TimePassed = 0;
            m_Character.Animator.SetTrigger(LAND_ANIMTRIGGER_NAME);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            m_TimePassed += Time.deltaTime;

            if (m_TimePassed < LANDING_TIME) return;
            
            m_Character.Animator.SetTrigger(MOVE_ANIMTRIGGER_NAME);
            m_StateMachine.ChangeState(m_Character.Landing);       
        }
    }
}
