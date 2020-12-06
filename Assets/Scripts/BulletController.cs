using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{   
    private GameController gameController;
    private GameObject bulletExplosion;
    
    void Start() 
    {
        gameController = FindObjectOfType(typeof(GameController)) as GameController;
    }
    void OnBecameInvisible()
    {
    	Destroy(this.gameObject);
    }
    void spawnBulletExplosion()
    {
        bulletExplosion = Instantiate(gameController.prefBulletExplosion, transform.position, transform.rotation);
        
        bulletExplosion.GetComponent<ReSkin>().idBullet = GetComponent<ReSkin>().idBulletAtual;
        
    }
    void destroy()
    {
        Destroy(this.gameObject);
        Destroy(bulletExplosion,.35f);
    }
    void OnTriggerEnter2D(Collider2D other) {
        
        switch(other.gameObject.tag)
            {
                case "enemy":
                    spawnBulletExplosion();
                    //bulletExplosion.transform.localScale = new Vector3(.5f, .5f, .5f);
                    
                    other.SendMessage("takeDamage", gameController.damage, SendMessageOptions.DontRequireReceiver);
                                        
                    destroy();
                    break;

                case "Player":
                    
                    spawnBulletExplosion();                    
                    other.SendMessage("takeDamage",SendMessageOptions.DontRequireReceiver);
                    
                    destroy();
                                        
                    break;
                
                case "collectable":

                    spawnBulletExplosion();
                    //bulletExplosion.transform.localScale = new Vector3(.5f, .5f, .5f);
                    destroy();
                    
                    other.GetComponent<Collider2D>().enabled = false;
                    other.GetComponentInChildren<FlyingCollectable>().SendMessage("drop", SendMessageOptions.DontRequireReceiver);
                    break;
            }
        
    }
    void OnCollisionEnter2D(Collision2D other) 
    {
         switch(other.gameObject.tag)
            {
                case "Player":
                    spawnBulletExplosion();
                    destroy();
                    break;
            }   
    }
    
    

}

