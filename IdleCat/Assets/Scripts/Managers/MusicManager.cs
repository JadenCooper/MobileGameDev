using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource MainGameMusic;
    public List<AudioClip> MusicPlaylist = new List<AudioClip>();
    private int songIndex = 0;
    private void Start()
    {
        RandomSong();
    }
    private void Update()
    {
        if (!MainGameMusic.isPlaying && Application.isFocused)
        {
            NextSong();
        }
    }
    public void NextSong()
    {
        songIndex++;
        if (songIndex > (MusicPlaylist.Count - 1))
        {
            songIndex = 0;
        }
        SetMusic();
    }
    public void PreviousSong()
    {
        songIndex--;
        if (songIndex < 0)
        {
            songIndex = MusicPlaylist.Count - 1;
        }
        SetMusic();
    }
    public void SetMusic()
    {
        MainGameMusic.clip = MusicPlaylist[songIndex];
        PlayMusic();
    }
    public void PlayMusic()
    {
        MainGameMusic.Play();
    }
    public void PauseMusic()
    {
        MainGameMusic.Pause();
    }

    public void RandomSong()
    {
        songIndex = Random.Range(0, MusicPlaylist.Count - 1);
        SetMusic();
    }
}
