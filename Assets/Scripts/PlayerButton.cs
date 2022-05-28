using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerButton : EventTrigger
{
    // Start is called before the first frame update
    public PlayerStats playerStat;
    public GameObject teamPanel,content;
    public RectTransform[] emptyPlayersImgs;
    Vector3 startPos, currentPos;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        //Debug.Log(playerStat.name + " is being pressed");
    }

    public override void OnPointerDown(PointerEventData data)
    {
        Debug.Log("OnPointerDown called.");
        startPos = GetComponent<RectTransform>().localPosition;
        
        
    }
    public override void OnBeginDrag(PointerEventData data)
    {

        MyTeamManager myTeamManager = teamPanel.GetComponent<MyTeamManager>();
        Debug.Log("OnBeginDrag called.");
        transform.localScale = Vector3.one * 1.1f;
        transform.SetParent(teamPanel.transform);
        //if (System.Array.Exists(myTeamManager.mainPlayers, x=>x==playerStat)) {
        //    myTeamManager.AddToMainFormation(null, System.Array.IndexOf(myTeamManager.mainPlayers, playerStat));
        //    myTeamManager.mainPlayers[System.Array.IndexOf(myTeamManager.mainPlayers, playerStat)] = null;
        //}
        if (myTeamManager.myPlayers.MyformationPlayers.Contains(playerStat)) {
            if (System.Array.Exists(myTeamManager.mainPlayers, x => x == playerStat))
                Debug.Log("index of the dragged Player: " + myTeamManager.myPlayers.MyformationPlayers.IndexOf(playerStat));
                myTeamManager.under_stats[myTeamManager.myPlayers.MyformationPlayers.IndexOf(playerStat)].gameObject.SetActive(false);
                myTeamManager.mainPlayers[System.Array.IndexOf(myTeamManager.mainPlayers, playerStat)] = null;
                myTeamManager.myPlayers.MyformationPlayers[myTeamManager.myPlayers.MyformationPlayers.IndexOf(playerStat)] = null;
            //myTeamManager.AddToMainFormation(null, System.Array.IndexOf(myTeamManager.mainPlayers, playerStat));



        }

    }

    public override void OnDrag(PointerEventData data)
    {
        
        
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(teamPanel.GetComponent<RectTransform>(), Input.GetTouch(0).position, null, out pos);
        GetComponent<RectTransform>().localPosition = pos;
        //Debug.Log(pos);

        
        
    }

    public override void OnEndDrag(PointerEventData data)
    {
        MyTeamManager myTeamManager = teamPanel.GetComponent<MyTeamManager>();
        transform.localScale = Vector3.one;
        if (checkOverlap() != null)
        {
            if (myTeamManager.mainPlayers[System.Array.IndexOf(emptyPlayersImgs, checkOverlap())] == null)
            {
                GetComponent<RectTransform>().localPosition = checkOverlap().localPosition;
                myTeamManager.mainPlayers[System.Array.IndexOf(emptyPlayersImgs, checkOverlap())] = playerStat;
                myTeamManager.AddToMainFormation(playerStat, System.Array.IndexOf(emptyPlayersImgs, checkOverlap()));
                myTeamManager.under_stats[System.Array.IndexOf(emptyPlayersImgs, checkOverlap())].gameObject.SetActive(true);
                myTeamManager.under_stats[System.Array.IndexOf(emptyPlayersImgs, checkOverlap())] .text= "PWR: " + playerStat.power + "\n" + "SPD: " + playerStat.speed + "\n" + "STM: " + playerStat.stamina;
            }
            else {
                transform.localScale = Vector3.one;
                
                transform.SetParent(content.transform);
            }
            
            
            

        }
        else { 
        
            transform.localScale = Vector3.one;
            Debug.Log("OnEndDrag called.");
            transform.SetParent(content.transform);
        
        }
        teamPanel.GetComponent<MyTeamManager>().saveData();

        //LeanTween.moveLocal(this.gameObject, Vector3.zero, 0.5f);
        //GetComponent<RectTransform>().localPosition
    }

    public RectTransform checkOverlap() {

        foreach (RectTransform rectT in emptyPlayersImgs) {

            if (rectOverlaps(GetComponent<RectTransform>(), rectT)) {

                Debug.Log(rectT.gameObject.name + "is overlapping");
                return rectT;
            }
            
        }
        return null;
    }
    bool rectOverlaps(RectTransform rectTrans1, RectTransform rectTrans2)
    {
        Rect rect1 = new Rect(rectTrans1.localPosition.x, rectTrans1.localPosition.y, rectTrans1.rect.width, rectTrans1.rect.height);
        Rect rect2 = new Rect(rectTrans2.localPosition.x, rectTrans2.localPosition.y, rectTrans2.rect.width, rectTrans2.rect.height);

        return rect1.Overlaps(rect2);
    }
    
}
