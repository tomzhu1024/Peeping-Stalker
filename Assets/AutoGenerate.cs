using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AutoGenerater : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI textDisplay;
    public string[] sentence;
    private int index;
    public float waittime;
    public GameObject continueButton;
    public GameObject backgroundPic;

    IEnumerator Type()
    {
        foreach (char letter in sentence[index].ToCharArray())
        {
            textDisplay.text += letter;
            yield return new WaitForSeconds(waittime);
        }
    }

    public void NextSentence()
    {
        continueButton.SetActive(false);
        if (index < sentence.Length - 1)
        {
            index++;
            textDisplay.text = "";
            StartCoroutine(Type());
        }
        else
        {
            textDisplay.text = "";
            continueButton.SetActive(false);
            backgroundPic.SetActive(false);
            // clear the task
        }
    }
    void Start()
    {
        // uncomment to print one sentence
        StartCoroutine(Type());
    }

    // Update is called once per frame
    void Update()
    {
        if (textDisplay.text == sentence[index])
        {
            continueButton.SetActive(true);
        }
        //NextSentence();
    }
}
