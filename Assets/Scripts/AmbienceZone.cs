using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbienceZone : MonoBehaviour
{
    [SerializeField] private AudioClip ambienceClip;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject == GameManager.Player.gameObject)
        {
            MusicPlayer.PlayAmbience(ambienceClip);
        }
    }
}
