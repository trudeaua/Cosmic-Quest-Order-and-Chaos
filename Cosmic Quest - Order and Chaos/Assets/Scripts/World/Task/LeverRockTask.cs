using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverRockTask : Task
{
    public Draggable[] rocks;
    protected CharacterColour[] activeColours;
    
    private void Awake()
    {
        _Puzzles = GetComponents<Puzzle>();
    }

    protected override void Start()
    {
        activeColours = PlayerManager.Instance.GetActivePlayerColours();
        rocks = GetComponentsInChildren<Draggable>();
        doors = GetComponentsInChildren<Door>();

        foreach (Draggable rock in rocks)
        {
            
        }
    }
}
