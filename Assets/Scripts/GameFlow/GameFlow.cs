using UnityEngine;

public class GameFlow : MonoBehaviour {
    private int _curLevelNum = 0;

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
        if (_curLevelNum>6) {
        Messenger.Broadcast(MsgType.StartEnding);
        UnityEngine.SceneManagement.SceneManager.LoadScene("LastScene");
    }
    }

    private void OnNoCustomer() {
        Messenger.Broadcast(MsgType.StartEnding);
    }
}