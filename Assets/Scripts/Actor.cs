using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    [SerializeField] protected float moveSpeed = 1f;
    [SerializeField] protected Animation2D idleAnim;
    [SerializeField] protected Animation2D moveAnim;
    [SerializeField] protected bool isFacingRight;
    [SerializeField] protected AudioSource moveSource;
    [SerializeField] protected AudioClip[] moveSounds;
    [SerializeField] protected int[] footstepFrames;

    protected new Rigidbody2D rigidbody2D;
    protected Animator2D animator2D;
    protected SpriteRenderer spriteRenderer;

    protected virtual void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator2D = GetComponent<Animator2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected virtual void Start()
    {
        animator2D.OnFrameChanged.AddListener(UpdateFootsteps);
    }

    protected void UpdateAnims(bool isMoving)
    {
        if (isMoving)
        {
            animator2D.Play(moveAnim, true);
            spriteRenderer.flipX = isFacingRight ? rigidbody2D.velocity.x < 0 : rigidbody2D.velocity.x > 0;
        }
        else
        {
            animator2D.Play(idleAnim, true);
        }
    }

    protected void UpdateFootsteps()
    {
        foreach (int frame in footstepFrames)
        {
            if (frame == animator2D.GetFrame())
            {
                PlayMoveSound();
                break;
            }
        }
    }

    protected void PlayMoveSound()
    {
        moveSource.PlayOneShot(moveSounds[Random.Range(0, moveSounds.Length)]);
    }
}
