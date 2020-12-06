using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlatformController : MonoBehaviour
{
    private GameController gameController;
    public Transform stopPos,camFight;
    public bool isMovel, canMove;
    bool isAlive;
    Vector3 spawnCachePos;
    void Start() {

        gameController = FindObjectOfType(typeof(GameController)) as GameController;
        
        isAlive = true;
        
        spawnCachePos = gameController.reSpawn.position;
        Debug.Log("Cache "+spawnCachePos);
        
    }
    void Update() {

        if(canMove)moveUp();
        
        
    }
    void onWay()
    {
        GetComponent<Collider2D>().enabled = false;
        StartCoroutine("onCollision");
    }
    void onWayAirShot(bool on)
    {
        GetComponent<Collider2D>().enabled = on;
        
    }
    IEnumerator onCollision()
    {
        yield return new WaitForSeconds(1);
        GetComponent<Collider2D>().enabled = true;
    }
    public void moveUp()
    {
        transform.parent.position = Vector3.MoveTowards(transform.parent.position, stopPos.position, 1f* Time.deltaTime);

        
        if(!gameController.canFollow)
        {
            
            if(gameController.idStage == 2)gameController.isBossFight = true;
            if(transform.parent.position == stopPos.position)canMove = false;
            
        }
        
        
    }
    public bool itIsMovel()
    {
        return isMovel;
    }
    void OnCollisionEnter2D(Collision2D col)
    {
    	switch(col.gameObject.tag)
    	{
    		case "Player":
				
    			if(isMovel && transform.position.y < col.gameObject.transform.position.y)
                {
                    if(gameController.idStage == 2)gameController.camFightPosition.position = new Vector3(camFight.position.x, camFight.position.y,-10);
                    canMove = true;
                    
				}
				break;
			
    	}
    }
    void OnCollisionExit2D(Collision2D col)
    {
    	switch(col.gameObject.tag)
    	{
    		case "Player":
            
                
				isAlive = col.gameObject.GetComponent<PlayerController>().isAlive;
    			if(isMovel && transform.position.y < col.gameObject.transform.position.y)
                {
                    if(!isAlive)gameController.reSpawn.position = transform.parent.position;
                    else gameController.reSpawn.localPosition = spawnCachePos;
                    canMove = false;
                    Debug.Log("Cache2: "+gameController.reSpawn.position);
				}
				break;
			
    	}
    }
    

}