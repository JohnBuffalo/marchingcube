using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MDH.MarchingCube
{
    /// <summary>
    /// 执行MarchingCube的单位体素
    /// </summary>
    public class Cell
    {
        public List<Triangle> triangleList = new List<Triangle>();

        public void Refresh()
        {
            triangleList.Clear();
        }

        public void AddTriangle(Vector3 p1, Vector3 p2, Vector3 p3)
        {
            var tri = new Triangle(p1, p2, p3);
            triangleList.Add(tri);
        }

    }
}