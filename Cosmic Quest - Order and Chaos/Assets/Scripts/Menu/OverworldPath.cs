using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldPath : MonoBehaviour
{
    public OverworldLevel from;
    public OverworldLevel to;
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        SetPathVisibility(from.chaosVoid.cleared);
    }

    public void UnlockIfCleared()
    {
        if (from.chaosVoid.cleared)
        {
            SetPathVisibility(true);
            anim.SetTrigger("Unlock");
        }
    }

    private void SetPathVisibility(bool visible)
    {
        MeshRenderer pathMeshRenderer = GetComponent<MeshRenderer>();
        pathMeshRenderer.enabled = visible;
    }
}
