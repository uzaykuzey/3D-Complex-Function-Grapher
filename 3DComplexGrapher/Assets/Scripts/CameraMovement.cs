using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Camera main;
    private float rho;
    private float theta;
    private float phi;
    private Vector3 lastMouseWorldPosition;
    private Vector3 mouseWorldVelocity;
    private bool rotate;
    // Start is called before the first frame update
    void Start()
    {
        main = Camera.main;
        rho = main.transform.position.magnitude;
        phi = Mathf.PI / 2;
        theta = -Mathf.PI/2;
        lastMouseWorldPosition = Input.mousePosition;
        rotate = false;
    }

    // Update is called once per frame
    void Update()
    {

        
    }


    private void FixedUpdate()
    {
        Vector3 currentMouseWorldPosition = Input.mousePosition;
        mouseWorldVelocity = (currentMouseWorldPosition - lastMouseWorldPosition);
        lastMouseWorldPosition = currentMouseWorldPosition;

        if (Input.GetMouseButton(0))
        {
            phi += mouseWorldVelocity.y * Mathf.PI / 180;
            theta -= mouseWorldVelocity.x * Mathf.PI / 180;
        }
        else if(rotate)
        {
            theta += 0.01f;
        }

        phi = Mathf.Min(Mathf.PI-0.01f, phi);
        phi = Mathf.Max(0.01f, phi);
        if(theta<0)
        {
            theta += 2*Mathf.PI;
        }
        if(theta >= 2 * Mathf.PI)
        {
            theta-= 2 * Mathf.PI;
        }
        main.transform.position = new Vector3(rho * Mathf.Sin(phi) * Mathf.Cos(theta),
                                              rho * Mathf.Cos(phi), 
                                              rho * Mathf.Sin(phi) * Mathf.Sin(theta)
                                              );
        main.transform.LookAt(Vector3.zero);


    }
}
