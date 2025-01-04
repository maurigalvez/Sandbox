using UnityEngine;
using UnityEngine.InputSystem;
namespace Gameplay
{
    public class Camera_FollowTarget : MonoBehaviour
    {
        [SerializeField] private Transform m_FollowTarget;
        [SerializeField] private float m_RotationSpeed = 10f;
        [SerializeField] private float m_BottomClamp = -40f;
        [SerializeField] private float m_TopClamp = 70f;
        [SerializeField] private PlayerInput m_PlayerInput;

        private float m_CineMachineTargetPitch;
        private float m_CineMachineTargetYaw;
        protected InputAction m_MouseXAxis;
        protected InputAction m_MouseYAxis;

        private void Awake()
        {
            m_MouseXAxis = m_PlayerInput.actions["MouseX"];
            m_MouseYAxis = m_PlayerInput.actions["MouseY"];
            Cursor.lockState = CursorLockMode.Locked;
        }

        private float GetMouseInput(bool isXAxis)
        {
            return Input.GetAxis(isXAxis ? "Mouse X" : "Mouse Y") * m_RotationSpeed * Time.deltaTime;
            /*if(isXAxis)
                return m_MouseXAxis.ReadValue<float>() * m_RotationSpeed * Time.deltaTime;

            return m_MouseXAxis.ReadValue<float>() * m_RotationSpeed * Time.deltaTime;*/
        }

        private void LateUpdate()
        {
            CameraLogic();
        }

        private void CameraLogic()
        {
            float mouseX = GetMouseInput(true);
            float mouseY = GetMouseInput(false);

            m_CineMachineTargetPitch = GetRotation(m_CineMachineTargetPitch, mouseY, m_BottomClamp, m_TopClamp, true);
            m_CineMachineTargetYaw = GetRotation(m_CineMachineTargetYaw, mouseX, float.MinValue, float.MaxValue, false);

            ApplyRotations(m_CineMachineTargetPitch, m_CineMachineTargetYaw);
        }

        private float GetRotation(float currentRotation, float input, float min, float max, bool isXAxis)
        {
            currentRotation += isXAxis ? -input : input;
            return Mathf.Clamp(currentRotation, min, max);
        }

        private void ApplyRotations(float pitch, float yaw)
        {
            m_FollowTarget.rotation = Quaternion.Euler(pitch, yaw, m_FollowTarget.eulerAngles.z);
        }
    }
}
