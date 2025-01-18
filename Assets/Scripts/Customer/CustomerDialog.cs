using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using Febucci.UI;
public class CustomerDialog : MonoBehaviour {
    private EDialogType _curDialogType;
    public TextMeshProUGUI dialogText;
    public bool isPlaying = false;
    private void Awake() {
        _curDialogType = EDialogType.None;
    }
    [Button("PlayDialog")]
    public void PlayDialog(EDialogType dialogType) {
        _curDialogType = dialogType;
        // TODO: 播放对话，这里用协程可能会好点
        StartCoroutine(PlayDialogCoroutine());

    }

    IEnumerator PlayDialogCoroutine() {
        dialogText.transform.GetComponent<TextAnimatorPlayer>().ShowText("sdffsf dsfd");//
        
        isPlaying = true;
        while(true){
            yield return null;
            if(!isPlaying){
                break;
            }
        }
        while(true){
            yield return null;
            if(Input.GetMouseButtonDown(0)){
                break;
            }
        }
        OnDialogComplete();
    }

    public void DialogShowed(){
        isPlaying = false;

    }


    private void OnDialogComplete() {
        Messenger.Broadcast(MsgType.DialogComplete, _curDialogType);
        _curDialogType = EDialogType.None;
    }
}

