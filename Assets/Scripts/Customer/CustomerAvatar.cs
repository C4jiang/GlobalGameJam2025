using UnityEngine;
public class CustomerAvatar : MonoBehaviour {
    private Animation _avatarAnimation;

    private void Awake() {
        _avatarAnimation = GetComponent<Animation>();
    }

    public void PlayAnimation(string animationName) {
        // todo
    }
}