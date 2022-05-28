using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu()]
public class FormationPlayersUI : ScriptableObject
{
    // Start is called before the first frame update
    public Vector3[] UIpositions;
    public Vector3[] PlayersPositions;
}
