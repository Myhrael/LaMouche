using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardEvent : MonoBehaviour
{
    public void endAttack()
    {
        GetComponentInParent<AstroController>().endAttack();
    }
}
