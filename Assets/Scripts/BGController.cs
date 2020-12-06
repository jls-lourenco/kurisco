using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGController : MonoBehaviour
{

	[Header("Settings BG")]
	private	float	offSet;
	public	float	speedOffSet;


	private SpriteRenderer meshRenderer;
	private	Material currentMaterial;


    // Start is called before the first frame update
    void Start()
    {
    	meshRenderer = GetComponent<SpriteRenderer>();
    	currentMaterial = meshRenderer.material;

        
    }

    // Update is called once per frame
    void Update()
    {
        offSet += speedOffSet;
        currentMaterial.SetTextureOffset("_MainTex", new Vector2(offSet,0));
		
    }
}
