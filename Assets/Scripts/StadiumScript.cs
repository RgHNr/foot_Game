using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StadiumScript : MonoBehaviour
{
    // Start is called before the first frame update
    public StadiumInfo stadiumSO;

    public GameObject stadiumImg;
    public OwnedPlayers ownedPlayers;
    int id;
     int price;

    public Button buyButton, useButton;

    StadiumScript[] allStadium;
    SaveManager saveManager;
    TMP_Text price__text;
    Shop shop;
    void Start()
    {
        saveManager = new SaveManager(ownedPlayers);
        buyButton = transform.GetChild(1).GetComponent<Button>();
       useButton= transform.GetChild(2).GetComponent<Button>();
        price__text = transform.GetChild(3).GetComponent<TMP_Text>();   
        Image Img = stadiumImg.GetComponent<Image>();
        Img.sprite = stadiumSO.img;
        id = stadiumSO.id;
        price = stadiumSO.price;
        price__text.text = stadiumSO.price.ToString("N0");
        checkStatus();
        shop = gameObject.transform.root.GetComponent<Shop>();
        Debug.Log(shop.name);
    }

    // Update is called once per frame
   

    public void buyStadium() {

        if (price <= ownedPlayers.Mycoins)
        {

            ownedPlayers.ownedStadiumsId.Add(id);
            findAllStadiums();      
            shop.AddCoins(-stadiumSO.price);
            saveManager.saveData();
        }
        else {
            GameObject.Find("Money_button").GetComponent<Button>().onClick.Invoke();
        }
            
    }
    public void useStadium()
    {

        ownedPlayers.StadiumID = id;
        findAllStadiums();
        
        saveManager.saveData();


    }

    public void checkStatus() {


        if (ownedPlayers.StadiumID == id)
        {
            buyButton.interactable = false;
            useButton.interactable = false;
        }
        if (ownedPlayers.ownedStadiumsId.Contains(id) && ownedPlayers.StadiumID != id)
        {
            buyButton.interactable = false;
            useButton.interactable = true;
        }
        if (!ownedPlayers.ownedStadiumsId.Contains(id))
        {

            buyButton.interactable = true;
            useButton.interactable = false;
        }
    }

    public void findAllStadiums() {
        allStadium = FindObjectsOfType(typeof(StadiumScript)) as StadiumScript[];
        foreach (var st in allStadium) {
            st.checkStatus();
        }

    }
    
}
