using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleAudioManager
{
    public class Manager : MonoBehaviour
    {
        #region PROPERTIES

        /// <summary>
        /// Singleton
        /// </summary>
        public static Manager instance => _instance;
        private static Manager _instance = null;

        /// <summary>
        /// The attached audio source
        /// </summary>
        [Header("CONFIGURATIONS")]
        [Tooltip("The audio source prefab which will be used in the audio source pool.")] public GameObject audioSourcePrefab = null;
        private List<AudioSource> sourcePool = new List<AudioSource>();
        private int _currentSourceIndex = -1;

        [Tooltip("Should the current song loop?")] public bool loopCurrentSong = true;
        private Coroutine _loop;
        private Song.Data _currentSong;
        private int _currentIntensity;
        
        /// <summary>
        /// The time before either a non-looping clip ends or the next loop of a looping clip begins
        /// </summary>
        public float clipTimeRemaining => (float)(_nextLoopStartTime - AudioSettings.dspTime);
        private double _nextLoopStartTime = 0;

        [Tooltip("Should the manager play the first song on awake?")] public bool playOnAwake = true;
        [Tooltip("The maximum volume for the audio clips.")][Range(0f, 1f)] public float maxVolume = 1f;
        [Tooltip("The amount of time it will take for different songs to blend between one-another.")] public float defaultSongBlendDuration = 1f;
        [Tooltip("The amount of time it will take for different intensities of the same song to blend between one-another.")] public float defaultIntensityBlendDuration = 1f;

        [Space(8f)]
        /// <summary>
        /// The available songs for the manager
        /// </summary>
        [Tooltip(
            "The list of available songs for the manager to play.\n" +
            "-To create a new song:\n" +
            "  -Right Click and select:\n" +
            "    Create->SimpleAudioManager->Song\n" +
            "  -Add the intensity clips or any other desired clips\n" +
            "  -Set the reverb tail time\n" +
            "    (Seconds before the end of a clip to loop it)\n" +
            "    (Shown in parentheses on Ovani Folders)\n" +
            "  -Drag & Drop your songs onto this list")] [SerializeField] private List<Song> _songs = new List<Song>();
        private List<Song.Data> _data = new List<Song.Data>();

        #endregion

        #region PUBLIC METHODS

        /// <summary>
        /// Shorthand set Intensity
        /// </summary>
        public void SetIntensity(int pIntensity) => SetIntensity(pIntensity, defaultIntensityBlendDuration, defaultIntensityBlendDuration);

        /// <summary>
        /// Sets the intensity for the current song
        /// </summary>
        public void SetIntensity(int pIntensity, float pBlendOutDuration, float pBlendInDuration)
        {
            if (_currentSong.intensityClips.Count > Mathf.Max(pIntensity, 0))
            {
                PlaySong( new PlaySongOptions() {
                    song = _data.IndexOf(_currentSong),
                    intensity = Mathf.Max(pIntensity, 0),
                    startTime = sourcePool[_currentSourceIndex].time,
                    blendOutTime = pBlendOutDuration,
                    blendInTime = pBlendInDuration
                });
            }
        }

        /// <summary>
        /// Plays the specified song and attempts to match the current intensity
        /// </summary>
        public void PlaySong(int pSong) => PlaySong( new PlaySongOptions() {
                song = pSong,
                intensity = _currentIntensity,
                startTime = 0f,
                blendOutTime = defaultSongBlendDuration,
                blendInTime = defaultSongBlendDuration
            });

        /// <summary>
        /// Plays the specified song
        /// </summary>
        public void PlaySong(PlaySongOptions pOptions)
        {
            //  Updates the data collection
            _UpdateSongData();

            //  Confirm song exists
            if (_data == null || _data.Count == 0 || _data.Count <= pOptions.song) return;
            //  Confirm clip exists
            if (_data[pOptions.song].intensityClips == null || _data[pOptions.song].intensityClips.Count == 0) return;
            //  Confirm we aren't blending to the same clip
            if (pOptions.song == _data.IndexOf(_currentSong) && pOptions.intensity == _currentIntensity) return;
            //  Do our best to match intensity
            if (_data[pOptions.song].intensityClips.Count <= pOptions.intensity) pOptions.intensity = _data[pOptions.song].intensityClips.Count - 1;

            //  Get the next available audio source
            if (_currentSourceIndex != -1)
            {
                AudioSource _current = sourcePool[_currentSourceIndex];
                //  If the blendoutduration is less than 0, kill the playback after the reverb tail length
                if (pOptions.blendOutTime >= 0f) StartCoroutine(_FadeVolume(_current, _current.volume, 0f, pOptions.blendOutTime));
                else StartCoroutine(_FadeVolume(_current, _current.volume, _current.volume, _currentSong.reverbTail));
            }
            _currentSourceIndex = sourcePool.IndexOf(_GetNextSource());
            AudioSource _nextSource = sourcePool[_currentSourceIndex];
            _currentSong = _data[pOptions.song];
            _currentIntensity = pOptions.intensity;
            AudioClip _clip = _currentSong.intensityClips[pOptions.intensity];
            _nextSource.gameObject.name = _clip.name;
            if (loopCurrentSong)
            {
                if (_loop != null) StopCoroutine(_loop);
                _loop = StartCoroutine(_Loop(pOptions.song, pOptions.intensity, _clip.length - _currentSong.reverbTail - pOptions.startTime));
            }
            else _nextLoopStartTime = AudioSettings.dspTime + _clip.length;
            StartCoroutine(_FadeVolume(_nextSource, 0f, maxVolume, pOptions.blendInTime));
            _nextSource.clip = _clip;
            _nextSource.time = pOptions.startTime;
            _nextSource.Play();
        }

        /// <summary>
        /// Play song options
        /// </summary>
        public struct PlaySongOptions
        {
            public int song;
            public int intensity;
            public float startTime;
            public float blendOutTime;
            public float blendInTime;
        }

        /// <summary>
        /// Stops the current song playing
        /// </summary>
        public void StopSong()
        {
            sourcePool.ForEach(s => s.Stop());
            StopCoroutine(_loop);
            _currentSourceIndex = -1;
        }

        #endregion

        #region PRIVATE METHODS

        /// <summary>
        /// Config
        /// </summary>
        private void Awake()
        {
            _instance = _instance ?? this;
            if (_instance != this) DestroyImmediate(gameObject);
            if (playOnAwake) PlaySong(0);
        }

        private void OnDestroy() => _instance = _instance == this ? null : _instance;

        /// <summary>
        /// Builds the data pool for the songs
        /// </summary>
        private void _UpdateSongData()
        {
            _data.Clear();
            _songs.ForEach(s => _data.Add(s.ToSongData()));
        }

        /// <summary>
        /// Gets the next available source that is not playing
        /// </summary>
        private AudioSource _GetNextSource()
        {
            AudioSource next = sourcePool.Find(s => !s.isPlaying);
            if (next == null)
            {
                next = Instantiate(audioSourcePrefab, transform).GetComponent<AudioSource>();
                sourcePool.Add(next);
            }
            next.gameObject.SetActive(true);
            return next;
        }

        /// <summary>
        /// Force an audio source to fade in or out
        /// </summary>
        private IEnumerator _FadeVolume(AudioSource pSource, float pStart, float pEnd, float pDuration)
        {
            pDuration = Mathf.Max(pDuration, 0f);
            float duration = 0f;
            while (duration < pDuration)
            {
                yield return new WaitForEndOfFrame();
                duration += Time.unscaledDeltaTime;
                pSource.volume = Mathf.SmoothStep(pStart, pEnd, duration / pDuration);
            }
            pSource.volume = pEnd;
            if (pSource.volume == 0f || pEnd == pStart)
            {
                pSource.Stop();
                pSource.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Plays the provided song after a wait period
        /// </summary>
        private IEnumerator _Loop(int pSong, int pIntensity, float pWaitTime)
        {
            _nextLoopStartTime = AudioSettings.dspTime + pWaitTime;
            yield return new WaitForSecondsRealtime(pWaitTime);
            if (!loopCurrentSong) yield break;
            PlaySong( new PlaySongOptions() {
                    song = pSong,
                    intensity = pIntensity,
                    startTime = 0f,
                    blendOutTime = -1f,
                    blendInTime = 0f
                });
        }

        #endregion
    }
}

/*
 * Written by Ovani Sound & Brutiful Games
 * No credit required.
 * Revision: 06/20/2023
 */