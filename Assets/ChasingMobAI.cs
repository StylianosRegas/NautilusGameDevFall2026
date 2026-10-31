using UnityEngine;

public class ChasingMobAI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float cooldownTimer = 0f;
    public float damageCooldown = 4f;
    public Transform target;

    private void OnCollisionStay2D(Collision collision)
    {
        cooldownTimer -= Time.deltaTime;
        if (target && (cooldownTimer == 0))
        {
            Attack();
            cooldownTimer = damageCooldown;
        }
    }

    private void OnCollisionExit2D(Collision collision)
    {
        cooldownTimer = damageCooldown;
    }

    void Attack()
    { 
    
    }
}
