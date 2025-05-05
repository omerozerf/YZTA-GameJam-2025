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
    [SerializeField] private Sprite _blueSprite;
    [SerializeField] private Sprite _redSprite;
    [SerializeField] private AudioClip _jumpSound;
    [SerializeField] private AudioClip _dieSound;
    [SerializeField] private AudioSource _audioSource;
    
    private Rigidbody2D m_Rigidbody;
    private SpriteRenderer m_SpriteRenderer;
    private int m_DirectionMultiplier;

    private BoxCollider2D m_BoxCollider;
    
    private bool m_IsGrounded;
    private bool m_WasGrounded;

    
    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_BoxCollider = GetComponent<BoxCollider2D>();
        
        ChangeColor(_characterColorType);
    }

    private void Update()
    {
        var inputX = Input.GetAxisRaw("Horizontal");
        var input = new Vector2(inputX, 0f);
        
        Move(input);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            TryJump();
        }


        if (!m_IsGrounded)
        {
            var direction = Mathf.Sign(m_Rigidbody.linearVelocity.x);
            transform.Rotate(Vector3.forward * (-600f * direction * Time.deltaTime));
        }
    }

    private void FixedUpdate()
    {
        m_WasGrounded = m_IsGrounded;
        _groundCheckPoint.localRotation = Quaternion.identity;
        var boxSize = m_BoxCollider.size * 1.25f;
        m_IsGrounded = Physics2D.OverlapBox(_groundCheckPoint.position, boxSize, 0f, _groundLayer);

        if (!m_WasGrounded && m_IsGrounded)
        {
            transform.DOKill();
            transform.localScale = new Vector3(Mathf.Sign(transform.localScale.x), 1f, 1f);
            transform.DOPunchScale(new Vector3(0.2f, -0.1f, 0), 0.2f, 10, 1);
            float zRotation = transform.eulerAngles.z;
            float snappedZ = Mathf.Round(zRotation / 90f) * 90f;
            transform.rotation = Quaternion.Euler(0, 0, snappedZ);
        }
    }

    private void OnDrawGizmos()
    {
        BoxCollider2D col = GetComponent<BoxCollider2D>();
        if (col != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(_groundCheckPoint.position, col.size * 1.25f);
        }
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

        m_SpriteRenderer.sprite = (_characterColorType == CharacterColorType.Blue) ? _blueSprite : _redSprite;

        transform.DOKill();
        transform.localScale = new Vector3(Mathf.Sign(transform.localScale.x), 1f, 1f);
        transform.DOPunchScale(Vector3.one * 0.2f, 0.2f, 8, 1);
    }
    
    public void TryJump()
    {
        if (m_IsGrounded)
        {
            m_Rigidbody.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
            transform.DOScaleY(1.2f, 0.1f).SetLoops(2, LoopType.Yoyo);
            if (_jumpSound) _audioSource.PlayOneShot(_jumpSound);
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
        
        _audioSource.PlayOneShot(_dieSound);
    }
}