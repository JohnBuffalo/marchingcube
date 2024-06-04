using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MDH.MarchingCube
{
    /// <summary>
    /// 执行MarchingCube的单位体素
    /// </summary>
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class Cell:MonoBehaviour
    {
        public List<Triangle> triangleList = new List<Triangle>();
        private Mesh cellMesh;

        public void Refresh()
        {
            triangleList.Clear();
        }

        public void AddTriangle(Vector3 p1, Vector3 p2, Vector3 p3)
        {
            var tri = new Triangle(p1, p2, p3);
            triangleList.Add(tri);
        }

        public bool SetMesh()
        {
            if (triangleList.Count == 0) return false;

            GetComponent<MeshFilter>().mesh = cellMesh = new Mesh()
            {
                name = $"{triangleList.Count}"
            };

            var verteices = new List<Vector3>();
            var triIndex = new List<int>();

            for (int i = 0; i < triangleList.Count; i++)
            {
                verteices.AddRange(triangleList[i].vertex);
            }

            for (int i = 0; i < verteices.Count; i++)
            {
                triIndex.Add(i);
            }

            cellMesh.SetVertices(verteices);
            cellMesh.triangles = triIndex.ToArray();
            
            return true;
        }

    }
}