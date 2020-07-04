using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Message : MonoBehaviour
{
    public Text title, content;

    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Pop(string _title, string _content)
    {
        title.text = _title;
        content.text = _content;
        anim.SetTrigger("Pop");
    }
}
