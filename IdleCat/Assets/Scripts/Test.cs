using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : Intractable
{
    public override void Interact()
    {
        Destroy(gameObject);
    }
}
