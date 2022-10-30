using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform followTarget;
    [SerializeField] private float followTime = 0.2f;
    [SerializeField] private SpriteRenderer jumpscareBG;
    [SerializeField] private Animator2D jumpscareImg;
    [SerializeField] private Animation2D jumpscareAnim;
    [SerializeField] private AudioClip jumpscareClip;
    [SerializeField] private float jumpscareTime = 0.25f;
    [SerializeField] private AudioSource jumpscareSource;

    private Vector2 currVel;

    private void FixedUpdate()
    {
        Vector2 targetPos = Vector2.SmoothDamp(transform.position, followTarget.position, ref currVel, followTime);
        transform.position = (Vector3)targetPos + Vector3.forward * transform.position.z;
    }

    public static void Jumpscare()
    {
        CameraController instance = Camera.main.GetComponent<CameraController>();
        instance.jumpscareBG.enabled = true;
        instance.jumpscareImg.GetComponent<SpriteRenderer>().enabled = true;
        instance.jumpscareImg.Play(instance.jumpscareAnim, false, false);
        instance.jumpscareSource.PlayOneShot(instance.jumpscareClip);
        instance.StartCoroutine(instance.JumpscareCR());
    }

    private IEnumerator JumpscareCR()
    {
        yield return new WaitForSeconds(jumpscareTime);
        jumpscareImg.GetComponent<SpriteRenderer>().enabled = false;
    }
}
