using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D m_Rigidbody;

    [SerializeField]
    private float m_MoveSpeed = 7.5f;
    private float m_MoveInput;

    [SerializeField]
    private GameObject m_LeftLeg;
    [SerializeField]
    private GameObject m_RightLeg;

    private Animator m_Animator;

    private bool m_IsFaceRight;
    public bool IsFaceRight { get => m_IsFaceRight; set => m_IsFaceRight = value; }

    private void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();

        CheckDirectionCheck();
    }

    private void Move()
    {
        m_MoveInput = UserInput.Instance.MoveInput.x;

        if (m_MoveInput > 0 || m_MoveInput < 0)
        {
            m_Animator.SetBool("IsWalking", true);
            TurnCheck();
        }
        else
        {
            m_Animator.SetBool("IsWalking", false);
        }

        m_Rigidbody.velocity = new Vector2(m_MoveInput * m_MoveSpeed, m_Rigidbody.velocity.y);
    }

    private void CheckDirectionCheck()
    {
        m_IsFaceRight = m_RightLeg.transform.position.x > m_LeftLeg.transform.position.x;
    }

    private void TurnCheck()
    {
        if (UserInput.Instance.MoveInput.x > 0 && !m_IsFaceRight)
        {
            Turn();
        }
        else if (UserInput.Instance.MoveInput.x < 0 && m_IsFaceRight)
        {
            Turn();
        }
    }

    private void Turn()
    {
        var rot = new Vector3(transform.rotation.x, m_IsFaceRight ? 180f : 0f, transform.rotation.z);
        transform.rotation = Quaternion.Euler(rot);
        m_IsFaceRight = !m_IsFaceRight;
    }

    private void Update()
    {
        Move();
    }
}
