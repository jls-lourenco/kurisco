using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private GameController gameController;
    private PlayerController playerController;
    public float speedCam;
	public Transform L, R, T, D;
	public float offSetX, offSetY;
	private Vector3 posPlayer;
    //public bool canFollow;

    [Header("Background Inf")]
    public Transform backGrounds;
    public BGController mountains;

    private void Start() {
        
        gameController = FindObjectOfType(typeof(GameController)) as GameController;
        if(gameController.idStage == 2)
        {
            T.position = new Vector3(0,500,0);
            R.position = new Vector3(0,0,0);
        }
    }
    void FixedUpdate() {

        if(playerController == null || !playerController.isAlive) 
        {
            playerController = FindObjectOfType(typeof(PlayerController)) as PlayerController;
            return;
        }

		posPlayer = new Vector3(playerController.transform.position.x + offSetX, playerController.transform.position.y + offSetY, -10);
        
        if(gameController.canFollow)
        {
            //Limit move cam left and right
            if(posPlayer.x <= L.position.x){ posPlayer = new Vector3(L.position.x, posPlayer.y, -10);}
            if(posPlayer.x >= R.position.x){ posPlayer = new Vector3(R.position.x, posPlayer.y, -10);}
            //Limit move cam down and top
            if(posPlayer.y <= D.position.y){ posPlayer = new Vector3(posPlayer.x, D.position.y, -10);}
            else if(posPlayer.y >= T.position.y){ posPlayer = new Vector3(posPlayer.x, T.position.y, -10);}

            if(posPlayer.x > transform.position.x || posPlayer.y > transform.position.y) 
            {
                transform.position = Vector3.MoveTowards(transform.position, posPlayer, speedCam);
                mountains.speedOffSet = .0006f;

            }else{mountains.speedOffSet = 0;}
            
            backGrounds.position = new Vector3(transform.position.x, transform.position.y, 0);
            
        } 
        else if(gameController.isBossFight)
        {
            transform.position = Vector3.MoveTowards(transform.position, gameController.camFightPosition.position, speedCam/2);
            backGrounds.position = new Vector3(transform.position.x, transform.position.y, 0); 
            mountains.speedOffSet = 0;
            
        }else{mountains.speedOffSet = 0;}
        
    }
   
}