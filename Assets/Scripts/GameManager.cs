using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update

    public OwnedPlayers ownedPlayers;

    public capsuleMove[] players;

    SaveManager saveManager;

    PlayerStats[] allPlayers;
    public Material stadium_Mat;
    private void Awake()
    {
        for (int i = 0; i < players.Length; i++)
        {
            players[i].PlayerStat = ownedPlayers.MyformationPlayers[i];
        }
        saveManager = new SaveManager(ownedPlayers);
        allPlayers = ownedPlayers.allPlayers;
        getFomationData(saveManager.GetData());
        if (SceneManager.GetActiveScene().buildIndex >= 3)
        {
            Instantiate(ownedPlayers.allStadiums[ownedPlayers.StadiumID].stadium);
            stadium_Mat.SetTexture("_BaseMap", ownedPlayers.OwnedTextures[ownedPlayers.currentTextureID]);
        }
            
    }

    public void getFomationData(OwnedTosave OTS)
    {
        ownedPlayers.energyValue = OTS.energyValue;
        ownedPlayers.Mycoins = OTS.Mycoins;
        ownedPlayers.NumberColor = OTS.NumberColor;
        ownedPlayers.StadiumID = OTS.stadiumId;
        ownedPlayers.ownedStadiumsId = OTS.ownedStadium;
        ownedPlayers.currentTextureID = OTS.currentTextureID;
        int a = 0;

        foreach (var p in OTS.formaPlayers)
        {
            ownedPlayers.MyformationPlayers[a] = null;
            for (int i = 0; i < allPlayers.Length; i++)
            {

                if (allPlayers[i].number == p.number)
                {
                    Debug.Log("the player is: " + allPlayers[i] + p.number.ToString() + "   index: " + a);
                    ownedPlayers.MyformationPlayers[a] = allPlayers[i];

                }

            }
            a++;
        }

        int b = 0;
        ownedPlayers.ownedPlayers.AddRange(new PlayerStats[OTS.owPlayers.Count - ownedPlayers.ownedPlayers.Count]);

        foreach (var op in OTS.owPlayers)
        {

            for (int i = 0; i < allPlayers.Length; i++)
            {

                if (op.number == allPlayers[i].number)
                {
                    ownedPlayers.ownedPlayers[b] = allPlayers[i];
                }
            }
            b++;


        }


    }

    // Update is called once per frame
    void Update()
    {

       

        


    }

    //IEnumerator generateGround() {

    //    while (true) {
    //        Instantiate(ground, ground.transform.position + new Vector3(0, 0, groundCount), Quaternion.identity);
    //        groundCount += 100;
    //        yield return new WaitForSeconds(5);
    //    }
    //}

    public void restart() {

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void loadFootballScene() {
        SceneManager.LoadScene(1);
    }
    public void loadShop() {
        SceneManager.LoadScene(2);
    }
  

    

    
}
