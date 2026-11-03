using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    [Header("Audio Clips")]
    public AudioClip bgmClip;
    public AudioClip buttonClickSound;
    public AudioClip jumpSound;
    public AudioClip shootSound;

    [Header("UI Elements (optional)")]
    public Slider musicSlider;
    public Slider sfxSlider;
    public Button musicToggleButton;
    public Button sfxToggleButton;
    public Sprite musicOnIcon, musicOffIcon;
    public Sprite sfxOnIcon, sfxOffIcon;

    private bool musicMuted = false;
    private bool sfxMuted = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            bgmSource.clip = bgmClip;
            bgmSource.loop = true;
            bgmSource.Play();
        }
        else
        {
            Destroy(gameObject); // tránh tạo nhiều AudioManager nếu có sẵn
        }
    }

    void Start()
    {
        // Load trạng thái lưu trước đó nếu có
        musicMuted = PlayerPrefs.GetInt("MusicMuted", 0) == 1;
        sfxMuted = PlayerPrefs.GetInt("SFXMuted", 0) == 1;

        float savedMusicVol = PlayerPrefs.GetFloat("MusicVolume", 1f);
        float savedSFXVol = PlayerPrefs.GetFloat("SFXVolume", 1f);

        bgmSource.volume = savedMusicVol;
        sfxSource.volume = savedSFXVol;

        bgmSource.mute = musicMuted;
        sfxSource.mute = sfxMuted;

        if (musicSlider != null)
        {
            musicSlider.onValueChanged.AddListener(SetMusicVolume);
            musicSlider.value = savedMusicVol;
        }

        if (sfxSlider != null)
        {
            sfxSlider.onValueChanged.AddListener(SetSFXVolume);
            sfxSlider.value = savedSFXVol;
        }

        if (musicToggleButton != null)
        {
            musicToggleButton.onClick.AddListener(ToggleMusic);
            musicToggleButton.image.sprite = musicMuted ? musicOffIcon : musicOnIcon;
        }

        if (sfxToggleButton != null)
        {
            sfxToggleButton.onClick.AddListener(ToggleSFX);
            sfxToggleButton.image.sprite = sfxMuted ? sfxOffIcon : sfxOnIcon;
        }
    }

    public void SetMusicVolume(float value)
    {
        bgmSource.volume = value;
        PlayerPrefs.SetFloat("MusicVolume", value);
    }

    public void SetSFXVolume(float value)
    {
        sfxSource.volume = value;
        PlayerPrefs.SetFloat("SFXVolume", value);
    }

    public void ToggleMusic()
    {
        musicMuted = !musicMuted;
        bgmSource.mute = musicMuted;
        PlayerPrefs.SetInt("MusicMuted", musicMuted ? 1 : 0);

        if (musicToggleButton != null && musicToggleButton.image != null)
            musicToggleButton.image.sprite = musicMuted ? musicOffIcon : musicOnIcon;
    }

    public void ToggleSFX()
    {
        sfxMuted = !sfxMuted;
        sfxSource.mute = sfxMuted;
        PlayerPrefs.SetInt("SFXMuted", sfxMuted ? 1 : 0);

        if (sfxToggleButton != null && sfxToggleButton.image != null)
            sfxToggleButton.image.sprite = sfxMuted ? sfxOffIcon : sfxOnIcon;
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip != null && !sfxMuted)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    public void PlayButtonClick() => PlaySFX(buttonClickSound);
    public void PlayJumpSound() => PlaySFX(jumpSound);
    public void PlayShootSound() => PlaySFX(shootSound);
}
