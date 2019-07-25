using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class landingSpot : MonoBehaviour
{
    public Transform rayOrigin;
    public Camera camera;

    public GameObject landObject;
    

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        if(Physics.Raycast(ray, out hit, 2))
        {
            landObject = hit.collider.gameObject;
        }
        else
        {
            landObject = null;
        }
    }
}
