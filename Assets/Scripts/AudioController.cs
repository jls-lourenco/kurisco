using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [Header("Audio Source")]
    public AudioSource music;
    public AudioSource fx;

    [Header("Music")]
    public AudioClip musicTitle;
    public AudioClip[] musicStage;
    public AudioClip musicStageClear;
    public AudioClip musicGameOver;
    
    [Header("FX")]
    public AudioClip[] fxLasers;
    public AudioClip fxLaserBoss;
    public AudioClip fxExplosion;
    public AudioClip fxBomb;
    public AudioClip fxMenuUp;
    public AudioClip fxMenuDown;
    public AudioClip fxButton;
    public AudioClip fxPowerUp;
    public AudioClip fxShield;
	public AudioClip fxhitKill;
    public AudioClip fxSteps;
    
    // Start is called before the first frame update
    void Start()
    {
        startPlay(); 
    }

	void startPlay()
    {
        if(PlayerPrefs.GetInt("FirstTime") == 0)
        {
            PlayerPrefs.SetInt("FirstTime", 1);

            PlayerPrefs.SetFloat("FxVolume", .6f);
            PlayerPrefs.SetFloat("MusicVolume", .8f);
            
            
        }

        fx.volume = PlayerPrefs.GetFloat("FxVolume");
        music.volume = PlayerPrefs.GetFloat("MusicVolume");

        fx.mute = PlayerPrefs.GetInt("MuteFx") == 1 ? true : false;
        music.mute = PlayerPrefs.GetInt("MuteMusic") == 1? true : false;

        music.clip = musicTitle;
        music.loop = true;
    }
    
    public IEnumerator changeMusic(AudioClip newMusic)
	{
		float volumeMax = music.volume;

		for(float volume = music.volume; volume > 0; volume -= 0.01f)
		{
			music.volume = volume;
			yield return new WaitForEndOfFrame();
		}


		music.clip = newMusic;
		music.Play();

		for(float volume = 0; volume < volumeMax; volume += 0.01f)
		{
			music.volume = volume;
			yield return new WaitForEndOfFrame();
		}
	}
	public void playFx(AudioClip clip)
	{
		fx.PlayOneShot(clip);
	}
    
        
}
