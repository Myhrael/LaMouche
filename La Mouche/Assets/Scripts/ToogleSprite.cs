using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToogleSprite : MonoBehaviour
{
    public bool enableState = true;

    private List<SpriteRenderer> renderers = new List<SpriteRenderer>();

    // Start is called before the first frame update
    void Start()
    {
        getRenderers(transform);
        toogle();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void toogle()
    {
        enableState = !enableState;
        foreach(SpriteRenderer renderer in renderers)
        {
            renderer.enabled = enableState;
        }
    }

    private void getRenderers(Transform root)
    {
        foreach (Transform child in root)
        {
            SpriteRenderer renderer = child.GetComponent<SpriteRenderer>();
            if (child.childCount == 0 && renderer != null) renderers.Add(renderer);
            else getRenderers(child);
        }
    }
}
