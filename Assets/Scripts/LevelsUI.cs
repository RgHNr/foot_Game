using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class LevelsUI : MonoBehaviour
{
    // Start is called before the first frame update
    public OwnedPlayers ownedPlayers;
    public Image energyBarImg,energyimg2;
    int energyPrice;
    SaveManager saveManager;
    public GameObject energyPanel,mainContainer;
    public Transform targetButton,targetButton2;
    public TMP_Text coins_text;
    
    void Start()
    {
        saveManager = new SaveManager(ownedPlayers);
        energyimg2.fillAmount = ownedPlayers.energyValue;
        energyBarImg.fillAmount = ownedPlayers.energyValue;
        coins_text.text = ownedPlayers.Mycoins.ToString("N0");
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void ProceedToLoad(int sceneIndex)
    {
        if (ownedPlayers.energyValue >= 0.2f)
        {
            ownedPlayers.energyValue -= 0.2f;
            saveManager.saveData();
        }
        else {
            GameObject.Find("energy_Bar").GetComponent<Button>().onClick.Invoke();
            return;
        }


        bool isMissingAPlayer = false;
        foreach (var p in ownedPlayers.MyformationPlayers) {
            if (p == null) {
                isMissingAPlayer = true;
            }
            
        
        }

        if (isMissingAPlayer) return;

        //GameObject.Find("LevelLoader").GetComponent<LevelLoader>().FadeInAnim(sceneIndex);
        GameObject.Find("LevelChanger").GetComponent<LevelChanger>().startLevelLoading(sceneIndex);

    }
    public void openPanel() {
        mainContainer.transform.localPosition = targetButton.localPosition;
        energyPanel.SetActive(true);
        LeanTween.value(energyPanel, 0, 0.9f, 0.3f).setOnUpdate((float x) => { energyPanel.GetComponent<Image>().color = new Color(0, 0, 0, x); });
        LeanTween.scale(mainContainer, Vector3.one, 0.3f);
        LeanTween.move(mainContainer, targetButton2.transform, 0.3f);
    }
    public void closePanel() {
        LeanTween.value(energyPanel, 0.9f, 0, 0.5f).setOnUpdate((float x) => { energyPanel.GetComponent<Image>().color = new Color(0, 0, 0, x); }).setOnComplete(setNotActive);
        LeanTween.scale(mainContainer, Vector3.zero, 0.3f) ;
        LeanTween.move(mainContainer, targetButton.transform, 0.3f) ;
    }

    public void setNotActive() {

        energyPanel.SetActive(false);
    }
    public void setEnergyPrice(int price)
    {
        energyPrice = price;
    }
    public void fillEnergyBar(float value)
    {

        if (ownedPlayers.Mycoins < energyPrice)
        {
            Debug.Log("not enough coins");
            GameObject.Find("Money_button").GetComponent<Button>().onClick.Invoke();
            return;
        }
        if (ownedPlayers.energyValue == 1)
            return;



        LeanTween.value(energyBarImg.fillAmount, energyBarImg.fillAmount + value, 1f).setOnUpdate((float x) => energyBarImg.fillAmount = x);
        LeanTween.value(energyimg2.fillAmount, energyimg2.fillAmount + value, 1f).setOnUpdate((float x) => energyimg2.fillAmount = x);
        ownedPlayers.energyValue += value;
        ownedPlayers.energyValue = Mathf.Clamp01(ownedPlayers.energyValue);
        ownedPlayers.Mycoins -= energyPrice;
        coins_text.text = ownedPlayers.Mycoins.ToString("N0");
        saveManager.saveData();
    }

    
}
