using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    private AudioSource _audioSourcePrefab;
    private AudioSource _bgmSource;
    private float _bgmVolume = 1f;
    [SerializeField] private List<AudioSource> _effectSources = new List<AudioSource>();
    [SerializeField] private int _defaultSourceCount;
    private Dictionary<string, AudioClip> _audios = new Dictionary<string, AudioClip>();

    public float BGMVolume
    {
        private get { return _bgmVolume; }
        set
        {
            _bgmSource.volume = value;
            _bgmVolume = value;
        }
    }
    new protected void Awake()
    {
        base.Awake();

        _audioSourcePrefab = ResourceCache.GetResource<AudioSource>("Prefabs/DefaultSoundObject");

        for (int i = 0; i < _defaultSourceCount; ++i)
        {
            _effectSources.Add(Instantiate(_audioSourcePrefab, transform));
        }

        _bgmSource = Instantiate(_audioSourcePrefab, transform);

        AudioClip[] clips = Resources.LoadAll<AudioClip>("Sounds");
        foreach (AudioClip clip in clips)
        {
            _audios.Add(clip.name, clip);
        }
    }

    public void BGMPlay(string name, bool loop, float volume)
    {
        if (_bgmSource.isPlaying)
        {
            _bgmSource.Stop();
        }
        _bgmSource.loop = loop;
        _bgmSource.clip = GetClip(name);
        _bgmSource.volume = volume;
        _bgmSource.Play();
    }

    public void BGMPlay(string name)
    {
        if (_bgmSource.isPlaying)
        {
            _bgmSource.Stop();
        }
        _bgmSource.loop = true;
        _bgmSource.clip = GetClip(name);
        _bgmSource.volume = _bgmVolume;
        _bgmSource.Play();
    }

    public void BGMStop()
    {
        if (_bgmSource.isPlaying == false) return;
        _bgmSource.Stop();
    }

    public void EffectPlay(string name, Vector3 position)
    {
        AudioSource source = GetEffectAudioSource();
        source.clip = GetClip(name);
        source.loop = false;
        source.volume = 1f;
        source.transform.position = position;
        source.Play();
    }

    public void EffectPlay(string name, bool loop, float volume, Vector3 position)
    {
        AudioSource source = GetEffectAudioSource();
        source.clip = GetClip(name);
        source.loop = loop;
        source.volume = volume;
        source.transform.position = position;
        source.Play();
    }

    private AudioClip GetClip(string name)
    {
        if (_audios.TryGetValue(name, out var clip) == false)
        {
            Debug.Assert(clip != null, "클립 없음");
        }
        return clip;
    }

    private AudioSource GetEffectAudioSource()
    {
        AudioSource source = null;
        bool success = false;
        for (int i = 0; i < _effectSources.Count; i++)
        {
            if (_effectSources[i].isPlaying == false)
            {
                source = _effectSources[i];
                success = true;
                break;
            }
        }

        if (success == false)
        {
            source = Instantiate(_audioSourcePrefab, transform);
            _effectSources.Add(source);
        }

        return source;
    }
}
