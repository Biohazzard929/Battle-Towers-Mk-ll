using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        // make sure battle select does NOT loop
        battleSelectMusic.loop = false;
        // and your bgm tracks also do NOT loop individually
        foreach (var track in bgm)
            track.loop = false;
    }

    public AudioSource menuMusic;
    public AudioSource battleSelectMusic;
    public AudioSource[] bgm;
    private int currentBGM;
    private bool playingBGM;
    private bool playingBattleSelect;

      public AudioSource[] sfx;

    // Start is called before the first frame update
    void Start()
    {

    }

   

    void Update()
    {
        // 1) If we’re in battle‑select mode and that clip has just finished…
        if (playingBattleSelect && !battleSelectMusic.isPlaying)
        {
            playingBattleSelect = false;
            PlayBGM();              // ← kick off the playlist
        }

        // 2) Your existing playlist logic
        if (playingBGM)
        {
            if (!bgm[currentBGM].isPlaying)
            {
                currentBGM = (currentBGM + 1) % bgm.Length;
                bgm[currentBGM].Play();
            }
        }
    }

    public void StopMusic()
    {
        menuMusic.Stop();
        battleSelectMusic.Stop();
        foreach (var track in bgm) track.Stop();

        playingBGM = false;
        // ensure battle‑select flag is off if you manually stop
        playingBattleSelect = false;
    }

    public void PlayMenuMusic()
    {
        StopMusic();
        menuMusic.Play();
    }

    public void PlayBattleSelectMusic()
    {
        StopMusic();
        playingBattleSelect = true;   // ← enter battle‑select mode
        battleSelectMusic.Play();
    }

    public void PlayBGM()
    {
        StopMusic();                  // stop anything else
        // pick a random starting point
        currentBGM = Random.Range(0, bgm.Length);
        bgm[currentBGM].Play();
        playingBGM = true;
    }

    public void PlaySFX(int sfxToPlay)
    {
        sfx[sfxToPlay].Stop();
        sfx[sfxToPlay].Play();
    }
}
