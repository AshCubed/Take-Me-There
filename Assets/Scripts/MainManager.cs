using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    #region Singleton

    public static MainManager instance;

    void Awake()
    {
        instance = this;
        canPlayerMove = true;
    }

    #endregion

    public GameObject player;

    [SerializeField] private string playerTag;

    [SerializeField] private string pickUpItem;

    [SerializeField] private string[] enemyTags;

    [SerializeField] private string mainPostProcessingTag;

    //A bool which controls if the player can move their camera
    //and if they can phyiscally move
    public bool canPlayerMove { get; set; }

    public bool isGamePaused { get; set; }

    public string GetPlayerTag()
    {
        return playerTag;
    }

    public string GetPickUpItemTag()
    {
        return pickUpItem;
    }

    public string[] GetEnemyTags()
    {
        return enemyTags;
    }

    public string GetMainPostProcessingTag()
    {
        return mainPostProcessingTag;
    }
}
