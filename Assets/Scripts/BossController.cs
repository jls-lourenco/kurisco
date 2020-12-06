using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public GameController gameController;
    
    public float lifeCannon, lifeBoss;
    public GameObject prefBullet, prefLaser, prefPortal;
    
    //public Transform laser;
    public BossShotRoutine[] cannons;
    public int qtdCannons;
    public int idBullet;
    public float laserTime,laserRate,shotRate, qtdShot;    
    private GameObject laserShot;
    public GameObject regenerate;
    public Transform[] explosions;
    public Transform portal, camFight;
    private int qtdRegenerations;
    public bool isCannonDown;
    
    // Start is called before the first frame update
    void Start()
    {
        gameController = FindObjectOfType(typeof(GameController)) as GameController;

        qtdCannons = cannons.Length -1;
        regenerate.SetActive(false);
        idBullet = 3;
        gameController.clear.position = portal.position;
        //gameController.camFightPosition.position = new Vector3(camFight.position.x,camFight.position.y,-10);
    }
    public void spawnBullet(Transform cannon)
    {
        if(isCannonDown)lookAtPlayer(cannon);

        gameController.playFx(gameController.audioController.fxLasers[idBullet-3]);
        GameObject temp = Instantiate(prefBullet, cannon.position, cannon.rotation);
        temp.transform.tag = gameController.reloadTag(Tags.ENEMY);
        
        temp.GetComponent<ReSkin>().idBullet = idBullet;
        temp.layer = LayerMask.NameToLayer("BulletEnemy");
        
        if(isCannonDown)temp.GetComponent<Rigidbody2D>().velocity = cannon.up * (gameController.bulletSpeed /2);
        else temp.GetComponent<Rigidbody2D>().velocity = new Vector2((gameController.bulletSpeed + 2) * -1, 0);
    }
    public void lookAtPlayer(Transform cannon)
    {
        if(!isCannonDown)return;
        
        PlayerController player = FindObjectOfType(typeof(PlayerController)) as PlayerController;
        if(player != null)
        {
        
        cannon.parent.up = (player.transform.position - cannon.parent.position) * -1;
        cannon.up = (player.transform.position - cannon.position);
        
        /*
        float angle = (Mathf.Atan2 (player.transform.position.y, player.transform.position.x)* Mathf.Rad2Deg) +95f;
        cannon.parent.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        cannon.up = (player.transform.position - transform.position);
        */

        }
        
    }
    public void spawnLaser(Transform shot)
    {
        gameController.playFx(gameController.audioController.fxLaserBoss);

        laserShot = Instantiate(prefLaser, shot.position, shot.rotation);
        
        laserShot.GetComponentInChildren<ReSkin>().idBullet = idBullet;
        Destroy(laserShot,laserTime);
        
        
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player" && lifeBoss > 0)
        {
            /**gameController.isBossFight = false;
            if(!gameController.isBossFight)
            {
                
            }**/
            GetComponent<Collider2D>().enabled = false;
            gameController.isBossFight = true;
            gameController.canFollow = false;
            if(isCannonDown)StartCoroutine("shotRoutine1");
            else StartCoroutine("shotRoutine2");
        
        }
    }
    public void destroy()
    {
        gameController.playFx(gameController.audioController.fxExplosion);
        cannons[cannons.Length -1].GetComponent<Collider2D>().enabled = false;
        Destroy(laserShot);
        gameController.audioController.fx.Stop();
        StartCoroutine("explosionsRoutine");
        
    }
    IEnumerator explosionsRoutine()
    {
        
        GameObject temp = null;
        
        for(int i = 0; i < 2;i++)
        {   
            
            temp = Instantiate(gameController.prefExplosion, transform.position + new Vector3(1,-1.8f,0), transform.rotation);
            
            yield return new WaitForSeconds(.3f);
            gameController.playFx(gameController.audioController.fxExplosion);
            Destroy(temp,1f);
            gameController.camAnimator.SetTrigger("shake");

            temp = Instantiate(gameController.prefExplosion, transform.position + new Vector3(0,-1f,0), transform.rotation);
            
            yield return new WaitForSeconds(.3f);
            gameController.playFx(gameController.audioController.fxExplosion);
            Destroy(temp,1f);
            gameController.camAnimator.SetTrigger("shake");

            temp = Instantiate(gameController.prefExplosion, transform.position + new Vector3(-1,1f,0), transform.rotation);
            
            yield return new WaitForSeconds(.3f);
            gameController.playFx(gameController.audioController.fxExplosion);
            Destroy(temp,1f);
            gameController.camAnimator.SetTrigger("shake");

            temp = Instantiate(gameController.prefExplosion, transform.position + new Vector3(1,.5f,0), transform.rotation);
            
            yield return new WaitForSeconds(.3f);
            gameController.playFx(gameController.audioController.fxExplosion);
            Destroy(temp,1f);
            gameController.camAnimator.SetTrigger("shake");
        
        }

        temp = Instantiate(gameController.bombExplosion, transform.position, transform.rotation);
        temp.GetComponent<Collider2D>().enabled = false;
        Destroy(temp,.8f);
        gameController.camAnimator.SetTrigger("shake");
        //Regenerate
        if(isCannonDown)StartCoroutine("clear");
        else StartCoroutine("Bossregeneration");


    }
    IEnumerator shotRoutine1()
    {
        idBullet= 4;
        shot(cannons[2]);
        yield return new WaitForSeconds(3);
        shot(cannons[0]);
        shot(cannons[1]);
        yield return new WaitForSeconds(3);
        StartCoroutine("shotRoutine1");
    }
    IEnumerator shotRoutine2()
    {
        shot(cannons[5]);
        yield return new WaitForSeconds(3);
        shot(cannons[0]);
        shot(cannons[1]);
        yield return new WaitForSeconds(3);
        shot(cannons[2]);
        yield return new WaitForSeconds(3);
        shot(cannons[3]);
        shot(cannons[4]);
        
        StartCoroutine("shotRoutine2");
    }
    void shot(BossShotRoutine cannon)
    {
        if(cannon == null || !cannon.gameObject.activeSelf)return;
        cannon.StartCoroutine("waitForShot");
    }
    IEnumerator Bossregeneration()
    {
        if(qtdRegenerations == 2)
        {
            StartCoroutine("clear");
        }
        else
        {
            qtdRegenerations += 1;
            regenerate.SetActive(true);
            
            for(int i = cannons.Length -1; i >= 0;i--)
            {
                yield return new WaitForSeconds(.2f);
                cannons[i].gameObject.SetActive(true);
                cannons[i].life = lifeCannon;
                if(i == 2)idBullet += 1;
            }
            qtdCannons = cannons.Length -1;
            lifeBoss = lifeCannon * 2;
            cannons[5].GetComponent<Collider2D>().enabled = true;
            
            regenerate.SetActive(false);
        }
        
        
    }
    IEnumerator clear()
    {
        Instantiate(prefPortal, portal.position, transform.rotation);
        GetComponentInChildren<SpriteRenderer>().enabled = false;
        gameController.state = GameState.CUTSCENE;
        yield return new WaitForSeconds(1.5f);
        gameController.stageClear = true;
        Destroy(this.gameObject);
    }
    
}
