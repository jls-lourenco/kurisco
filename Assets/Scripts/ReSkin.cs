using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class ReSkin : MonoBehaviour
{
	private GameController gameController;
    private EnemyController enemyController;
    private BossController bossController;
	private SpriteRenderer sRender;
	public Sprite[] sprites;
	public string spriteSheetName;
	public string loadedSpriteSheetName;
    
	private Dictionary<string, Sprite> spriteSheet;

    public Tags currentTag;
    public int idBullet;
    public int idBulletAtual;
    public bool isHUD;
    private Image hud;

    // Start is called before the first frame update
    void Start()
    {
    	gameController = FindObjectOfType(typeof(GameController)) as GameController;
        enemyController = GetComponentInParent<EnemyController>();
        bossController = GetComponentInParent<BossController>();
     	
        if(isHUD){hud = GetComponent<Image>();}
     	else {sRender = GetComponent<SpriteRenderer>();}
                
    }

    void LateUpdate()
    {
        
    	if(currentTag == Tags.PLAYER)
        {
            idBullet = gameController.idBullet;
            if(isHUD){ spriteSheetName = gameController.spriteSheetName_H[gameController.idBullet].name;}
            else {spriteSheetName = gameController.spriteSheetName_P[gameController.idBullet].name;}
                
        }  
        else if(currentTag == Tags.ENEMY)
        {  
            if(enemyController != null)idBullet = enemyController.idBullet;
            if(bossController != null)idBullet = bossController.idBullet;
            spriteSheetName = gameController.spriteSheetName_E[idBullet].name;
        }
        else if(currentTag == Tags.SHOT)
        {
            //Just for PLayer
            if(gameController.slotShot[gameController.index] == idBullet && gameController.slotUp[gameController.index])
            {
                switch(gameController.slotShot[gameController.index])
                {
                    case 0:
                        idBullet = 3;
                        break;
                    case 1:
                        idBullet = 4;
                        break;
                    case 2:
                        idBullet = 5;
                        break;
                }
            }  
            spriteSheetName = gameController.spriteSheetName_S[idBullet].name;
        }
        if(idBullet != idBulletAtual)
        {
            if(enemyController != null)idBullet = enemyController.idBullet;
            if(bossController != null)idBullet = bossController.idBullet;
            else idBulletAtual = idBullet;
            loadSpriteSheet();
        }
        if (loadedSpriteSheetName != spriteSheetName){loadSpriteSheet(); }

        if(isHUD) {hud.sprite = spriteSheet[hud.sprite.name];}
        else {sRender.sprite = spriteSheet[sRender.sprite.name];}
    }

    void loadSpriteSheet()
    {
        sprites = Resources.LoadAll<Sprite>(spriteSheetName);
        spriteSheet = sprites.ToDictionary(x => x.name, x => x);
        loadedSpriteSheetName = spriteSheetName;
                
    }
    
    
}
