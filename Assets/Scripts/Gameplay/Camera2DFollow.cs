using System.Collections;
using UnityEngine;

// The game object follows another game object with or without damping
public class Camera2DFollow : MonoBehaviour
{
    public Transform target;
    public float damping = 1;
    public float lookAheadFactor = 3;
    public float lookAheadReturnSpeed = 0.5f;
    public float lookAheadMoveThreshold = 0.1f;

    private float m_OffsetZ;
    private Vector3 m_LastTargetPosition;
    private Vector3 m_CurrentVelocity;
    private Vector3 m_LookAheadPos;

    private bool yBoundTop = false;
    private bool yBoundBottom = false;
    private bool xBoundRight = false;
    private bool xBoundLeft = false;
    private Vector3 boundPosition;

    // Use this for initialization
    private void Start()
    {
        m_LastTargetPosition = target.position;
        m_OffsetZ = (transform.position - target.position).z;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!(target == null))
        {
            // only update lookahead pos if accelerating or changed direction
            float xMoveDelta = (target.position - m_LastTargetPosition).x;

            bool updateLookAheadTarget = Mathf.Abs(xMoveDelta) > lookAheadMoveThreshold;

            if (updateLookAheadTarget)
            {
                m_LookAheadPos = lookAheadFactor * Vector3.right * Mathf.Sign(xMoveDelta);
            }
            else
            {
                m_LookAheadPos = Vector3.MoveTowards(m_LookAheadPos, Vector3.zero, Time.deltaTime * lookAheadReturnSpeed);
            }

            Vector3 aheadTargetPos = target.position + m_LookAheadPos + Vector3.forward * m_OffsetZ;
            Vector3 newPos = Vector3.SmoothDamp(transform.position, aheadTargetPos, ref m_CurrentVelocity, damping);

            #region "Camera Bounds"
            // Checks for bounds (camera)
            if (xBoundRight == true && boundPosition.x < newPos.x)
            {
                newPos.x = boundPosition.x;
            }
            else if (xBoundLeft == true && boundPosition.x > newPos.x)
            {
                newPos.x = boundPosition.x;
            }
            if (yBoundTop == true && boundPosition.y < newPos.y)
            {
                newPos.y = boundPosition.y;
            }
            else if (yBoundBottom == true && boundPosition.y > newPos.y)
            {
                newPos.y = boundPosition.y;
            }
            #endregion

            // Assigns the position if within bounds
            transform.position = newPos;

            m_LastTargetPosition = target.position;
        }
    }

    #region "Camera Bounds Check"
    // On trigger
    void OnTriggerEnter2D(Collider2D hit)
    {
        // Checks for bounds
        if (hit.transform.name == "RightBound" && hit.transform.position.x > transform.position.x)
        {
            xBoundRight = true;
            boundPosition.x = transform.position.x;
        }
        if (hit.transform.name == "LeftBound" && hit.transform.position.x < transform.position.x)
        {
            xBoundLeft = true;
            boundPosition.x = transform.position.x;
        }
        if (hit.transform.name == "TopBound" && hit.transform.position.y > transform.position.y)
        {
            yBoundTop = true;
            boundPosition.y = transform.position.y;
        }
        if (hit.transform.name == "BottomBound" && hit.transform.position.y < transform.position.y)
        {
            yBoundBottom = true;
            boundPosition.y = transform.position.y;
        }
    }

    void OnTriggerExit2D(Collider2D hit)
    {
        // Checks for bounds
        if (hit.transform.position.y > transform.position.y)
        {
            yBoundTop = false;
        }
        if (hit.transform.position.y < transform.position.y)
        {
            yBoundBottom = false;
        }
        if (hit.transform.position.x > transform.position.x)
        {
            xBoundRight = false;
        }
        if (hit.transform.position.x < transform.position.x)
        {
            xBoundLeft = false;
        }
    }
    #endregion
}
