using UnityEngine;
namespace Gameplay.FSM
{
    public class Player_JumpingState : PlayerState
    {
        bool m_Grounded;
        float m_GravityValue;
        float m_JumpHeight;
        float m_PlayerSpeed;

        Vector3 m_AirVelocity;

        private const float JUMP_CONSTANT = -3.0f;

        public Player_JumpingState(Player_Character _character, StateMachine _stateMachine) : base(_character, _stateMachine) { }

        public override void Enter()
        {
            base.Enter();
            m_Grounded = false;
            m_GravityValue = m_Character.GetGravityValue();
            m_JumpHeight = m_Character.GetJumpHeight();
            m_PlayerSpeed = m_Character.GetPlayerSpeed();
            m_GravityVelocity.y = 0;

            m_Character.Animator.SetFloat(SPEED_ANIM_NAME, 0);
            m_Character.Animator.SetTrigger(JUMP_ANIMTRIGGER_NAME);

            Jump();
        }

        public override void HandleInput()
        {
            base.HandleInput();
            m_Input = m_MoveAction.ReadValue<Vector2>();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (m_Grounded)
            {
                m_StateMachine.ChangeState(m_Character.Landing);
            }              
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            if(!m_Grounded)
            {
                m_Velocity = m_Character.PlayerVelocity;
                m_AirVelocity = new Vector3(m_Input.x, 0, m_Input.y);

                m_Velocity = m_Velocity.x * m_Character.CameraTransform.right.normalized + m_Velocity.z * m_Character.CameraTransform.forward.normalized;
                m_Velocity.y = 0f;
                
                m_AirVelocity = m_AirVelocity.x * m_Character.CameraTransform.right.normalized + m_AirVelocity.z * m_Character.CameraTransform.forward.normalized;
                m_AirVelocity.y = 0f;

                m_Character.Controller.Move(m_GravityVelocity * Time.deltaTime + (m_AirVelocity * m_Character.GetAirControl() + m_Velocity * (1 - m_Character.GetAirControl())) * m_PlayerSpeed * Time.deltaTime);
            }
            m_GravityVelocity.y += m_GravityValue * Time.deltaTime;
            m_Grounded = m_Character.Controller.isGrounded;
        }

        private void Jump()
        {
            m_GravityVelocity.y += Mathf.Sqrt(m_JumpHeight * JUMP_CONSTANT * m_GravityValue);
        }
    }
}
