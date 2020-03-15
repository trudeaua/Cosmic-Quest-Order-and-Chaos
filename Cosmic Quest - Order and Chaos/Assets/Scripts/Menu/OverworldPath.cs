using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldPath : MonoBehaviour
{
    public OverworldLevel from;
    public OverworldLevel to;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        SetPathVisibility(from.chaosVoid.cleared);
    }

    // Update is called once per frame
    void Update()
    {
        
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
