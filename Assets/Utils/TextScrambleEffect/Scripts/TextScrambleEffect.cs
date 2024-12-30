using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// [RequireComponent(typeof(Text))]
public class TextScrambleEffect : MonoBehaviour {
    public string randomText = "章节六的镇魂曲遥远深空间银河猎户座C射线雨中眼泪消失唐怀瑟之门安";

    public Color textNormalColor = Color.white;
    public Color textRandomColor = Color.gray;

    public float timeInterval = 0.01f;

    public bool isAutoPlay = false;
    public float autoPlayInterval = 2f;
    float autoPlaytick = 0;

    bool isAnim = false;

    int currentIndex = 0;
    Text textControl;

    string lastInputText = "";
    string oldText = "";
    int textLength = 0;
    List<TextScramble> textScrambles = new List<TextScramble>();
    int textFrame = 0;
    float tick = 0;


    // Use this for initialization
    void Start () {
        textControl = GetComponent<Text>();

        textControl.color = textNormalColor;
	}

    public void SetText(string newText)
    {
        if (textControl == null)
            return;

        if (isAnim) {
            textControl.text = lastInputText;
        }

        // randomText = newText + randomText;
        

        textScrambles.Clear();

        oldText = textControl.text;
        lastInputText = newText;
        textLength = Mathf.Max(oldText.Length, newText.Length);
        for(int i=0;i<textLength;i++)
        {
            TextScramble ts = new TextScramble();
            if (i< oldText.Length)
                ts.from = oldText.Substring(i, 1);
            else
                ts.from = "";

            if (i<newText.Length)
                ts.to = newText.Substring(i, 1);
            else
                ts.to = "";

            ts.startTime = Mathf.Floor(Random.value * 40f);
            ts.endTime = ts.startTime + Mathf.Floor(Random.value * 40f);
            ts.randomText = randomChar(); 

            textScrambles.Add(ts);
        }

        textFrame = 0;

        isAnim = true;
    }

    // //字符串去除重复
    // private static string StringEliminateDuplicate(string str)
    // {
    //     var strArray = str.Distinct().ToArray(); //字符去重
    //     return string.Join(string.Empty, strArray); //字符成串
    // }
	
	// Update is called once per frame
	void Update ()
    {
        int complete = 0;
        string outputText = "";

        if (isAnim)
        {
            tick += Time.deltaTime;
            if (tick < timeInterval)
                return;

            tick = 0;

            textFrame += 1;

            for (int i = 0; i < textScrambles.Count; i++)
            {
                TextScramble ts = textScrambles[i];
                if (textFrame >= ts.endTime)
                {
                    //完成
                    complete += 1;
                    outputText += ts.to;
                }
                else if (textFrame >= ts.startTime)
                {
                    //随机文字
                    if (Random.value < 0.28f)
                        ts.randomText = randomChar();
                    
                    outputText += "<color=#" + ColorUtility.ToHtmlStringRGBA(textRandomColor) + ">" + ts.randomText + "</color>";
                }
                else
                {
                    outputText += ts.from;
                }
            }

            if (complete == textScrambles.Count)
            {
                //完成！
                isAnim = false;
            }

            textControl.text = outputText;
        }
	}

    string randomChar()
    {
        return randomText.Substring((int)(Mathf.Floor( Random.value * randomText.Length)),1);
    }

}
