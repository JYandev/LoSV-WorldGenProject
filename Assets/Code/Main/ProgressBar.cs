using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour {
    /* Class for the ProgressBar UI
     */

    [SerializeField]
    private Image[] myBars;
    [SerializeField]
    private Text myText;

    public void setProgress (float newPercentage, int step) {
        if (step <= myBars.Length) {
            myBars[step].fillAmount = newPercentage;
        }
    }
    public void resetProgress () {
        foreach (Image bar in myBars) {
            bar.fillAmount = 0.0f;
        }
    }
    public void setText (string newText) {
        if (myText) {
            myText.text = newText;
        }
    }
}
