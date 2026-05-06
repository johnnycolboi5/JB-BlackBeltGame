using TMPro;
using UnityEngine;
public class Create3DText : MonoBehaviour
{
    void Start()
    {
        GameObject textObj = new GameObject("3DText");
        var tmp = textObj.AddComponent<TextMeshPro>();
        tmp.text = "Hello World!";
        tmp.fontSize = 5;
        tmp.alignment = TextAlignmentOptions.Center;
        textObj.transform.position = new Vector3(0, 1, 0);
    }
}