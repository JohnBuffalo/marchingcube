using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MDH.MarchingCube
{
    public class Debugger
    {
        public void DrawVertex(Grid grid)
        {
            //draw vertex
            var center = grid.position;
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(center, 0.1f);
            Gizmos.color = Color.blue;
            for (int j = 0; j < grid.vertices.Length; j++)
            {
                var vertex = grid.vertices[j];
                Gizmos.DrawSphere(vertex.worldPosition, 0.05f);
            }
        }

        public void DrawFrame(Grid grid)
        {
            Gizmos.color = Color.gray;
            Gizmos.DrawLine(grid.vertices[6].worldPosition, grid.vertices[5].worldPosition);
            Gizmos.DrawLine(grid.vertices[5].worldPosition, grid.vertices[1].worldPosition);
            Gizmos.DrawLine(grid.vertices[1].worldPosition, grid.vertices[2].worldPosition);
            Gizmos.DrawLine(grid.vertices[2].worldPosition, grid.vertices[6].worldPosition);
            Gizmos.DrawLine(grid.vertices[7].worldPosition, grid.vertices[4].worldPosition);
            Gizmos.DrawLine(grid.vertices[4].worldPosition, grid.vertices[0].worldPosition);
            Gizmos.DrawLine(grid.vertices[0].worldPosition, grid.vertices[3].worldPosition);
            Gizmos.DrawLine(grid.vertices[3].worldPosition, grid.vertices[7].worldPosition);
            for (int j = 0; j < 4; j++)
            {
                Gizmos.DrawLine(grid.vertices[j].worldPosition, grid.vertices[(j + 1) % 4].worldPosition);
                Gizmos.DrawLine(grid.vertices[j + 4].worldPosition, grid.vertices[(j + 1) % 4 + 4].worldPosition);
            }
        }

        public void DrawActiveVertex(Grid grid)
        {
            Gizmos.color = Color.blue;
            for (int j = 0; j < grid.vertices.Length; j++)
            {
                var vertex = grid.vertices[j];
                if(vertex.active)
                    Gizmos.DrawSphere(vertex.worldPosition, 0.05f);
            }
        }

        public void DrawCellMesh(Cell cell)
        {
            Gizmos.color = Color.red;
            var triangles = cell.triangleList;
            for (int i = 0; i < triangles.Count; i++)
            {
                var tri = triangles[i];
                var verts = tri.vertex;
                // Gizmos.DrawLine(verts[0], verts[1]);   
                // Gizmos.DrawLine(verts[1], verts[2]);   
                // Gizmos.DrawLine(verts[2], verts[0]);
                
                Gizmos.DrawSphere(verts[0],0.05f);
                Gizmos.DrawSphere(verts[1],0.05f);
                Gizmos.DrawSphere(verts[2],0.05f);
            }
        }
    }
}