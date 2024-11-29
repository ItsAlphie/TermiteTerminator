using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser: MonoBehaviour
{
   //[SerializeField] private float defDistanceRay = 100;
    public Transform laserFirePoint;
    public LineRenderer m_lineRenderer;
    Transform m_transform;
    public Vector2 targetPosition;
     // Start is called before the first frame update
    void Start()
    {
          
    }

    // Update is called once per frame
    void Update()
    {

        
    }
    
    private void Awake(){
        m_transform = GetComponent<Transform>();
    }
    
    public void Draw2DRay(Vector2 startPos, Vector2 endPos)
    {
        m_lineRenderer.SetPosition(0, startPos);
        m_lineRenderer.SetPosition(1, endPos);
    }   
}
