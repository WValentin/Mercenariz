using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NPC : Creature
{
    public City _city;
    
    public void JoinCity(City city)
    {
        _city = city;
    }
}