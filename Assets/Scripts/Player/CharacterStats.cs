using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [SerializeField] protected Stat health;
    public bool isDead { get; protected set; }

    // Start is called before the first frame update
    public virtual void Start()
    {
        health.SetToMax();
        isDead = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual bool IncreaseHealth(int x)
    {
        if (health.GetCurrentValue() < health.GetMaxValue())
        {
            health.IncreaseStat(x);
            if (health.GetCurrentValue() >= health.GetMaxValue())
            {
                health.SetToMax();
            }
            Debug.Log(transform.name + " could heal", gameObject);
            return true;
        }
        else
        {
            Debug.Log(transform.name + "Health Max", gameObject);
            return false;
        }
    }

    public virtual void TakeDamage(int damage)
    {
        damage = Mathf.Clamp(damage, 0, int.MaxValue);

        health.DecreaseStat(damage);
        if (health.GetCurrentValue() < 0)
        {
            health.SetToZero();
            Die();
            Debug.Log(transform.name + " Is dead");
            return;
        }
        Debug.Log(transform.name + " Health takes " + damage + " damage.", gameObject);
    }

    public virtual void Die() //override for enemy, player etc
    {
        isDead = true;
        Debug.Log(transform.name + " died. ");
        //Destroy(gameObject);
    }
}
