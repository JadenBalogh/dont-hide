using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Actor
{
    protected void Update()
    {
        Player player = GameManager.Player;
        Vector3 targetDir;

        if (!player.IsHidden)
        {
            targetDir = player.transform.position - transform.position;
        }
        else
        {
            targetDir = transform.position - player.transform.position;
        }

        rigidbody2D.velocity = targetDir.normalized * moveSpeed;

        UpdateAnims(true);
    }
}
