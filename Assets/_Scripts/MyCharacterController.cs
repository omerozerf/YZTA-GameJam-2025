using System;
using UnityEngine;
using UnityEngine.Serialization;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody2D))]
public class MyCharacterController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private CharacterColorType _characterColorType;
    [SerializeField] private float _jumpForce;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private Transform _groundCheckPoint;
    [SerializeField] private Transform _spawnPoint;
    
    private Rigidbody2D m_Rigidbody;
    private SpriteRenderer m_SpriteRenderer;
    private int m_DirectionMultiplier;
    
    private bool m_IsGrounded;
    private bool m_WasGrounded;

    
    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        
        ChangeColor(_characterColorType);
    }

    private void Update()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        Vector2 input = new Vector2(inputX, 0f);
        
        Move(input);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            TryJump();
        }

        // Add rotation effect only when airborne and moving horizontally
        if (!m_IsGrounded && Mathf.Abs(m_Rigidbody.velocity.x) > 0.1f)
        {
            float direction = Mathf.Sign(m_Rigidbody.velocity.x);
            transform.Rotate(Vector3.forward * -600f * direction * Time.deltaTime);
        }
    }

    private void FixedUpdate()
    {
        m_WasGrounded = m_IsGrounded;
        m_IsGrounded = Physics2D.OverlapCircle(_groundCheckPoint.position, 0.1f, _groundLayer);

        if (!m_WasGrounded && m_IsGrounded)
        {
            transform.DOPunchScale(new Vector3(0.2f, -0.1f, 0), 0.2f, 10, 1);
            transform.rotation = Quaternion.identity;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_groundCheckPoint.position, 0.1f);
    }


    public void Move(Vector2 input)
    {
        var velocity = new Vector2(input.x * m_DirectionMultiplier, 0f) * _moveSpeed;
        m_Rigidbody.linearVelocity = new Vector2(velocity.x, m_Rigidbody.linearVelocity.y);

        if (velocity.x > 0)
        {
            transform.localScale = new Vector3(1f, transform.localScale.y, transform.localScale.z);
        }
        else if (velocity.x < 0)
        {
            transform.localScale = new Vector3(-1f, transform.localScale.y, transform.localScale.z);
        }
    }

    public void ChangeColor(CharacterColorType newColorType)
    {
        _characterColorType = newColorType;
        m_DirectionMultiplier = (_characterColorType == CharacterColorType.Blue) ? 1 : -1;

        Color targetColor = (_characterColorType == CharacterColorType.Blue) ? Color.blue : Color.red;
        m_SpriteRenderer.DOColor(targetColor, 0.25f);

        transform.DOPunchScale(Vector3.one * 0.2f, 0.2f, 8, 1);
    }
    
    public void TryJump()
    {
        if (m_IsGrounded)
        {
            m_Rigidbody.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
            transform.DOScaleY(1.2f, 0.1f).SetLoops(2, LoopType.Yoyo);
        }
    }

    public CharacterColorType GetColorType()
    {
        return _characterColorType;
    }

    public void Die()
    {
        m_Rigidbody.linearVelocity = Vector2.zero;
        transform.position = _spawnPoint.position;
    }
}