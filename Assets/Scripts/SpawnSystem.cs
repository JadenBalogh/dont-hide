using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSystem : MonoBehaviour
{
    [SerializeField] private float minSpawnDist = 10f;
    [SerializeField] private float maxSpawnDist = 20f;
    [SerializeField] private Transform[] spawnpoints;
    [SerializeField] private Monster monsterPrefab;
    [SerializeField] private float maxSpawnInterval = 20f;
    [SerializeField] private float minSpawnInterval = 5f;
    [SerializeField] private float spawnSanityThreshold = 0.7f;

    private Coroutine spawnLoop;

    private void Start()
    {
        spawnLoop = StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            if (GameManager.Player.Sanity > spawnSanityThreshold || GameManager.Player.IsHidden)
            {
                yield return null;
                continue;
            }

            float spawnInterval = Mathf.Lerp(minSpawnInterval, maxSpawnInterval, GameManager.Player.Sanity);

            yield return new WaitForSeconds(spawnInterval);

            SpawnMonster();
        }
    }

    public void CancelSpawns()
    {
        StopCoroutine(spawnLoop);
    }

    public void SpawnMonster()
    {
        Player player = GameManager.Player;
        Transform targetSpawn = null;

        foreach (Transform spawnpoint in spawnpoints)
        {
            float playerDist = Vector2.Distance(player.transform.position, spawnpoint.position);

            if (playerDist > minSpawnDist && playerDist < maxSpawnDist)
            {
                targetSpawn = spawnpoint;
                break;
            }
        }

        if (targetSpawn == null)
        {
            return;
        }

        Instantiate(monsterPrefab, targetSpawn.position, Quaternion.identity);
    }
}
