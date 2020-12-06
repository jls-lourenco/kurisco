using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerController : MonoBehaviour
{
    private GameController gameController;
    
    private Rigidbody2D enemyRb;
    private Animator animator;
    public bool isLookingLeft, isJumped;
    public float moveSpeed;
    public float life;
    public Transform rayJump;
    public LayerMask layerTarget, layerTarget2;
    private float dir;
    
    void Start() 
    {
        gameController = FindObjectOfType(typeof(GameController)) as GameController;
        
        enemyRb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        if(!isLookingLeft){ flip();}
        else{moveSpeed *= -1;}
        dir = transform.localScale.x;
        
    }
    void Update() {
        
        enemyRb.velocity = new Vector2(moveSpeed, enemyRb.velocity.y);
         //Jump
        //Debug.DrawRay(rayJump.position, new Vector3(dir, 0) * 0.5f, Color.yellow);
        bool ray = Physics2D.Raycast(rayJump.position, new Vector3(dir, 0), 0.5f, layerTarget) 
        || Physics2D.Raycast(rayJump.position, new Vector3(dir, 0), 0.5f, layerTarget2);
        
        if(ray && !isJumped)jump();
        else if (enemyRb.velocity.y < 0) isJumped = false;
    }
    
    void LateUpdate()
    { 
        animator.SetFloat("jumpSpeed", enemyRb.velocity.y);
    }
    void flip()
    {
        isLookingLeft = !isLookingLeft;
        float x = transform.localScale.x;
        x *= -1;
        transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
        
    }
    void takeDamage(float damage) 
    {
        if(life > 0){life -= damage;}
            
        if(life <= 0){ StartCoroutine("waitDestroy");}
        
    }
    IEnumerator waitDestroy()
    {
        GetComponent<SpriteRenderer>().sortingOrder = 12;
        animator.SetTrigger("die");
        GetComponent<Collider2D>().enabled = false;
        moveSpeed *=-1;
        enemyRb.velocity = Vector2.zero;
        enemyRb.AddForce(new Vector2(gameController.kickX * -transform.localScale.x, gameController.kickY));
        
        yield return new WaitForSeconds(.5f);
        gameController.playFx(gameController.audioController.fxExplosion);
        GameObject temp = Instantiate(gameController.prefExplosion, transform.position + new Vector3(0,.5f,0), transform.rotation); 
        Destroy(this.gameObject);
        Destroy(temp,.8f);
        
    }
    
    void jump()
    {
        isJumped = true;
        enemyRb.velocity = Vector2.zero;

        if(isLookingLeft)enemyRb.AddForce(new Vector2(-gameController.kickX, gameController.kickY));
        else enemyRb.AddForce(new Vector2(gameController.kickX, gameController.kickY));

    }
    void OnBecameInvisible()
    {
       Destroy(this.gameObject);
    }
}