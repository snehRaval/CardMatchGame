using UnityEngine;

namespace CyberSpeed.CardsMatchGame
{
    public class AudioManager : MonoBehaviour
    {
        [Header("Audio Sources")]
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource sfxSource;

        [Header("Music")]
        [SerializeField] private AudioClip backgroundMusic;
        [Range(0f, 1f)]
        public float musicVolume = 0.5f;
        
        [Header("SFX")]
        [SerializeField] private AudioClip cardFlipSound;
        [SerializeField] private AudioClip cardMatchSound;
        [SerializeField] private AudioClip cardMismatchSound;
        [SerializeField] private AudioClip gameOverSound;
        [SerializeField] private AudioClip buttonClickSound;
        [Range(0f, 1f)]
        public float sfxVolume = 0.7f;
         
        [Header("Settings")]
        public bool musicEnabled = true;
        public bool sfxEnabled = true;
        
        public static AudioManager Instance { get; private set; }

        private void Awake()
        {
            // Singleton pattern
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

        }

        private void Start()
        {
            LoadAudioSettings();
            PlayBackgroundMusic();
        }
        
        private void PlayBackgroundMusic()
        {
            if (musicEnabled && musicSource != null && backgroundMusic != null)
            {
                musicSource.clip = backgroundMusic;
                musicSource.volume = musicVolume;
                musicSource.loop = true;
                musicSource.Play();
            }
        }

        public void StopMusic()
        {
            musicSource.Stop();
        }

        public void SetMusicVolume(float volume)
        {
            musicSource.volume = Mathf.Clamp01(volume);
        }
        
        public void ToggleMusic()
        {
            musicEnabled = !musicEnabled;
        
            if (musicSource != null)
            {
                if (musicEnabled)
                {
                    PlayBackgroundMusic();
                }
                else
                {
                    musicSource.Stop();
                }
            }
        
            SaveAudioSettings();
        }
    
        public void ToggleSFX()
        {
            sfxEnabled = !sfxEnabled;
            SaveAudioSettings();
        }
        
        public void PlayCardFlip()
        {
            PlaySFX(cardFlipSound);
        }

        public void PlayMatchFound()
        {
            PlaySFX(cardMatchSound);
        }

        public void PlayMismatch()
        {
            PlaySFX(cardMismatchSound);
        }

        public void PlayGameOver()
        {
            PlaySFX(gameOverSound);
        }

        public void PlayButtonClick()
        {
            PlaySFX(buttonClickSound);
        }
        
        private void PlaySFX(AudioClip clip)
        {
           
            if (sfxEnabled && sfxSource != null && clip != null) 
                sfxSource.PlayOneShot(clip);
        }

        public void SetSFXVolume(float volume)
        {
            sfxSource.volume = Mathf.Clamp01(volume);
        }
        
        private void SaveAudioSettings()
        {
            PlayerPrefs.SetInt("MusicEnabled", musicEnabled ? 1 : 0);
            PlayerPrefs.SetInt("SFXEnabled", sfxEnabled ? 1 : 0);
            PlayerPrefs.SetFloat("MusicVolume", musicVolume);
            PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
            PlayerPrefs.Save();
        }
        
        private void LoadAudioSettings()
        {
            musicEnabled = PlayerPrefs.GetInt("MusicEnabled", 1) == 1;
            sfxEnabled = PlayerPrefs.GetInt("SFXEnabled", 1) == 1;
            musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
            sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 0.7f);
        }
    }
}
