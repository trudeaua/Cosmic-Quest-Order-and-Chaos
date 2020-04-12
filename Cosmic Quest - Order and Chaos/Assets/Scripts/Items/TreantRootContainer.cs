using System.Collections.Generic;
using UnityEngine;

public class TreantRootContainer : MonoBehaviour
{
    public GameObject rootPrefab;
    public int numRoots;
    public float radius;
    
    private List<TreantRoot> _roots;

    private void Start()
    {
        _roots = new List<TreantRoot>();
        
        // Instantiate the roots
        for (int i = 0; i < numRoots; i++)
        {
            GameObject newRoot = Instantiate(rootPrefab, transform);
            _roots.Add(newRoot.GetComponent<TreantRoot>());
        }
    }

    public void StartAttack()
    {
        // Initiate each root's start
        foreach (TreantRoot root in _roots)
        {
            // Move root to random position
            Vector2 circlePos = Random.insideUnitCircle * radius;
            root.transform.position = transform.position + new Vector3(circlePos.x, 0, circlePos.y);
            StartCoroutine(root.StartAttack());
        }
    }
}
