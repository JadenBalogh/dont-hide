using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Actor
{
    [SerializeField] private float interactRadius = 1f;
    [SerializeField] private LayerMask interactLayer;
    [SerializeField] private Color hideColor;

    private Interactable currInteractable = null;
    private bool canMove = true;

    protected override void Awake()
    {
        base.Awake();

        GameManager.SetPlayer(this);
    }

    protected void Update()
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

        UpdateAnims(movement != Vector2.zero);

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
