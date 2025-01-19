using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using Febucci.UI;
using I2.Loc;

public class CustomerDialog : MonoBehaviour {
    private EDialogType _curDialogType;
    public TextMeshProUGUI dialogText;
    public bool isPlaying = false;

    private CanvasGroup _canvasGroup;
    
    private void Awake() {
        _curDialogType = EDialogType.None;
        _canvasGroup = GetComponent<CanvasGroup>();
    }
    
    // [Button("PlayDialog")]
    // public void PlayDialog(EDialogType dialogType) {
    //     _curDialogType = dialogType;
    //     StartCoroutine(PlayDialogCoroutine());
    // }

    public IEnumerator PlayDialog(List<string> dialogs, EDialogType dialogType) {
        _curDialogType = dialogType;
        dialogText.text = "";
        _canvasGroup.DOFade(1, 1.6f).SetEase(Ease.OutCubic);
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
        yield return new WaitForSeconds(1.5f);
        for(var i = 0; i < dialogs.Count; i++){
            var TRANS = LocalizationManager.GetTranslation(dialogs[i]);
            dialogText.transform.GetComponent<TextAnimatorPlayer>().ShowText(TRANS);
        
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
        }

        OnDialogComplete();
    }

    public void DialogShowed(){
        isPlaying = false;
    }


    private void OnDialogComplete()
    {
        _canvasGroup.DOFade(0, 1.4f).SetEase(Ease.OutCubic);
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
        Messenger.Broadcast(MsgType.DialogComplete, _curDialogType);
        _curDialogType = EDialogType.None;
    }
}

