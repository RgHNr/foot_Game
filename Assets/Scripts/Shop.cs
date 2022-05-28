using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [SerializeField]
    private Image _speedStat, _powerStat,_stamina,_agility;

    [SerializeField]
    private TMP_Text speedValue, powerValue,staminaValue,agilityValue;

    public TextMesh number_text;
    [SerializeField]
    bool fill;

    float startTime;


    [SerializeField]
    GameObject playerStatPanel, MyPlayersPanel, energyPanel, stadiumPanel, jerseyPanel, GetCoinsPanel, homePanel; 
    
    float elapsedTime;
    public GameObject player_Button;
    public GameObject content;
    public PlayerStats[] allPlayers;
    // Start is called before the first frame update

    public List<GameObject> currentOpenedPanels= new List<GameObject>();

    public GameObject Player;

    public Material player_mat;

    public Texture[] kits;
    int kitCounter;
    public ScrollRect scroll;
    public OwnedPlayers ownedPlayers;

    public Button[] KitUseButton;
    public TMP_Text coinsText;

    public Button buyButton;
    public TMP_Text PriceText;
    PlayerStats PlayerToBuy;
    int PlayerPrice;
    public Color[] colors;

    SaveManager SaveManager;

    public GameObject contractPanel;
    public GameObject mainContainer;
    public GameObject targetButton;

     GameObject boughtButton;
    public Button myTeamButton;
    public GameObject shineyImg1,shiney_img2;

    GameObject Img1,img2;

    public Button[] BuyKitButtons, useKitButtons;
    int KitID;

    public Image energyBarImg;
    int energyPrice;
    public TMP_Text ContractPriceText;
    private void Awake()
    {
        SaveManager = new SaveManager(ownedPlayers);
        getFomationData(SaveManager.GetData());
    }
    void Start()
    {


        player_mat.SetTexture("_BaseMap", ownedPlayers.OwnedTextures[ownedPlayers.currentTextureID]);
        //CheckOwnedKits(KitUseButton);
        CheckKitsButtons();
        availablePlayers();
        scroll.horizontalNormalizedPosition = 0;
        _speedStat.fillAmount = 0;
        _powerStat.fillAmount = 0;
        fill = true;
        startTime = Time.time;
        updateCoins();
        number_text.color = ownedPlayers.NumberColor;
        myTeamButton.onClick.Invoke();
        energyBarImg.fillAmount = ownedPlayers.energyValue;
    }

    // Update is called once per frame
    void Update()
    {


        //if (elapsedTime <= 5)
        //{
        //    lerpImageValue(70f, _speedStat, speedValue);
        //    lerpImageValue(85f, _powerStat, powerValue);
        //}
        if (scroll.horizontalNormalizedPosition < 0)
            scroll.horizontalNormalizedPosition = 0;

        if (scroll.horizontalNormalizedPosition > 1)
            scroll.horizontalNormalizedPosition = 1;
    }

    public void lerpImageValue(float value, Image img,TMP_Text text) {

        elapsedTime = Time.time - startTime;   
       
        
        if (fill)
        {
            //Debug.Log("lerping");
            img.fillAmount = Mathf.Lerp(0, value / 100, 0.7f*elapsedTime/startTime);

        }
       
        
        if (!fill) {

            img.fillAmount = Mathf.Lerp(img.fillAmount, 0, 0.7f * Time.deltaTime);
        }
        text.SetText((img.fillAmount*100).ToString("F0")+"/100");
    }

    public void availablePlayers() {

        foreach (Transform ch in content.transform)
        {
            Destroy(ch.gameObject);
        }
        Debug.Log("array size: " + ownedPlayers.allPlayers.Length);
        for (int i = 0; i < ownedPlayers.allPlayers.Length; i++)
        {
            int x = i;
            if (ownedPlayers.ownedPlayers.Contains(ownedPlayers.allPlayers[x]))
            {
                //but.GetComponent<Button>().image.color = Color.gray;
                //foreach (Image im in but.GetComponentsInChildren<Image>())
                //{
                //    im.color = Color.gray;
                //}
                continue;
            }

            GameObject but = Instantiate(player_Button);
            but.GetComponentInChildren<Text>().text =ownedPlayers.allPlayers[i].number.ToString();
            but.transform.SetParent(content.transform);
            but.GetComponent<Image>().sprite =ownedPlayers.allPlayers[x].img;
            but.transform.localScale = Vector3.one;
            but.GetComponent<Button>().onClick.AddListener (delegate { getPlayerStats(ownedPlayers.allPlayers[x]);
            
            });
            
        }

    }
    public void updateCoins() {

        coinsText.text = ownedPlayers.Mycoins.ToString("N0");
    }

    public void getFomationData(OwnedTosave OTS) {
        ownedPlayers.energyValue = OTS.energyValue;
        ownedPlayers.Mycoins = OTS.Mycoins;
        ownedPlayers.NumberColor = OTS.NumberColor;
        ownedPlayers.StadiumID = OTS.stadiumId;
        ownedPlayers.ownedStadiumsId = OTS.ownedStadium;
        ownedPlayers.currentTextureID = OTS.currentTextureID;
        int a = 0;
        
        foreach (var p in OTS.formaPlayers) {
            ownedPlayers.MyformationPlayers[a] = null;
            for (int i = 0; i < allPlayers.Length; i++) {

                if (allPlayers[i].number == p.number)
                {
                    Debug.Log("the player is: " + allPlayers[i] + p.number.ToString() +"   index: " + a);
                    ownedPlayers.MyformationPlayers[a] = allPlayers[i];
                    
                }
                
            }
            a++;
        }

        int b = 0;
        ownedPlayers.ownedPlayers.AddRange(new PlayerStats[OTS.owPlayers.Count-ownedPlayers.ownedPlayers.Count] ) ;
       
        foreach (var op in OTS.owPlayers) {

            for (int i = 0; i < allPlayers.Length; i++) {

                if (op.number == allPlayers[i].number) {
                    ownedPlayers.ownedPlayers[b] = allPlayers[i];
                }
            }
            b++;


        }

        
    }

    public void AddCoins(int amount) {
        ownedPlayers.Mycoins += amount;
        updateCoins();
        SaveManager.saveData();
    }



    public void getPlayerStats(PlayerStats playerStats) {
        if (ownedPlayers.ownedPlayers.Contains(playerStats))
        {
            PriceText.gameObject.SetActive(false);
            buyButton.interactable = false;
        }      
        else 
        {
            PlayerToBuy = playerStats;
            buyButton.interactable = true;
            PlayerPrice = playerStats.Price;
            PriceText.gameObject.SetActive(true);
            PriceText.text = PlayerPrice.ToString("N0");
            
        }
            
        Debug.Log("name of player stat: " + playerStats);
        _powerStat.fillAmount = 0;
        _speedStat.fillAmount = 0;
        _stamina.fillAmount = 0;
        _agility.fillAmount = 0;
        number_text.text = playerStats.number.ToString();
        StartCoroutine(LerpValues(playerStats.speed, playerStats.power, playerStats.stamina, playerStats.agility));

    }

    public void buyPlayer() { 
        if(PlayerToBuy != null)
        {

            if (PlayerToBuy.Price <= ownedPlayers.Mycoins)
            {
                mainContainer.transform.localScale = Vector3.one;
                contractPanel.SetActive(true);
                LeanTween.value(contractPanel, 0, 0.9f, 0.5f).setOnUpdate((float x) => { contractPanel.GetComponent<Image>().color = new Color(0, 0, 0, x); });
                LeanTween.moveLocalY(mainContainer,0f, 0.75f).setEaseInOutBack();
                ContractPriceText.text = PlayerToBuy.Price.ToString("N0")+" ?";
                
            }
            else {
                Debug.Log("not enough coins");
                coinsText.transform.parent.GetComponent<Button>().onClick.Invoke();
            }
        }
            
    }

    public void ConfirmBuy() {

        AddCoins(-PlayerToBuy.Price);
        ownedPlayers.ownedPlayers.Add(PlayerToBuy);
        PriceText.gameObject.SetActive(false);
        buyButton.interactable = false;
        availablePlayers();
        SaveManager.saveData();
        //LeanTween.value(contractPanel, 0.9f, 0, 0.75f).setOnUpdate((float x) => { contractPanel.GetComponent<Image>().color = new Color(0, 0, 0, x); }).setOnComplete(setNotACtive);
        LeanTween.scale(mainContainer, Vector3.zero, 0.4f).setOnComplete(closeContrctPanel);
        boughtButton = Instantiate(player_Button);
        Img1 = Instantiate(shineyImg1);
        img2 = Instantiate(shiney_img2);
        img2.transform.SetParent(transform);
        Img1.transform.SetParent(transform);
        RectTransform rect1 = Img1.GetComponent<RectTransform>();
        RectTransform rect2 = img2.GetComponent<RectTransform>();
        rectSettings(rect1);
        rectSettings(rect2);
        LeanTween.rotateZ(Img1, 360 * 2, 1.2f).setOnComplete(ScaleShineyImg);
        LeanTween.rotateZ(img2, -315 * 2, 1.2f);


        boughtButton.GetComponentInChildren<Text>().text = PlayerToBuy.number.ToString();
        boughtButton.transform.SetParent(transform);
        boughtButton.GetComponent<RectTransform>().anchorMin = new Vector2(.5f,.5f);
        boughtButton.GetComponent<RectTransform>().anchorMax = new Vector2(.5f, .5f);
        boughtButton.GetComponent<RectTransform>().sizeDelta = new Vector2(170, 170);


        boughtButton.transform.localScale = Vector3.zero;
        LeanTween.scale(boughtButton, Vector2.one, 0.2f).setOnComplete(moveAndScale);
    }
    public void ScaleShineyImg() {

        LeanTween.scale(Img1, Vector3.zero, 0.2f);
        LeanTween.scale(img2, Vector3.zero, 0.2f);
    }

    public void rectSettings(RectTransform rect)
    {
        rect.localPosition = Vector3.zero;
        rect.anchorMin = new Vector2(.5f, .5f);
        rect.anchorMax = new Vector2(.5f, .5f);
        rect.sizeDelta = new Vector2(400, 400);
    }
    public void moveAndScale() {

        LeanTween.scale(boughtButton, Vector3.zero, 0.7f).delay=1f;
        LeanTween.move(boughtButton, targetButton.transform, 0.7f).delay=1f;
    }

    public void closeContrctPanel()
    {
        LeanTween.value(contractPanel, 0.9f, 0, 0.75f).setOnUpdate((float x) => { contractPanel.GetComponent<Image>().color = new Color(0, 0, 0, x); }).setOnComplete(setNotACtive);
        LeanTween.moveLocalY(mainContainer, 600f, 0.75f).setEaseInOutBack();

        //LeanTween.alpha(contractPanel, 0f, 1f).setOnComplete(setNotACtive);
    }

    void setNotACtive() {
        contractPanel.SetActive(false);
    }

    IEnumerator LerpValues(float speed, float power, float stamina, float agility) {
        
        float start = 0;
        float rate = 1f/ 2f;
        while (start<1)
        {
            start += Time.deltaTime * rate;
            elapsedTime = Time.time - startTime;
            //source.localPosition = Vector3.Lerp(source.localPosition, target, (Time.time - startTime) / overTime);
            //lerpImageValue(speed, _speedStat, speedValue);
            //lerpImageValue(power, _powerStat, powerValue );
            _speedStat.fillAmount = Mathf.Lerp(0, speed/100, start);
            _powerStat.fillAmount = Mathf.Lerp(0, power / 100, start);
            _stamina.fillAmount = Mathf.Lerp(0, stamina/100, start);
            _agility.fillAmount = Mathf.Lerp(0, agility/100, start);

            speedValue.SetText((_speedStat.fillAmount * 100).ToString("F0"));
            powerValue.SetText((_powerStat.fillAmount * 100).ToString("F0"));
            staminaValue.SetText((_stamina.fillAmount * 100).ToString("F0"));
            agilityValue.SetText((_agility.fillAmount * 100).ToString("F0"));


            //lerpImageValue(stamina, _stamina, staminaValue);
            //lerpImageValue(agility, _agility,agilityValue);
            yield return null;
        }

    }


    public void OpenPanel(GameObject panel) {
        
        
        LeanTween.scale(panel, Vector3.one, 0.5f).setEaseInOutQuart();
        currentOpenedPanels.Add(panel);
        LeanTween.move(Player, new Vector3(1.82f, -0.35f, -7.12f), 1f).setEaseInOutQuart();
    }



    public void movePanelUp(GameObject panel) {
        LeanTween.moveLocalY(panel, -156.09f, 0.5f);
    }

    public void closePanels() {

        Debug.Log(currentOpenedPanels.Count);
       

            for (int i = 0; i < currentOpenedPanels.Count; i++)
            {
                LeanTween.scale(currentOpenedPanels[i], Vector3.zero, 0.5f).setEaseInBack();
                currentOpenedPanels.Remove(currentOpenedPanels[i]);
            }
            //foreach (var panel in currentOpenedPanels) {
            //    LeanTween.scale(panel, Vector3.zero, 0.5f).setEaseInOutQuart().setDelay(1f);
            //    currentOpenedPanels.Remove(panel);
            //}
       
    }
    public void resetContentPos(RectTransform content) {

        content.localPosition = new Vector3(0, content.localPosition.y, content.localPosition.z);


    }

    public void movePlayer()
    {

        LeanTween.move(Player, new Vector3(0.87f, 0.21f, -7.12f), 1f).setEaseInOutQuart();
    }

    public void CheckKitsButtons() {
        for (int i = 0; i < ownedPlayers.OwnedTextures.Count; i++)
        {
            if (ownedPlayers.ownedTexturesID.Contains(i))
            {
                if (ownedPlayers.currentTextureID == i)
                {

                    BuyKitButtons[i].interactable = false;
                    useKitButtons[i].interactable = false;
                }
                else
                {
                    BuyKitButtons[i].interactable = false;
                    useKitButtons[i].interactable = true;
                }
            }
            else {
                BuyKitButtons[i].interactable = true;
                useKitButtons[i].interactable = false;
            }
        }
        
    }

    public void setKkitID(int id) {

        KitID=id;

    }
    public void buyKit(int price) {

        if (price <= ownedPlayers.Mycoins)
        {

            ownedPlayers.ownedTexturesID.Add(KitID);
            CheckKitsButtons();
            AddCoins(-price);
            SaveManager.saveData();
        }
        else
        {
            coinsText.transform.parent.GetComponent<Button>().onClick.Invoke();
        }    
    }
    public void setTexture(Texture kit) {

        player_mat.SetTexture("_BaseMap", kit);
        ownedPlayers.kitTexture = kit;
        ownedPlayers.currentTextureID = KitID;
        CheckKitsButtons();
        SaveManager.saveData();
    }
    public void setBackNumberColor(int colorIndex) {

        ownedPlayers.NumberColor = colors[colorIndex];
        number_text.color = ownedPlayers.NumberColor;
        SaveManager.saveData();
    }
    public void changeKit() {

        if (scroll.horizontalNormalizedPosition >= 0.9)
            return;
        //scroll.horizontalNormalizedPosition=LeanTween.easeInOutQuart(scroll.horizontalNormalizedPosition, scroll.horizontalNormalizedPosition+0.2f,1f);
        LeanTween.value(scroll.horizontalNormalizedPosition, scroll.horizontalNormalizedPosition+0.25f, .5f).setOnUpdate((float a)=> { scroll.horizontalNormalizedPosition = a; } );
        Debug.Log("horizotalPos= " + scroll.horizontalNormalizedPosition);
    }
    public void moveLeft() {
        if (scroll.horizontalNormalizedPosition <= 0.1)
            return;
        LeanTween.value(scroll.horizontalNormalizedPosition, scroll.horizontalNormalizedPosition - 0.25f, .5f).setOnUpdate((float a) => { scroll.horizontalNormalizedPosition = a; });
        Debug.Log("horizotalPos= " + scroll.horizontalNormalizedPosition);
    }

    //public void BuyKit(Texture kit) {

    //    ownedPlayers.OwnedTextures.Add( kit);
    //    CheckOwnedKits(KitUseButton);
    //    SaveManager.saveData();
    //}

    //public void CheckOwnedKits(Button[] UseButtons) {

    //    foreach (int i in ownedPlayers.ownedTexturesID)
    //    {
    //        if (ownedPlayers.currentTextureID==i)
    //        {

    //            UseButtons[i].interactable = false;
    //        }
    //        else {
    //            UseButtons[i].interactable = true;
    //        }
    //    }
    //}
    public void setEnergyPrice(int price) {
        energyPrice = price;
    }
    public void fillEnergyBar(float value) {
     
        if (ownedPlayers.Mycoins < energyPrice) {
            Debug.Log("not enough coins");
            GameObject.Find("Money_button").GetComponent<Button>().onClick.Invoke();
            return;
        }

        if (ownedPlayers.energyValue == 1)
            return;
        
        LeanTween.value(energyBarImg.fillAmount, energyBarImg.fillAmount + value, 1f).setOnUpdate((float x) => energyBarImg.fillAmount = x);
        ownedPlayers.energyValue += value;
        ownedPlayers.energyValue = Mathf.Clamp01(ownedPlayers.energyValue);
        AddCoins(-energyPrice);

        SaveManager.saveData();
    }
}
