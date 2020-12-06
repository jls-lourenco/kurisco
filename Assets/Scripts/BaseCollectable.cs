using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCollectable : MonoBehaviour
{
    private CollectableController cc;
    public GameObject lightOn;
    public SpriteRenderer srLight, srBase;
    public Sprite[] sLight, sBase;
    
    // Start is called before the first frame update
    void Start()
    {
        cc              = GetComponentInParent<CollectableController>();
        cc.sRender      = GetComponent<SpriteRenderer>();
        cc.collider2d   = GetComponent<Collider2D>();

        reloadSprite();
        cc.reloadCollectable();
                
    }

    void reloadSprite()
    {
        switch(cc.type)
        {
            case typeCollectable.BULLET0:
                
                srLight.sprite = sLight[0];
                srBase.sprite = sBase[0];

                break;
            case typeCollectable.BULLET1:

                srLight.sprite = sLight[1];
                srBase.sprite = sBase[1];

                break;
            case typeCollectable.BULLET2:
                
                srLight.sprite = sLight[2];
                srBase.sprite = sBase[2];

                break;
        }
    }
    void interacao()
    {
        cc.interacao();
        StartCoroutine("blink");
    }
    IEnumerator blink()
    {
        yield return new WaitForSeconds(1f);
        for(int i = 0; i < 5;i++)
        {
            yield return new WaitForSeconds(0.1f);
            lightOn.SetActive(false);
            yield return new WaitForSeconds(0.1f);
            lightOn.SetActive(true);
            
        }
        
        Destroy(lightOn);
        Destroy(cc.gameObject,3);
        
        
    }
    
}
