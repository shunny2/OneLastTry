using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public AudioSource music;
    public Slider volume;
    
    private static AudioManager instance;

    void Start()
    {
        volume.value = PlayerPrefs.GetFloat("MusicVol");
    }
    
    void Update()
    {
        music.volume = volume.value;
    }

    public void VolumePrefs()
    {
        //Saving Volume
        PlayerPrefs.SetFloat("MusicVol", music.volume);
    }
}
