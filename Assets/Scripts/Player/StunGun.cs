using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunGun : MonoBehaviour
{
    [SerializeField] private GameObject stunGun;
    [SerializeField] private GameObject crosshair;
    [SerializeField] private float speed;
    [SerializeField] private float returnSpeed;
    [SerializeField] private float distanceToReturn;
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private GameObject imgBullet;
    [SerializeField] private Animator gunSpriteAnimator;

    private GameObject _stunGunHolder;
    private Transform _startPos;
    private EnemyAI _currentEnemy;
    private Rigidbody _stunGunRb;
    private Collider _stunGunCol;
    private bool _hasLaunched;
    private PlayerStats playerStats;
    private int layer_mask;

    // Start is called before the first frame update
    void Start()
    {
        _stunGunHolder = stunGun.transform.parent.gameObject;
        _stunGunRb = stunGun.GetComponent<Rigidbody>();
        _stunGunCol = stunGun.GetComponent<Collider>();
        _stunGunCol.enabled = false;
        _stunGunRb.constraints = RigidbodyConstraints.FreezeAll;
        _startPos = _stunGunHolder.transform;
        _hasLaunched = false;
        playerStats = GetComponent<PlayerStats>();
        // _recall = false;
        imgBullet.SetActive(true);
        stunGun.GetComponent<StunHit>().onEnemyHitCallback += OnStunGunHit;
        stunGun.SetActive(false);
        layer_mask = LayerMask.GetMask("Enemy");
    }

    // Update is called once per frame
    void Update()
    {   
        /*_lineRenderer.SetPosition(0, MainManager.instance.player.transform.position);
        _lineRenderer.SetPosition(1, stunGun.transform.position);*/
    }

    private void FixedUpdate()
    {
        if (!playerStats.isDead && MainManager.instance.canPlayerMove) FireStunGun();
        /*if (IsLookingAtEnemy())
        {
            AudioManager.instance.PlaySounds("Breathing");
        }
        else
        {
            AudioManager.instance.PlaySounds("Breathing");
        }*/
    }

    bool IsLookingAtEnemy()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ViewportToWorldPoint(crosshair.transform.position),
            Camera.main.transform.forward, out hit, Mathf.Infinity, layer_mask))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void FireStunGun()
    {
        if (_hasLaunched == false)
        {
            RaycastHit hit;
            //Debug.DrawRay(Camera.main.ViewportToWorldPoint(crosshair.transform.position), Camera.main.transform.forward, Color.red);
            if (Physics.Raycast(Camera.main.ViewportToWorldPoint(crosshair.transform.position), 
                Camera.main.transform.forward, out hit, Mathf.Infinity, layer_mask))
            {
                //if (((IList)MainManager.instance.GetEnemyTags()).Contains(hit.collider.tag))
                {
                    //Debug.Log(hit.collider.tag, hit.transform.gameObject);
                    if (Input.GetButtonDown("Fire1"))
                    {
                        if(gunSpriteAnimator.GetCurrentAnimatorStateInfo(0).IsTag("spriteFire")) 
                            gunSpriteAnimator.SetTrigger("spriteFire");
                        stunGun.SetActive(true);
                        AudioManager.instance.PlaySounds("GunShoot");
                        imgBullet.SetActive(false);
                        _hasLaunched = true;
                        _stunGunCol.enabled = true;
                        _stunGunRb.useGravity = true;
                        _stunGunRb.constraints = RigidbodyConstraints.None;
                        _stunGunRb.AddForce((hit.point - stunGun.transform.position)
                                            * (float)(speed + _stunGunRb.velocity.magnitude));
                        stunGun.transform.parent = null;
                    }
                }
            }
        }
        else
        {
            if (Input.GetButtonDown("Fire1"))
            {
                RecallStunGun();
            }
        }

        if (_hasLaunched && _currentEnemy && Vector3.Distance(_currentEnemy.transform.position, transform.position) > distanceToReturn)
        {
            RecallStunGun();
        }

        void RecallStunGun()
        {
            if (_currentEnemy)
            {
                _currentEnemy.EnemyStateChanger(EnemyAI.EnemyStates.PATROL);
                _stunGunRb.useGravity = true;
                _stunGunRb.constraints = RigidbodyConstraints.None;
                stunGun.transform.parent = null;
                _currentEnemy = null;
            }

            stunGun.transform.LeanMove(_startPos.position, .2f).setOnComplete(() => {
                //_recall = false;
                if (!gunSpriteAnimator.GetCurrentAnimatorStateInfo(0).IsTag("spriteReload"))
                    gunSpriteAnimator.SetTrigger("spriteReload");
                stunGun.SetActive(false);
                AudioManager.instance.PlaySounds("GunReload");
                imgBullet.SetActive(true);
                _stunGunCol.enabled = false;
                _stunGunRb.useGravity = false;
                _stunGunRb.constraints = RigidbodyConstraints.FreezeAll;
                stunGun.transform.parent = _stunGunHolder.transform;
                stunGun.transform.position = _startPos.transform.position;
                _hasLaunched = false;
            });
            /*stunGun.transform.position = Vector3.MoveTowards(stunGun.transform.position, _startPos.position,
            returnSpeed * Time.deltaTime);*/
            //hook.transform.position = Vector3.Lerp(hook.transform.position, _startPos.position, returnSpeed * Time.deltaTime);

            /*var dist = Vector3.Distance(stunGun.transform.position, _startPos.position);
            if (dist <= 0.5f || stunGun.transform.position == _startPos.position)
            {
                _recall = false;
                _stunGunRb.useGravity = false;
                _stunGunRb.constraints = RigidbodyConstraints.FreezeAll;
                stunGun.transform.parent = _stunGunHolder.transform;
                stunGun.transform.position = _startPos.transform.position;
                _hasLaunched = false;
            }*/
        }
    }

    void OnStunGunHit(GameObject enemy)
    {
        if (((IList)MainManager.instance.GetEnemyTags()).Contains(enemy.tag))
        {
            _currentEnemy = enemy.GetComponent<EnemyAI>();
            _currentEnemy.EnemyStateChanger(EnemyAI.EnemyStates.STUNNED);
            _stunGunRb.useGravity = false;
            _stunGunRb.constraints = RigidbodyConstraints.FreezeAll;
            stunGun.transform.parent = _currentEnemy.gameObject.transform;
        }
    }
}
