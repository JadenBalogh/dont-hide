using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Actor
{
    [SerializeField] private float followDelay = 0.2f;
    [SerializeField] private float leaveDelay = 0.6f;

    private Vector3 targetDir;
    private Coroutine leaveCR;

    protected void Update()
    {
        Player player = GameManager.Player;

        if (!player.IsHidden)
        {
            if (leaveCR != null)
            {
                StopCoroutine(leaveCR);
                leaveCR = null;
                Debug.Log("Stopping!!");
            }
            targetDir = player.transform.position - transform.position;
        }
        else
        {
            if (leaveCR == null) leaveCR = StartCoroutine(LeaveCR());
        }

        rigidbody2D.velocity = targetDir.normalized * moveSpeed;

        UpdateAnims(true);
    }

    private IEnumerator LeaveCR()
    {
        Debug.Log("Starting!!");

        Player player = GameManager.Player;

        yield return new WaitForSeconds(followDelay);

        targetDir = Vector3.zero;

        yield return new WaitForSeconds(leaveDelay);

        targetDir = transform.position - player.transform.position;

        while (player.IsHidden)
        {
            yield return null;
        }
    }
}
