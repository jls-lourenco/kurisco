using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{   
    private GameController gameController;
    
    public enum typeShot{BOMB, EXPLOSION}
    public typeShot whatShot;
    public float damage;

    void Start() 
    {
        gameController = FindObjectOfType(typeof(GameController)) as GameController;
        
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        switch(whatShot)
        {
            case typeShot.BOMB:

                if(other.gameObject.tag == "enemy" || other.gameObject.tag == "Untagged" || other.gameObject.tag == "platform" && transform.position.y > other.transform.position.y)
                {
                    GameObject temp = Instantiate(gameController.bombExplosion, transform.position, transform.rotation);
                                            
                    Destroy(this.gameObject);
                    Destroy(temp,.8f);
                                                    
                }
                break;

            case typeShot.EXPLOSION:
                gameController.playFx(gameController.audioController.fxBomb);
                gameController.camAnimator.SetTrigger("shake");
                if(other.gameObject.tag == "enemy")
                {
                    other.SendMessage("takeDamage", damage, SendMessageOptions.DontRequireReceiver);
                                        
                }
                break;


        }
        
        
    }
    
    

}

