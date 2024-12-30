using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class testGUI : MonoBehaviour {
    public TextScrambleEffect effect;
    public InputField inputField;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetEffectInputText()
    {
        effect.SetText(inputField.text);
    }

    //private void OnGUI()
    //{
    //    if (GUI.Button(new Rect(10,10,130,35),"Next Text"))
    //    {
    //        effect.SetNextText();
    //    }
    //}
}
