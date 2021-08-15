using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item")]
public class Item : ScriptableObject
{
    //public new string name = "new Item"; //override old definition
    [TextArea(1, 20)] public string description;
    public Sprite icon = null;
}
