using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circler : NPC
{
    float i = 0;


    public override void Start()
    {
        _state = States.Idle;

        // test
        //CirclerBuilderBehavior test = new CirclerBuilderBehavior(this);
        _behavior = new CirclerBuilderBehavior(this);
    }

    public override void Update()
    {
        _behavior.CustomUpdate();
    }

    
}
