using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    public int level = 0;

    private bool destructed = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float destroy()
    {
        destructed = true;
        Debug.Log("changing color");
        //GetComponent<Shader>().    SetColor(0, new Color(0.3f, 0.3f, 0.3f));

        return level / 10f;
    }
}
