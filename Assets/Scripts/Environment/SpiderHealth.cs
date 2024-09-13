using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderHealth : MonoBehaviour, IDamageable
{
    [SerializeField]
    private float m_MaxHealth = 1.0f;

    private float m_CurrentHealth = 0.0f;

    public bool HasTakenDamage { get; set; }

    private void Start()
    {
        m_CurrentHealth = m_MaxHealth;
    }

    public void Damage(float value)
    {
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
