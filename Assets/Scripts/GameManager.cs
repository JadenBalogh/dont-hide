using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public static Player Player { get; private set; }
    public static SpawnSystem SpawnSystem { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        SpawnSystem = GetComponent<SpawnSystem>();
    }

    public static void SetPlayer(Player player)
    {
        Player = player;
    }

    public static void DestroyMonsters()
    {
        foreach (Monster monster in GameObject.FindObjectsOfType<Monster>())
        {
            Destroy(monster.gameObject);
        }
    }
}
