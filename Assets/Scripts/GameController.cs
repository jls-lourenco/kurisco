using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState { CUTSCENE, GAMEPLAY}
public enum Tags { PLAYER, ENEMY, SHOT }

public class GameController : MonoBehaviour
{
    public GameState state;
    public AudioController audioController;
    public float kickX, kickY;
    public int idStage;
    
    [Header("Player Info")]
	public Texture[] spriteSheetName_P;
    public Texture[] spriteSheetName_H;
    public Transform		reSpawn;
    public GameObject       player;
    public bool haveShield;
    public int  qtdLifes;
    public int  qtdCurrentLifes;
    public int  qtdBombs;
    public float shieldDuration;
    public float fAmount;
    
    
	[Header("Enemy Info")]
	public Texture[] spriteSheetName_E;
    public GameObject flying,runner;
    public Transform[]  spawnPos;
    public bool isBossFight;

    [Header("Bullet and Bomb Info")]
    public int idBullet;
    //public int idBulletAtual;
    public float damage;
    public float[] slotDamage;
    public Texture[] spriteSheetName_S;
    public 	GameObject	prefBullet;
    public GameObject prefBulletExplosion;
    public GameObject prefExplosion;
    public GameObject bombExplosion;
    public 	GameObject	prefBomb;
    public 	float		bulletSpeed;
    public 	float 		delayShot;

    [Header("Collectables Info")]
    public bool shotHud;
    public int[] slotShot;
    public bool[] slotUp;
    public int index;
    public GameObject collectable;
    
    [Header("Camera Info")]
    public Camera cam;
    public Animator camAnimator;
    public Transform camFightPosition;
    public bool canFollow;
    
    [Header("HUD Info")]
    public GameObject panelPause;
    public GameObject panelFade;
    public Color[] colorOn, colorOff;

    public Transform clear;
    public bool stageClear;
    private Animator fadeAnimator;
    
    void Start()
    {
        audioController = FindObjectOfType(typeof(AudioController))	as AudioController;
        camAnimator = cam.GetComponent<Animator>();
        fadeAnimator = panelFade.GetComponentInChildren<Animator>();
        idStage = PlayerPrefs.GetInt("idStage");
        qtdLifes = PlayerPrefs.GetInt("qtdLifes");
        qtdCurrentLifes = qtdLifes;
        qtdBombs = PlayerPrefs.GetInt("qtdBombs");

        idBullet = PlayerPrefs.GetInt("idBulet");
        index = PlayerPrefs.GetInt("index");

        shotHud = PlayerPrefs.GetInt("slotHud") == 1 ? true : false;
        //slotUp = new bool[2]{false,false};
        slotUp = new bool[2]{ PlayerPrefs.GetInt("slotUp0") == 1 ? true: false, PlayerPrefs.GetInt("slotUp1") == 1 ? true: false};
        //slotShot = new int[2]{0, 0};
        slotShot = new int[2]{PlayerPrefs.GetInt("slotShot0"), PlayerPrefs.GetInt("slotShot1")};
        //slotDamage = new float[3]{.5f, 1f, 1.5f};
        slotDamage = new float[3]{PlayerPrefs.GetFloat("slotDamage0"), PlayerPrefs.GetFloat("slotDamage1"), PlayerPrefs.GetFloat("slotDamage0")};

        damage = slotDamage[0];

        StartCoroutine("fadeOut");
        
    }
    IEnumerator fadeOut()
    {
        yield return new WaitForSeconds(.15f);
        fadeAnimator.SetTrigger("fadeIn");
        
        
    }
    void Update() {

        if(Input.GetButtonDown("Cancel"))panelPause.GetComponentInChildren<SceneController>().resume(panelPause);
        
    }
    public void reloadBullet(int newIdBullet)
    {
        idBullet = newIdBullet;
    }
    public void addShot(int newIdBullet)
    {   
        reloadBullet(newIdBullet);
        slotUp[index] = slotShot[index] == idBullet ? true : false;
        damageUp();
        slotShot[index] = idBullet;
        
                
    }
    public void damageUp()
    {
        damage = slotUp[index] ? slotDamage[idBullet] + .5f: slotDamage[idBullet];
        print(damage);
    }
    public void addLife(int newLife)
    {
        qtdCurrentLifes += newLife;

    }
    public void addBomb(int newBomb)
    {
        qtdBombs += newBomb;

    }
    public string reloadTag(Tags currentTag)
    {
        string tag = "";

        switch(currentTag)
        {
            case Tags.PLAYER:
                tag = "Player";
                break;
            case Tags.ENEMY:
                tag = "enemy";
                break;
        }

        return tag;
    }
    public void reSpawnPlayer()
    {
        state = GameState.GAMEPLAY;
        if(qtdCurrentLifes > 0)StartCoroutine("invulnerable");
        else gameOver();
    }
    public void spawnRunner()
    {
        StartCoroutine("waitForSpawnEnemy");
    }
    IEnumerator invulnerable()
    {
        yield return new WaitForSeconds(1f);
        haveShield = true;
        GameObject newPlayer = Instantiate(player,reSpawn.position, reSpawn.rotation);
        newPlayer.transform.position = new Vector3(reSpawn.position.x, reSpawn.position.y, 0);
        
        
    }
    IEnumerator victory()
    {
        //yield return new WaitForSeconds(3);
        fadeAnimator.SetTrigger("fadeOut");

        Save();
        yield return new WaitForSeconds(1.5f);

        if(idStage <=3)SceneManager.LoadScene("StageClear");
        else {SceneManager.LoadScene("GameClear");}
        changeMusic(audioController.musicStageClear);
    }
    void Save()
    {
        idStage += 1;        

        PlayerPrefs.SetInt("idStage", idStage);
        PlayerPrefs.SetInt("qtdLifes", qtdCurrentLifes);
        PlayerPrefs.SetInt("qtdBombs", qtdBombs);
        
        PlayerPrefs.SetInt("slotShot0", slotShot[0]);
        PlayerPrefs.SetInt("slotShot1", slotShot[1]);

        PlayerPrefs.SetInt("idBulet", idBullet);
        PlayerPrefs.SetInt("index", index);
        
        PlayerPrefs.SetInt("slotHud", shotHud ? 1: 0);
        PlayerPrefs.SetInt("slotUp0", slotUp[0] ? 1: 0);
        PlayerPrefs.SetInt("slotUp1", slotUp[1] ? 1: 0);
        
        PlayerPrefs.SetFloat("slotDamage0", slotDamage[0]);
        PlayerPrefs.SetFloat("slotDamage1", slotDamage[1]);
        PlayerPrefs.SetFloat("slotDamage2", slotDamage[2]);
        
        
    }
    IEnumerator fade()
    {
        audioController.fx.Stop();
        fadeAnimator.SetTrigger("fadeOut");
        yield return new WaitForSecondsRealtime(1);
        
        SceneManager.LoadScene("GameOver");
        changeMusic(audioController.musicGameOver);

    }
    
    public void gameOver()
    {
        StartCoroutine("fade");
        
    }
    public void playFx(AudioClip clip)
	{
		audioController.playFx(clip);
	}
    public void changeMusic(AudioClip newMusic)
	{
		 audioController.StartCoroutine("changeMusic",newMusic);
	}

}
