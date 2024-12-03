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
        rho += -(Plotting.Instance.function == null ? 750: 100) * Input.mouseScrollDelta.y * Time.deltaTime;
    }


    private void FixedUpdate()
    {

        if (Input.GetMouseButton(0))
        {
            float f = Mathf.PI * Mathf.Sqrt(rho / 120) / (Plotting.Instance.function==null ? 10: 30);
            phi += Input.GetAxis("Mouse Y") * f;
            theta -= Input.GetAxis("Mouse X") * f;
        }
        else if(rotate)
        {
            theta += 0.01f;
        }

        rho += -20 * Input.mouseScrollDelta.y;
        rho = Mathf.Clamp(rho, 60f, 180f);

        phi = Mathf.Clamp(phi, 0.01f, Mathf.PI - 0.01f);

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
