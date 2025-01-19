using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using Febucci.UI;
using I2.Loc;
using UnityEngine.UIElements.Experimental;

public class StartIntro : MonoBehaviour {
    public TextMeshProUGUI dialogText;
    public bool isPlaying = false;
    public List<string> dialogList;
    CanvasGroup canvasGroup;
    
    private void Awake() {
        Messenger.AddListener(MsgType.StartIntro, StartIntroPlay);
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void StartIntroPlay() {
        StartCoroutine(PlayDialogCoroutine());
    }

    IEnumerator PlayDialogCoroutine() {
        canvasGroup.DOFade(1, 1.4f).SetEase(Ease.OutCubic);
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        yield return new WaitForSeconds(0.7f);
        for(var i = 0; i < dialogList.Count; i++){
            string dialog = LocalizationManager.GetTranslation(dialogList[i]);
            dialogText.transform.GetComponent<TextAnimatorPlayer>().ShowText(dialog);
            isPlaying = true;
            while(true){
                yield return null;
                if(!isPlaying){
                    break;
                }
                if(Input.GetMouseButtonDown(0))
                {
                    DialogShowed();
                    dialogText.transform.GetComponent<TextAnimatorPlayer>().StopShowingText();
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
        canvasGroup.DOFade(0, 1.4f).SetEase(Ease.OutCubic);
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        
        Messenger.Broadcast(MsgType.IntroEnd);
    }
}

