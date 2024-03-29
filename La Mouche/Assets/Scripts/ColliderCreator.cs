﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderCreator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        List<GameObject> childs = new List<GameObject>();
        getChilds(transform, childs);

        foreach (GameObject obj in childs)
        {
            MeshCollider collider = obj.AddComponent<MeshCollider>() as MeshCollider;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private List<GameObject> getChilds(Transform parent, List<GameObject> childs)
    {
        foreach(Transform child in parent)
        {
            if(child.childCount == 0 && child.GetComponent<Collider>() == null) childs.Add(child.gameObject);
            else getChilds(child, childs);
        }

        return childs;
    }
}
