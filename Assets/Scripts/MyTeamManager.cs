using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
public class MyTeamManager : MonoBehaviour
{

    [SerializeField]
    RectTransform[] player_empty;
    public TMP_Text[] under_stats;


    public OwnedPlayers myPlayers;

    public GameObject butt_trigg;

    public GameObject content;
    // Start is called before the first frame update
    public PlayerStats[] mainPlayers = new PlayerStats[4];
    public SaveManager saveManager;
    bool isFormed;
    void Start()
    {
        
        saveManager = new SaveManager(myPlayers);
       
        ViewOwnedPlayers();
        for (int i = 0; i < myPlayers.MyformationPlayers.Count; i++)
        {
            mainPlayers[i] = myPlayers.MyformationPlayers[i];
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void MakeFormation() {
        if (isFormed)
            return;
        int i = 0;
        foreach (var p in myPlayers.MyformationPlayers) {
            if (p == null) {
                under_stats[i].gameObject.SetActive(false);
                i++;
                continue;

            }
            if (p != null) {
                GameObject but = Instantiate(butt_trigg);
                but.GetComponentInChildren<Text>().text = p.number.ToString();
                but.transform.SetParent(transform);
                but.transform.localScale = Vector3.one;
                but.GetComponent<RectTransform>().anchorMin = new Vector2(0, 1);
                but.GetComponent<RectTransform>().anchorMax = new Vector2(0, 1);
                but.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100);
                but.transform.localPosition = player_empty[i].localPosition;
                but.GetComponent<PlayerButton>().playerStat = p;
                but.GetComponent<PlayerButton>().teamPanel = this.gameObject;
                but.GetComponent<PlayerButton>().content = content;
                but.GetComponent<PlayerButton>().emptyPlayersImgs = player_empty;
                but.GetComponent<PlayerButton>().emptyPlayersImgs = player_empty;
                but.GetComponent<Image>().sprite = but.GetComponent<PlayerButton>().playerStat.img;
                under_stats[i].text = "PWR: " + p.power + "\n" + "SPD: " + p.speed + "\n" + "STM: " + p.stamina;
                i++;
            }
        }
        isFormed = true;
    }

    public void ViewOwnedPlayers() {

        foreach (Transform ch in content.transform) {
            Destroy(ch.gameObject);
        }
        Debug.Log("array size: " + myPlayers.ownedPlayers.Count);
        for (int i = 0; i < myPlayers.ownedPlayers.Count; i++)
        {
            
            int x = i;

            if (myPlayers.MyformationPlayers.Contains(myPlayers.ownedPlayers[i])) {
                continue;

            }
            GameObject but = Instantiate(butt_trigg);
            but.GetComponentInChildren<Text>().text = myPlayers.ownedPlayers[i].number.ToString();
            but.transform.SetParent(content.transform);
            but.transform.localScale = Vector3.one;
            but.GetComponent<PlayerButton>().playerStat = myPlayers.ownedPlayers[i];
            but.GetComponent<PlayerButton>().teamPanel=this.gameObject;
            but.GetComponent<PlayerButton>().content = content;
            but.GetComponent<PlayerButton>().emptyPlayersImgs = player_empty;
            but.GetComponent<Image>().sprite = but.GetComponent<PlayerButton>().playerStat.img;
            //but.GetComponent<Button>().onClick.AddListener(delegate {
            //    getPlayerStats(allPlayers[x]);

            //});
        }
    }

    public void AddToMainFormation(PlayerStats player, int index) {

        myPlayers.MyformationPlayers[index]=player;
        saveManager.saveData();
    }

    public void saveData() {

        saveManager.saveData();
    }
}
