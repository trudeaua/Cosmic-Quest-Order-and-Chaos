using UnityEngine;

public class BossFight : MonoBehaviour
{
    public GameObject boss;
    
    public void StartFight()
    {
        GameManager.Instance.SetBossState();
        MusicManager.Instance.PlayMusic();
        boss.SetActive(true);
    }

    public void BossDefeated()
    {
        GameManager.Instance.SetVictoryState();
    }
}