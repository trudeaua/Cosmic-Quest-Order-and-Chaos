using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TorchPuzzle : Puzzle
{
    // List of torches involved with this puzzle
    public Torch[] torches;
    
    // Count of torches that are currently lit
    protected int NumLit => torches.Count(t => t.isLit);

    protected override void Start()
    {
        base.Start();
        
        // Subscribe to torch interaction events
        foreach (Torch torch in torches)
        {
            torch.onInteract += OnInteract;
        }
        
        // Invoke event based on current status
        if (NumLit == 0)
            SetComplete();
        else
            ResetPuzzle();
    }

    protected virtual void OnInteract(bool isLit)
    {
        if (NumLit == 0)
            SetComplete();
        else
            ResetPuzzle();
    }
}