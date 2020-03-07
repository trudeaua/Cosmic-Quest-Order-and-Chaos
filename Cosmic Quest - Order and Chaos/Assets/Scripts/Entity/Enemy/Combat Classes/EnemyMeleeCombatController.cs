using UnityEngine;

public class EnemyMeleeCombatController : EnemyCombatController
{
    [Header("Secondary Attack")]
    public float secondaryAttackCooldown = 1f;
    public float secondaryAttackDelay = 0.6f;
    [SerializeField] protected AudioHelper.EntityAudioClip secondaryAttackSFX;

    [Header("Tertiary Attack")]
    public float tertiaryAttackCooldown = 1f;
    public float tertiaryAttackDelay = 0.6f;
    [SerializeField] protected AudioHelper.EntityAudioClip tertiaryAttackSFX;
    
    public override void PrimaryAttack()
    {
        base.PrimaryAttack();
    }

    public override void SecondaryAttack()
    {
        base.SecondaryAttack();
    }

    public override void TertiaryAttack()
    {
        base.TertiaryAttack();
    }
}