using UnityEngine;

public class BossFight : MonoBehaviour
{
    public GameObject boss;
    [Tooltip("Should the music play right when the boss fight starts?")]
    public bool playMusicOnStart = true;
    
    public void StartFight()
    {
        GameManager.Instance.SetBossState();
        if (playMusicOnStart)
        {
            MusicManager.Instance.PlayMusic();
        }
        boss.SetActive(true);
    }

    public void BossDefeated()
    {
        GameManager.Instance.SetVictoryState();
    }
}