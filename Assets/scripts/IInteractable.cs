using System.Collections.Generic;
using UnityEngine;

// Anything the player can walk up to and hold the interact key on. The interactor finds the
// nearest registered one, floats the prompt over its Anchor, and calls Interact when the hold fills.
public interface IInteractable
{
    Transform Anchor { get; }
    void Interact(PlayerInteractor interactor);
}

// Shared registry so the interactor can find samples and the lever through one list.
public static class Interactables
{
    public static readonly List<IInteractable> All = new List<IInteractable>();
}
