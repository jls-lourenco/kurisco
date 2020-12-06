using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    private AudioController audioController;
    private LoadXml xml;
    public Animator screenAnimator;
    public Text[] textsScene;
    public enum screenPanel {MENU, OPTIONS, CREDITS, CONTROLS, HUD, PAUSE, GAMEOVER, STAGECLEAR, GAMECLEAR}
    public screenPanel sPanel;
    private float time;

    private void Awake() {

        audioController = FindObjectOfType(typeof(AudioController))	as AudioController;
        xml = FindObjectOfType(typeof(LoadXml)) as LoadXml;
        screenAnimator = GetComponent<Animator>();
        
    }
    private void OnEnable() {
        
        if(sPanel != screenPanel.HUD) audioController.playFx(audioController.fxMenuUp);
        
        fillTexts();
    }
    private void Start() 
    {
        if(audioController == null)SceneManager.LoadScene(0);
        
    }
    public void changeMusic(string nameScene)
    {
        AudioClip music = null;
        if(nameScene == "Menu") music = audioController.musicTitle;
        else music = audioController.musicStage[PlayerPrefs.GetInt("idStage")-1];
        audioController.StartCoroutine("changeMusic",music);
        
    }
    public void loadScene(string nameScene)
    {
        if(nameScene == "Stage1")
        {
            PlayerPrefs.SetInt("idStage", 1);
            PlayerPrefs.SetInt("qtdLifes", 4);
            PlayerPrefs.SetInt("qtdBombs", 1);
            
            PlayerPrefs.SetInt("slotShot0",0);
            PlayerPrefs.SetInt("slotShot1",0);
            
            PlayerPrefs.SetInt("idBulet", 0);
            PlayerPrefs.SetInt("index", 0);

            PlayerPrefs.SetInt("slotHud",0);
            PlayerPrefs.SetInt("slotUp0",0);
            PlayerPrefs.SetInt("slotUp1",0);
            
            PlayerPrefs.SetFloat("slotDamage0",.5f);
            PlayerPrefs.SetFloat("slotDamage1",1f);
            PlayerPrefs.SetFloat("slotDamage2",1.5f);
        }

        audioController.playFx(audioController.fxButton);
        screenAnimator.SetTrigger("popDown");
        StartCoroutine("waitForLoadScene",nameScene);
        
    }
    public void loadGame()
    {
        if(PlayerPrefs.GetInt("idStage") > 3)PlayerPrefs.SetInt("idStage",3);

        string nameScene = "Stage" + PlayerPrefs.GetInt("idStage");
        audioController.playFx(audioController.fxButton);
        screenAnimator.SetTrigger("popDown");
        StartCoroutine("waitForLoadScene",nameScene);
        
    }
    public void showScreen(GameObject panel)
    {
        audioController.playFx(audioController.fxButton);
        screenAnimator.SetTrigger("popDown");    
        StartCoroutine("waitForAnimation",panel);
        
    }
    public void resume(GameObject panelPause)
    {   
        Animator fume = panelPause.GetComponentInChildren<Animator>();
        if(Time.timeScale > 0)time = Time.timeScale;
        
        if(panelPause.activeSelf)
        { 
            audioController.playFx(audioController.fxButton);
            fume.SetTrigger("fadeIn");
            screenAnimator.SetTrigger("popDown");
            StartCoroutine("waitToResume", panelPause);
        }
        else 
        {
            panelPause.SetActive(true);
            audioController.music.Pause();
            Time.timeScale = 0;
            
        }
    }
    public void menu(Animator panelFadeOut)
    {
        panelFadeOut.SetTrigger("fadeOut");
        loadScene("Menu");

    }
    IEnumerator waitToResume(GameObject panelPause)
    {
        yield return new WaitUntil(() => transform.localScale == Vector3.zero);
        Time.timeScale = time;
        panelPause.SetActive(false);
        audioController.music.Play();
        
    }
    IEnumerator waitForAnimation(GameObject panel)
    {
        yield return new WaitUntil(() => transform.localScale == Vector3.zero);
        panel.SetActive(true);
        this.gameObject.SetActive(false);

    }
    IEnumerator waitForLoadScene(string nameScene)
    {
        //yield return new WaitUntil(() => transform.localScale == Vector3.zero);
        yield return new WaitForSecondsRealtime(1);
        Time.timeScale = 1;
        
        SceneManager.LoadScene(nameScene);

    }
    public void changeLanguage(string idiome)
    {
        audioController.playFx(audioController.fxButton);
        if(idiome == PlayerPrefs.GetString("idiome"))return;
        
        PlayerPrefs.SetString("idiome", idiome);
        xml.loadXmlData();
        fillTexts();
        
    }
    void fillTexts()
    {
        int id = 0;
        switch(sPanel)
        {
            case screenPanel.MENU:
                
                audioController.music.volume = PlayerPrefs.GetFloat("MusicVolume"); 
                Button load = textsScene[1].GetComponentInParent<Button>();
                Color c = textsScene[1].color;
                load.interactable = PlayerPrefs.GetInt("idStage") > 1 ? true : false;
                if(!load.interactable) textsScene[1].color = Color.gray;
                else textsScene[1].color = c;
                
                foreach(string t in xml.menu_screen)
                {
                    textsScene[id].text = t;
                    id += 1;
                }
                
                break;

            case screenPanel.OPTIONS:
                audioController.music.volume = 0.2f; 
                foreach(string t in xml.options_screen)
                {
                    textsScene[id].text = t;
                    id += 1;
                }
                break;

            case screenPanel.CREDITS:
                audioController.music.volume = 0.2f; 
                foreach(string t in xml.credits_screen)
                {
                    textsScene[id].text = t;
                    id += 1;
                }
                break;
            
            case screenPanel.CONTROLS:
                audioController.music.volume = 0.2f; 
                foreach(string t in xml.controls_screen)
                {
                    textsScene[id].text = t;
                    id += 1;
                }
                break;

            case screenPanel.HUD:
                foreach(string t in xml.hud_screen)
                {
                    textsScene[id].text = t;
                    id += 1;
                }
                break;

            case screenPanel.PAUSE:
                foreach(string t in xml.pause_screen)
                {
                    textsScene[id].text = t;
                    id += 1;
                }
                break;

            case screenPanel.GAMEOVER:
                foreach(string t in xml.gameover_screen)
                {
                    textsScene[id].text = t;
                    id += 1;
                }
                break;

            case screenPanel.STAGECLEAR:
                foreach(string t in xml.stageclear_screen)
                {
                    textsScene[id].text = t;
                    id += 1;
                }

                textsScene[1].text += PlayerPrefs.GetInt("idStage") -1;
                string[] result = new string[4]{PlayerPrefs.GetInt("qtdLifes")+"x", PlayerPrefs.GetInt("slotUp0") == 1 ? 2+"x": 1+"x",
                PlayerPrefs.GetInt("slotUp1") == 1 ? 2+"x": 1+"x", PlayerPrefs.GetInt("qtdBombs")+"x"};

                foreach(string t in result)
                {
                    textsScene[id].text = t;
                    id += 1;
                }
                break;

            case screenPanel.GAMECLEAR:
                foreach(string t in xml.gameclear_screen)
                {
                    textsScene[id].text = t;
                    id += 1;
                }
                break;    
        }
        
    }
    public void Exit()
    {
        Application.Quit();
    }
}
