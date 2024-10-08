using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D m_Rigidbody;

    [Header("Move")]
    [SerializeField]
    private float m_MoveSpeed = 7.5f;
    private float m_MoveInput;

    [Header("Jump")]
    [SerializeField]
    private float m_JumpForce = 5.0f;
    [SerializeField]
    private float m_JumpTime = 0.5f;
    private float m_JumpTimeCounter;
    private bool m_IsJumping = false;
    private bool m_IsFalling;

    [Header("Ground")]
    [SerializeField]
    private float m_ExtraHeight = 0.25f;
    [SerializeField]
    private LayerMask m_Ground;

    [Header("Leg")]
    [SerializeField]
    private GameObject m_LeftLeg;
    [SerializeField]
    private GameObject m_RightLeg;

    private Collider2D m_Collider;
    private Animator m_Animator;
    private RaycastHit2D m_GroundHit;

    private Coroutine m_ResetTrigger;

    private bool m_IsFaceRight;
    public bool IsFaceRight { get => m_IsFaceRight; set => m_IsFaceRight = value; }

    private void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        m_Collider = GetComponent<Collider2D>();

        CheckDirectionCheck();
    }

    private void Update()
    {
        Move();
        Jump();
    }

    #region Movement
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
    #endregion

    #region Jump
    private void Jump()
    {
        if (UserInput.Instance.Controls.Jumping.Jump.WasPerformedThisFrame() && IsGrounded())
        {
            m_IsJumping = true;
            m_JumpTimeCounter = m_JumpTime;
            m_Rigidbody.velocity = new Vector2(m_Rigidbody.velocity.x, m_JumpForce);

            m_Animator.SetTrigger("jump");
        }

        if (UserInput.Instance.Controls.Jumping.Jump.IsPressed())
        {
            if (m_JumpTimeCounter > 0 && m_IsJumping)
            {
                m_Rigidbody.velocity = new Vector2(m_Rigidbody.velocity.x, m_JumpForce);
                m_JumpTimeCounter -= Time.deltaTime;
            }
            else if (!m_JumpTimeCounter.Equals(0))
            {
                m_IsFalling = true;
                m_IsJumping = false;
            }
            else
            {
                m_IsJumping = false;
            }
        }

        if (UserInput.Instance.Controls.Jumping.Jump.WasReleasedThisFrame())
        {
            m_IsFalling = true;
            m_IsJumping = false;
        }

        if (!m_IsJumping && CheckForLand())
        {
            m_Animator.SetTrigger("land");
            m_ResetTrigger = StartCoroutine(Reset());
        }
    }
    #endregion

    #region Turn Check
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

    private void CheckDirectionCheck()
    {
        m_IsFaceRight = m_RightLeg.transform.position.x > m_LeftLeg.transform.position.x;
    }
    #endregion

    #region Ground Check
    private bool IsGrounded()
    {
        m_GroundHit = Physics2D.BoxCast(m_Collider.bounds.center, m_Collider.bounds.size, 0f, Vector2.down, m_ExtraHeight, m_Ground);
        if (m_GroundHit.collider != null)
        {
            return true;
        }
        return false;
    }

    private bool CheckForLand()
    {
        if (m_IsFalling)
        {
            if (IsGrounded())
            {
                m_IsFalling = false;
                return true;
            }
        }
        return false;
    }

    private IEnumerator Reset()
    {
        yield return null;
        m_Animator.ResetTrigger("land");
    }
    #endregion
}
