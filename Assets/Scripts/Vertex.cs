using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MDH.MarchingCube
{
    public class Vertex
    {
        public Vector3 worldPosition = Vector3.zero;
        public bool active = false;

        public Vertex(Vector3 wp)
        {
            worldPosition = wp;
            active = false;
        }

        public Vertex(float x, float y, float z)
        {
            worldPosition.x = x;
            worldPosition.y = y;
            worldPosition.z = z;
            active = false;
        }
    }
}

