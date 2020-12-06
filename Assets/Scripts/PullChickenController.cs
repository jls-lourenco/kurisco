using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullChickenController : MonoBehaviour
{
    
    public GameObject portal;
    public Collider2D[] platfomrs;
    private GameController gameController;
    private PlayerController player;
    private AudioController audioController;
    private Rigidbody2D enemyRb;
    private Animator animator;
    public Transform rayToward, rayUp;

    private int idAnimation;
    private float dir;
    public float JumpX, jumpY;
    public bool isLookingLeft, isDead;
    public float moveSpeed;
    public float life;
    private float mSpeed;

    public LayerMask layerTarget;
    private SpriteRenderer sRenderer;
    private bool canJump, canAtack;
    

    void Start() 
    {
        gameController = FindObjectOfType(typeof(GameController)) as GameController;
        player = FindObjectOfType(typeof(PlayerController)) as PlayerController;
        audioController = FindObjectOfType(typeof(AudioController)) as AudioController;
        enemyRb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sRenderer = GetComponent<SpriteRenderer>();

        JumpX *= -1;
        dir = -1;
        moveSpeed *= -1;
        mSpeed = moveSpeed;
        gameController.clear.position = portal.transform.position;
        portal.SetActive(false);
    }
    void Update() {
        
        if(player == null)
        {
            player = FindObjectOfType(typeof(PlayerController)) as PlayerController;
            return;
        }       
        
        float distance = Mathf.Floor(Vector2.Distance(player.transform.position, enemyRb.transform.position));
        
        if(player.isAlive && !gameController.canFollow && distance <= 10)
        {              
            followPlayer();
            atack();
        }
        else {idAnimation = 0; moveSpeed = 0;}
        
                
    } 
    void LateUpdate()
    { 
        animator.SetInteger("idAnimation", idAnimation);
        animator.SetFloat("jumpSpeed", enemyRb.velocity.y);
        animator.SetBool("isDead", isDead);
    }
    void flip()
    {
        isLookingLeft = !isLookingLeft;
        float x = transform.localScale.x;
        x *= -1;
        transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
        moveSpeed *= -1;
        mSpeed *= -1;
        JumpX *= -1;
        dir *= -1;
        
    }
    void followPlayer()
    {        
        float pX = Mathf.Floor(player.transform.position.x);
        float pY = Mathf.Floor(player.transform.position.y);
        float eX = Mathf.Floor(enemyRb.position.x);
        float eY = Mathf.Floor(enemyRb.position.y);
        
        if(pX < eX && !isLookingLeft){flip();}
        else if(pX > eX && isLookingLeft){flip();}
        
        //Jump
        //Debug.DrawRay(rayUp.position, new Vector3(dir, 1) * 1.5f, Color.yellow);
        //Debug.DrawRay(rayUp.position, new Vector3(0, 1) * 3.3f, Color.yellow);
        bool rayJump = Physics2D.Raycast(rayUp.position, new Vector3(dir, 1), 1.5f, layerTarget)
        || Physics2D.Raycast(rayUp.position, new Vector3(0, 1), 3.3f, layerTarget);        
        
        if(rayJump && pY > eY && player.isGrounded && enemyRb.velocity.y == 0 && !canJump)
        {
            canJump = true; 
            enemyRb.AddForce(new Vector2(JumpX,jumpY)); 
            StartCoroutine("waitForJump");
        }        
        else
        {
            
            enemyRb.velocity = new Vector2(moveSpeed, enemyRb.velocity.y);
            sRenderer.sortingOrder = enemyRb.velocity.y != 0 ? 13 : 9;
            
            //Vector2 target = new Vector2(player.transform.position.x, enemyRb.position.y);
            //enemyRb.position = Vector2.MoveTowards(enemyRb.position, target, moveSpeed * Time.deltaTime);
        }
    }
    void atack()
    {
        //Atack     
        //Debug.DrawRay(rayToward.position, Vector3.right * dir * .4f, Color.yellow);
        RaycastHit2D rayAtack = Physics2D.Raycast(rayToward.position, Vector3.right * dir, .4f, layerTarget);
        
        if(rayAtack)
        {
            if(!canAtack)
            {
                canAtack = true;
                moveSpeed = 0;
                idAnimation = 0;
                StartCoroutine("waitForFollow");
            }
            
        }
        else if(!canAtack)
        {
            moveSpeed = mSpeed;
            idAnimation = 1;
        }
    }
    void takeDamage(float damage) 
    {        
        if(life > 0){ life -= damage; }
        if(life <= 0)
        {
            StartCoroutine("waitDestroy");
            
        }

    }
    IEnumerator waitDestroy()
    {        
        GetComponent<Collider2D>().enabled = false;
        isDead = true;
        mSpeed *= -1;
        enemyRb.velocity = Vector2.zero;
        enemyRb.AddForce(new Vector2(gameController.kickX * -transform.localScale.x, gameController.kickY));
        
        yield return new WaitForSeconds(.5f);
        audioController.playFx(audioController.fxExplosion);
        
        GameObject temp = Instantiate(gameController.prefExplosion, transform.position + new Vector3(0,.5f,0), transform.rotation);
        sRenderer.enabled = false;
        Destroy(temp,.7f);
        yield return new WaitForSeconds(1.5f);
        gameController.state = GameState.CUTSCENE;
        gameController.stageClear = true;
        foreach(Collider2D cols in platfomrs)cols.enabled = false;
        Destroy(this.gameObject);
        portal.SetActive(true);
        
    }
    IEnumerator waitForJump()
    {
        yield return new WaitForSeconds(.5f);
        canJump = false;
    }
    IEnumerator waitForFollow()
    {
        
        animator.SetBool("isAtack", canAtack);
        
        yield return new WaitForSeconds(.5f);
                
        canAtack = false;
        animator.SetBool("isAtack", canAtack);
        
    }

}
