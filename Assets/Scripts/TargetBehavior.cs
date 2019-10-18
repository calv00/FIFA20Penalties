using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetBehavior : MonoBehaviour
{
    [SerializeField]
    private RectTransform m_AccPowerRT;
    [SerializeField]
    private float m_PowerMaxTime = 2f;
    [SerializeField]
    private float m_PowerImageFactor = 4f;
    [SerializeField]
    private float m_RunTime = 3f;
    [SerializeField]
    private float m_PreciseShootTimeWindow = 0.5f;
    [SerializeField]
    private float m_NormalShootTimeWindow = 1f;
    [SerializeField]
    private Color m_PreciseGoodColor = new Color(0f, 1f, 0f, 1f);
    [SerializeField]
    private Color m_PreciseNormalColor = new Color(1f, 1f, 0f, 1f);
    [SerializeField]
    private Color m_PreciseBadColor = new Color(1f, 0f, 0f, 1f);
    [SerializeField]
    private BallBehavior m_Ball;
    [SerializeField]
    private CharacterBehavior m_Character;
    [SerializeField]
    private GoalkeeperBehavior m_Goalkeeper;
    [SerializeField]
    private GoalTextBehavior m_GoalText;
    [SerializeField]
    private float m_ShotMaxPower = 60f;

    private float m_ScreenX = 0f;
    private float m_ScreenY = 0f;
    private RectTransform m_RectTransform;
    private bool m_ShotButtonPressed = false;
    private bool m_OnRunning = false;
    private bool m_ShotButtonPressedAgain = false;
    private float m_PowerAccTime = 0f;
    private float m_RunAccTime = 0f;
    private Vector3 m_PowerInitialScale;
    private Image m_Image;
    private float m_ScreenTopLimit;
    enum ShotType { Good, Normal, Bad};
    private ShotType m_ShotType = ShotType.Normal;
    private float m_ShotPower;
    private float m_PowerScaleValue;
    private bool m_ShootEnded = false;

    // Start is called before the first frame update
    void Start()
    {
        m_RectTransform = GetComponent<RectTransform>();
        m_PowerInitialScale = m_AccPowerRT.localScale;
        m_Image = GetComponent<Image>();
        m_PowerScaleValue = m_AccPowerRT.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_ShootEnded)
        {
            if (m_OnRunning)
            {
                m_RunAccTime += Time.deltaTime;
                if (m_RunAccTime > m_RunTime)
                {
                    m_OnRunning = false;
                    m_RunAccTime = 0f;
                    if (!m_ShotButtonPressedAgain)
                    {
                        m_ShotType = ShotType.Normal;
                        m_PowerScaleValue = m_AccPowerRT.localScale.x;
                    }
                    m_ShotButtonPressedAgain = false;
                    m_AccPowerRT.localScale = m_PowerInitialScale;
                    m_Image.color = new Color(1f, 1f, 1f, 1f);
                    m_Ball.Shoot(GetRay(), m_ShotPower);
                    if (m_Goalkeeper!= null)
                        m_Goalkeeper.TriggerGoalkeeperAnimation();
                    m_ShootEnded = true;
                }
                else
                {
                    if (!m_ShotButtonPressedAgain && Input.GetKeyDown("joystick button 1"))
                    {
                        if ((m_RunAccTime + m_PreciseShootTimeWindow) >= m_RunTime)
                        {
                            m_Image.color = m_PreciseGoodColor;
                            m_ShotType = ShotType.Good;
                        }
                        else if ((m_RunAccTime + m_NormalShootTimeWindow) >= m_RunTime)
                        {
                            m_Image.color = m_PreciseNormalColor;
                            m_ShotType = ShotType.Normal;
                        }
                        else
                        {
                            m_Image.color = m_PreciseBadColor;
                            m_ShotType = ShotType.Bad;
                        }
                        m_ShotButtonPressedAgain = true;
                        m_PowerScaleValue = m_AccPowerRT.localScale.x;
                        m_AccPowerRT.localScale = m_PowerInitialScale; //ToDo: Animate the scale resetting
                    }

                }
            }
            else
            {
                if (Input.GetKeyDown("joystick button 1"))
                {
                    m_ShotButtonPressed = true;
                }
                if (Input.GetKeyUp("joystick button 1"))
                {
                    m_ShotButtonPressed = false;
                    m_OnRunning = true;
                    m_ShotPower = Mathf.Lerp(0f, m_ShotMaxPower, (m_PowerAccTime / m_PowerMaxTime));
                    m_PowerAccTime = 0f;
                    m_Character.TriggerShootAnimation();
                }
                if (m_ShotButtonPressed)
                {
                    m_PowerAccTime += Time.deltaTime;
                    if (m_PowerAccTime > m_PowerMaxTime)
                    {
                        m_PowerAccTime = m_PowerMaxTime;
                    }
                    float PowerScaleFactor = m_PowerAccTime * m_PowerImageFactor;
                    if (m_PowerAccTime * m_PowerImageFactor > m_PowerInitialScale.x)
                    {
                        m_AccPowerRT.localScale = new Vector3(m_PowerAccTime * m_PowerImageFactor, m_PowerAccTime * m_PowerImageFactor);
                    }
                }
            }
            MoveTarget();
        }
        else
        {
            if (Input.GetKeyDown("joystick button 0"))
            {
                m_Ball.Reset();
                m_Character.Reset();
                if (m_Goalkeeper != null)
                    m_Goalkeeper.Reset();
                m_GoalText.Reset();
                m_ShootEnded = false;
            }
        }
    }

    private void MoveTarget()
    {
        float AxisHorizontal = Input.GetAxis("Horizontal");
        m_ScreenX =  AxisHorizontal * (Screen.width / 2);
        float AxisVertical = Input.GetAxis("Vertical");
        m_ScreenY = AxisVertical * (Screen.height / 2);

        m_ScreenTopLimit = ((m_RectTransform.pivot.y - 0.5f) * m_RectTransform.rect.height);
        if (m_ScreenY > ((Screen.height / 2) + m_ScreenTopLimit))
        {
            m_ScreenY = (Screen.height / 2) + m_ScreenTopLimit;
        }
        else if (m_ScreenY < m_ScreenTopLimit)
        {
            m_ScreenY = m_ScreenTopLimit;
        }

        Vector3 RectPosition = m_RectTransform.anchoredPosition;
        RectPosition.x = m_ScreenX;
        RectPosition.y = m_ScreenY;
        m_RectTransform.anchoredPosition = RectPosition;
    }

    private Vector3 GetRay()
    {
        Vector3 ReturnPoint = Vector3.zero;
        float RandomRadius;
        if (m_ShotType == ShotType.Good)
        {
            float PosX = (m_RectTransform.anchoredPosition.x) + (Screen.width / 2);
            float PosY = (m_RectTransform.anchoredPosition.y) + (Screen.height / 2) + (m_ScreenTopLimit * -1f);
            Vector3 TargetPos = new Vector3(PosX, PosY, 0);
            Ray PreciseRay = Camera.main.ScreenPointToRay(TargetPos);
            Debug.DrawRay(PreciseRay.origin, PreciseRay.direction * 25, Color.yellow);
            RaycastHit PreciseHit;
            if (Physics.Raycast(PreciseRay.origin, PreciseRay.direction, out PreciseHit, 25))
            {
                ReturnPoint = PreciseHit.point;
            }
            return ReturnPoint;
        }
        else if (m_ShotType == ShotType.Normal)
        {
            RandomRadius = m_PowerScaleValue * (m_RectTransform.rect.width / 2);
        }
        else
        {
            RandomRadius = m_PowerScaleValue * (m_RectTransform.rect.width / 2) * 2;
        }
        
        float RandomX = m_RectTransform.anchoredPosition.x + (Random.insideUnitCircle * RandomRadius).x;
        float RandomY = m_RectTransform.anchoredPosition.y + (Random.insideUnitCircle * RandomRadius).y;
        float RandomposX = RandomX + (Screen.width / 2);
        float RandomposY = RandomY + (Screen.height / 2) + (m_ScreenTopLimit * -1f);
        
        Vector3 RandomTargetPos = new Vector3(RandomposX, RandomposY, 0);
        Ray RandomRay = Camera.main.ScreenPointToRay(RandomTargetPos);
        Debug.DrawRay(RandomRay.origin, RandomRay.direction * 25, Color.yellow);
        RaycastHit RandomHit;
        if (Physics.Raycast(RandomRay.origin, RandomRay.direction, out RandomHit, 25))
        {
            ReturnPoint = RandomHit.point;
        }
        return ReturnPoint;
    }
}
