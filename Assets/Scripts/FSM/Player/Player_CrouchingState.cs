using UnityEngine;
namespace Gameplay.FSM
{ 
    public class Player_CrouchingState : PlayerState
    {
        float m_PlayerSpeed;
        bool m_BelowCeiling;
        bool m_CrouchHeld;
        bool m_Grounded;
        float m_GravityValue;
        Vector3 m_CurrentVelocity;

        public Player_CrouchingState(Player_Character _character, StateMachine _stateMachine) : base(_character, _stateMachine) { }

        public override void Enter()
        {
            base.Enter();
            m_Character.Animator.SetTrigger(CROUCH_ANIMTRIGGER_NAME);
            m_BelowCeiling = false;
            m_CrouchHeld = false;
            m_GravityVelocity.y = 0;

            m_PlayerSpeed = m_Character.GetCrouchSpeed();
            m_Character.Controller.height = m_Character.GetCrouchColliderHeight();
            m_Character.Controller.center = new Vector3(0, m_Character.GetCrouchColliderHeight() / 2, 0);
            m_Grounded = m_Character.Controller.isGrounded;
            m_GravityValue = m_Character.GetGravityValue();
        }

        public override void Exit()
        {
            base.Exit();
            m_Character.Controller.height = m_Character.NormalColliderHeight;
            m_Character.Controller.center = new Vector3(0, m_Character.NormalColliderHeight / 2, 0);
            m_GravityVelocity.y = 0f;
            m_Character.PlayerVelocity = new Vector3(m_Input.x, 0, m_Input.y);
            m_Character.Animator.SetTrigger(MOVE_ANIMTRIGGER_NAME);
        }

        public override void HandleInput()
        {
            base.HandleInput();
            if (m_CrouchAction.triggered && !m_BelowCeiling)
                m_CrouchHeld = true;

            m_Input = m_MoveAction.ReadValue<Vector2>();
            m_Velocity = new Vector3(m_Input.x, 0, m_Input.y);
            m_Velocity = m_Velocity.x * m_Character.CameraTransform.right.normalized + m_Velocity.z * m_Character.CameraTransform.forward.normalized;
            m_Velocity.y = 0;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            m_Character.Animator.SetFloat(SPEED_ANIM_NAME, m_Input.magnitude,m_Character.GetSpeedDampTime(), Time.deltaTime);
            m_Character.Animator.SetFloat(XDIRECTION_ANIM_NAME, m_Input.x, m_Character.GetSpeedDampTime(), Time.deltaTime);
            m_Character.Animator.SetFloat(YDIRECTION_ANIM_NAME, m_Input.y, m_Character.GetSpeedDampTime(), Time.deltaTime);

            if (m_CrouchHeld)
                m_StateMachine.ChangeState(m_Character.Standing);
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            m_BelowCeiling = CheckCollisionOverlap(m_Character.transform.position + Vector3.up * m_Character.NormalColliderHeight);
            m_GravityVelocity.y *= m_GravityValue * Time.deltaTime;
            m_Grounded = m_Character.Controller.isGrounded;
            if (m_Grounded && m_GravityVelocity.y < 0)
                m_GravityVelocity.y = 0;

            m_CurrentVelocity = Vector3.Lerp(m_CurrentVelocity, m_Velocity, m_Character.GetVelocityDampTime());
            m_Character.Controller.Move(m_CurrentVelocity * Time.deltaTime * m_PlayerSpeed + m_GravityVelocity * Time.deltaTime);

            if (m_Velocity.magnitude > 0)
                RotateToCameraLook();
        }

        /// <summary>
        /// This is used to check if player can stand up!
        /// </summary>
        private bool CheckCollisionOverlap(Vector3 targetPosition)
        {
            // Change for something more readable
            int layerMask = 1 << 8;
            layerMask = ~layerMask;

            RaycastHit hit;
            Vector3 direction = targetPosition - m_Character.transform.position;
            if(Physics.Raycast(m_Character.transform.position, direction, out hit, m_Character.NormalColliderHeight, layerMask))
            {
                Debug.DrawRay(m_Character.transform.position, direction * hit.distance, Color.yellow);
                return true;
            }
            Debug.DrawRay(m_Character.transform.position, direction * m_Character.NormalColliderHeight, Color.yellow);
            return false;
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
