using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Actor
{
    protected void Update()
    {
        Vector3 playerPos = GameManager.Player.transform.position;

        Vector2 movement = (playerPos - transform.position).normalized;
        rigidbody2D.velocity = movement * moveSpeed;

        UpdateAnims(true);
    }
}
