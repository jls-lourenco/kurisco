using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    private GameController gameController;
    public Animator		animator;
    
    
    // Start is called before the first frame update
    void Awake()
    {
        gameController = FindObjectOfType(typeof(GameController)) as GameController;
        animator 	= GetComponent<Animator>();

        animator.SetBool("shield", true);

    }

    IEnumerator onShield()
    {
        yield return new WaitForSeconds(gameController.shieldDuration);  
        
        animator.SetBool("shield", false);
        
        yield return new WaitForSeconds(.6f);
        
        gameController.haveShield = false;
        this.gameObject.SetActive(false);
        
    }
    void OnTriggerEnter2D(Collider2D other)
	{
		switch(other.gameObject.tag)
		{
			case "enemy":
                
				other.SendMessage("takeDamage",1,SendMessageOptions.DontRequireReceiver);
                
				break;
		}
    }
}
