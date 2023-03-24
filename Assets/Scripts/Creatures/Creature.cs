using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Creature : MonoBehaviour
{
    public States _state;

    [SerializeField]
    public CreatureBehavior _behavior;


    public abstract void Start();

    public abstract void Update();

}