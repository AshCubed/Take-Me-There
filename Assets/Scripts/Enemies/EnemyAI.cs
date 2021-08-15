using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    public enum EnemyStates {PATROL, CHASE, STUNNED, STOPPED };
    [SerializeField] private EnemyStates currentEnemyState;
    private NavMeshAgent agent;
    private Renderer enemyRenderer;
    private GameObject player;
    private PlayerStats playerStats;
    [SerializeField] private int damage = 10;
    [SerializeField] private float dmgIncrement = 1f;
    private string playerTag;
    private Camera cam;

    [SerializeField] private BoxCollider colliderArea;
    private Vector3 currentPatrolPoint;
    private Vector3 cubeSize;
    private Vector3 cubeCenter;
    [SerializeField] private GameObject stunedLighiting;

    private void Awake()
    {
        if (colliderArea)
        {
            Transform cubeTrans = colliderArea.gameObject.GetComponent<Transform>();
            cubeCenter = cubeTrans.position;

            // Multiply by scale because it does affect the size of the collider
            cubeSize.x = cubeTrans.localScale.x * colliderArea.size.x;
            cubeSize.y = cubeTrans.localScale.y * colliderArea.size.y;
            cubeSize.z = cubeTrans.localScale.z * colliderArea.size.z;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        stunedLighiting.SetActive(false);
        enemyRenderer = GetComponent<Renderer>();
        player = MainManager.instance.player;
        playerStats = player.GetComponent<PlayerStats>();
        playerTag = MainManager.instance.GetPlayerTag();
        cam = Camera.main;
        agent = GetComponent<NavMeshAgent>();
        EnemyStateChanger(EnemyStates.PATROL);

    }

    // Update is called once per frame
    void Update()
    {
        EnemyStateHandler();
        //CanSeePlayer();
        //IsPlayerFacingEnemy();
    }


    /// <summary>
    /// Check to see if the Enemy can just see the player
    /// </summary>
    /// <returns>true if the raycast has hit the player</returns>
    bool CanSeePlayer()
    {
        RaycastHit hit;
        Vector3 rayDir = player.transform.position - transform.position;
        Debug.DrawRay(transform.position, rayDir, Color.green);
        if (Physics.Raycast(transform.position, rayDir, out hit))
        {
            //Debug.Log(hit.collider.tag, hit.collider.gameObject);
            if (hit.transform.CompareTag(playerTag))
            {
                //Debug.Log("Player in sight!");
                return true;
            }
            else
            {
                //Debug.Log("Player NOT in sight!" + hit.transform.gameObject.name, gameObject);
                return false;
            }
        }
        return false;
    }
    
    /// <summary>
    /// Check to see if this enemy is being rendered thus is visible by the player
    /// </summary>
    /// <returns>true if this gameobject is being rendered</returns>
    bool IsPlayerFacingEnemy()
    {
        return enemyRenderer.IsVisibleFrom(cam);
    }

    /// <summary>
    /// Called in Update, handles what the enemy should be doing in each state
    /// </summary>
    void EnemyStateHandler()
    {
        switch (currentEnemyState)
        {
            case EnemyStates.PATROL:
                if (agent.remainingDistance <= agent.stoppingDistance)
                    EnemyStateChanger(EnemyStates.PATROL);
                else if (CanSeePlayer() && IsPlayerFacingEnemy())
                    EnemyStateChanger(EnemyStates.STOPPED);
                else if (CanSeePlayer())
                    EnemyStateChanger(EnemyStates.CHASE);
                break;
            case EnemyStates.CHASE:
                if (CanSeePlayer() && IsPlayerFacingEnemy())
                    EnemyStateChanger(EnemyStates.STOPPED);
                else if (!CanSeePlayer())
                    EnemyStateChanger(EnemyStates.PATROL);
                break;
            case EnemyStates.STUNNED:
                break;
            case EnemyStates.STOPPED:
                if (CanSeePlayer() && !IsPlayerFacingEnemy())
                    EnemyStateChanger(EnemyStates.CHASE);
                else if (!CanSeePlayer())
                    EnemyStateChanger(EnemyStates.PATROL);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Transitions to different enemy states, used in EnemyStateHandler
    /// </summary>
    /// <param name="state">EnemyStates to transition to</param>
    public void EnemyStateChanger(EnemyStates state)
    {
        currentEnemyState = state;

        if (state != EnemyStates.STOPPED) StopAllCoroutines();
        if (state != EnemyStates.STUNNED) stunedLighiting.SetActive(false);

        switch (currentEnemyState)
        {
            case EnemyStates.PATROL:
                currentPatrolPoint = GetRandomPosition();
                agent.SetDestination(currentPatrolPoint);
                break;
            case EnemyStates.CHASE:
                agent.SetDestination(player.transform.position);
                break;
            case EnemyStates.STUNNED:
                stunedLighiting.SetActive(true);
                StartCoroutine(DealStunDamage());
                break;
            case EnemyStates.STOPPED:
                agent.SetDestination(transform.position);
                StartCoroutine(DealDamage());
                break;
            default:
                break;
        }
    }

    private Vector3 GetRandomPosition()
    {
        if (colliderArea)
        {
            // You can also take off half the bounds of the thing you want in the box, so it doesn't extend outside.
            // Right now, the center of the prefab could be right on the extents of the box
            Vector3 randomPosition = new Vector3(
                Random.Range(-cubeSize.x / 2, cubeSize.x / 2),
                Random.Range(-cubeSize.y / 2, cubeSize.y / 2),
                Random.Range(-cubeSize.z / 2, cubeSize.z / 2));
            return cubeCenter + randomPosition;
        }
        else
        {
            Debug.Log("NO COLLIDER AREA ASSIGNED TO " + name, gameObject);
            return Vector3.zero;
        }
    }

    IEnumerator DealDamage()
    {
        yield return new WaitForSeconds(dmgIncrement);
        if (!playerStats.isDead)
        {
            playerStats.TakeDamage(damage);
            StartCoroutine(DealDamage());
        }
    }

    IEnumerator DealStunDamage()
    {
        yield return new WaitForSeconds(dmgIncrement);
        if (CanSeePlayer() && IsPlayerFacingEnemy())
            playerStats.TakeDamage(damage);
        StartCoroutine(DealStunDamage());
    }
}
