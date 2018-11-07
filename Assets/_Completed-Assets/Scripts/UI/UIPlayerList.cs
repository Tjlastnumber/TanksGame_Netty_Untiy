using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIPlayerList : MonoBehaviour
{
    public Text orginObject;
    public Transform parent;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddItem(string text, Color color)
    {
        var userName = Instantiate(orginObject, parent);
        userName.text = text;
        userName.color = color;
        userName.name = text;
    }

    public void RemoveItem(string text)
    {
        DestroyObject(GameObject.Find(text));
    }
}
