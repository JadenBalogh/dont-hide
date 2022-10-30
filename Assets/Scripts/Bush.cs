using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bush : Interactable
{
    [SerializeField] private string hideText = "Hide";
    [SerializeField] private string leaveText = "Leave";
    [SerializeField] private Transform playerHideLoc;
    [SerializeField] private Transform playerOutLoc;

    private bool isActive = false;

    public override void SetFocused(bool focused)
    {
        base.SetFocused(focused);

        tooltip.text = isActive ? leaveText : hideText;
    }

    public override void Use()
    {
        isActive = !isActive;

        Transform targetLoc = isActive ? playerHideLoc : playerOutLoc;
        GameManager.Player.transform.position = targetLoc.position;

        GameManager.Player.SetHiding(isActive);
    }
}
