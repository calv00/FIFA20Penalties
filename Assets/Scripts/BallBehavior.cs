using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehavior : MonoBehaviour
{

    private Rigidbody m_Rigidbody;
    private Vector3 m_BallInitialPosition;

    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_BallInitialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shoot(Vector3 shotTarget, float shotPower)
    {
        Vector3 ShotDirection = (shotTarget - transform.position).normalized;
        m_Rigidbody.AddForce(ShotDirection * shotPower, ForceMode.Impulse);
        Debug.DrawRay(transform.position, ShotDirection * 25, Color.red);
    }

    public void Reset()
    {
        m_Rigidbody.velocity = Vector3.zero;
        m_Rigidbody.angularVelocity = Vector3.zero;
        transform.position = m_BallInitialPosition;
    }
}
