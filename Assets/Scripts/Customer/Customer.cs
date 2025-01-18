using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour
{
    public LevelData levelData;
    public int levelNumber;
    public bool TestRun;

    private CustomerDialog _curCustomDialog;
    private CustomerAvatar _curCustomAvatar;

    // Start is called before the first frame update
    void Start()
    {
        if (levelData == null)
        {
            Debug.LogError("LevelData is not assigned.");
            return;
        }

        Messenger.AddListener<int>(MsgType.CreateCustomer, OnCreateCustomer);
        Messenger.AddListener(MsgType.BubbleSuccess, OnBubbleSuccess);
        Messenger.AddListener(MsgType.BubbleFail, OnBubbleFail);
    }

    private void Update() {
        if (TestRun) {
            OnCreateCustomer(levelNumber);
            TestRun = false;
        }
    }

#region handler
    private void OnCreateCustomer(int levelNumber) {
        CleanCustom();
        LevelData.Level level = GetLevel(levelNumber);
        if (level != null && level.prefab != null)
        {
            GameObject prefabInstance = Instantiate(level.prefab, transform);
            prefabInstance.transform.localPosition = Vector3.zero; // 可根据需要调整位置
            _curCustomDialog = prefabInstance.GetComponent<CustomerDialog>();
            _curCustomAvatar = prefabInstance.GetComponent<CustomerAvatar>();

            OnCustomIntro();
        }
        else
        {
            Debug.LogError("Level or Prefab is not found.");
            Messenger.Broadcast(MsgType.NoCustomer);
        }
    }

    private void OnBubbleSuccess() {
        _curCustomDialog.PlayDialog(EDialogType.Succ);
        // todo 播放成功动画
    }

    private void OnBubbleFail() {
        _curCustomDialog.PlayDialog(EDialogType.Fail);
        // todo 播放失败动画
    }
#endregion

#region  utils
    private void OnCustomIntro() {
        _curCustomAvatar.PlayAnimation("Intro"); // example
        _curCustomDialog.PlayDialog(EDialogType.Intro);
    }

    private LevelData.Level GetLevel(int levelNumber)
    {
        if (levelData == null || levelData.GetLevels() == null)
        {
            return null;
        }

        foreach (LevelData.Level level in levelData.GetLevels())
        {
            if (level.levelNumber == levelNumber)
            {
                return level;
            }
        }
        return null;
    }

    private void CleanCustom() {
        // todo 清理上次的客户
        if (_curCustomAvatar != null)
        {
            Destroy(_curCustomAvatar);
            _curCustomAvatar = null;
        }
        _curCustomDialog = null;
    }
#endregion
}
