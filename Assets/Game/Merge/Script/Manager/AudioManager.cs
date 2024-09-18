using UnityEngine;
using System.Collections.Generic;
using System.Collections;
// using DG.Tweening;
namespace Merge
{
    public class AudioManager : Singleton<AudioManager>
    {
        public static int MusicSetting
        {
            get { return PlayerPrefs.GetInt("music_setting", 1); }
            private set { PlayerPrefs.SetInt("music_setting", value); }
        }
        public static int SoundSetting
        {
            get { return PlayerPrefs.GetInt("sound_setting", 1); }
            private set { PlayerPrefs.SetInt("sound_setting", value); }
        }
        public static float soundVolume
        {
            get { return PlayerPrefs.GetFloat("sound_vol", 1); }
            set { PlayerPrefs.SetFloat("sound_vol", value); }
        }
        public static float musicVolume
        {
            get { return PlayerPrefs.GetFloat("music_vol", 1); }
            set
            {
                if (value <= 0)
                {
                    Instance.EnableMusic(false);
                }
                if (musicVolume <= 0 && value > 0)
                {
                    Instance.EnableMusic(true);
                }
                PlayerPrefs.SetFloat("music_vol", value);
            }
        }
        [SerializeField] AudioContainerSO commonSound;
        [SerializeField] AudioContainerSO musics;
        [SerializeField] AudioSource soundPlayer;
        [SerializeField] AudioSource musicPlayer;
        [field: SerializeField] private List<AudioSource> activeAudioSources = new List<AudioSource>();
        [field: SerializeField] private List<AudioSource> inActiveAudioSources = new List<AudioSource>();
        protected void Awake()
        {
            for (int i = 0; i < 2; i++)
            {
                AudioSource audioSource = Instantiate(musicPlayer, transform);
                inActiveAudioSources.Add(audioSource);
            }
            DontDestroyOnLoad(gameObject);
        }
        public void PlayOneShot(AudioClip audioClip, float volume, float pitch = 1, float delay = 0)
        {
            if (SoundSetting != 1) return;
            if (audioClip == null) return;
            StartCoroutine(IEDeplayPlayOneShot(audioClip, volume, pitch, delay));
        }
        public void PlayOneShot(string clipName, float volume, float pitch = 1, float delay = 0)
        {
            if (SoundSetting != 1) return;
            AudioClip clip = commonSound.GetClip(clipName);
            PlayOneShot(clip, volume, pitch, delay);
        }
        IEnumerator IEDeplayPlayOneShot(AudioClip audioClip, float volume, float pitch, float delay = 0)
        {
            float timer = delay;
            while (timer > 0)
            {
                timer -= Time.deltaTime;
                yield return null;
            }
            float newVolume = volume;
            soundPlayer.pitch = pitch;
            soundPlayer.PlayOneShot(audioClip, newVolume);
        }
        public void PlayMusic(string clipName, float volume, bool isLoop)
        {
            if (MusicSetting != 1)
            {
                volume = 0;
            }
            AudioClip clip = musics.GetClip(clipName);
            if (clip == null) return;
            if (IsPlaying(clip.name) != null) return;
            AudioSource source = GetAudioSource();
            source.clip = clip;
            source.loop = isLoop;
            source.Play();

        }
        public void PlayMusic(AudioClip clip, float volume, bool isLoop)
        {
            if (MusicSetting != 1)
            {
                volume = 0;
            }
            if (clip == null) return;
            if (IsPlaying(clip.name) != null) return;
            AudioSource source = GetAudioSource();
            source.clip = clip;
            source.loop = isLoop;
            //source.volume = volume;
            source.Play();

        }
        private AudioSource IsPlaying(string clipName)
        {
            for (int i = 0; i < activeAudioSources.Count; i++)
            {
                if (activeAudioSources[i].clip.name == clipName)
                {
                    return activeAudioSources[i];
                }
            }
            return null;
        }
        private AudioSource GetAudioSource()
        {
            AudioSource audioSource = null;
            if (inActiveAudioSources.Count > 0)
            {
                audioSource = inActiveAudioSources[0];
                inActiveAudioSources.RemoveAt(0);
            }
            else
            {
                audioSource = musicPlayer.gameObject.AddComponent<AudioSource>();
            }
            activeAudioSources.Add(audioSource);
            return audioSource;
        }

        public void StopSound()
        {
            soundPlayer.Stop();
        }

        public void StopMusic(string musicName)
        {
            var source = IsPlaying(musicName);
            if (source)
            {
                source.Stop();
                activeAudioSources.Remove(source);
                inActiveAudioSources.Add(source);
            }
        }
        public void StopAllMusic()
        {
            for (int i = 0; i < activeAudioSources.Count; i++)
            {
                activeAudioSources[i].Stop();
            }
            inActiveAudioSources.AddRange(activeAudioSources);
            activeAudioSources.Clear();
        }
        public void ResumeMusic()
        {
            musicPlayer.Play();
        }
        public void EnableMusic(bool status)
        {
            MusicSetting = status ? 1 : 0;
            if (MusicSetting != 1)
            {
                for (int i = 0; i < activeAudioSources.Count; i++)
                {
                    activeAudioSources[i].volume = 0;
                }
            }
            else
            {
                for (int i = 0; i < activeAudioSources.Count; i++)
                {
                    activeAudioSources[i].volume = 1;
                }
            }
        }
        public void EnableSound(bool status)
        {
            SoundSetting = status ? 1 : 0;
        }

    }
    public static class AUDIO_CLIP_NAME
    {
        public static string CLICK_BUTTON = "SFX_UI_Button_Click";
        public static string SOUND_COUNTDOWN_CLOCK = "SFX_CountDown_Clock";
        public static string End_COUNTDOWN_CLOCK = "SFX_End_CountDown";
    }
}