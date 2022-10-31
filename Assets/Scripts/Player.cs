using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class Player : Actor
{
    [SerializeField] private float interactRadius = 1f;
    [SerializeField] private LayerMask interactLayer;
    [SerializeField] private Color hideColor;
    [SerializeField] private float hiddenSanityDrain = 0.01f;
    [SerializeField] private PostProcessVolume volume;
    [SerializeField] private float minVignette = 0.4f;
    [SerializeField] private float maxVignette = 0.6f;
    [SerializeField] private float minGrain = 0.1f;
    [SerializeField] private float maxGrain = 0.7f;
    [SerializeField] private float oneOffMultMin = 1f;
    [SerializeField] private float oneOffMultMax = 0.5f;
    [SerializeField] private float fogDelay = 15f;
    [SerializeField] private float fogInterval = 45f;
    [SerializeField] private float fogDuration = 15f;
    [SerializeField] private float fogSanityDrain = 0.02f;
    [SerializeField] private bool autoStart = false;

    public bool IsHidden { get; private set; }
    public float Sanity { get; private set; }

    private Interactable currInteractable = null;
    private bool canMove = true;
    private bool isFogActive = false;

    protected override void Awake()
    {
        base.Awake();

        GameManager.SetPlayer(this);
    }

    protected override void Start()
    {
        base.Start();

        SetSanity(1f);

        if (autoStart)
        {
            StartGameplay();
        }
    }

    public void StartGameplay()
    {
        StartCoroutine(FogLoop());
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

        if (IsHidden)
        {
            SetSanity(Sanity - hiddenSanityDrain * Time.deltaTime);
        }

        if (isFogActive)
        {
            SetSanity(Sanity - fogSanityDrain * Time.deltaTime);
        }

        UpdateAnims(movement != Vector2.zero);

        UpdateInteractables();
    }

    private IEnumerator FogLoop()
    {
        yield return new WaitForSeconds(fogDelay);

        while (true)
        {
            CameraController.SetFogActive(true);
            isFogActive = true;

            yield return new WaitForSeconds(fogDuration);

            CameraController.SetFogActive(false);
            isFogActive = false;

            yield return new WaitForSeconds(fogInterval);
        }
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

    public void SetCanMove(bool canMove)
    {
        this.canMove = canMove;
    }

    public void SetHiding(bool hiding)
    {
        IsHidden = hiding;
        spriteRenderer.color = hiding ? hideColor : Color.white;
        canMove = !hiding;
    }

    public void SetSanity(float sanity)
    {
        Sanity = sanity;

        if (volume.profile.TryGetSettings<Vignette>(out Vignette vignette))
        {
            vignette.intensity.value = Mathf.Lerp(minVignette, maxVignette, 1f - sanity);
        }

        if (volume.profile.TryGetSettings<Grain>(out Grain grain))
        {
            grain.intensity.value = Mathf.Lerp(minGrain, maxGrain, 1f - sanity);
        }

        float oneOffMult = Mathf.Lerp(oneOffMultMin, oneOffMultMax, 1f - sanity);
        MusicPlayer.SetOneOffMult(oneOffMult);
    }
}

