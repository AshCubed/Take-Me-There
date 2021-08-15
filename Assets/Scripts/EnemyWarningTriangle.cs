using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class EnemyWarningTriangle : MonoBehaviour
{
    [SerializeField] private GameObject canvasToSpawn;
    [SerializeField] private GameObject img;
    [SerializeField] private Vector3 offset;

    private GameObject _player;
    private List<EnemiesWaypoint> enemiesWaypoints;
    private Camera _camera;

    private void Start()
    {
        enemiesWaypoints = new List<EnemiesWaypoint>();
        _player = MainManager.instance.player;
        _camera = Camera.main;
    }

    private void LateUpdate()
    {
        if (enemiesWaypoints.Count != 0)
        {
            enemiesWaypoints.ForEach(x => {
                Image image = x.img.GetComponent<Image>();

                float minX = image.GetPixelAdjustedRect().width / 2;
                float maxX = Screen.width - minX;

                float minY = image.GetPixelAdjustedRect().height / 2;
                float maxY = Screen.height - minY;


                Vector2 pos = _camera.WorldToScreenPoint(x.currentEnemy.transform.position + offset);

                if (Vector3.Dot((x.currentEnemy.transform.position - _player.transform.position), _player.transform.forward) < 0)
                {
                    //Debug.Log("0");
                    //Target is behind player
                    if (pos.x < Screen.width / 2)
                    {
                        //Debug.Log("1");
                        pos.x = maxX;
                    }
                    else
                    {
                        //Debug.Log("2");
                        pos.x = minX;
                    }
                }

                pos.x = Mathf.Clamp(pos.x, minX, maxX);
                pos.y = Mathf.Clamp(pos.y, minY, maxY);

                image.transform.position = pos;
            });
        }
    }

    public void SpawnTargetPos(GameObject enemy)
    {
        GameObject newGo1 = Instantiate(img, new Vector2(0f, 0f), Quaternion.identity, canvasToSpawn.transform);
        EnemiesWaypoint neww = new EnemiesWaypoint(enemy, newGo1);
        enemiesWaypoints.Add(neww);
    }

    public void DeleteTargetPos(EnemiesWaypoint enemiesWay)
    {
        Destroy(enemiesWay.img);
        enemiesWaypoints.Remove(enemiesWay);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((IList)MainManager.instance.GetEnemyTags()).Contains(other.tag))
        {
            SpawnTargetPos(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        EnemiesWaypoint go = enemiesWaypoints.Find(x => x.currentEnemy == other.gameObject);
        if (go != null)
        {
            DeleteTargetPos(go);
        }
    }
}

[System.Serializable]
public class EnemiesWaypoint
{
    public GameObject currentEnemy;
    public GameObject img;

    public EnemiesWaypoint()
    {

    }

    public EnemiesWaypoint(GameObject enemy, GameObject image)
    {
        currentEnemy = enemy;
        img = image;
    }
}