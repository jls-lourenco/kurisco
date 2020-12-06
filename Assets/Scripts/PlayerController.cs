using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	private GameController gameController;
	
	public Shield	shield;
	private Rigidbody2D	playerRb;
	private Animator	playerAnimator;
	private SpriteRenderer	playerSr;
	
	private GameObject 	platform;
	public 	Transform	spawnBullet;
    public 	Transform	spawnBomb;
	public Transform	groundCheck1;
	public Transform	groundCheck2;
	
	public LayerMask	whatIsGround;
	private int	idAnimation;

	public bool	isGrounded;
	public bool	isLookingLeft;
	public bool isJumping;
	public bool	isAlive;
	public bool	canSpawn;
	public bool isCancelledAS;
	public float	moveSpeed;
	public float	jumpForce;
	private float	jumpSpeed;
	private float	horizontal;
	private float	vertical;
	public float	jumpGravity;
	private float	gravityScale;
	public float	airTime;
	private float	bulletSpeedX, bulletSpeedY, dirX, dirY;

	private bool canStep;

    // Start is called before the first frame update
    void Start()
    {
		gameController = FindObjectOfType(typeof(GameController)) as GameController;
		
		shield 			= GetComponentInChildren<Shield>();
        playerRb 		= GetComponent<Rigidbody2D>();
        playerSr 		= GetComponent<SpriteRenderer>();
        playerAnimator 	= GetComponent<Animator>();   

        gameController.bulletSpeed = moveSpeed;
		gravityScale = playerRb.gravityScale;
		shield.gameObject.SetActive(false);
		//gameController.haveShield = true;
		isAlive = true;
		intro();
		
    }
	public void intro()
	{	
		StartCoroutine("alphaToColor");
	}
    // Update is called once per frame
    void Update()
    {
		if(gameController.stageClear && isAlive)
		{
			if(Mathf.Abs(transform.position.x) != Mathf.Abs(gameController.clear.position.x))
			{
				if (isLookingLeft && transform.position.x < gameController.clear.position.x)flip();
				else if (!isLookingLeft && transform.position.x > gameController.clear.position.x)flip();
				
				//if(jumpSpeed == 0 && platform.gameObject.tag == "platform"){playerRb.AddForce(new Vector2(0, jumpForce/2));jumpSpeed = 1;}
				
				for(int i = 1;i < 6;i++)playerAnimator.SetLayerWeight(i, 0);
				idAnimation = 1;
				
				Vector3 target = new Vector3(gameController.clear.position.x,transform.position.y,transform.position.z);
				
				transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
				
			}
			else{StartCoroutine("colorToAlpha");idAnimation = 0;gameController.stageClear = false;}
		
		}
		if(gameController.state == GameState.CUTSCENE){jumpSpeed = playerRb.velocity.y;return;}

		if(!isAlive || (Time.timeScale == 0))
		{
			if(!isAlive && platform != null)platform.SendMessage("onWayAirShot", true, SendMessageOptions.DontRequireReceiver);

			for(int i = 1;i < 6;i++)playerAnimator.SetLayerWeight(i, 0);			
			return;
		}
		else { jump(); walk(); shieldOn();}
		

    }
    void LateUpdate()
    {
		playerAnimator.SetInteger("idAnimation", idAnimation);
		playerAnimator.SetBool("isGrounded", isGrounded);
    	playerAnimator.SetFloat("jumpSpeed", jumpSpeed);
		playerAnimator.SetBool("canJump", isJumping);
		
		//right shift
		if(Input.GetButtonDown("Fire2") && isAlive)
		{
			
			gameController.index = gameController.index < gameController.slotShot.Length-1 ? gameController.index += 1: gameController.index = 0;
			gameController.reloadBullet(gameController.slotShot[gameController.index]);
			gameController.damageUp();
			
			gameController.shotHud = !gameController.shotHud;
			
		}
		
    }
    void FixedUpdate()
    {
    	isGrounded = Physics2D.OverlapArea(groundCheck1.position, groundCheck2.position, whatIsGround);
    	
    }
	void animationLayer()
	{
		//Layer 0 = looking_side, Layer 1 = looking_up, Layer 2 = looking_down
        
        //Change from idle animation to running animation
        if (horizontal != 0){ idAnimation = 1;} else{ idAnimation = 0; }
		
        if (vertical > 0)
    	{
    		//Layers
    		playerAnimator.SetLayerWeight(1, 1);//looking_up
            playerAnimator.SetLayerWeight(2, 0);//looking_down
            playerAnimator.SetLayerWeight(3, 0);
			
            shoot(4);//shoot_up            
    	}
    	else if(vertical < 0)
    	{
    		//Layers
            playerAnimator.SetLayerWeight(2, 1);//looking_down
    		playerAnimator.SetLayerWeight(1, 0);//looking_up
            playerAnimator.SetLayerWeight(3, 0);
			
            shoot(5);//shoot_down
    	}
    	else if(vertical == 0)
    	{
    		//Layers
    		playerAnimator.SetLayerWeight(1, 0);//looking_up
            playerAnimator.SetLayerWeight(2, 0);//looking_down
            
            shoot(3);//shooting_side
            
            playerAnimator.SetLayerWeight(4, 0);//shooting_up
            playerAnimator.SetLayerWeight(5, 0);//shooting_down
			   
    	}
		
	}
    void walk()
    {
		
    	if(!playerAnimator.GetBool("isBomb"))
		{	
			horizontal = Input.GetAxisRaw("Horizontal");
			vertical = Input.GetAxisRaw("Vertical");
		}
		
		animationLayer();
        
        //Flip player
        if (horizontal < 0 && !isLookingLeft){ flip(); }
        else if (horizontal > 0 && isLookingLeft) { flip(); }

        //Movement the player
        jumpSpeed = playerRb.velocity.y;
        playerRb.velocity = new Vector2(horizontal * moveSpeed, jumpSpeed);
		
		if(jumpSpeed > 0 && !isGrounded){isJumping = true;}
		else if(isGrounded && jumpSpeed <= 0 && canStep)
		{gameController.playFx(gameController.audioController.fxSteps);canStep = false;}
		else if(!isGrounded)canStep = true;;
		
		
    }
    void jump()
    {
    	if(Input.GetButtonDown("Jump") && isGrounded && jumpSpeed == 0)
    	{	
			if(horizontal == 0 && vertical < 0 && platform.gameObject.tag == "platform")
	    	{
				platform.SendMessage("onWay", SendMessageOptions.DontRequireReceiver);
				playerAnimator.SetBool("canFall", true);					
				
			}
			else { isJumping = true; playerRb.AddForce(new Vector2(0, jumpForce));}
			
    	}
				
    }
	///This function is response to shot
    void shoot(int layer)
    {
		//Set the bulllet disrection
		dirX = spawnBullet.localRotation.eulerAngles.x * transform.localScale.x;
		dirY = spawnBullet.localRotation.eulerAngles.y * vertical;
    	//Sift
    	if(Input.GetButton("Fire3") && horizontal == 0){ playerAnimator.SetBool("isAngled", true); } 
    	else if(Input.GetButtonUp("Fire3")){ playerAnimator.SetBool("isAngled", false); }
    	
		//Ctrl
		if(Input.GetButton("Fire1"))
		{
			playerAnimator.SetLayerWeight(layer, 1);
			
			if(canSpawn && !isJumping && (dirX !=0 || dirY != 0)){ StartCoroutine("spawnBullets"); }
			if(canSpawn && playerAnimator.GetBool("isAirShot")){ StartCoroutine("spawnBullets"); }
			
		}
		else if(Input.GetButtonUp("Fire1")){ playerAnimator.SetLayerWeight(layer, 0);}
		//Alt
		else if(Input.GetButtonDown("Bomb") && horizontal == 0 && vertical == 0 && isGrounded && gameController.qtdBombs > 0)
		{gameController.qtdBombs -= 1;playerAnimator.SetBool("isBomb", true);}

		if (Input.GetButtonDown("Fire1") && isJumping && airTime > 0 && !isCancelledAS)
		{
			
			if(platform != null) 
			{
				if(platform.gameObject.tag == "platform" && transform.position.y <= platform.transform.position.y) 
				{platform.SendMessage("onWayAirShot", false, SendMessageOptions.DontRequireReceiver);}
			}
			StopCoroutine("cancelAirtShot");
			StartCoroutine("cancelAirtShot");
			Time.timeScale = 0.55f;
			playerAnimator.SetBool("isAirShot",true);
			airTime -= 1;		
			moveSpeed = 0;
			gameController.bulletSpeed = 8;
			playerRb.velocity = new Vector2(0, 0.9f);
			playerRb.gravityScale = jumpGravity;
			
			if(airTime == 0){playerAnimator.SetBool("isAirShot", false);playerRb.gravityScale = gravityScale; }

		}
		else if(Input.GetButtonUp("Fire1") || isGrounded) { playerRb.gravityScale = gravityScale; Time.timeScale = 1f;}
		
		if(isGrounded)
		{ 
			if(airTime < 5 && platform != null)platform.SendMessage("onWayAirShot", true, SendMessageOptions.DontRequireReceiver);
			isJumping = false; moveSpeed = 4;gameController.bulletSpeed = moveSpeed; airTime = 5;playerAnimator.SetBool("isAirShot", false);
			isCancelledAS = false;
		}
		
    }
	IEnumerator cancelAirtShot()
	{
		yield return new WaitForSeconds(gameController.delayShot * 2);
		isCancelledAS = true;
		playerAnimator.SetBool("isAirShot", false);
		playerRb.gravityScale = gravityScale; 
		if(isAlive)Time.timeScale = 1f;
		
	}
	void shieldOn()
	{
		if(gameController.haveShield)
        {
			if(!shield.gameObject.activeSelf) gameController.playFx(gameController.audioController.fxShield);
            shield.gameObject.SetActive(true);
            shield.StartCoroutine("onShield");
			
        }
	}
	void spawnBombs()
	{
		GameObject bomb = Instantiate(gameController.prefBomb, spawnBomb.position, spawnBomb.transform.rotation);
		bomb.GetComponent<Rigidbody2D>().AddForce(new Vector2(210 * transform.localScale.x, 240));
		playerAnimator.SetBool("isBomb", false);
	}
    void flip()
    {
    	isLookingLeft = !isLookingLeft;
    	float x = transform.localScale.x;
    	x *= -1;
    	transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
		
    }
	public void takeDamage()
	{	
		
		if(!isAlive || gameController.haveShield)return;
		
		gameController.playFx(gameController.audioController.fxhitKill);
		Time.timeScale = 0.55f;
		this.gameObject.layer = LayerMask.NameToLayer("TransparentFX");
		//transform.tag = gameController.reloadTag(Tags.ENEMY);
		gameController.qtdCurrentLifes -= 1;
		isAlive = false;
		playerRb.gravityScale = gravityScale;
		playerAnimator.SetBool("isDie",true);
		playerRb.velocity = Vector2.zero;
		playerRb.AddForce(new Vector2(gameController.kickX * -transform.localScale.x, gameController.kickY));
		StartCoroutine("blink");
		
	}
	IEnumerator spawnBullets()
    {
		gameController.playFx(gameController.audioController.fxLasers[gameController.idBullet]);
		
		canSpawn = false;
		
		GameObject bullet = Instantiate(gameController.prefBullet, spawnBullet.position, spawnBullet.rotation);
		//Set bullet speed
		bulletSpeedX = gameController.bulletSpeed + moveSpeed;
		bulletSpeedY = gameController.bulletSpeed + moveSpeed;
		
		if(Mathf.Abs(playerRb.velocity.x) != 0)bulletSpeedY = bulletSpeedY - moveSpeed/2;

		bullet.transform.localScale = new Vector3(.4f * transform.localScale.x, .4f, .4f);
		bullet.transform.tag = gameController.reloadTag(Tags.PLAYER);
		bullet.GetComponent<ReSkin>().idBullet = gameController.idBullet;
		bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(bulletSpeedX * dirX, bulletSpeedY * dirY);	
		
    	yield return new WaitForSeconds(gameController.delayShot);
		
		canSpawn = true;
		
    }
	IEnumerator blink()
    {
		
        yield return new WaitForSeconds(2f);
		Time.timeScale = 1;
        for(int i = 0; i < 5;i++)
        {
            yield return new WaitForSeconds(0.1f);
			playerSr.enabled = false;
			yield return new WaitForSeconds(0.1f);
			playerSr.enabled = true;
			
        }
        gameController.reSpawnPlayer();
        Destroy(this.gameObject);
                
    }
	IEnumerator blinkColor()
	{
		for(int i = 0; i < 10;i++)
        {
            yield return new WaitForSeconds(0.1f);
			playerSr.color = gameController.colorOn[gameController.idBullet];
			yield return new WaitForSeconds(0.1f);
			playerSr.color = Color.white;
			
        }
	}
	IEnumerator colorToAlpha()
	{
		
		yield return new WaitForSeconds(1);
		
		Color cor = playerSr.color;
		for(int i = 0; i < 10;i++)
		{
			cor.a -= 0.1f;
			playerSr.color = cor;
			yield return new WaitForSeconds(0.25f);
			
		}
		gameController.StartCoroutine("victory");
	}
	IEnumerator alphaToColor()
	{
		Color cor = new Color(1,1,1,0);
		playerSr.color = cor;
		
		if(gameController.state == GameState.CUTSCENE)yield return new WaitForSeconds(1.5f);
		isJumping = true;
		playerRb.AddForce(new Vector2(0, jumpForce/2));

		for(int i = 0; i < 10;i++)
		{
			
			cor.a += 0.1f;
			playerSr.color = cor;
			yield return new WaitForSeconds(0.25f);
			
		}
		gameController.state = GameState.GAMEPLAY;
		print("alpha "+playerSr.color.a);
	}
    void OnCollisionStay2D(Collision2D col)
    {
    	switch(col.gameObject.tag)
    	{
    		case "platform":
				
    			platform = col.gameObject;
				transform.parent = platform.transform;
				playerAnimator.SetBool("canFall", false);
				
				break;
			case "Untagged":
    			
				platform = col.gameObject;
				playerAnimator.SetBool("canFall", false);
				break;
			case "enemy":
				takeDamage();
				break;
			
    	}
    }
	void OnCollisionExit2D(Collision2D col)
    {
        switch (col.gameObject.tag)
        {
            case "platform":
                transform.parent = null;
                break;
        }
    }
	void OnTriggerEnter2D(Collider2D other)
	{
		switch(other.gameObject.tag)
		{
			case "collectable":
				if(!isAlive) return;
				StartCoroutine("blinkColor");
				gameController.playFx(gameController.audioController.fxPowerUp);
				other.SendMessage("interacao", SendMessageOptions.DontRequireReceiver);

				typeCollectable  type = other.GetComponentInParent<CollectableController>().type;
				
				if(type == typeCollectable.SHIELD)
				{
					shield.StopCoroutine("onShield");
					shieldOn();
					shield.animator.SetBool("shield", true);
					gameController.fAmount = gameController.shieldDuration;
				}
				
				
				break;
			case "Respawn":
			
				Collider2D collider = other.GetComponent<Collider2D>();
				
				if(collider.GetType().Name == "EdgeCollider2D")
				{
					collider.enabled = false;
					gameController.canFollow = false;	
					other.SendMessage("spawnEnemy", SendMessageOptions.DontRequireReceiver);
				}
				else
				{
					gameController.canFollow = true;
					Destroy(other.gameObject);
					
				}

				break;
			case "enemy":
				
				//Fall in the hole
				if(other.gameObject.layer == LayerMask.NameToLayer("TransparentFX"))
				{
					shield.animator.SetBool("shield", false);
					gameController.haveShield = false;
					takeDamage();
				}//Here is just to claw of miniBoss pullChick, if is laser don't run animation of explosion
				else if(other.gameObject.layer != LayerMask.NameToLayer("BulletEnemy"))takeDamage();
				break;
			case "Finish":
				isJumping = true;
				playerRb.AddForce(new Vector2(0, jumpForce));
				break;
		
			
		}
	}
	
	
}
