using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTrigger : MonoBehaviour 
{
    private GameController gameController;
    private Vector2 posL, posR;
    public int qtdL,qtdR;
    public float posY;
    public bool havePower;
    public bool isFlying;
    public GameObject enemy;

    void Start() {

        gameController = FindObjectOfType(typeof(GameController)) as GameController;

        if(isFlying) enemy = gameController.flying;
        else enemy = gameController.runner;
        
        posY = transform.position.y;

    }
    void spawnEnemy()
    {
        spawnPowerUp();

        if(qtdL == 0 && qtdR == 0)return;
        if(isFlying)posY = transform.position.y + 3;
        posL = new Vector2(gameController.spawnPos[0].position.x, posY);
        posR = new Vector2(gameController.spawnPos[1].position.x, posY);
        
        StartCoroutine("spawnEnemyR");
        StartCoroutine("spawnEnemyL");
        
    }
    IEnumerator spawnEnemyR()
    {
        for(int i = 0; i < qtdR;i++)
        {
            GameObject t = Instantiate(enemy, posR, gameController.spawnPos[1].rotation);
            yield return new WaitForSeconds(.7f);
            
        }
                
    }
    IEnumerator spawnEnemyL()
    {
        for(int i = 0; i < qtdL;i++)
        { 
            GameObject t = Instantiate(enemy, posL, gameController.spawnPos[0].rotation);
            if(!isFlying)t.GetComponent<RunnerController>().isLookingLeft = false;
            yield return new WaitForSeconds(.7f);
        }
                
    }
    void spawnPowerUp()
    {
        if(!havePower)return;
        
        Vector3 posColectable = new Vector3(gameController.spawnPos[1].position.x, transform.position.y + 3, 10);
        
        GameObject t = Instantiate(gameController.collectable, posColectable, gameController.spawnPos[1].rotation);

        switch(Random.Range(0,3))
        {
            case 0:
                t.GetComponent<CollectableController>().type = typeCollectable.SHIELD;
                break;
            case 1:
                t.GetComponent<CollectableController>().type = typeCollectable.BOMB;
                break;
            case 2:
                t.GetComponent<CollectableController>().type = typeCollectable.UP1;
                break;
        }
    }
    
}
