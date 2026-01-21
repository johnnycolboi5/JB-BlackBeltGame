using System.Collections;
using System.Collections.Generic;
using System.Xml.Xsl;
using TMPro;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public TextMeshProUGUI TutorialText;

    public string[] String;

    private int counter = 0;
    
    void Start()
    {
        ShowNextText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowNextText()
    {
        TutorialText.text = String[counter];
        counter++;
       
    }

    public void ClearText()
    {
        TutorialText.text = "";
    }
}
