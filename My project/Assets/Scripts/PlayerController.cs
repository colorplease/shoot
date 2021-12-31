using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]float yRot;
    [SerializeField]float yPos;
    [SerializeField]float turnSpeed;
    
    [SerializeField]Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        LeftArrow();
        TurnSpeed();
        RightArrow();
        UpArrow();
        DownArrow();
        Interact();
    }

    void Interact()
    {
      if (Input.GetMouseButton(0))
      {
          Ray ray = cam.ScreenPointToRay(Input.mousePosition);
          if (Physics.Raycast(ray, out RaycastHit hitInfo))
          {
              hitInfo.collider.transform.position = hitInfo.point;

          }
      }  
    }

    void LeftArrow()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (transform.rotation.y <= 360f)
            {
                yRot += Time.deltaTime * turnSpeed;
            }
            
             transform.rotation = Quaternion.Euler(transform.rotation.x, yRot, transform.rotation.z);
        }
    }

    void TurnSpeed()
    {
        if (Input.mouseScrollDelta.y > 0 && turnSpeed <= 150)
        {
            turnSpeed += Input.mouseScrollDelta.y * 10;
        }
        else if (turnSpeed > 0)
        {
            turnSpeed += Input.mouseScrollDelta.y * 10;
        }
    }

    void RightArrow()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            if (transform.rotation.y <= 360f)
            {
                yRot -= Time.deltaTime * turnSpeed;
            }
            
            transform.rotation = Quaternion.Euler(transform.rotation.x, yRot, transform.rotation.z);
        }
    }

    void UpArrow()
    {
       if (Input.GetKey(KeyCode.UpArrow))
        {
            if (transform.position.y <= 17.61f)
            {
                yPos += Time.deltaTime * turnSpeed * 0.1f;
                transform.position = new Vector3 (transform.position.x, yPos, transform.position.z);
            }
            
        } 
    }

    void DownArrow()
    {
       if (Input.GetKey(KeyCode.DownArrow))
        {
            if (transform.position.y >= -1f)
            {
                yPos -= Time.deltaTime * turnSpeed * 0.1f;
                transform.position = new Vector3 (transform.position.x, yPos, transform.position.z);
            }
            
        } 
    }
}
