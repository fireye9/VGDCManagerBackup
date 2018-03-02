using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GStatePopupText : MonoBehaviour {
    public Animator animator;
    public Text text;
    private void Start()
    {
        text = animator.GetComponentInChildren<Text>();
        AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);
        Destroy(this.gameObject, clipInfo[0].clip.length);
    }

    public void SetText(string s)
    {
        text.text = s;
    }
}
