using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoacherAlert : MonoBehaviour
{
    private Poacher poacher;

    public void SetPoacher(Poacher p)
    {
        poacher = p;
    }

    public void Alert()
    {
        poacher.Alerted();
    }
}
