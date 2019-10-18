using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoalTextBehavior : MonoBehaviour
{

    private Animation m_Animation;
    private Text m_Text;

    // Start is called before the first frame update
    void Start()
    {
        m_Animation = GetComponent<Animation>();
        m_Text = GetComponent<Text>();
        m_Text.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TriggerAnimation()
    {
        m_Text.enabled = true;
        m_Animation.Play();
    }

    public void Reset()
    {
        m_Text.enabled = false;
    }
}
