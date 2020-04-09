using UnityEngine;

public class BossFight : MonoBehaviour
{
    public GameObject boss;
    
    public void StartFight()
    {
        GameManager.Instance.SetBossState();
        boss.SetActive(true);
    }

    public void BossDefeated()
    {
        GameManager.Instance.SetPlayState();
    }
}