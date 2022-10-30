using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float interactRadius = 1f;
    [SerializeField] private LayerMask interactLayer;
    [SerializeField] private Color hideColor;
    [SerializeField] private Animation2D idleAnim;
    [SerializeField] private Animation2D moveAnim;

    private Interactable currInteractable = null;
    private bool canMove = true;

    private new Rigidbody2D rigidbody2D;
    private SpriteRenderer spriteRenderer;
    private Animator2D animator2D;
    private AudioSource audioSource;

    private void Awake()
    {
        GameManager.SetPlayer(this);

        rigidbody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator2D = GetComponent<Animator2D>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        if (!canMove)
        {
            moveX = 0;
            moveY = 0;
        }

        Vector2 movement = new Vector2(moveX, moveY).normalized;
        rigidbody2D.velocity = movement * moveSpeed;

        bool isMoving = movement != Vector2.zero;

        if (isMoving)
        {
            animator2D.Play(moveAnim, true);
            spriteRenderer.flipX = rigidbody2D.velocity.x > 0;
        }
        else
        {
            animator2D.Play(idleAnim, true);
        }

        audioSource.mute = !isMoving;

        UpdateInteractables();
    }

    private void UpdateInteractables()
    {
        if (currInteractable != null)
        {
            currInteractable.SetFocused(false);
            currInteractable = null;
        }

        Collider2D col = Physics2D.OverlapCircle(transform.position, interactRadius, interactLayer);

        if (col != null && col.TryGetComponent<Interactable>(out Interactable target))
        {
            currInteractable = target;
            currInteractable.SetFocused(true);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            currInteractable.Use();
        }
    }

    public void SetHiding(bool hiding)
    {
        spriteRenderer.color = hiding ? hideColor : Color.white;
        canMove = !hiding;
    }
}
