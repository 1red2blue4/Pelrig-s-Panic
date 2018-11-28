﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour {

    public int health;
    public int damage;
    public bool canAttack = false;

    public void TakeDamage(int hit)
    {
        health -= hit;
        if (health  <= 0)
        {
            Destroy(gameObject);
        }
    }
}