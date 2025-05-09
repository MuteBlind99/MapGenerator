using System;
using System.Collections;
using ProceduralLevelGenerator.Unity.Generators.DungeonGenerator;
using Unity.VisualScripting;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    [SerializeField] private DungeonGeneratorBase generator;
    [SerializeField] private GameObject hub;
    [SerializeField] private GameObject dungeon;

    [SerializeField] public float speed = 3f;

    public Rigidbody2D rigidbody;

    // public Animator animator;

    public SpriteRenderer spriteRenderer;

    private Vector2 movement;

    private Coroutine dashCoroutine;

    private bool _canDash = true;

    [SerializeField] private float dashDistance = 3f;

    [SerializeField] private float dashDuration = 0.2f;

    [SerializeField] private float dashCooldown = 2f;
    
    public bool isInvincible = false;

    private bool dashPressed = false;
    
    public Weapon Weapon;
    
    Vector2 _mousePosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        //CurentPosPlayer = "Hub"
    }

    // Update is called once per frame
    void Update()
    {
        dashPressed = Input.GetKeyDown(KeyCode.Space);

        OnMove();
        if (dashPressed && _canDash)
        {
            Debug.Log("Space Pressed");
            Vector2 direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            StartCoroutine(Dash(direction));
        }
        if (Input.GetMouseButtonDown(0))
        {
            Weapon.Fire();
        }

        _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        
    }

    public void Generate()
    {
        //if(apuit sur a && curentPosPlayer == "Hub")
        //ui chargment.setactive(true)
        //hub.setacive(false)
        //dungeon.setactive(true)
        generator.Generate();
        AstarPath.active.Scan();
        //ui chargment.setactive(false)
        //currentPosPlayer = "Dungeon"

        //if(apuit sur a && curentPosPlayer == "Dungeon")
        //ui chargment.setactive(true)
        //hub.setacive(true)
        //dungeon.setactive(false)
        //ui chargment.setactive(false)
        //currentPosPlayer = "Hub"d
    }

    IEnumerator Dash(Vector2 direction)
    {
        _canDash = false;
        isInvincible = true;

        float elapsed = 0f;
        Vector2 startPosition = rigidbody.position;
        Vector2 targetPosition = startPosition + direction * dashDistance;

        while (elapsed < dashDuration)
        {
            rigidbody.MovePosition(Vector2.Lerp(startPosition, targetPosition, elapsed / dashDuration));
            elapsed += Time.deltaTime;
            yield return null;
        }

        rigidbody.MovePosition(targetPosition);

        isInvincible = false;
        yield return new WaitForSeconds(dashCooldown);
        _canDash = true;
    }

    void FixedUpdate()
    {
        rigidbody.linearVelocity = movement * speed;
        Vector2 aimDirection = _mousePosition - rigidbody.position;
        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg * 90;
        rigidbody.rotation = aimAngle;
    }

    void OnMove()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement = movement.normalized;

        //animator.SetFloat("Speed", movement.sqrMagnitude);

        if (movement.x != 0)
        {
            spriteRenderer.flipX = movement.x < 0;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        // if (other.CompareTag("Obstacle"))
        // {
        //     movement = Vector2.zero;
        // }
    }
}