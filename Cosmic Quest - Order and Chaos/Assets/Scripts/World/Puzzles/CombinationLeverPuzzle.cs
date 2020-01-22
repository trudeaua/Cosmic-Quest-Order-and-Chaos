using UnityEngine;

public class CombinationLeverPuzzle : LeverPuzzle
{
    // The expected order of colours
    [SerializeField] private CharacterColour[] combination;

    public new void AddColour(CharacterColour c)
    {
        _received.Add(c);

        // If we have the correct number of elements in buffer then check combination
        if (_received.Count == combination.Length)
        {
            for (int i = 0; i < combination.Length; i++)
            {
                // Incorrect combination
                if (_received[i] != combination[i])
                {
                    // Clear the received buffer
                    _received.Clear();
                    // Play a failure sound?
                    return;
                }
            }
            
            // Combination was correct
            SetComplete();
        }
    }
}
