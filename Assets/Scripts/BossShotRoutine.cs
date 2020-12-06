using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShotRoutine : MonoBehaviour
{
    private BossController boss;
    private Animator animator;
    public Transform spawn;
    public float life;
    public bool isACannon;
    private bool isShot;

    // Start is called before the first frame update
    void Start()
    {
        boss = GetComponentInParent<BossController>();
        animator = GetComponent<Animator>();
        
        
        life = boss.lifeCannon;

    }
    void Update() 
    {
        if(isACannon)boss.lookAtPlayer(spawn);
    }
    void LateUpdate()
    {
        if(isACannon)
        {
            animator.SetBool("isShot", isShot);
            
        }
        if(!isACannon)
        {
            animator.SetBool("isShot", isShot);
        }
    }
    void takeDamage(float damage)
    {
        
        if(isACannon)
        {
            if(life > 0 && boss.qtdCannons > 0){life -= damage;}
            
            if(life <= 0){ isShot = false; destroyCannon();boss.qtdCannons -= 1;}
        }
        else if(!isACannon && boss.qtdCannons == 0)
        {
            if(boss.lifeBoss > 0){boss.lifeBoss -= damage;}
            
            if(boss.lifeBoss <= 0) { isShot = false; boss.destroy(); }
        }
        
    }
    void destroyCannon() {

        GameObject temp = Instantiate(boss.gameController.prefExplosion, transform.position, transform.rotation);
        //Destroy(this.gameObject);
        this.gameObject.SetActive(false);
        Destroy(temp,.7f);
    }
    
    IEnumerator waitForShot()
    {
        if(isACannon)
        {
            for(int i = 0; i < boss.qtdShot; i++)
            {
                if(boss.lifeBoss > 0)
                {
                    isShot = true;
                    
                    boss.spawnBullet(spawn);

                    yield return new WaitForSeconds(.2f);
                    isShot = false;
                    yield return new WaitForSeconds(boss.shotRate);
                }
            }
        }
        else
        {
            yield return new WaitForSeconds(boss.laserRate);
            if(boss.lifeBoss > 0)
            {
                isShot = true; 
                boss.spawnLaser(spawn);
                yield return new WaitForSeconds(boss.laserTime);
                isShot = false;            
            }
            
        
        }
    }
    
    
}
