using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterController : MonoBehaviour
{
    private EnemyController enemyController;
    private Rigidbody2D enemyRb;
    private bool isLookingLeft;
    

    void Start() 
    {
        enemyController = GetComponentInParent<EnemyController>();
        enemyRb = GetComponent<Rigidbody2D>();

        isLookingLeft = true;
        enemyController.wakeUp(true);
    }
    void Update() 
    {
        enemyController.lookForPlayer();
        
        if(enemyController.canFollow)
        {
            float playerPosX  = Mathf.Floor(enemyController.player.transform.position.x);
            float enemyPosX   = Mathf.Floor(transform.position.x);

            enemyController.lookAtPlayer();

            if(playerPosX < enemyPosX && !isLookingLeft){flip(); }
            else if(playerPosX > enemyPosX && isLookingLeft){flip();}

        }
    }
    void LateUpdate() 
    {
        enemyController.updateAnimations();    
    }
    void flip()
    {
        isLookingLeft = !isLookingLeft;
        float x = transform.localScale.x;
        x *= -1;
        transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
        enemyController.dir = -transform.localScale.x;
    }
    void takeDamage(float damage) 
    {
        if(enemyController.life > 0){ enemyController.life -= damage; }
        if(enemyController.life <= 0)
        {
            StartCoroutine("waitDestroy");
            
        }

    }
    IEnumerator waitDestroy()
    {
        enemyController.wakeUp(false);
        GetComponent<Collider2D>().enabled = false; 
        enemyController.GetComponent<Collider2D>().enabled = false;       
        enemyRb.constraints = RigidbodyConstraints2D.FreezeRotation;
        enemyRb.AddForce(new Vector2(enemyController.gameController.kickX * transform.localScale.x, enemyController.gameController.kickY));
        
        yield return new WaitForSeconds(.5f);
        
        enemyController.destroy();
        
    }
}