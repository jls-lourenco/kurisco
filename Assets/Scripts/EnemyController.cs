using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameController gameController;
    
    public PlayerController player;
    public LayerMask target;
    public GameObject enemy, prefBullet;
    public Transform spawnBullet, basePos;
    private Animator animator;
    private int idAnimation;
    private float vSpeed;
    public float dir, life, dealyShot;
    private bool canShot;
    public bool canFollow;
    public int idBullet;
    
    
    void Start()
    {
        gameController = FindObjectOfType(typeof(GameController)) as GameController;
        player = FindObjectOfType(typeof(PlayerController)) as PlayerController;
        
        animator = enemy.GetComponent<Animator>();
        
        dir = -1;
        
    }
    public void lookForPlayer()
    {
        if(player == null)
        {
            player = FindObjectOfType(typeof(PlayerController)) as PlayerController;
            //return;
        }
    }
    public void lookAtPlayer() 
    {   
        lookForPlayer();
        float dirRayY = 0;
        float sizeRay = 5f;
        float posPlayer = Mathf.Floor(player.transform.position.y);
        float posEnemy = Mathf.Floor(basePos.position.y);
        if(posPlayer == posEnemy){idAnimation = 0; vSpeed = 0;sizeRay = 10f;}
        else if(posPlayer > posEnemy){idAnimation =1; vSpeed = gameController.bulletSpeed+2; dirRayY = 1;}
        else if(posPlayer < posEnemy){idAnimation = 2;vSpeed = -(gameController.bulletSpeed + 2); dirRayY = -1;}
        
        Debug.Log(sizeRay);
        Debug.DrawRay(spawnBullet.position, new Vector3(dir, dirRayY) * sizeRay, Color.red);

        RaycastHit2D laserSight = Physics2D.Raycast(spawnBullet.position, new Vector3(dir, dirRayY), sizeRay, target);
            
        if(laserSight && !canShot){StartCoroutine("onShot");} 
        else{stopShot();}
        

    }
    public void wakeUp(bool status)
    {
        if(animator == null)animator = enemy.GetComponent<Animator>();
        animator.SetBool("onArea", status);
    }
    void stopShot() 
    {   
        
        canShot = false;
        StopCoroutine("onShot");

    }
    
    IEnumerator onShot()
    {
    	yield return new WaitForSeconds(dealyShot);
        spawnBulletEnemy();     	
        StopCoroutine("onShot");
    	
    }
    void spawnBulletEnemy()
    {
        if(enemy == null){return;}
        
        canShot = true;
        gameController.playFx(gameController.audioController.fxLasers[idBullet]);

        GameObject temp = Instantiate(prefBullet, spawnBullet.position, spawnBullet.rotation);
        temp.transform.localScale = new Vector3(.4f, .4f, .4f);
        temp.transform.tag = gameController.reloadTag(Tags.ENEMY);
        
        temp.GetComponent<ReSkin>().idBullet = idBullet;
        temp.layer = LayerMask.NameToLayer("BulletEnemy");
        temp.GetComponent<Rigidbody2D>().velocity = new Vector2((gameController.bulletSpeed + 2) * dir, vSpeed);
        
    }
    public void destroy()
    {
        gameController.playFx(gameController.audioController.fxExplosion);
        enemy.GetComponent<Collider2D>().enabled = false;
        GameObject temp = Instantiate(gameController.prefExplosion, enemy.transform.position + new Vector3(0,.5f,0), transform.rotation); 

        Destroy(enemy);
        Destroy(temp, .7f);
        
    }
    public void updateAnimations()
    {
        animator.SetInteger("idAnimation", idAnimation);
		animator.SetBool("canShot", canShot);
    }
        
    void OnTriggerStay2D(Collider2D other) 
    {
        if(enemy == null){return;}
        if(other.gameObject.tag == "Player")
        {
            if(!animator.GetBool("onArea")){wakeUp(true);}
            canFollow = true;
        }

    }
    void OnTriggerExit2D(Collider2D other) 
    {
        if(enemy == null){return; }
        
        if(other.gameObject.tag == "Player")
        {
            canFollow = false;
            idAnimation = 0;
            stopShot();

        }
        

    }
}