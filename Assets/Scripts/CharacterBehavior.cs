using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBehavior : MonoBehaviour
{

    private Animator m_AnimationController;
    private Vector3 m_Position;
    private Quaternion m_Rotation;

    // Start is called before the first frame update
    void Start()
    {
        m_AnimationController = GetComponent<Animator>();
        m_Position = transform.position;
        m_Rotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TriggerShootAnimation()
    {
        m_AnimationController.SetTrigger("ShootAnimTrigger");
    }

    public void Reset()
    {
        transform.position = m_Position;
        transform.rotation = m_Rotation;
    }
}
