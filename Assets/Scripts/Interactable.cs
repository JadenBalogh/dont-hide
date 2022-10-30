using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class Interactable : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI tooltip;

    public virtual void SetFocused(bool focused)
    {
        tooltip.enabled = focused;
    }

    public virtual bool IsUsable()
    {
        return true;
    }

    public abstract void Use();
}
