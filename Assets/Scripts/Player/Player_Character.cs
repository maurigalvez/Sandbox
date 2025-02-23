using UnityEngine;
using UnityEngine.InputSystem;
namespace Gameplay
{
    using FSM;
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent (typeof(Animator))]
    [RequireComponent(typeof(PlayerInput))]
    public class Player_Character : MonoBehaviour
    {
        [Header("Control Settings")]
        [SerializeField] private float m_PlayerSpeed = 5.0f;
        [SerializeField] private float m_CrouchSpeed = 2.0f;
        [SerializeField] private float m_SprintSpeed = 7.0f;
        [SerializeField] private float m_JumpHeight = 0.8f;
        [SerializeField] private float m_GravityMultiplier = 2;
        [SerializeField] private float m_RotationSpeed = 5.0f;
        [SerializeField] private float m_CrouchColliderHeight = 1.35f;
        [Header("Animation Smoothing")]
        [Range(0, 1)]
        [SerializeField] private float m_SpeedDampTime = 0.1f;
        [Range(0, 1)]
        [SerializeField] private float m_VelocityDampTime = 0.9f;
        [Range(0, 1)]
        [SerializeField] private float m_RotationDampTime = 0.2f;
        [Range(0, 1)]
        [SerializeField] private float m_AirControl = 0.5f;

        private float m_GravityValue = -9.81f;        
        public float NormalColliderHeight { get; private set; }
        public CharacterController Controller { get; private set; }
        public PlayerInput PlayerInput { get; private set; }
        public Transform CameraTransform { get; private set; }
        public Animator Animator { get; private set; }
        public Vector3 PlayerVelocity { get; set; }

        public float GetPlayerSpeed() => m_PlayerSpeed;

        public float GetSprintSpeed() => m_SprintSpeed;
        public float GetCrouchSpeed() => m_CrouchSpeed;
        public float GetJumpHeight() => m_JumpHeight;
        public float GetGravityValue() => m_GravityValue;
        public float GetSpeedDampTime() => m_SpeedDampTime;
        public float GetVelocityDampTime() => m_VelocityDampTime;
        public float GetRotationDampTime() => m_RotationDampTime;
        public float GetAirControl() => m_AirControl;
        public float GetCrouchColliderHeight() => m_CrouchColliderHeight;

        private StateMachine m_MovementSM;
        public Player_StandingState Standing { get; private set; }
        public Player_JumpingState Jumping { get; private set; }
        public Player_LandingState Landing { get; private set; }
        public Player_CrouchingState Crouching { get; private set; }
        public Player_SprintingState Sprinting { get; private set; }
        public Player_CombatState Combat { get; private set; }
        public Player_AttackState Attack { get; private set; }
        private void Start()
        {
            Controller = GetComponent<CharacterController>();
            PlayerInput = GetComponent<PlayerInput>();
            Animator = GetComponent<Animator>();
            CameraTransform = Camera.main.transform;

            NormalColliderHeight = Controller.height;
            m_GravityValue *= m_GravityMultiplier;

            m_MovementSM = new StateMachine();
            Standing = new Player_StandingState(this, m_MovementSM);
            Jumping = new Player_JumpingState(this, m_MovementSM);
            Landing = new Player_LandingState(this, m_MovementSM);
            Crouching = new Player_CrouchingState(this, m_MovementSM);
            Sprinting = new Player_SprintingState(this, m_MovementSM);
            Combat = new Player_CombatState(this, m_MovementSM);
            Attack = new Player_AttackState(this, m_MovementSM);

            m_MovementSM.Initialize(Standing);
        }

        private void Update()
        {
            if (m_MovementSM.IsActive)
                m_MovementSM.CurrentState.LogicUpdate();
        }

        private void FixedUpdate()
        {
            if(m_MovementSM.IsActive)
                m_MovementSM.CurrentState.PhysicsUpdate();
        }
    }
}
