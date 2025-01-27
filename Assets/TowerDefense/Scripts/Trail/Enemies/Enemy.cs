using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public List<Effect> ActiveEffects;

    public Transform rootPart;
    public ParticleSystem bloodParticle;

    public int nodeIndex;
    public float damageResistance = 1f;
    public float maxHealth;
    public float health;
    public float speed;
    public int ID;

    private bool hasReachedEnd = false;


    private float slowDownTime = 0f;
    private float originalSpeed;
    private bool isSlowed = false;

    public delegate void EnemyReachedEndDelegate();
    public static event EnemyReachedEndDelegate OnEnemyReachedEnd;

    public void Init()
    {
        ActiveEffects = new List<Effect>();
        health = maxHealth;
        transform.position = GameLoopManager.nodePositions[0];
        nodeIndex = 0;

        originalSpeed = speed;

        if (bloodParticle != null)
        {
            bloodParticle.Stop();
        }
    }

    public void Tick()
    {
        // Handle active effects (such as damage-over-time)
        for (int i = 0; i < ActiveEffects.Count; i++)
        {
            if (ActiveEffects[i].ExpireTime > 0f)
            {
                if (ActiveEffects[i].DamageDelay > 0f)
                {
                    ActiveEffects[i].DamageDelay -= Time.deltaTime;
                }
                else
                {
                    GameLoopManager.EnqueueDamageData(new EnemyDamageData(this, ActiveEffects[i].Damage, 1f));
                    ActiveEffects[i].DamageDelay = 1f / ActiveEffects[i].DamageRate;
                }

                ActiveEffects[i].ExpireTime -= Time.deltaTime;
            }
        }

        ActiveEffects.RemoveAll(x => x.ExpireTime <= 0f);

        if (isSlowed)
        {
            slowDownTime -= Time.deltaTime;
            if (slowDownTime <= 0f)
            {
                ResetSpeed(); 
            }
        }

        // Check if enemy has reached the last waypoint
        if (!hasReachedEnd && nodeIndex >= GameLoopManager.nodePositions.Length)
        {
            hasReachedEnd = true;
            OnEnemyReachedEnd?.Invoke();
        }
    }

    public void PlayBloodEffect()
    {
        if (bloodParticle != null)
        {
            bloodParticle.transform.position = rootPart.position;
            bloodParticle.Play();
        }
    }

    public void ApplySlowdown(float duration, float factor)
    {
        if (!isSlowed) 
        {
            isSlowed = true;
            slowDownTime = duration;
            speed *= factor;  
        }
    }

    private void ResetSpeed()
    {
        speed = originalSpeed;
        isSlowed = false;
    }
}
