using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour {

    public int health;
    public int damage;
    public int meterUnitsFilled;
    public bool canAttack = false;
    [SerializeField] public int maxMeter;
    [SerializeField] public string characterName;

    public void TakeDamage(int hit)
    {
        health -= hit;
        if (health  <= 0)
        {
            health = 0;
        }
    }

    public void GainMeter(int amt)
    {
        meterUnitsFilled += amt;

        if (meterUnitsFilled > maxMeter)
        {
            meterUnitsFilled = maxMeter;
        }
    }

    public void EmptyMeter()
    {
        meterUnitsFilled = 0;
    }
}