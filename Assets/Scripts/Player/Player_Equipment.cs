using UnityEngine;
namespace Gameplay.Equipment
{
    public class Player_Equipment : MonoBehaviour
    {
        [SerializeField] private Transform m_WeaponHolder;
        [SerializeField] private GameObject m_WeaponPrefab;
        [SerializeField] private Transform m_WeaponSheath;

        private GameObject m_CurrentWeaponInstance;
        private Transform m_CurrentWeaponTransform;
        private IDamageBox m_CurrentWeaponBox;

        private void Awake()
        {
            m_CurrentWeaponInstance = Instantiate(m_WeaponPrefab);
            m_CurrentWeaponTransform = m_CurrentWeaponInstance.transform;
            m_CurrentWeaponBox = m_CurrentWeaponInstance.GetComponentInChildren<IDamageBox>();
            SheathWeapon();
        }

        public void DrawWeapon()
        {
            m_CurrentWeaponTransform.SetParent(m_WeaponHolder);
            m_CurrentWeaponTransform.localPosition = Vector3.zero;
            m_CurrentWeaponTransform.localRotation = Quaternion.identity;
        }

        public void SheathWeapon()
        {
            m_CurrentWeaponTransform.SetParent(m_WeaponSheath);
            m_CurrentWeaponTransform.localPosition = Vector3.zero;
            m_CurrentWeaponTransform.localRotation = Quaternion.identity;
        }

        public void StartDealDamage()
        {
            if (m_CurrentWeaponBox != null)
                m_CurrentWeaponBox.StartDealDamage();
        }

        public void EndDealDamage()
        {
            if (m_CurrentWeaponBox != null)
                m_CurrentWeaponBox.EndDealDamage();
        }
    }
}
