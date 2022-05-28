using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class coinsManager : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject ParentPanel;
    public GameObject coinsContent;
    GameObject MadeCoinsPanel;
    public TMPro.TMP_Text coins_text;
    public OwnedPlayers ownedPlayers;
    void Start()
    {
        coins_text.text = ownedPlayers.Mycoins.ToString("N0");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenPanel(GameObject panel) {

        MadeCoinsPanel = Instantiate(panel);
        MadeCoinsPanel.transform.SetParent(ParentPanel.transform,false);
        RectTransform rect = MadeCoinsPanel.GetComponent<RectTransform>();
        MadeCoinsPanel.transform.localPosition = Vector3.zero;
        rect.anchorMin = new Vector2(0f, 0f);
        rect.anchorMax = new Vector2(1f, 1f);
        LeanTween.value(MadeCoinsPanel, 0, 0.9f, 0.5f).setOnUpdate((float x) => { MadeCoinsPanel.GetComponent<Image>().color = new Color(0, 0, 0, x); });
        coinsContent = MadeCoinsPanel.transform.GetChild(0).gameObject;
        LeanTween.moveLocalY(coinsContent, 0f, 0.75f).setEaseInOutBack();
        coinsContent.transform.GetChild(0).gameObject.GetComponent<Button>().onClick.AddListener(delegate { Destroy(MadeCoinsPanel.gameObject); });

    }

    public void closePanel() { 
    
    }
}
