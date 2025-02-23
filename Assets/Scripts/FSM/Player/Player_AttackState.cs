using UnityEngine;
namespace Gameplay.FSM
{
    public class Player_AttackState : PlayerState
    {
        private float m_TimePassed;
        private float m_ClipLength;
        private float m_ClipSpeed;
        private bool m_Attack;

        public Player_AttackState(Player_Character _character, StateMachine _stateMachine) : base(_character, _stateMachine) { }

        public override void Enter()
        {
            base.Enter();
            m_Attack = false;
            m_Character.Animator.applyRootMotion = false;
            m_TimePassed = 0;
            m_Character.Animator.SetTrigger(ATTACK_TRIGGER);
            m_Character.Animator.SetFloat(SPEED_ANIM_NAME, 0);
        }

        public override void HandleInput()
        {
            base.HandleInput();
            if(m_AttackAction.triggered)
               m_Attack = true;            
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            m_TimePassed += Time.deltaTime;
            m_ClipLength = m_Character.Animator.GetCurrentAnimatorClipInfo(1)[0].clip.length;
            m_ClipSpeed = m_Character.Animator.GetCurrentAnimatorStateInfo(1).speed;

            if (m_TimePassed >= m_ClipLength / m_ClipSpeed && m_Attack)
                m_StateMachine.ChangeState(m_Character.Attack);
            if(m_TimePassed >= m_ClipLength/m_ClipSpeed)
            {
                m_StateMachine.ChangeState(m_Character.Combat);
                m_Character.Animator.SetTrigger(MOVE_ANIMTRIGGER_NAME);
            }
        }

        public override void Exit()
        {
            base.Exit();
            m_Character.Animator.applyRootMotion = false;
        }
    }
}
