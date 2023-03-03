using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : Unit
{
    protected override void Start()
    {
        base.Start();
        splash = transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
        unitName = "SOLDIER";
    }
}
