using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Reporter : MonoBehaviour
{
    /// <summary>
    /// 播放action的速度（默认是每秒4个字）
    /// </summary>
    public float speed = 4;
    /// <summary>
    /// action的文本内容
    /// </summary>
    public string action;
    /// <summary>
    /// action的耗时
    /// </summary>
    public float duration;
    /// <summary>
    /// 当前完成的文本位置
    /// </summary>
    public int curPos = 0;
    [Min(0)]
    /// <summary>
    /// 当前完成的文本，在整个Action的位置
    /// </summary>
    public int curActionPos = 0;
    /// <summary>
    /// 每行指令文本的长度（默认是每行8个字）
    /// </summary>
    public int length = 8;

    public TextMeshProUGUI current;
    public List<TextMeshProUGUI> pasts;
    // Start is called before the first frame update
    void Start()
    {
        this.StartCoroutine(ActionCoroutine());
    }
    public void DoAction(string action)
    {
        this.action = action;
        this.curActionPos = 0;
    }
    private string _curText = "";
    IEnumerator ActionCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1 / speed);
            if (curPos == length || (curActionPos == action.Length && curActionPos > 0))
            {
                //开始新一行指令文本
                int i = 0;
                for (; i < pasts.Count - 1; i++)
                {
                    pasts[i].text = pasts[i + 1].text;
                }
                pasts[i].text = current.text;
                _curText = "";
                curPos = 0;
            }
            if (curActionPos < action.Length)
            {
                //在当前位置输入新的文字
                _curText += action[curActionPos];
            }
            else
            {
                _curText += "＞";
            }
            var curText = _curText;
            //补足至8个字符
            while (curText.Length < length)
            {
                curText += "·";
            }
            current.text = curText;
            //完成当前位置的输入
            curPos++;
            curActionPos++;
        }
    }

}
