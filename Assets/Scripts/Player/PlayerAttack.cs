using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField]
    private Transform m_AttackTransform;
    [SerializeField]
    private float m_AttackRange = 1.5f;
    [SerializeField]
    private LayerMask m_AttackableLayer;
    [SerializeField]
    private float m_DamageValue = 1.0f;
    [SerializeField]
    private float m_TimeBtmAttacks = 0.15f;

    private float m_AttackTimeCounter;

    private RaycastHit2D[] m_Hits;

    private Animator m_Anim;

    private List<IDamageable> m_Damages = new List<IDamageable>();

    public bool ShouldBeDamaging { get; private set; } = false;

    private void Awake()
    {
        m_Anim = GetComponent<Animator>();

        m_AttackTimeCounter = m_TimeBtmAttacks;
    }

    private void Update()
    {
        if (UserInput.Instance.Controls.Attack.Attack.WasPressedThisFrame() && m_AttackTimeCounter >= m_TimeBtmAttacks)
        {
            //Attack();
            m_Anim.SetTrigger("attack");
            m_AttackTimeCounter = 0;
        }

        m_AttackTimeCounter += Time.deltaTime;
    }

    private void Attack()
    {
        m_Hits = Physics2D.CircleCastAll(m_AttackTransform.position, m_AttackRange, transform.right, 0.0f, m_AttackableLayer);

        foreach (var hit in m_Hits)
        {
            var damage = hit.collider.gameObject.GetComponent<IDamageable>();
            if (damage != null && !damage.HasTakenDamage)
            {
                damage.Damage(m_DamageValue);
                m_Damages.Add(damage);
            }
        }

        m_Damages.ForEach(c =>
        {
            c.HasTakenDamage = false;
        });
        m_Damages.Clear();
    }

    public IEnumerator DamageWhileSlashIsActive()
    {
        ShouldBeDamageToTrue();

        while (ShouldBeDamaging)
        {
            m_Hits = Physics2D.CircleCastAll(m_AttackTransform.position, m_AttackRange, transform.right, 0.0f, m_AttackableLayer);

            foreach (var hit in m_Hits)
            {
                var damage = hit.collider.gameObject.GetComponent<IDamageable>();
                if (damage != null && !damage.HasTakenDamage)
                {
                    damage.Damage(m_DamageValue);
                    m_Damages.Add(damage);
                }
            }
            yield return null;
        }

        m_Damages.ForEach(c => 
        {
            c.HasTakenDamage = false;
        });
        m_Damages.Clear();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(m_AttackTransform.position, m_AttackRange);
    }

    #region Anim Call
    public void ShouldBeDamageToTrue()
    {
        ShouldBeDamaging = true;
    }

    public void ShouldBeDamageToFalse()
    {
        ShouldBeDamaging = false;
    }
    #endregion
}
