using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    private PlayerController player ;
    public SpriteRenderer srLight;
    public Transform basePortal;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType(typeof(PlayerController)) as PlayerController;
        srLight = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(player != null)srLight.sortingOrder = basePortal.position.y > player.transform.position.y ? 8 : 11;
        else player = FindObjectOfType(typeof(PlayerController)) as PlayerController;
        
    }
}
