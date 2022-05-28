using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    // Start is called before the first frame update

    Animator animator;

    int sceneToload;
    bool shouldFadeOut;
    private void Awake()
    {
        animator = GetComponent<Animator>();
       
        animator.SetTrigger("fadeOut");
       
        
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FadeInAnim(int SceneIndex)
    {
        animator.SetTrigger("fadeIn");
        sceneToload = SceneIndex;

    }

    public void loadScene() 
    {
        SceneManager.LoadScene(sceneToload);
        
    }
    public void setFadeBool(bool setBool) {
        shouldFadeOut = setBool;
    }

   
}
