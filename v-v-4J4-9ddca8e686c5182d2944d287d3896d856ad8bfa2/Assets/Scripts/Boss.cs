using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public int damage = 20;
    private float damageCooldown = 1f;
    private float lastDamageTime;
    public Slider healthBar;
    public bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.gameObject.SetActive(true);
    }

    void Update()
    {
        healthBar.value = currentHealth;
    }

    public void TakeDamage(int damage)
    {
        if (Time.time >= lastDamageTime + damageCooldown)
        {
            currentHealth -= damage;

            PoingtAnim fist = GetComponent<PoingtAnim>();
            if (fist != null)
            {
                fist.PlayHurtAnimation();
            }

            HandAnim hand1 = GetComponentInChildren<HandAnim>();
            if (hand1 != null)
            {
                hand1.PlayHurtAnimation();
            }

            HandAnim2 hand2 = GetComponentInChildren<HandAnim2>();
            if (hand2 != null)
            {
                hand2.PlayHurtAnimation();
            }

            if (currentHealth <= 0)
            {
                Die();
            }

            lastDamageTime = Time.time;
            if (healthBar != null)
            {
                healthBar.value = currentHealth;
            }
        }
    }

    void Die()
    {
        Debug.Log("Boss mort");
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
        isDead = true;

        PoingtAnim fist = GetComponent<PoingtAnim>();
        if (fist != null)
        {
            fist.StopFistRoutine();
            fist.bossIsDead = true;

            fist.PlayDeathAnimation();
        }

        HandAnim hand1 = GetComponentInChildren<HandAnim>();
        if (hand1 != null)
        {
            hand1.StopHandRoutine();
            hand1.PlayDeathAnimation();
        }

        HandAnim2 hand2 = GetComponentInChildren<HandAnim2>();
        if (hand2 != null)
        {
            hand2.StopHandRoutine();
            hand2.PlayDeathAnimation();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerMovement player = collision.GetComponent<PlayerMovement>();
        HandAnim hand1 = GetComponent<HandAnim>();
        HandAnim2 hand2 = GetComponent<HandAnim2>();
        PoingtAnim fist = GetComponent<PoingtAnim>();
        
        if (player != null && ((hand1 != null && hand1.isMoving) || (hand2 != null && hand2.isMoving) || (fist != null && fist.isMoving)))
        {
            if (Time.time >= lastDamageTime + damageCooldown)
            {
                player.TakeDamage(damage);
                lastDamageTime = Time.time;
            }
            else if (player.GetComponent<PlayerCombat>().hasAttacked)
            {
                player.TakeDamage(damage);
                lastDamageTime = Time.time;
            }
        }
    }
}
