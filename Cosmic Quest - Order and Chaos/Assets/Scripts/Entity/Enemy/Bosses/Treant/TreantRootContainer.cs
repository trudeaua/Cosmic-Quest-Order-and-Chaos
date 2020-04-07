using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreantRootContainer : MonoBehaviour
{
    public List<TreantRoot> roots;

    public IEnumerator StartAttack()
    {
        foreach (TreantRoot root in roots)
        {
            root.StartAttack();
        }

        yield return null;
    }
}
