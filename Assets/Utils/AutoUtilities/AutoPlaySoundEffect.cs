using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AutoPlaySoundEffect : MonoBehaviour {
    public string soundName;
    void OnEnable() {
        Messenger.Broadcast(MsgType.PlaySE, soundName);
    }
}