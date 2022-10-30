using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Actor
{
    [SerializeField] private float followDelay = 0.2f;
    [SerializeField] private float leaveDelay = 0.6f;
    [SerializeField] protected AudioSource screechSource;

    private Vector3 targetDir;
    private Coroutine leaveCR;
    private bool isColliding = false;

    protected override void Start()
    {
        base.Start();

        if (!GameManager.Player.IsHidden)
        {
            screechSource.Play();
        }
    }

    protected void Update()
    {
        Player player = GameManager.Player;

        if (!player.IsHidden)
        {
            if (leaveCR != null)
            {
                StopCoroutine(leaveCR);
                leaveCR = null;
                screechSource.Play();
            }
            targetDir = player.transform.position - transform.position;
        }
        else
        {
            if (leaveCR == null)
            {
                leaveCR = StartCoroutine(LeaveCR());
            }
        }

        Vector3 shiftDir = Vector3.zero;

        if (isColliding)
        {
            shiftDir = Vector2.Perpendicular(targetDir).normalized;
        }

        rigidbody2D.velocity = (shiftDir + targetDir.normalized) * moveSpeed;

        UpdateAnims(true);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        isColliding = true;
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        isColliding = false;
    }

    private IEnumerator LeaveCR()
    {
        Player player = GameManager.Player;

        yield return new WaitForSeconds(followDelay);

        targetDir = Vector3.zero;
        screechSource.Stop();

        yield return new WaitForSeconds(leaveDelay);

        targetDir = transform.position - player.transform.position;

        while (player.IsHidden)
        {
            yield return null;
        }
    }
}
