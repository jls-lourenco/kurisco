using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    private GameController gameController;
    public GameObject[] itensHud;
    public Text[] txtHud;

    private int qtdLifes;
    private int qtdSlots;
    
    // Start is called before the first frame update
    void Start()
    {
        gameController = FindObjectOfType(typeof(GameController)) as GameController;
        qtdLifes = gameController.qtdCurrentLifes;
        qtdSlots = 4;
        gameController.fAmount = gameController.shieldDuration;
        
    }

    // Update is called once per frame
    void Update()
    {   
        //Update Texts
        Text txt = null;
        for(int i = 0; i <itensHud.Length;i++)
        {
            txt = itensHud[i].GetComponentInParent<Text>();
            if(itensHud[i].activeSelf) txt.color = gameController.colorOn[gameController.idBullet];
            else if(i != 1 && i != 2 && i != 3 && i != 8 && i != 9 && i != 10)txt.color = gameController.colorOff[gameController.idBullet];
            
        }

        lifeHud();    
        shieldHud();
        bombHud();
        shotsHud();
    }
    void lifeHud()
    {
        for(int i = 0; i <qtdSlots;i++)
        {
            if(i >= qtdLifes)itensHud[i].SetActive(false);
        }
                    
        if(gameController.qtdCurrentLifes < qtdLifes)
        {
            qtdLifes -= 1;
            itensHud[qtdLifes].SetActive(false);
            
        }
        else if(gameController.qtdCurrentLifes > qtdLifes)
        {
            itensHud[qtdLifes].SetActive(true);
            qtdLifes +=1;
            
        }
        
        else if( gameController.qtdCurrentLifes > qtdSlots)
        {
            txtHud[6].text = "+"+(gameController.qtdCurrentLifes - qtdSlots)+"x";
            txtHud[6].color = txtHud[3].color;
        }
        else
        {
            txtHud[6].text = "+0x";
            txtHud[6].color = gameController.colorOff[gameController.idBullet];
        }

    }
    void shieldHud()
    {        
        
        if(gameController.haveShield)
        {
            itensHud[7].SetActive(true);

            if(gameController.fAmount == gameController.shieldDuration)
            { 
                StopCoroutine("shieldTime");
                StartCoroutine("shieldTime");
            }
        }
        else 
        {
            gameController.fAmount = gameController.shieldDuration;
            itensHud[7].SetActive(false);
            StopCoroutine("shieldTime");
        }

        itensHud[8].SetActive(itensHud[7].activeSelf);
        

    }
    void bombHud()
    {
        txtHud[5].text = gameController.qtdBombs.ToString() + "x";
        txtHud[5].color = txtHud[2].color;
        
        if(gameController.qtdBombs > 0)itensHud[6].SetActive(true);
        else itensHud[6].SetActive(false);
    }
    void shotsHud()
    { 
        itensHud[4].SetActive(!gameController.shotHud);
        itensHud[5].SetActive(gameController.shotHud);
        
        if(itensHud[4].activeSelf && gameController.slotUp[gameController.index])
        {
            itensHud[9].SetActive(true);

        }else itensHud[9].SetActive(false);
        
        if(itensHud[5].activeSelf && gameController.slotUp[gameController.index])
        {
            itensHud[10].SetActive(true);

        }else itensHud[10].SetActive(false);
    }
    IEnumerator shieldTime()
    {
        itensHud[8].GetComponent<Image>().fillAmount = gameController.fAmount / gameController.shieldDuration;
        gameController.fAmount -= 1;
        yield return new WaitForSeconds(1);
        
        StartCoroutine("shieldTime");
    }
}
