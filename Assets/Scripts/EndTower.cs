using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTower : Interactable
{
    [SerializeField] private CanvasGroup endScreen;

    public override void Use()
    {
        endScreen.alpha = 1f;
        GameManager.DestroyMonsters();
        GameManager.SpawnSystem.CancelSpawns();
    }
}
