using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshCell : Object
{
    public int xIndex;
    public int zIndex;
    public Mesh mesh;
    public MeshCell(int xIndep, int zIndexp, Mesh meshp)
    {
        xIndex = xIndep;
        zIndex = zIndexp;
        mesh = meshp;
        mesh.name = "tm_" + xIndex + "" + zIndex;
    }
}