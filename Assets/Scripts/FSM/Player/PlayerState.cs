using UnityEngine;
using UnityEngine.InputSystem;
namespace Gameplay.FSM
{
    public class PlayerState : State
    {
        protected Player_Character m_Character;
        protected StateMachine m_StateMachine;

        protected Vector3 m_GravityVelocity;
        protected Vector3 m_Velocity;
        protected Vector2 m_Input;

        protected InputAction m_MoveAction;
        protected InputAction m_JumpAction;
        protected InputAction m_CrouchAction;
        protected InputAction m_SprintAction;

        protected const string SPEED_ANIM_NAME = "Speed";
        protected const string MOVE_ANIMTRIGGER_NAME = "Move";
        protected const string JUMP_ANIMTRIGGER_NAME = "Jump";
        protected const string LAND_ANIMTRIGGER_NAME = "Land";
        protected const string CROUCH_ANIMTRIGGER_NAME = "Crouch";
        protected const string SPRINTJUMP_ANIMTRIGGER_NAME = "SprintJump";
        protected const string XDIRECTION_ANIM_NAME = "XDirection";
        protected const string YDIRECTION_ANIM_NAME = "YDirection";

        public PlayerState(Player_Character _character, StateMachine _stateMachine)
        {
            m_Character = _character;
            m_StateMachine = _stateMachine;

            m_MoveAction = m_Character.PlayerInput.actions["Move"];
            m_JumpAction = m_Character.PlayerInput.actions["Jump"];
            m_CrouchAction = m_Character.PlayerInput.actions["Crouch"];
            m_SprintAction = m_Character.PlayerInput.actions["Sprint"];
        }

        public override void Enter()
        {

        }

        public override void Exit()
        {

        }

        public virtual void HandleInput() { }

        public override void LogicUpdate()
        {
            HandleInput();
        }
    }
}
