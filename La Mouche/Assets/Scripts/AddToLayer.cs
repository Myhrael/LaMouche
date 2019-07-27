using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddToLayer : MonoBehaviour
{

    public int layer = 9;

    // Start is called before the first frame update
    void Start()
    {
        List<GameObject> childs = new List<GameObject>();
        getChilds(transform, childs);

        foreach (GameObject obj in childs)
        {
            obj.layer = layer;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private List<GameObject> getChilds(Transform parent, List<GameObject> childs)
    {
        foreach (Transform child in parent)
        {
            if (child.childCount == 0) childs.Add(child.gameObject);
            else getChilds(child, childs);
        }

        return childs;
    }

    private void addToLayerRec(Transform root)
    {
        root.gameObject.layer = layer;
        foreach(Transform child in transform)
        {
            if(child != root) addToLayerRec(child);
        }
    }
}
