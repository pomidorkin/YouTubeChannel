using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SlimeParent : MonoBehaviour
{
    public bool hasEaten;
    public bool hasHunted;
    public bool triedReproduce;
    public bool altruist;
    public bool currupted;

    // Navigation
    public bool positionReached;
    protected abstract void Hunt();
}
