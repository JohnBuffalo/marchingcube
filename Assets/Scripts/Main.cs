using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

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
        public float isoLevel = 10f;
        public Cell CellPrefab;
        private List<Grid> grids = new List<Grid>();
        private List<Cell> cells = new List<Cell>();
        private Debugger debugger = new Debugger();
        public GameObject brush;
        private GameObject activeHandler;
        private GameObject unactiveHandler;
        private float ActiveEclipse = 0.01f;
        private Transform root;
        private Transform pool;
        private void Awake()
        {
            root = GameObject.Find("root").transform;
            pool = GameObject.Find("pool").transform;
            CreateHandlers();
            CreateGrids();
        }

        private void CreateGrids()
        {
            grids.Clear();
            cells.Clear();
            for (int x = 0; x < length; x += gridSize)
            {
                for (int z = 0; z < width; z += gridSize)
                {
                    for (int y = 0; y < height; y += gridSize)
                    {
                        var grid = new Grid(new Vector3(x, y, z), gridSize);
                        grid.SetIsoLevel();
                        grids.Add(grid);
                        var cellGo = Instantiate(CellPrefab, pool.transform);
                        var cell = cellGo.GetComponent<Cell>();
                        cells.Add(cell);
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
                    if (Vector3.Distance(vertex.worldPosition, activeHandler.transform.position) <= ActiveEclipse &&
                        !vertex.active)
                    {
                        vertex.active = true;
                        grid.dirty = true;
                    }

                    if (vertex.active &&Vector3.Distance(vertex.worldPosition, unactiveHandler.transform.position) <= ActiveEclipse)
                    {
                        vertex.active = false;
                        grid.dirty = true;
                    }
                }
            }
        }

        private void RefreshCellState()
        {
            //遍历所有被激活
            for (int i = 0; i < grids.Count; i++)
            {
                var grid = grids[i];
                if(!grid.dirty) continue;
                grid.dirty = false;
                var cell = cells[i];
                cell.Refresh();
                //1.根据空间体素与等值面的关系计算cube的pattern
                grid.RefreshCubeIndex(isoLevel);
                //2.根据cubeIndex查找边的pattern
                var cubeIndex = grid.cubeIndex;
                var edgeIndex = LookUpTable.EdgeTable[cubeIndex];
                if (edgeIndex == 0) continue;
                var vertPositions = RefreshTriangleVertices(0.5f, grid, edgeIndex);
                //3.根据边的pattern，查找以逆时针排列的三角形顶点数组，因提前以逆时针排列，可直接用于渲染
                for (int j = 0; LookUpTable.TriTable[cubeIndex, j] != -1; j += 3)
                {
                    var p1 = vertPositions[LookUpTable.TriTable[cubeIndex, j]];
                    var p2 = vertPositions[LookUpTable.TriTable[cubeIndex, j+1]];
                    var p3 = vertPositions[LookUpTable.TriTable[cubeIndex, j+2]];
                    cell.AddTriangle(p1,p2,p3);
                }
                var cellValid = cell.SetMesh();
                if (cellValid)
                {
                    cell.transform.SetParent(root);
                }
                else
                {
                    cell.transform.SetParent(pool);
                }
            }
        }

        private void Update()
        {
            RefreshVertexState();
            RefreshCellState();
        }

        
        public Vector3[] RefreshTriangleVertices(float isoLevel, Grid grid, int edgeIndex)
        {
            // Debug.Log(edgeIndex + " " + Convert.ToString(edgeIndex, 2));
            var verArray = grid.vertices;
            Vector3[] vertPositions = new Vector3[12];
            if ((edgeIndex & 1) > 0)
                vertPositions[0] = (VertexInterp(isoLevel, verArray[0], verArray[1]));
            
            if ((edgeIndex & 2) > 0)
                vertPositions[1] = (VertexInterp(isoLevel, verArray[1], verArray[2]));
            
            if ((edgeIndex & 4) > 0)
                vertPositions[2] = (VertexInterp(isoLevel, verArray[2], verArray[3]));
            
            if ((edgeIndex & 8) > 0)
                vertPositions[3] = (VertexInterp(isoLevel, verArray[3], verArray[0]));
            
            if ((edgeIndex & 16) > 0)
                vertPositions[4] = (VertexInterp(isoLevel, verArray[4], verArray[5]));
            
            if ((edgeIndex & 32) > 0)
                vertPositions[5] = (VertexInterp(isoLevel, verArray[5], verArray[6]));
            
            if ((edgeIndex & 64) > 0)
                vertPositions[6] = (VertexInterp(isoLevel, verArray[6], verArray[7]));
            
            if ((edgeIndex & 128) > 0)
                vertPositions[7] = (VertexInterp(isoLevel, verArray[7], verArray[4]));

            if ((edgeIndex & 256) > 0)
                vertPositions[8] = (VertexInterp(isoLevel, verArray[0], verArray[4]));
            
            if ((edgeIndex & 512) > 0)
                vertPositions[9] = (VertexInterp(isoLevel, verArray[1], verArray[5]));
            
            if ((edgeIndex & 1024) > 0)
                vertPositions[10] = (VertexInterp(isoLevel, verArray[2], verArray[6]));
            
            if ((edgeIndex & 2048) > 0)
                vertPositions[11] = (VertexInterp(isoLevel, verArray[3], verArray[7]));

            return vertPositions;
        }

        public Vector3 VertexInterp(float isoLevel, Vertex ver1, Vertex ver2)
        {
            // var p1 = ver1.worldPosition;
            // var p2 = ver2.worldPosition;
            // var v1 = ver1.isoLevel;
            // var v2 = ver2.isoLevel;
            // if (Mathf.Abs(isoLevel - v1) < 0.001f) return p1;
            // if (Mathf.Abs(isoLevel - v2) < 0.001f) return p2;
            // if (Mathf.Abs(v1 - v2) < 0.001f) return p1;
            // var mu = (isoLevel - v1) / (v2 - v1);
            // var result = p1 + mu * (p2 - p1);
            // return result;

            return (ver1.worldPosition + ver2.worldPosition) * isoLevel;
        }

        #region Debug

        private void OnDrawGizmos()
        {
            DebugDrawer();
        }

        private void DebugDrawer()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(Vector3.zero, 0.1f);

            for (int i = 0; i < grids.Count; i++)
            {
                var grid = grids[i];
                // draw vertex
                // debugger.DrawVertex(grid);
                // draw frame
                // debugger.DrawFrame(grid);
                // draw active
                // debugger.DrawActiveVertex(grid);
            }

            for (int i = 0; i < cells.Count; i++)
            {
                debugger.DrawCellMesh(cells[i]);
            }
        }

        private void CreateHandlers()
        {
            ActiveEclipse = gridSize / 2f * Mathf.Sqrt(3f);

            activeHandler = Instantiate(brush);
            activeHandler.gameObject.name = "ActiveBrush";
            activeHandler.transform.position =  new Vector3(-2,-2,-2);
            activeHandler.GetComponent<MeshRenderer>().materials[0].color = Color.magenta;

            unactiveHandler = Instantiate(brush);
            ;
            unactiveHandler.transform.name = "UnactiveBrush";
            unactiveHandler.transform.position = Vector3.down * 5;
            unactiveHandler.GetComponent<MeshRenderer>().materials[0].color = Color.white;
        }

        #endregion
    }
}