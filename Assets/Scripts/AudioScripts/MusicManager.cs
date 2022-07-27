using System.Collections.Generic;
using DarkTonic.MasterAudio;
using MEC;
using TigerForge;
using UnityEngine;

namespace AudioScripts
{
    public class MusicManager : MonoBehaviour
    {
        [SerializeField] private PlaylistController controller;
        
        private void Start()
        {
            Timing.RunCoroutine(SetInitialPlayListStatus());
            if (AudioStatus.musicOn==false)
            {
                controller.PausePlaylist();
            }
            EventManager.StartListening("MusicStatusChanged", OnMusicStatusChanged);
        }
        private IEnumerator<float> SetInitialPlayListStatus()
        {
            yield return Timing.WaitForOneFrame;
            yield return Timing.WaitForOneFrame;
            
            if (AudioStatus.musicOn == false)
            {
                controller.PausePlaylist();
            }
            if (AudioStatus.musicOn)
            {
                controller.UnpausePlaylist();
                controller.StartPlaylist("MainBackgroundLoop");
            }
        }

        private void OnMusicStatusChanged()
        {
            if (AudioStatus.musicOn == false)
            {
                controller.PausePlaylist();
            }
            if (AudioStatus.musicOn)
            {
                controller.StartPlaylist("MainBackgroundLoop");
                controller.UnpausePlaylist();
            }
        }
    }
}