using UnityEngine;
namespace Gameplay.FSM
{
    public class Player_StandingState : PlayerState
    {
        float m_GravityValue;
        bool m_Jump;
        bool m_Crouch;
        Vector3 m_CurrentVelocity;
        bool m_Grounded;
        bool m_Sprinting;
        float m_PlayerSpeed;

        Vector3 m_CVelocity;

        public Player_StandingState(Player_Character _character, StateMachine _stateMachine): base(_character, _stateMachine) { }

        public override void Enter()
        {
            base.Enter();

            m_Jump = false;
            m_Crouch = false;
            m_Sprinting = false;
            this.m_Input = Vector2.zero;
            this.m_Velocity = Vector3.zero;
            m_CurrentVelocity = Vector3.zero;
            m_GravityVelocity.y = 0;

            m_PlayerSpeed = m_Character.GetPlayerSpeed();
            m_Grounded = m_Character.Controller.isGrounded;
            m_GravityValue = m_Character.GetGravityValue();
        }

        public override void HandleInput()
        {
            base.HandleInput();

            if (m_JumpAction.triggered) m_Jump = true;

            if (m_CrouchAction.triggered) m_Crouch = true;

            if(m_SprintAction.triggered) m_Sprinting = true;

            m_Input = m_MoveAction.ReadValue<Vector2>();
            m_Velocity = new Vector3(m_Input.x, 0, m_Input.y);

            m_Velocity = m_Velocity.x * m_Character.CameraTransform.right.normalized + m_Velocity.z * m_Character.CameraTransform.forward.normalized;
            m_Velocity.y = 0f;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            m_Character.Animator.SetFloat(SPEED_ANIM_NAME, m_Input.magnitude, m_Character.GetSpeedDampTime(), Time.deltaTime);
            // Set sprinting
            if (m_Jump)
                m_StateMachine.ChangeState(m_Character.Jumping);
            if (m_Crouch)
                m_StateMachine.ChangeState(m_Character.Crouching);
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            m_GravityVelocity.y += m_GravityValue * Time.deltaTime;
            m_Grounded = m_Character.Controller.isGrounded;

            if (m_Grounded && m_GravityVelocity.y < 0)
                m_GravityVelocity.y = 0;

            m_CurrentVelocity = Vector3.SmoothDamp(m_CurrentVelocity, m_Velocity, ref m_CVelocity, m_Character.GetVelocityDampTime());
            m_Character.Controller.Move(m_CurrentVelocity * Time.deltaTime * m_PlayerSpeed + m_GravityVelocity * Time.deltaTime);

            if (m_Velocity.sqrMagnitude > 0f)
                m_Character.transform.rotation = Quaternion.Slerp(m_Character.transform.rotation, Quaternion.LookRotation(m_Velocity), m_Character.GetRotationDampTime());
        }

        public override void Exit()
        {
            base.Exit();
            m_GravityVelocity.y = 0;
            m_Character.PlayerVelocity = new Vector3(m_Input.x, 0, m_Input.y);
            if (m_Velocity.sqrMagnitude > 0)
                m_Character.transform.rotation = Quaternion.LookRotation(m_Velocity);
        }
    }
}