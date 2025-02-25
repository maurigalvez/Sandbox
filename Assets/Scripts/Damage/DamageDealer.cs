using System.Collections.Generic;
using UnityEngine;
namespace Gameplay
{
    [RequireComponent(typeof(Collider))]
    public class DamageDealer : MonoBehaviour, IDamageBox
    {
        [SerializeField] protected float m_WeaponDamage;

        private bool m_CanDealDamage;
        private List<IHitBox> m_HasDealtDamage;
        
        private void Awake()
        {
            m_CanDealDamage = false;
            m_HasDealtDamage = new List<IHitBox>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!m_CanDealDamage) return;

            if (other.TryGetComponent<IHitBox>(out IHitBox hit))
            {
                if(!m_HasDealtDamage.Contains(hit))
                {
                    Debug.Log("Damage");
                    m_HasDealtDamage.Add(hit);
                }
            }
        }

        public void StartDealDamage()
        {
            m_CanDealDamage = true;
            m_HasDealtDamage.Clear();
        }

        public void EndDealDamage()
        {
            m_CanDealDamage = false;
        }
    }
}
