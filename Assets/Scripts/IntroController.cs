using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IntroController : MonoBehaviour
{
    [SerializeField] private IntroSequence[] sequences;
    [SerializeField] private float seqDuration = 6f;
    [SerializeField] private float seqFadeTime = 2f;
    [SerializeField] private TextMeshProUGUI textbox;
    [SerializeField] private CanvasGroup backgroundGroup;
    [SerializeField] private CanvasGroup textGroup;
    [SerializeField] private AudioSource audioSource;

    private void Start()
    {
        StartCoroutine(IntroLoop());
    }

    private IEnumerator IntroLoop()
    {
        GameManager.Player.SetCanMove(false);

        foreach (IntroSequence seq in sequences)
        {
            textbox.text = seq.message;

            if (seq.audioClip != null)
            {
                audioSource.PlayOneShot(seq.audioClip);
            }

            StartCoroutine(FadeGroup(textGroup, true));
            yield return new WaitForSeconds(seqFadeTime);

            yield return new WaitForSeconds(seqDuration);

            StartCoroutine(FadeGroup(textGroup, false));
            yield return new WaitForSeconds(seqFadeTime);
        }

        StartCoroutine(FadeGroup(backgroundGroup, false));
        GameManager.Player.SetCanMove(true);
        GameManager.Player.StartGameplay();
    }

    private IEnumerator FadeGroup(CanvasGroup canvasGroup, bool active)
    {
        float target = active ? 1f : 0f;
        float fadeTimer = 0f;
        while (fadeTimer < seqFadeTime)
        {
            canvasGroup.alpha = Mathf.Lerp(1f - target, target, fadeTimer / seqFadeTime);
            fadeTimer += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = target;
    }

    [System.Serializable]
    public class IntroSequence
    {
        [TextArea] public string message;
        public AudioClip audioClip;
    }
}
