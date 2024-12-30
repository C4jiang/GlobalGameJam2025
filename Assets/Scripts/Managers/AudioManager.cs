using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.UI;
using FMOD.Studio;
using FMODUnity;
using System.Collections;
using FMOD;
using Sirenix.OdinInspector;
using UnityEngine.Serialization;
using Debug = UnityEngine.Debug;

public class AudioManager : Singleton<AudioManager>
{
    public float BGMSettingRatio = 0.8f;
    public float SFXSettingRatio = 0.3f;

    public EventReference titleBGM;
    private EventInstance _bgmInstance;
    private FMOD.GUID _lastBGMGuid;
    
    public List<MusicEffectInfo> musicEffectTable;
    private Dictionary<EMusicEffectType, MusicEffect> _musicEffects = new Dictionary<EMusicEffectType, MusicEffect>();
    
    private Bus _bgmBus;
    private Bus _soundEffectBus;
    
    
    void Start()
    {
        InitFMOD();
        PlayTitleBGM();

        Messenger.AddListener<EventReference>(MsgType.PlayBGM, PlayBGMByRef);
        Messenger.AddListener<EventReference>(MsgType.PlaySE, PlaySoundEffectByRef);
        // Messenger.AddListener(MsgType.LowerBGM, PlayMusicEffect3Eq);
        // Messenger.AddListener(MsgType.ResumeBGM, StopMusicEffect3Eq);
        Messenger.AddListener(MsgType.StopBGM, StopBGM);
        
        DontDestroyOnLoad(transform.gameObject);
    }

    private void InitFMOD()
    {
        _bgmBus = RuntimeManager.GetBus("bus:/Music");
        _bgmBus.setVolume(BGMSettingRatio);
        _soundEffectBus = RuntimeManager.GetBus("bus:/SoundEffect");
        _soundEffectBus.setVolume(SFXSettingRatio);
        
        musicEffectTable
            .ForEach(t => _musicEffects.Add(t.type, new MusicEffect(t.effectRef)));
    }

    public void PlayTitleBGM()
    {
        PlayBGMByRef(titleBGM);
    }

    public void PlayBGMByRef(EventReference fmodRef)
    {
        if(_lastBGMGuid == fmodRef.Guid)
        {
            return;
        }
        _lastBGMGuid = fmodRef.Guid;
        
        if (_bgmInstance.isValid())
        {
            _bgmInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            _bgmInstance.release();
        }
        _bgmInstance = RuntimeManager.CreateInstance(fmodRef);
        _bgmInstance.start();
    }

    public void StopBGM()
    {
        if (_bgmInstance.isValid())
        {
            _bgmInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            _bgmInstance.release();
        }
    }

    public void PlaySoundEffectByRef(EventReference path)
    {
        RuntimeManager.PlayOneShot(path);
    }
    
    public void ChangeBGMSettingVolume(float value)
    {
        BGMSettingRatio = value;
        _bgmBus.setVolume(BGMSettingRatio);
    }

    public void ChangeSFXSettingVolume(float value)
    {
        SFXSettingRatio = value;
        _soundEffectBus.setVolume(SFXSettingRatio);
    }

    
    [Button("播放3EQ效果")]
    public void PlayMusicEffect3Eq()
    {
        PlayMusicEffect(EMusicEffectType.EQ3);
    }

    [Button("关闭3EQ效果")]
    public void StopMusicEffect3Eq()
    {
        StopMusicEffect(EMusicEffectType.EQ3);
    }
    
    public void PlayMusicEffect(EMusicEffectType key)
    {
        if (_musicEffects.ContainsKey(key))
        {
            _musicEffects[key].Play();
        } else {
            Debug.LogWarning($"PlayMusicEffect fail: key[{key}] not found");
        }
    }
    
    public void StopMusicEffect(EMusicEffectType key)
    {
        if (_musicEffects.ContainsKey(key))
        {
            _musicEffects[key].Stop();
        } else {
            Debug.LogWarning($"StopMusicEffect fail: key[{key}] not found");
        }
    }
}

[System.Serializable]
public class MusicEffectInfo {
    public EMusicEffectType type;
    public EventReference effectRef;
}

public enum EMusicEffectType
{
    None,
    EQ3
}

public class MusicEffect
{
    private EventReference _eventRef;
    private EventInstance _effectInstance;

    public MusicEffect(EventReference eventRef)
    {
        _eventRef = eventRef;
    }
    
    public void Play()
    {
        try
        {
            _effectInstance = RuntimeManager.CreateInstance(_eventRef);
            _effectInstance.start();
        }
        catch (System.Exception e)
        {
            Debug.LogWarning($"CreateInstance Path[{_eventRef} Effect, fail[{e.Message}]");
        }
    }
    
    public void Stop()
    {
        if (_effectInstance.isValid())
        {
            _effectInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
    }
}