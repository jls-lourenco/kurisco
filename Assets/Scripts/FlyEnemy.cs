using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyEnemy : MonoBehaviour
{
    private AudioController audioController;
    private GameController gameController;
    private PlayerController player;
    private Rigidbody2D enemyRb;
    public float moveSpeed;
    public float life;

    
    // Start is called before the first frame update
    void Start()
    {
        audioController = FindObjectOfType(typeof(AudioController)) as AudioController;
        gameController = FindObjectOfType(typeof(GameController)) as GameController;
        player = FindObjectOfType(typeof(PlayerController)) as PlayerController;
        enemyRb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(player != null)
        {
            if(player.isAlive)enemyRb.position = Vector2.MoveTowards(enemyRb.position, player.transform.position + Vector3.up, moveSpeed * Time.deltaTime);
        }
        else player = FindObjectOfType(typeof(PlayerController)) as PlayerController;
    }
    void takeDamage(float damage) 
    {        
        if(life > 0){ life -= damage; }
        if(life <= 0)
        {
            GetComponent<Collider2D>().enabled = false;

            audioController.playFx(audioController.fxExplosion);
            
            GameObject temp = Instantiate(gameController.prefExplosion, transform.position, transform.rotation);
            Destroy(this.gameObject);
            Destroy(temp,.7f);
            
        }

    }
    
}
