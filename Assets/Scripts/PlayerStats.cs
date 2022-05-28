using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu() ]
public class PlayerStats : ScriptableObject
{
    // Start is called before the first frame update
    public float speed;
    public float power;
    public int number;
    public Sprite img;
    public int agility;
    public int stamina;
    public int Price;

}
