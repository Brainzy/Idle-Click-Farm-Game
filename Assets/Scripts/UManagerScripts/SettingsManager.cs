using TigerForge;
using TMPro;
using UnityEngine;

namespace UManagerScripts
{
    public class SettingsManager : MonoBehaviour
    {    
        [SerializeField] private GameObject settingsCanvas;
        [SerializeField] private TextMeshProUGUI soundText;
        [SerializeField] private TextMeshProUGUI musicText;

        private void Awake()
        {
            AudioStatus.soundOn = !ES3.KeyExists("SoundStatus") || ES3.Load<bool>("SoundStatus");
            AudioStatus.musicOn = !ES3.KeyExists("MusicStatus") || ES3.Load<bool>("MusicStatus");
            soundText.SetText(AudioStatus.soundOn ? "On" : "Off");
            musicText.SetText(AudioStatus.musicOn ? "On" : "Off");
        }
        
        public void OpenCloseSettings()
        {
            EventManager.EmitEvent("UIClick");
            if (settingsCanvas.activeSelf)
            {
                settingsCanvas.SetActive(false);
            }
            else
            {
                settingsCanvas.SetActive(true);
            }
        }

        public void OnSoundButton()
        {
            if (soundText.text == "On")
            {
                soundText.SetText("Off");
                AudioStatus.soundOn = false;
                ES3.Save("SoundStatus", false);
            }
            else
            {
                soundText.SetText("On");
                AudioStatus.soundOn = true;
                ES3.Save("SoundStatus", true);
                EventManager.EmitEvent("UIClick");
            }
        }
        
        public void OnMusicButton()
        {
            EventManager.EmitEvent("UIClick");
            if (musicText.text == "On")
            {
                musicText.SetText("Off");
                AudioStatus.musicOn = false;
                ES3.Save("MusicStatus", false);
            }
            else
            {
                musicText.SetText("On");
                AudioStatus.musicOn = true;
                ES3.Save("MusicStatus", true);
            }
            EventManager.EmitEvent("MusicStatusChanged");
        }
        
    }
}
