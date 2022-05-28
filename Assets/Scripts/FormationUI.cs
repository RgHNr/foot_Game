using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FormationUI : MonoBehaviour
{
    public RectTransform origin;
    public RectTransform[] players;

    public OwnedPlayers ownedPlayers;
    LevelChanger levelChanger;
    public SphereMove ball;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < players.Length; i++)
        {
            players[i].gameObject.GetComponentInChildren<Text>().text = ownedPlayers.ownedPlayers[i].number.ToString();
        }

        //player.localPosition = Vector3.Lerp(player.localPosition, origin.localPosition + new Vector3(Random.Range(0, 21), Random.Range(0, 21), 0),0.1f*Time.deltaTime);
        makeFormation(ownedPlayers.myFormation);    
       
        levelChanger= levelChanger = GameObject.Find("LevelChanger").GetComponent<LevelChanger>();
    }
    

    public void makeFormation(FormationPlayersUI formation) {
        int i = 0;
        foreach (var player in players) {

            //player.localPosition = Vector3.Lerp(player.localPosition, origin.localPosition + new Vector3(Random.Range(0, 21), Random.Range(0, 21), 0),0.1f*Time.deltaTime);
            Vector3 newPos = formation.UIpositions[i];         
             StartCoroutine(MoveObject(player,newPos , 1.5f));
            i++;
        }

        ownedPlayers.myFormation = formation;
    }

    IEnumerator MoveObject(RectTransform source, Vector3 target, float overTime)
    {
        float startTime = Time.unscaledTime;
        while (Time.unscaledTime < startTime + overTime )
        {
            source.localPosition = Vector3.Lerp(source.localPosition, target, (Time.unscaledTime - startTime) / overTime);
            yield return null;
        }
        
    }

    public void CloseFormationPanel() {

        this.gameObject.SetActive(false);
        Time.timeScale = 1;
        levelChanger.outsideAnimation();
        IsclickingUI();
        SphereMove.isPaused = false;
    }

    public void IsclickingUI() {
        ball.isButtonClick = true;
        
    }
}
