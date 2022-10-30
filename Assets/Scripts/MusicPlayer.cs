using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    private static MusicPlayer instance;

    [SerializeField] private AudioSource loopSource;
    [SerializeField] private AudioSource ambientSource;
    [SerializeField] private AudioSource oneOffSource;
    [SerializeField] private AudioClip startAmbience;
    [SerializeField] private AudioClip[] oneOffs;
    [SerializeField] private float oneOffMinInterval = 4f;
    [SerializeField] private float oneOffMaxInterval = 10f;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    private void Start()
    {
        loopSource.clip = startAmbience;
        loopSource.Play();

        StartCoroutine(OneOffLoop());
    }

    private IEnumerator OneOffLoop()
    {
        yield return new WaitForSeconds(oneOffMaxInterval);

        while (true)
        {
            AudioClip oneOff = oneOffs[Random.Range(0, oneOffs.Length)];
            oneOffSource.PlayOneShot(oneOff);

            float interval = Random.Range(oneOffMinInterval, oneOffMaxInterval);
            yield return new WaitForSeconds(interval);
        }
    }

    public static void PlayAmbience(AudioClip ambience)
    {
        instance.ambientSource.clip = ambience;
        instance.ambientSource.Play();
    }
}
