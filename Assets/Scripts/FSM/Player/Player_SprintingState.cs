using UnityEngine;
namespace Gameplay.FSM
{
    public class Player_SprintingState : PlayerState
    {
        float m_GravityValue;
        Vector3 m_CurrentVelocity;
        bool m_Grounded;
        bool m_Sprint;
        float m_PlayerSpeed;
        bool m_Jumping;
        Vector3 m_cVelocity;

        public Player_SprintingState(Player_Character _character, StateMachine _stateMachine) : base(_character, _stateMachine) { }

        public override void Enter()
        {
            base.Enter();
            m_Sprint = false;
            m_Jumping = false;
            m_Input = Vector2.zero;
            m_Velocity = Vector3.zero;
            m_CurrentVelocity = Vector3.zero;
            m_GravityVelocity.y = 0;

            m_PlayerSpeed = m_Character.GetSprintSpeed();
            m_Grounded = m_Character.Controller.isGrounded;
            m_GravityValue = m_Character.GetGravityValue();
        }

        public override void HandleInput()
        {
            base.HandleInput();
            m_Input = m_MoveAction.ReadValue<Vector2>();
            m_Velocity = new Vector3(m_Input.x, 0, m_Input.y);
            m_Velocity = m_Velocity.x * m_Character.CameraTransform.right.normalized + m_Velocity.z * m_Character.CameraTransform.forward.normalized;
            m_Velocity.y = 0;
            if (m_SprintAction.triggered || m_Input.magnitude == 0)
                m_Sprint = false;
            else
                m_Sprint = true;
            // If jumping triggered here
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (m_Sprint)
            {
                m_Character.Animator.SetFloat(SPEED_ANIM_NAME, m_Input.magnitude + 0.5f, m_Character.GetSpeedDampTime(), Time.deltaTime);
                m_Character.Animator.SetFloat(XDIRECTION_ANIM_NAME, m_Input.x, m_Character.GetSpeedDampTime(), Time.deltaTime);
                m_Character.Animator.SetFloat(YDIRECTION_ANIM_NAME, m_Input.y, m_Character.GetSpeedDampTime(), Time.deltaTime);
            }
            else
                m_StateMachine.ChangeState(m_Character.Standing);
            // if sprint jump
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            m_GravityVelocity.y += m_GravityValue * Time.deltaTime;
            m_Grounded = m_Character.Controller.isGrounded;
            if(m_Grounded && m_GravityVelocity.y < 0)
                m_GravityVelocity.y = 0;
            m_CurrentVelocity = Vector3.SmoothDamp(m_CurrentVelocity, m_Velocity, ref m_cVelocity, m_Character.GetVelocityDampTime());

            m_Character.Controller.Move(m_CurrentVelocity * Time.deltaTime * m_PlayerSpeed + m_GravityVelocity * Time.deltaTime);

            if (m_Velocity.sqrMagnitude > 0f)
            {
                RotateToCameraLook();
            }
        }

        private void RotateToCameraLook()
        {
            var lookRotation = Quaternion.LookRotation(m_Character.CameraTransform.forward);
            var newEuler = m_Character.transform.rotation.eulerAngles;
            newEuler.y = lookRotation.eulerAngles.y;
            m_Character.transform.rotation = Quaternion.Slerp(m_Character.transform.rotation, Quaternion.Euler(newEuler), m_Character.GetRotationDampTime());
        }
    }
}
