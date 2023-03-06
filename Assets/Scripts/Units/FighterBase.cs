using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class FighterBase : MonoBehaviour
{
    const float MINIMUM_ATTACK_TIME = 0.3f;
    [SerializeField] private GameObject _enemy; // inimigo
    [SerializeField] private FighterSO _fighterSO;
    [SerializeField] private GameObject _avatar;
    [SerializeField] private float moveSpeed = 0f; // velocidade de movimento do personagem
    [SerializeField] private float jumpForce = 0f; // força do pulo
    [SerializeField] private float groundCheckRadius = 0.2f; // raio de detecção do chão
    [SerializeField] private Transform groundCheck; // ponto de detecção do chão
    [SerializeField] private Transform handAttack; // ponto de ataque
    [SerializeField] private Transform footAttack; // ponto de ataque
    [SerializeField] private AudioClip damageSound;
    [SerializeField] private AudioClip deathSound;

    private AudioSource audioSource;
    public LayerMask groundLayer; // camada do chão
    private Rigidbody2D rb; // referência ao componente Rigidbody2D
    private Animator anim; // referência ao componente Animator
    private bool defending = false; // indica se o personagem está defendendo
    private bool isGrounded = false; // indica se o personagem está no chão
    private float horizontalInput = 0; // entrada horizontal do jogador
    private float health = 100f; // vida do personagem
    private float attackTimer; // contador de tempo entre ataques
    private bool _isenemyNotNull;


    public float timeBetweenAttacks = 1f; // tempo entre ataques
    public FighterSO FighterSO => _fighterSO;
    public FighterBase Enemy => _enemy.GetComponent<FighterBase>();
    public float Health => health;
    public bool IsDead => health <= 0;
    public bool CanAttack => attackTimer <= 0;


    public void SetEnemy(GameObject enemy)
    {
        _enemy = enemy;
        _isenemyNotNull = _enemy != null;
    }

    public int GetEnemyDirection()
    {
        if (_isenemyNotNull)
        {
            return _enemy.transform.position.x > transform.position.x ? 1 : -1;
        }

        return 1;
    }

    public void Punch()
    {
        if (health <= 0) return;

        anim.ResetTrigger("Punch");
        if (attackTimer > 0) return;
        defending = false;
        horizontalInput = 0;
        attackTimer = timeBetweenAttacks;
        anim.SetTrigger("Punch");
    }

    public void Kick()
    {
        if (health <= 0) return;

        anim.ResetTrigger("Kick");
        if (attackTimer > 0) return;
        attackTimer = timeBetweenAttacks;
        defending = false;
        horizontalInput = 0;
        anim.SetTrigger("Kick");
    }

    public void Jump()
    {
        if (health <= 0) return;

        if (isGrounded)
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse); // aplica a força do pulo
    }

    public void Move(float horizontalInput)
    {
        if (health <= 0) return;


        int faceDirection = GetEnemyDirection();
        defending = faceDirection == -1 && horizontalInput > 0 || faceDirection == 1 && horizontalInput < 0;
        this.horizontalInput = horizontalInput * (defending ? 0.5f : 1);
    }

    public void Special()
    {
        Debug.Log("FighterBase.Special()");
    }

    public void TakeDamage(float damage)
    {
        if (defending)
        {
            damage /= 2;
        }

        rb.AddForce(new Vector2(jumpForce * -GetEnemyDirection(), 0), ForceMode2D.Impulse);
        health = Math.Max(0, health - damage);
        audioSource.PlayOneShot(damageSound);
        if (health <= 0)
        {
            Die();
        }
    }

    public void DamageNearbyEnemiesAt(float damage, float range, Vector2 position)
    {
        anim.ResetTrigger("Punch");
        anim.ResetTrigger("Kick");
        var colliders = Physics2D.OverlapCircleAll(position, range);
        foreach (var collider in colliders)
        {
            var enemy = collider.GetComponent<FighterBase>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
    }

    public void PunchNearbyEnemiesAt()
    {
        DamageNearbyEnemiesAt(_fighterSO.Force / 10f, .25f, handAttack.position);
    }

    public void KickNearbyEnemiesAt()
    {
        DamageNearbyEnemiesAt(_fighterSO.Force / 10f, .25f, footAttack.position);
    }

    void Die()
    {
        anim.SetBool("IsDead", true);
    }

    private void Awake()
    {
        timeBetweenAttacks = MINIMUM_ATTACK_TIME / (_fighterSO.Agility / 100f);
        moveSpeed = _fighterSO.Agility / 10;
        jumpForce = _fighterSO.Force * 10;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // obtém a referência ao componente Rigidbody2D
        anim = GetComponent<Animator>(); // obtém a referência ao componente Animator
        audioSource = GetComponent<AudioSource>();
        if (_avatar != null && _fighterSO.Avatar != null)
            _avatar.GetComponent<SpriteRenderer>().sprite = _fighterSO.Avatar;
        _isenemyNotNull = _enemy != null;
    }

    void Update()
    {
        attackTimer -= Time.deltaTime; // decrementa o contador de tempo entre ataques

        if (health > 0)
            health = Math.Min(100, health + Time.deltaTime * .5f); // incrementa a vida do personagem
        anim.SetFloat("Speed", (horizontalInput)); // define a velocidade de movimento do personagem
        anim.SetBool("isGrounded", isGrounded); // define se o personagem está no chão
    }

    void FixedUpdate()
    {
        isGrounded =
            Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius,
                groundLayer); // verifica se o personagem está no chão
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y); // move o personagem na horizontal
        transform.localScale = new Vector3(GetEnemyDirection(), 1, 1); // inverte a direção do personagem
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}