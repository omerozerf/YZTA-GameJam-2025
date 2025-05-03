using System;
using UnityEngine;
using UnityEngine.Serialization;

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
    }

    private void FixedUpdate()
    {
        m_IsGrounded = Physics2D.OverlapCircle(_groundCheckPoint.position, 0.1f, _groundLayer);
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
    }

    public void ChangeColor(CharacterColorType newColorType)
    {
        _characterColorType = newColorType;
        m_DirectionMultiplier = (_characterColorType == CharacterColorType.Blue) ? 1 : -1;
        m_SpriteRenderer.color = (_characterColorType == CharacterColorType.Blue) ? Color.blue : Color.red;
    }
    
    public void TryJump()
    {
        if (m_IsGrounded)
        {
            m_Rigidbody.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
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