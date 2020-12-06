using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioOptions : MonoBehaviour
{
    private AudioController audioController;
    public Slider sliderFx;
    public Slider sliderMusic;
    
    private AudioSource testVolume;
    private int muteF, muteM;

    public Image[] imgMute;
    public Sprite[] sptMute;

    
    // Start is called before the first frame update
    void Start()
    {
        audioController = FindObjectOfType(typeof(AudioController)) as AudioController;
        testVolume = GetComponent<AudioSource>();
        
        loadVolume();

        sliderFx.onValueChanged.AddListener(fxVolume);
        sliderMusic.onValueChanged.AddListener(musicVolume);

        sliderFx.interactable = !audioController.fx.mute;
        sliderMusic.interactable = !audioController.music.mute;
     
    }
    public void fxVolume(float value)
    {
        print("fx");
        
        audioController.fx.volume = value/10;
        testVolume.volume = audioController.fx.volume;//testVolume is just to test volume of source fx

        testVolume.PlayOneShot(audioController.fxButton);
    }
    public void musicVolume(float value)
    {
        print("music");

        testVolume.volume = value/10; //testVolume is just to test volume of source music
        testVolume.PlayOneShot(audioController.fxButton);
        
    }
    public void saveVolume()
    {

        PlayerPrefs.SetFloat("FxVolume", sliderFx.value/10);// Must be saved in float because the AudioSource just go from 0 to 1
        PlayerPrefs.SetFloat("MusicVolume", sliderMusic.value/10);
        PlayerPrefs.SetInt("MuteFx", muteF);
        PlayerPrefs.SetInt("MuteMusic", muteM);
    }
    public void loadVolume()
    {
        sliderFx.value = PlayerPrefs.GetFloat("FxVolume") * 10;// Must be loaded in int because the Slider is configured to receive from 0 to 10
        sliderMusic.value = PlayerPrefs.GetFloat("MusicVolume") * 10;
        
        imgMute[0].sprite = PlayerPrefs.GetInt("MuteFx") == 1 ? sptMute[0] : sptMute[1];
        imgMute[1].sprite = PlayerPrefs.GetInt("MuteMusic") == 1 ? sptMute[2] : sptMute[3];
        
        
    }
	public void muteFx()
	{
        audioController.playFx(audioController.fxButton);

		audioController.fx.mute = !audioController.fx.mute;
        muteF = audioController.fx.mute ? 1: 0;
        
        testVolume.mute = audioController.fx.mute;
        sliderFx.interactable = !audioController.fx.mute;

        imgMute[0].sprite = audioController.fx.mute ? sptMute[0] : sptMute[1];
        
	}
    public void muteMusic()
	{
        audioController.playFx(audioController.fxButton);
        
		audioController.music.mute = !audioController.music.mute;
        muteM = audioController.music.mute ? 1: 0;
        
        sliderMusic.interactable = !audioController.music.mute;

        imgMute[1].sprite = audioController.music.mute ? sptMute[2] : sptMute[3];
	}

    
}
