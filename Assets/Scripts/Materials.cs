using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Materials
{
    public MaterialData wood;
    public MaterialData stone;

    private void Start()
    {
        // TO DO
        // Set proper values for these ressources
        wood.name = "Wood";
        wood.weightPerUnit = 1;
        wood.value = 1;
        //wood.texture = 1;
    }
}

public struct MaterialData
{
    public string name;
    public float weightPerUnit;
    public float value;
    public Texture2D texture;
}
