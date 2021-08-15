using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private Transform currentCheckpoint;
    PlayerStats playerStats;
    string playerTag;
    bool hasPlayerPassedThrough;

    // Start is called before the first frame update
    void Start()
    {
        hasPlayerPassedThrough = false;
        playerStats = FindObjectOfType<PlayerStats>();
        playerTag = MainManager.instance.GetPlayerTag();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag) && !hasPlayerPassedThrough)
        {
            hasPlayerPassedThrough = true;
            playerStats.onNewCheckpointCallback.Invoke(currentCheckpoint);
            Debug.Log("CHECKPOINT");
        }
    }
}
