using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.UI;
using TMPro;

public class LevelChanger : MonoBehaviour
{
    // Start is called before the first frame update

    public Animator anim;
    int levelToLoad;
    public Slider slider;
    public TMP_Text Loadingtext;
    public CanvasGroup alpha;

    public Image big, small;
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void FadeToLevel(int level) {
        levelToLoad = level;
        anim.SetTrigger("fadeOut");

    }

    public void onFadeComplete() {

        SceneManager.LoadScene(levelToLoad);
        
    }


    public void outsideAnimation() {

        anim.SetTrigger("outsideBall");
    }
    public void outsideBall() {
        Debug.Log("ball out");
        var resetPos = FindObjectsOfType<MonoBehaviour>().OfType<IResetPos>();
        foreach (IResetPos item in resetPos) {
            item.resetPos();
        }

        SphereMove.IsInGoal = false;
    }

    public void goalAnimation() {
        anim.SetTrigger("goal");
    }
    public void GoalRoutine(){
        
    }

    public void startLevelLoading(int index) {
        alpha.gameObject.SetActive(true);
        if (alpha != null)
            LeanTween.value(0, 1f, 0.5f).setOnUpdate((float x) => { if(alpha!=null)
                    alpha.alpha = x; });
        StartCoroutine(LoadAsycLevel(index));
    }
    IEnumerator LoadAsycLevel(int index) {

        
        AsyncOperation oper = SceneManager.LoadSceneAsync(index);
        LeanTween.rotateZ(big.gameObject, 720, 1f).setLoopPingPong();
        LeanTween.rotateZ(small.gameObject, -360, 1f).setLoopPingPong();
        while (!oper.isDone) {

            float progress = Mathf.Clamp01(oper.progress / 0.9f);
            slider.value = progress;
            Loadingtext.text = ((int)progress * 100).ToString() + "%";

            yield return null;
        }
    }
   
}
