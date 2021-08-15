using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunHit : MonoBehaviour
{
    public delegate void OnEnemyHit(GameObject enemy);
    public OnEnemyHit onEnemyHitCallback;

    private void OnCollisionEnter(Collision collision)
    {
        onEnemyHitCallback.Invoke(collision.gameObject);
    }
}
