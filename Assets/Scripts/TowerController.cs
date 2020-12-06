using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    private EnemyController enemyController;
    public Transform spawnLoot;
    public GameObject preFabPowerUp;
    
    // Start is called before the first frame update
    void Start()
    {
        enemyController = GetComponentInParent<EnemyController>();
        enemyController.idBullet = Random.Range(0,3);
        
    }

    // Update is called once per frame
    void Update()
    {
        if(enemyController.canFollow){ enemyController.lookAtPlayer(); }  
        
    }
    void LateUpdate()
    {
        enemyController.updateAnimations();        
		
    }
    void takeDamage(float damage) 
    {
        if(enemyController.life > 0){ enemyController.life -= damage; }
        if(enemyController.life <= 0)
        {
            //int rand = Random.Range(0,10);
            //if( rand > 5 )spawnPowerUp();
            spawnPowerUp();
            enemyController.destroy();
        
        }

    }
    void spawnPowerUp()
    {
        GameObject temp = Instantiate(preFabPowerUp, spawnLoot.position, transform.rotation);
        temp.GetComponent<CollectableController>().type = setLoot();
    }
    typeCollectable setLoot()
    {
        
        typeCollectable t = 0;
        switch(enemyController.idBullet)
        {
            case 0:
                t = typeCollectable.BULLET0;
                break;
            case 1:
                t = typeCollectable.BULLET1;
                break;
            case 2:
                t = typeCollectable.BULLET2;
                break;
        }

        return t;
    } 
    
}
