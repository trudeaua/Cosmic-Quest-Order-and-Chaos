using System.Collections;
using UnityEngine;

public class MagmaRain : MonoBehaviour
{
    public float radius;
    public GameObject magmaBall;
    public float launchPeriod;

    private bool _isStarted;
    
    public void StartMagmaRain()
    {
        if (_isStarted) return;
        
        _isStarted = true;
        StartCoroutine(RainLoop());
    }

    public void StopMagmaRain()
    {
        _isStarted = false;
    }

    private IEnumerator RainLoop()
    {
        while (_isStarted)
        {
            GameObject ball = ObjectPooler.Instance.GetPooledObject(magmaBall);
            if (ball)
            {
                Vector3 pos = transform.position;
                Vector2 offset = Random.insideUnitCircle * radius;
                ball.transform.position = new Vector3(pos.x + offset.x, 30f, pos.z + offset.y);
                ball.SetActive(true);
            }
            
            yield return new WaitForSeconds(launchPeriod);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
