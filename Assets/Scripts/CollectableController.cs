using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum typeCollectable { SHIELD, UP1, BOMB, BULLET0, BULLET1, BULLET2 }
public class CollectableController : MonoBehaviour
{
    private GameController gameController;
    public typeCollectable type;
    public SpriteRenderer sRender;
    public Collider2D     collider2d;
    
    public Sprite[] sprites;
    
    void Start() {
        
        gameController = FindObjectOfType(typeof(GameController)) as GameController;
        
    }
    public void interacao()
    {
        switch(type)
        {
            case typeCollectable.SHIELD:

                gameController.haveShield = true;

                break;
            case typeCollectable.UP1:
                
                gameController.addLife(1);

                break;
            case typeCollectable.BOMB:
                
                gameController.addBomb(1);
                
                break;
            case typeCollectable.BULLET0:
                
                gameController.addShot(0);
                
                break;
            case typeCollectable.BULLET1:
                
                gameController.addShot(1);
                
                break;
            case typeCollectable.BULLET2:
                
                gameController.addShot(2);
                
                break;
        }
        
        sRender.enabled = false;
        collider2d.enabled = false;
    }
    
    public void reloadCollectable()
    {
        switch(type)
        {
            case typeCollectable.SHIELD:
                sRender.sprite = sprites[0];
                break;
            case typeCollectable.UP1:
                sRender.sprite = sprites[1];
                break;
            case typeCollectable.BOMB:
                sRender.sprite = sprites[2];
                break;
            case typeCollectable.BULLET0:
                sRender.sprite = sprites[0];
                break;
            case typeCollectable.BULLET1:
                sRender.sprite = sprites[1];
                break;
            case typeCollectable.BULLET2:
                sRender.sprite = sprites[2];
                break;
        }
        
        
    }
    
}
