using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [SerializeField]
    private float m_MaxHealth = 3.0f;

    private float m_CurrentHealth = 0.0f;

    private CinemachineImpulseSource m_ImpulseSource;

    public bool HasTakenDamage { get; set; }

    private void Start()
    {
        m_CurrentHealth = m_MaxHealth;
        m_ImpulseSource = GetComponent<CinemachineImpulseSource>();
    }

    public void Damage(float value)
    {
        CameraShakeManager.Instance.CameraShake(m_ImpulseSource);
        HasTakenDamage = true;
        m_CurrentHealth -= value;
        if (m_CurrentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        gameObject.SetActive(false);
    }
}
