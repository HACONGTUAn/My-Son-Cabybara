using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseItem : MonoBehaviour, IDamage
{
    public int id;
    public int healthPoint;
    public bool inIce;
    public static event Action<BaseItem> OnDestroyed;
    public ClassicMode classicMode;
    public virtual void Initialize(int id)
    {
        this.id = id;
    }
    public virtual void TakeDamage(int damage)
    {
        healthPoint -= damage;
        if (healthPoint <= 0)
        {         
            DestroyObject();
        }
    }
    protected void DestroyObject()
    {
        Destroy(gameObject);
        OnDestroyed?.Invoke(this);
    }
}