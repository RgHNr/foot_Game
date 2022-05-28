using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;


public class SaveManager 
{
    private OwnedPlayers ownedPlayers;
    string savePath;

    public SaveManager(OwnedPlayers owPlayers) {

        this.savePath = Application.persistentDataPath + "/data.dat";
        this.ownedPlayers = owPlayers;
    }

    public void saveData() {

        string dataToSave = JsonUtility.ToJson(new OwnedTosave(ownedPlayers));
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(savePath);
        bf.Serialize(file, dataToSave);
        file.Close();


    }

    public OwnedTosave GetData() {

        if (File.Exists(savePath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(savePath, FileMode.Open);
            string OwData = (string)bf.Deserialize(file);

            OwnedTosave ow =JsonUtility.FromJson<OwnedTosave>(OwData);
            file.Close();
            return ow;
        }
        else
            return null;
           
    }

    //public OwnedPlayers getOwned(OwnedTosave OTS)
    //{
    //    OwnedPlayers newOwned = (OwnedPlayers)ScriptableObject.CreateInstance("OwnedPlayers");
    //    newOwned.ownedPlayers = new List<PlayerStats>();
    //    newOwned.MyformationPlayers = new List<PlayerStats>();
    //    foreach (var p in OTS.owPlayers)
    //    {
    //        newOwned.ownedPlayers.Add(OTS.dataToStats(p));
    //    }
    //    foreach (var p in OTS.formaPlayers)
    //    {
    //        if (p != null) { 
            
    //            newOwned.MyformationPlayers.Add(OTS.dataToStats(p));
    //            Debug.Log(p.number);
    //        }
            
    //    }
    //    return newOwned;

    //}
}


[System.Serializable]
public class OwnedTosave

{
   
    public List<PlayerToSave> owPlayers=new List<PlayerToSave>();
    public List<PlayerToSave> formaPlayers=new List<PlayerToSave>();

    public Texture kitTexture;
    public Color NumberColor;
    public FormationPlayersUI myFormation;
    public List<Texture> OwnedTextures;
    public int stadiumId;
    public List<int> ownedStadium;
    public int Mycoins;
    public List<int> ownedTexturesID;
    public int currentTextureID;
    public float energyValue;

    public OwnedTosave(OwnedPlayers owned)
    {
        Mycoins = owned.Mycoins;
        stadiumId = owned.StadiumID;
        ownedStadium = owned.ownedStadiumsId;
        NumberColor = owned.NumberColor;
        ownedTexturesID = owned.ownedTexturesID;
        currentTextureID = owned.currentTextureID;
        energyValue = owned.energyValue;

        foreach (PlayerStats p in owned.ownedPlayers) {
            Debug.Log(p.name);
            PlayerToSave PTS = new PlayerToSave(p);


            this.owPlayers.Add(PTS);
        }
        foreach(PlayerStats p in owned.MyformationPlayers)
        {
            if (p != null)
                formaPlayers.Add(new PlayerToSave(p));
            else
                formaPlayers.Add(null);

        }
    
    }

   

   

}

[System.Serializable]

public class PlayerToSave 
{

    public float speed;
    public float power;
    public int number;
    public Sprite img;
    public int agility;
    public int stamina;
    public int Price;

    public PlayerToSave(PlayerStats player) {

        speed = player.speed;
        power = player.power;
        number = player.number;
        agility = player.agility;
        stamina = player.stamina;
        Price = player.Price;
    }

}
