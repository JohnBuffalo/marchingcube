using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MDH.MarchingCube
{
    //1.绘制空间网格坐标系，每个立方体网格称为一个grid
    //2.根据active handler 和 unactive handler的位置刷新顶点激活状态
    public class Main : MonoBehaviour
    {
        public int gridSize = 1;
        public int width = 20;
        public int length = 20;
        public int height = 20;
        private List<Grid> grids = new List<Grid>();
        private Debugger debugger = new Debugger();
        public GameObject brush;
        private GameObject activeHandler;
        private GameObject unactiveHandler;
        private float ActiveEclipse = 0.01f;
        private void Awake()
        {
            ActiveEclipse = gridSize / 2f * Mathf.Sqrt(3f);
            CreateHandlers();
            CreateGrids();
        }

        private void CreateHandlers()
        {
            activeHandler = Instantiate(brush);
            activeHandler.gameObject.name = "ActiveBrush";
            activeHandler.transform.position = Vector3.down;
            activeHandler.GetComponent<MeshRenderer>().materials[0].color = Color.magenta;
            
            unactiveHandler = Instantiate(brush);;
            unactiveHandler.transform.name = "UnactiveBrush";
            unactiveHandler.transform.position = Vector3.down;
            unactiveHandler.GetComponent<MeshRenderer>().materials[0].color = Color.white;
        }
        
        private void CreateGrids()
        {
            for (int x = 0; x < length; x+=gridSize)
            {
                for (int z = 0; z < width; z+=gridSize)
                {
                    for (int y = 0; y < height; y+=gridSize)
                    {
                        var grid = new Grid(new Vector3(x, y, z), gridSize);
                        grids.Add(grid);
                    }
                }
            }
        }

        private void RefreshVertexState()
        {
            if (grids == null) return;
            for (int i = 0; i < grids.Count; i++)
            {
                var grid = grids[i];
                for (int j = 0; j < grid.vertices.Length; j++)
                {
                    var vertex = grid.vertices[j];
                    if (Vector3.Distance(vertex.worldPosition , activeHandler.transform.position) <= ActiveEclipse)
                    {
                        vertex.active = true;
                    }
                    if (Vector3.Distance(vertex.worldPosition , unactiveHandler.transform.position) <= ActiveEclipse)
                    {
                        vertex.active = false;
                    }
                }
            }
        }

        private void Update()
        {
            RefreshVertexState();
        }

        private void OnDrawGizmos()
        {
            DebugDrawer();
        }

        private void DebugDrawer()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(Vector3.zero,0.1f);
            
            for (int i = 0; i < grids.Count; i++)
            {
                var grid = grids[i];
                
                //draw vertex
                // debugger.DrawVertex(grid);
                //draw frame
                debugger.DrawFrame(grid);
                //draw active
                debugger.DrawActiveVertex(grid);

            }
        }
    } 
}

