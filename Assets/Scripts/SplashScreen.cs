using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour {

    AudioController audioController;
    public SpriteRenderer logo;
    public Sprite[] sprites;
    private void Start() {
        
        audioController = FindObjectOfType(typeof(AudioController)) as AudioController;
        logo.sprite = sprites[0];

        StartCoroutine("splash");
    }
    IEnumerator splash()
    {
        yield return new WaitForSeconds(2f);
        
        Color cor = logo.color;
        
		for(int i = 0; i < 10;i++)
		{			
            cor.a -= 0.1f;
			logo.color = cor;

			yield return new WaitForSeconds(0.1f);
			
		}
        yield return new WaitForSeconds(.25f);
        logo.sprite = sprites[1];
        for(int i = 0; i < 10;i++)
		{
			cor.a += 0.1f;
			logo.color = cor;
			yield return new WaitForSeconds(0.1f);
			
		}
        yield return new WaitForSeconds(2);
        for(int i = 0; i < 10;i++)
		{
			cor.a -= 0.1f;
			logo.color = cor;
			yield return new WaitForSeconds(0.1f);
			
		}
        yield return new WaitForSeconds(.25f);
        audioController.music.Play();
        SceneManager.LoadScene("Menu");
    }

}