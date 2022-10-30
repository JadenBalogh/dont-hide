using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Animator2D : MonoBehaviour
{
    public UnityEvent OnFrameChanged { get; private set; }

    private bool isLocked;
    private Animation2D curr;
    private Animation2D prev;

    private Coroutine framesTimer;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        OnFrameChanged = new UnityEvent();
    }

    public void Play(Animation2D animation, bool looping, bool reset = false)
    {
        if (isLocked && looping) return;
        if (prev == animation && looping) return;

        if (!looping)
        {
            StartCoroutine(LockAnimation(animation, reset));
        }
        else
        {
            prev = animation;
        }

        PlayFrames(animation, looping);
    }

    public int GetFrame()
    {
        int idx = 0;

        foreach (Sprite frame in curr.Frames)
        {
            if (spriteRenderer.sprite == frame)
            {
                break;
            }
            idx++;
        }

        return idx;
    }

    private void PlayFrames(Animation2D animation, bool looping)
    {
        if (framesTimer != null) StopCoroutine(framesTimer);
        framesTimer = StartCoroutine(PlayFramesTimer(animation, looping));
    }

    private IEnumerator PlayFramesTimer(Animation2D animation, bool looping)
    {
        if (animation.FrameRate == 0 || animation.Frames.Length == 0)
        {
            Debug.LogError("ERROR: Animation either has no frames or 0 frame rate!");
            yield break;
        }

        curr = animation;

        WaitForSeconds frameDelay = new WaitForSeconds(1f / animation.FrameRate);
        while (true)
        {
            foreach (Sprite frame in animation.Frames)
            {
                spriteRenderer.sprite = frame;
                OnFrameChanged.Invoke();
                yield return frameDelay;
            }
            if (!looping) break;
        }
    }

    private IEnumerator LockAnimation(Animation2D animation, bool reset)
    {
        isLocked = true;
        yield return new WaitForSeconds(animation.Duration);
        if (reset) PlayFrames(prev, true);
        isLocked = false;
    }
}
