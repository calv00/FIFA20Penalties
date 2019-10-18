using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalkeeperBehavior : MonoBehaviour
{

    [SerializeField]
    private float m_AnimationTime = 1f;

    private Animator m_AnimationController;
    private Vector3 m_Position;
    private Quaternion m_Rotation;
    private bool m_AnimationPlayed = false;
    private float m_AnimationAccTime = 0f;

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
        if (m_AnimationPlayed)
        {
            if (m_AnimationAccTime > m_AnimationTime)
            {
                m_AnimationController.SetInteger("BlockAnimation", 0);
                m_AnimationPlayed = false;
                m_AnimationAccTime = 0f;
            }
            else
            {
                m_AnimationAccTime += Time.deltaTime;
            }
        }
    }

    public void TriggerGoalkeeperAnimation()
    {
        int RandomAnim = Random.Range(1, 5);
        m_AnimationController.SetInteger("BlockAnimation", RandomAnim);
        m_AnimationPlayed = true;
    }

    public void Reset()
    {
        m_AnimationController.SetInteger("BlockAnimation", 0);
        m_AnimationPlayed = false;
        transform.position = m_Position;
        transform.rotation = m_Rotation;
    }
}
