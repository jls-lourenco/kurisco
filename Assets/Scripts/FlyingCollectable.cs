using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingCollectable : MonoBehaviour
{
    private CollectableController cc;
    private Rigidbody2D    rigidbody2d;
    private Animator       animator;
    public GameObject item;
    public GameObject wings;

    public Transform checkPoint;
    public float    moveSpeed;
    private bool canMove;
    

    // Start is called before the first frame update
    void Start()
    {
        cc              = GetComponentInParent<CollectableController>();
        cc.sRender      = GetComponent<SpriteRenderer>();
        rigidbody2d     = item.GetComponent<Rigidbody2D>();
        cc.collider2d   = GetComponent<Collider2D>(); 
        animator        = GetComponent<Animator>();  
        
        cc.reloadCollectable();
        wingsOn();       
    }
    
    void LateUpdate()
    {
        if(canMove){ move(); }
        
    }
    void interacao()
    {
        cc.interacao();
                        
    }
    void move()
    {       
        item.transform.position = Vector3.MoveTowards(item.transform.position, checkPoint.position, moveSpeed * Time.deltaTime);
                
    }
    void wingsOn()
    {
        canMove = true; 
        wings.SetActive(true);
        item.GetComponent<Collider2D>().enabled = true;
        
    }
    void drop()
    {
        canMove = false;
        rigidbody2d.bodyType = RigidbodyType2D.Dynamic;
        animator.enabled = false;
        wings.SetActive(false);
    }
    void OnBecameInvisible() 
    {
        Destroy(item);
    }
    void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject.tag =="platform" && !canMove)item.transform.parent = other.gameObject.transform;
        
    }
}
