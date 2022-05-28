using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu()]
public class OwnedPlayers : ScriptableObject
{

    public PlayerStats[] allPlayers;
    public StadiumInfo[] allStadiums;
    public List<PlayerStats> ownedPlayers;

    public List<PlayerStats> MyformationPlayers;
    
    [Range(0,1)]
    public float energyValue;

    public Texture kitTexture;
    public Color NumberColor;
    public FormationPlayersUI myFormation;
    public List<Texture> OwnedTextures;
    public List<int> ownedTexturesID;
    public int currentTextureID;
    public int StadiumID;
    public List<int> ownedStadiumsId;

    public int Mycoins;
    
}
