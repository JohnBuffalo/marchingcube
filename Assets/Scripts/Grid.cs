using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MDH.MarchingCube
{
    /// <summary>
    /// 划分空间的体素单位
    /// </summary>
    public class Grid
    {
        private readonly float halfGridSize = 0.5f; 
        public Vertex[] vertices = new Vertex[8]; //顶点
        public Vector3 position = Vector3.zero; //立方体中心的世界坐标

        public Grid(Vector3 wp, int size)
        {
            position = wp;
            halfGridSize = size / 2f;
            vertices[0] = new Vertex(wp.x - halfGridSize, wp.y - halfGridSize, wp.z + halfGridSize);
            vertices[1] = new Vertex(wp.x + halfGridSize, wp.y - halfGridSize, wp.z + halfGridSize);
            vertices[2] = new Vertex(wp.x + halfGridSize, wp.y - halfGridSize, wp.z - halfGridSize);
            vertices[3] = new Vertex(wp.x - halfGridSize, wp.y - halfGridSize, wp.z - halfGridSize);
            vertices[4] = new Vertex(wp.x - halfGridSize, wp.y + halfGridSize, wp.z + halfGridSize);
            vertices[5] = new Vertex(wp.x + halfGridSize, wp.y + halfGridSize, wp.z + halfGridSize);
            vertices[6] = new Vertex(wp.x + halfGridSize, wp.y + halfGridSize, wp.z - halfGridSize);
            vertices[7] = new Vertex(wp.x - halfGridSize, wp.y + halfGridSize, wp.z - halfGridSize);
        }
    }
}

