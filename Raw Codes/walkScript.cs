using UnityEngine;

public class walkScript : MonoBehaviour
{
    public float lRSpeed = 15f , fBSpeed = 7.5f , rotX, sens = 2f;
    public GameObject cam;
    private bool inAir;

    void Update ()
    {
        float x = lRSpeed * Input.GetAxis("Horizontal") * Time.deltaTime;
        float z = fBSpeed * Input.GetAxis("Vertical") * Time.deltaTime;
        transform.Translate(x, 0, z);

        rotX += Input.GetAxis("Mouse Y") * sens;
        rotX = Mathf.Clamp(rotX, -90, 90);
        cam.transform.localEulerAngles = new Vector3(-rotX, 0, 0);
        transform.Rotate(0, Input.GetAxis("Mouse X") * sens, 0);

        if (Input.GetButtonDown("Jump") && inAir==false)
        {
            inAir = true;
			transform.Translate(0,2,0);
        }

        if(inAir==true)
        {
			if (Physics.Raycast(transform.position, -transform.up, 0.1f))
            {
                inAir = false;
            }
        }
    }
}