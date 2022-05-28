using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class UIManager : MonoBehaviour
{
    
    public Image healthBar;

    public Image chancesBar;

    public Image[] playersImage;

    //float health, maxHealth=100;

    float lerpHealthSpeed;

    public float runDam;
    public float healPoints;
    RectTransform rect;
    float rectWidth,rectHeight;


    public TMP_Text timerText;
    private float secondsCount;
    private int minuteCount;
    private int hourCount;
    // Start is called before the first frame update
    public  Slider curvePowerScrollBar;

    public GameObject formationPanel;
    public GameObject PausePanel, ContentPause;
    void Start()
    {
        Application.targetFrameRate = -1;
        //health = maxHealth;
        rect = GetComponent<RectTransform>();
        rectWidth = Screen.width;
        rectHeight = Screen.height;       
    }

    // Update is called once per frame
    void Update()
    {
        lerpHealthSpeed = 3f * Time.deltaTime;

        countUpTime();

        //if (Input.GetKey(KeyCode.Space))
        //{

        //    runDamage();
            
        //}
        //else {
        //    fillRun();
        //}

        //runHealth();
    }

    public void openPausePanel() {
        LeanTween.value(PausePanel, 0f, 0.9f, 0.75f).setOnUpdate((float x) => { PausePanel.GetComponent<Image>().color = new Color(0, 0, 0, x); }).setIgnoreTimeScale(true);
        LeanTween.moveLocalY(ContentPause, 0f, 0.75f).setEaseInOutBack().setIgnoreTimeScale(true);
        PausePanel.GetComponent<Image>().raycastTarget = true;
        TimeManager.pauseGame();
        SphereMove.isPaused = true;
    }
    public void ClosePause() {

        LeanTween.value(PausePanel, 0.9f, 0, 0.75f).setOnUpdate((float x) => { PausePanel.GetComponent<Image>().color = new Color(0, 0, 0, x); }).setIgnoreTimeScale(true);
        LeanTween.moveLocalY(ContentPause, 600f, 0.75f).setEaseInOutBack().setOnComplete(resumeGame).setIgnoreTimeScale(true);
    }

    public void resumeGame() {
        Time.timeScale = 1;
        SphereMove.isPaused = false;
        PausePanel.GetComponent<Image>().raycastTarget = false;
    }
    public void runHealth(float health, float maxHealth) {
        healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount,health/maxHealth, lerpHealthSpeed);
    }

    public void disbaleImage(int index) {
        playersImage[index].gameObject.SetActive(false) ;
    }

    public void playerImageVisual(int index,float Xpostion,float rayYheight,float direction) {

        playersImage[index].gameObject.SetActive(true);
        playersImage[index].GetComponent<RectTransform>().anchoredPosition = new Vector2(((rect.sizeDelta.x/2)-50) *direction,-rayYheight*((rect.sizeDelta.y/2)-50));

    }

    //public void runDamage(float health) {

    //    if (health > 2) {

    //        health -= runDam;

    //    }
    //}

    //public void fillRun(float health, float maxHealth) {
    //    if (health < maxHealth) {

    //        health += healPoints;
    //    }

    //}

    public void countUpTime() {
        secondsCount += Time.deltaTime*20;
        timerText.text =   minuteCount.ToString("00") + " : " + ((int)secondsCount).ToString("00") ;
        if (secondsCount >= 60)
        {
            minuteCount++;
            secondsCount = 0;
        }
        else if (minuteCount >= 90)
        {
            hourCount++;
            minuteCount = 0;
        }

    }

    public  float getCurvePower() {

        return curvePowerScrollBar.value;

       
    }

    public void OpenFormation() {
        SphereMove.isPaused = true;
        TimeManager.pauseGame();
        formationPanel.SetActive(true);
        formationPanel.GetComponent<FormationUI>().IsclickingUI();
        
    }
}
