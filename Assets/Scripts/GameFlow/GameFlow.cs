using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;

public class GameFlow : MonoBehaviour {
    public int _curLevelNum = 0;
    public int CurLevelNum => _curLevelNum;

    private void Awake() {
        _curLevelNum = 0;
        Messenger.AddListener(MsgType.IntroEnd, OnIntroEnd);
        Messenger.AddListener<EDialogType>(MsgType.DialogComplete, OnDialogComplete);
        Messenger.AddListener(MsgType.NoCustomer, OnNoCustomer);
    }

    private void Start() {
        Messenger.Broadcast(MsgType.StartIntro);
    }

    private void OnIntroEnd() {
        Messenger.Broadcast(MsgType.CreateCustomer, _curLevelNum);
    }

    private void OnDialogComplete(EDialogType dialogType) {
        if (EDialogType.Succ == dialogType) {
            _curLevelNum++;
            Messenger.Broadcast(MsgType.CreateCustomer, _curLevelNum);
        }
        
        if (EDialogType.Intro == dialogType) {
            Messenger.Broadcast(MsgType.BlowBubble, 1);
        }
    }
    [Button("NoCustomer")]
    private void OnNoCustomer() {
        Messenger.Broadcast(MsgType.StartEnding);
        StopAllCoroutines();
        SceneManager.LoadScene("LastScene", LoadSceneMode.Single);
    }
}