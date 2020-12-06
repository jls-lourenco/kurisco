using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{   
    void OnTriggerStay2D(Collider2D other) {
        
        if(other.gameObject.tag == "Player")
        {
            other.SendMessage("takeDamage",SendMessageOptions.DontRequireReceiver);
            
        }
    }
    
}
