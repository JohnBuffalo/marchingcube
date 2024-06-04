using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MDH.MarchingCube
{
    public class Triangle
    {
        public Vector3[] vertex = new Vector3[3];
        public Triangle(Vector3 p1, Vector3 p2, Vector3 p3)
        {
            vertex[0] = p1;
            vertex[1] = p2;
            vertex[2] = p3;
        }
    } 
}

