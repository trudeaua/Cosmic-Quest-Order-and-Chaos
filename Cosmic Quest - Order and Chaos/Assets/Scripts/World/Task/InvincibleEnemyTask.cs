using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvincibleEnemyTask : Task
{
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayersInTaskArea += 1;
            if (PlayersInTaskArea == numPlayers && !started)
            {
                CloseDoors();
                Puzzle[] puzzles = GetComponents<Puzzle>();
                if (puzzles != null)
                {
                    _Puzzles = puzzles;
                }
                SetupTask();
                PlayIntroDialogue();
            }
        }
    }
}
