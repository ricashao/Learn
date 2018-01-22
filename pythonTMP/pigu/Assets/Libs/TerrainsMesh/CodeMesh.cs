using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MeshData:Object{
    public Vector3[] vertices;
    public Vector2[] uv;
    public int[] triangles;
}

public class CodeMesh : MonoBehaviour {

    public Terrain terrain;

    // Use this for initialization
    void Start() {
        //GetTriangle(this.gameObject);
        //GetRect(this.gameObject);
        //GetCellXRect(this.gameObject);
        MeshFilter filter = GetComponent<MeshFilter>();

        int meshx = 32;
        int meshz = 32;

        //Mesh mesh = GetCellXZRect(meshx, meshz, .5f, .5f, .5f, .5f);
        Mesh mesh = GetCellXZRect(meshx, meshz);

        SetHeight(terrain, mesh, meshx, meshz);
        //修改完高度以后重新运算法线
        mesh.RecalculateNormals();
        //修改完高度以后重新运算切线
        mesh.RecalculateTangents();
        //修改完高度以后重新运算包围盒子
        mesh.RecalculateBounds();
        //更新网格
        filter.sharedMesh = mesh;

    }

    public GameObject GetTriangle(GameObject go)
    {
        //GameObject go = new GameObject("Triangle");
        MeshFilter filter = go.AddComponent<MeshFilter>();

        // 构建三角形的三个顶点，并赋值给Mesh.vertices
        Mesh mesh = new Mesh();
        filter.sharedMesh = mesh;
        mesh.vertices = new Vector3[] {
                new Vector3 (0, 0, 0),
                new Vector3 (0, 0, 1),
                new Vector3 (0, 1, 0),
            };

        // 构建三角形的顶点顺序，因为这里只有一个三角形，
        // 所以只能是(0, 1, 2)这个顺序。
        mesh.triangles = new int[3] { 0, 1, 2 };

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        // 使用Shader构建一个材质，并设置材质的颜色。
        Material material = new Material(Shader.Find("Diffuse"));
        material.SetColor("_Color", Color.yellow);

        // 构建一个MeshRender并把上面创建的材质赋值给它，
        // 然后使其把上面构造的Mesh渲染到屏幕上。
        MeshRenderer renderer = go.AddComponent<MeshRenderer>();
        renderer.sharedMaterial = material;

        return go;
    }

    public GameObject GetRect(GameObject go)
    {
        //GameObject go = new GameObject("Triangle");
        MeshFilter filter = go.AddComponent<MeshFilter>();

        // 构建三角形的三个顶点，并赋值给Mesh.vertices
        Mesh mesh = new Mesh();
        filter.sharedMesh = mesh;
        mesh.vertices = new Vector3[] {
                new Vector3 (0, 0, 0),
                new Vector3 (1, 0, 0),
                new Vector3 (1, 0, 1),
                new Vector3 (0, 0, 1),
            };

        // 构建三角形的顶点顺序，因为这里只有一个三角形，
        // 所以只能是(0, 1, 2)这个顺序。
        //mesh.triangles = new int[3] { 0, 1, 2 };//逆时针 背面
        //mesh.triangles = new int[3] { 0, 2, 1};//顺时针 正面

        mesh.triangles = new int[6] { 0, 2, 1, 0, 3, 2 };//顺时针 正面

        Vector2[] uv = new Vector2[mesh.vertices.Length];

        for (int i = 0; i < uv.Length; i += 4)
        {
            uv[i + 0] = new Vector2(0, 0);
            uv[i + 1] = new Vector2(1, 0);
            uv[i + 2] = new Vector2(1, 1);
            uv[i + 3] = new Vector2(0, 1);
        }

        mesh.uv = uv;

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        // 使用Shader构建一个材质，并设置材质的颜色。
        Material material = new Material(Shader.Find("Diffuse"));
        material.SetColor("_Color", Color.yellow);

        // 构建一个MeshRender并把上面创建的材质赋值给它，
        // 然后使其把上面构造的Mesh渲染到屏幕上。
        MeshRenderer renderer = go.AddComponent<MeshRenderer>();
        renderer.sharedMaterial = material;

        return go;
    }

    public GameObject GetCellXRect(GameObject go)
    {
        //GameObject go = new GameObject("Triangle");
        MeshFilter filter = go.AddComponent<MeshFilter>();

        // 构建三角形的三个顶点，并赋值给Mesh.vertices
        Mesh mesh = new Mesh();
        filter.sharedMesh = mesh;

        int xCount = 2;
        Vector3[] verticesLine = new Vector3[4 * xCount];

        for (int xi = 0; xi < xCount; xi++) {
            verticesLine[0 + xi * 4] = new Vector3(0 + xi, 0, 0);
            verticesLine[1 + xi * 4] = new Vector3(1 + xi, 0, 0);
            verticesLine[2 + xi * 4] = new Vector3(1 + xi, 0, 1);
            verticesLine[3 + xi * 4] = new Vector3(0 + xi, 0, 1);
        }

        mesh.vertices = verticesLine;

        int[] triangles = new int[6 * xCount]; //每个面6个
        for (int xi = 0; xi < xCount; xi++)
        {
            triangles[0 + xi * 6] = 0 + xi * 4;
            triangles[1 + xi * 6] = 2 + xi * 4;
            triangles[2 + xi * 6] = 1 + xi * 4;
            triangles[3 + xi * 6] = 0 + xi * 4;
            triangles[4 + xi * 6] = 3 + xi * 4;
            triangles[5 + xi * 6] = 2 + xi * 4;
        }
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        // 使用Shader构建一个材质，并设置材质的颜色。
        Material material = new Material(Shader.Find("Diffuse"));
        material.SetColor("_Color", Color.yellow);

        // 构建一个MeshRender并把上面创建的材质赋值给它，
        // 然后使其把上面构造的Mesh渲染到屏幕上。
        MeshRenderer renderer = go.AddComponent<MeshRenderer>();
        renderer.sharedMaterial = material;

        return go;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="wx">网格切割单位</param>
    /// <param name="wz">网格切割单位</param>
    /// <param name="uOffset"></param>
    /// <param name="vOffset"></param>
    /// <param name="uSize"></param>
    /// <param name="vSize"></param>
    /// <returns></returns>
    public static Mesh GetCellXZRect( int wx = 32,int wz = 32, 
                                      float uOffset = 0,float vOffset = 0,
                                      float uSize = 1f,float vSize = 1f,float sx = 1, float sz = 1)
    {
        //GameObject go = new GameObject("Triangle");
        // 构建三角形的三个顶点，并赋值给Mesh.vertices
        //Mesh mesh = new Mesh();   
        MeshData mesh = new MeshData();
       
        //单位为 1 面积 32x32
        int zCount = wx ;   
        int xCount = wz ;   

        int xLineCount = 4 * xCount; //一行顶点数

        Vector3[] verticesLine = new Vector3[xLineCount * zCount];
        /**/
        for (int zi = 0; zi < zCount; zi++)
        {
            for (int xi = 0; xi < xCount; xi++)
            {
                verticesLine[0 + xi * 4 + zi * xLineCount] = new Vector3((0 + xi) * sx, 0,( 0 + zi) * sz) ;
                verticesLine[1 + xi * 4 + zi * xLineCount] = new Vector3((1 + xi) * sx, 0,( 0 + zi) * sz);
                verticesLine[2 + xi * 4 + zi * xLineCount] = new Vector3((1 + xi) * sx, 0,( 1 + zi) * sz);
                verticesLine[3 + xi * 4 + zi * xLineCount] = new Vector3((0 + xi) * sx, 0,( 1 + zi) * sz);
            }
        }

        mesh.vertices = verticesLine;

        Vector2[] uv = new Vector2[verticesLine.Length];
        /*
        float uOffset = 0;
        float vOffset = 0;
        float uSize = 1f;
        float vSize = 1f;*/
        float u = uSize / zCount;
        float v = vSize / xCount;

        for (int zi = 0; zi < zCount; zi++)
        {
            for (int xi = 0; xi < xCount; xi++)
            {
                uv[0 + xi * 4 + zi * xLineCount] = new Vector2(uOffset + 0 + u * xi, vOffset + 0 + v * zi); //new Vector2(0, 0);
                uv[1 + xi * 4 + zi * xLineCount] = new Vector2(uOffset + u + u * xi, vOffset + 0 + v * zi); //new Vector2(1, 0);
                uv[2 + xi * 4 + zi * xLineCount] = new Vector2(uOffset + u + u * xi, vOffset + v + v * zi);
                uv[3 + xi * 4 + zi * xLineCount] = new Vector2(uOffset + 0 + u * xi, vOffset + v + v * zi);
            } 
        }

        mesh.uv = uv;
        
        int xLineTrianglesCount = 6 * xCount; //一行索引数

        int[] triangles = new int[xLineTrianglesCount * zCount]; //每个面3个 * 2 = 6个索引 矩形

        for (int zi = 0; zi < zCount; zi++)
        {
            for (int xi = 0; xi < xCount; xi++)
            {
                triangles[0 + xi * 6 + zi * xLineTrianglesCount] = 0 + xi * 4 + zi * xLineCount;
                triangles[1 + xi * 6 + zi * xLineTrianglesCount] = 2 + xi * 4 + zi * xLineCount;
                triangles[2 + xi * 6 + zi * xLineTrianglesCount] = 1 + xi * 4 + zi * xLineCount;
                triangles[3 + xi * 6 + zi * xLineTrianglesCount] = 0 + xi * 4 + zi * xLineCount;
                triangles[4 + xi * 6 + zi * xLineTrianglesCount] = 3 + xi * 4 + zi * xLineCount;
                triangles[5 + xi * 6 + zi * xLineTrianglesCount] = 2 + xi * 4 + zi * xLineCount;
            }
        }
      
        mesh.triangles = triangles;
       
        return optimizeMesh(mesh);
    }

    class VertexData
    {
        public Vector3 position;
        public Vector2 uv;
        public int indexOld;
        public int index;
        public int count;
        public string debug() {
            return string.Format(position + "index = {0},indexOld = {1},uv = {2},count = {3}", index, indexOld, uv, count);
        }
    }

    //网格顶点优化
    public static Mesh optimizeMesh(MeshData mesh) {
        //新顶点数组
        List<Vector3> newVertices = new List<Vector3>();
        List<Vector2> newUV = new List<Vector2>();
        //新顶点数位置映射
        Dictionary<Vector3, VertexData> v2i_dic = new Dictionary<Vector3, VertexData>();

        Dictionary<Vector3, List<int>> v2i_list_dic = new Dictionary<Vector3, List<int>>();

        Vector3[] vertices = mesh.vertices;
        Vector2[] uvArr = mesh.uv;

        for (int i = 0; i < vertices.Length; i++) {

            VertexData vertexData = null;

            v2i_dic.TryGetValue(vertices[i], out vertexData);

            if (vertexData == null) {
                vertexData = new VertexData();
                vertexData.position = vertices[i];
                vertexData.uv = uvArr[i];
                vertexData.indexOld = i;
                vertexData.index = newVertices.Count;
                vertexData.count = 1;

                v2i_dic.Add(vertices[i], vertexData);

                newVertices.Add(vertices[i]);
                newUV.Add(uvArr[i]);
            }
            else {
                vertexData.count++;
                //Debug.LogWarning(" 顶点 " + vertices[i] + " 位置： " + i + " 合并到位置： " + vertexData.debug());
            }
        }
        //Debug.LogWarning("旧顶点数:" + vertices.Length + ",新顶点数:" + newVertices.Count);

        //顶点索引
        int[] triangles = mesh.triangles;

        int[] trianglesOptimize = new int[triangles.Length];

        for (int i = 0; i < triangles.Length; i++)
        {
            Vector3 vertice = vertices[triangles[i]];

            VertexData vertexData;
            v2i_dic.TryGetValue(vertice, out vertexData);
            if (vertexData != null)
            {
                trianglesOptimize[i] = vertexData.index;
            }
            else {
                Debug.LogError("映射失败 " + vertice);
            }
        }

        Mesh optimizeMesh = new Mesh();
        optimizeMesh.vertices = newVertices.ToArray();
        optimizeMesh.triangles = trianglesOptimize;
        optimizeMesh.uv = newUV.ToArray();
        optimizeMesh.RecalculateNormals();
        optimizeMesh.RecalculateBounds();
        return optimizeMesh;
    }

    public static void SetHeight(Terrain terrain, Mesh mesh ,int meshx, int meshz)
    {
        int w = terrain.terrainData.heightmapWidth;
        int h = terrain.terrainData.heightmapHeight;
        int detailHeight = terrain.terrainData.detailHeight;
        float sy = terrain.terrainData.size.y;
        float[,] heightmap = terrain.terrainData.GetHeights(0, 0, w, h);

        SetHeightMap(mesh, meshx, meshz, heightmap, w, h, sy);
    }

    public static void SetHeight(Terrain terrain, Mesh mesh, int meshx, int meshz, int heightMapx, int heightMapy, int heightMapw, int heightMaph)
    {
        int w = terrain.terrainData.heightmapWidth;
        int h = terrain.terrainData.heightmapHeight;
        int detailHeight = terrain.terrainData.detailHeight;
        float sy = terrain.terrainData.size.y;
        float[,] heightmap = terrain.terrainData.GetHeights(0, 0, w, h);

        SetHeightMap(mesh, meshx, meshz, heightmap, w, h, sy);
    }

    //public static Dictionary<Vector2, float> HeightMapDic = new Dictionary<Vector2, float>();
    public static float[,] HeightMapDic;
   // public static float[,] HeightMapDic;
    /// <summary>
    /// 设置网格高度数据
    /// </summary>
    /// <param name="mesh">网格对象</param>
    /// <param name="meshw">网格对象x宽度</param>
    /// <param name="meshh">网格对象z宽度</param>
    /// <param name="heightmap">高度图数据</param>
    /// <param name="heightmapw">高度图宽度</param>
    /// <param name="heightmaph">高度图高度</param>
    /// <param name="sy">高度</param>
    public static void SetHeightMap(Mesh mesh ,int meshw,int meshh, float[,]  heightmap, int heightmapw, int heightmaph, float sy)
    {
        //HeightMapDic.Clear();
        HeightMapDic = new float[meshw + 2, meshh + 2];  

        float wb = (float)heightmapw / (float)meshw;
        float hb = (float)heightmaph / (float)meshh;

        Vector3[] vertices = mesh.vertices;
        for (int i =0;i< vertices.Length;i++) {
            Vector3 vertice = vertices[i];
            //int mapx = Mathf.RoundToInt(vertice.x * wb);   
            //int mapz = Mathf.RoundToInt(vertice.z * hb);
            int mapx = Mathf.FloorToInt((vertice.x ) * wb );
            int mapz = Mathf.FloorToInt((vertice.z ) * hb );

            //Debug.LogWarning("mapx " + mapx + ",mapz " + mapz);

            if (heightmapw <= mapx) mapx--;
            if (heightmaph <= mapz) mapz--;

            vertice.y = heightmap[mapz, mapx] * sy;
            vertices[i] = vertice;
            int x = (int)vertice.x;
            int z = (int)vertice.z;
            //Vector2 v2 = new Vector2(vertice.x, vertice.z);
           
            HeightMapDic[x,z] =vertice.y;

            // Debug.LogWarning("HeightMapDicAdd " + HeightMapDic[x, z]);
            // Debug.LogWarning("HeightMapDicAdd " + HeightMapDic[v2]);
        }

        mesh.vertices = vertices;
    }
    /*
    public static void Cutting1(Terrain terrain,int cellxNum, int cellzNum,List<MeshCell> list)
    {
        Vector3 size = terrain.terrainData.size;

        int heightmapWidth = terrain.terrainData.heightmapWidth;
        int heightmapHeight = terrain.terrainData.heightmapHeight;

        float heightmapWidth_size_x = heightmapWidth / size.x;
        float heightmapHeight_size_z = heightmapHeight / size.z;
        //单个地图块的尺寸
        int cellxSize = Mathf.RoundToInt(size.x / cellxNum);
        int cellzSize = Mathf.RoundToInt(size.z / cellzNum);

        int heightmapXOffset = 0;
        int heightmapZOffset = 0;

        int heightmapXSize = Mathf.RoundToInt(cellxSize * heightmapWidth_size_x);  
        int heightmapZSize = Mathf.RoundToInt(cellzSize * heightmapHeight_size_z); 

        float uOffset = 0;
        float vOffset = 0;
        float uSize = 1f / cellxNum;
        float vSize = 1f / cellzNum;

        for (int i=0; i < cellxNum; i++ )
        {
            for (int j = 0; j < cellzNum; j++)
            {
                uOffset = i * uSize;  
                vOffset = j * vSize;

                Mesh mesh = GetCellXZRect(cellxSize+1, cellzSize+1, uOffset, vOffset, uSize, vSize); 

                heightmapXOffset = i * Mathf.RoundToInt(cellxSize * heightmapWidth_size_x);
                heightmapZOffset = j * Mathf.RoundToInt(cellzSize * heightmapHeight_size_z);

                float sy = terrain.terrainData.size.y;
                float[,] heightmap = terrain.terrainData.GetHeights(heightmapXOffset, heightmapZOffset, heightmapXSize, heightmapZSize);

                SetHeightMap(mesh, cellxSize+1, cellzSize+1, heightmap, heightmapXSize, heightmapZSize, sy);

                list.Add(new MeshCell(i,j,mesh));
            }
        }
    }
    */

    public static Vector3[,] normal;
    public static Vector4[,] tangent;

    public static void Cutting(Terrain terrain, int cellxNum, int cellzNum, List<MeshCell> list) {

        Vector3 size = terrain.terrainData.size;

        int meshx = (int)size.x;
        int meshz = (int)size.z;

#if UNITY_EDITOR
        EditorUtility.DisplayProgressBar("Generate...", "terrainMesh GetCellRect", 0.15f);
#endif

        Mesh terrainMesh = CodeMesh.GetCellXZRect(meshx, meshz);

#if UNITY_EDITOR
        EditorUtility.DisplayProgressBar("Generate...", "terrainMesh SetHeight", 0.2f);
#endif

        CodeMesh.SetHeight(terrain, terrainMesh, meshx, meshz);

#if UNITY_EDITOR
        EditorUtility.DisplayProgressBar("Generate...", "terrainMesh Recalculate Normals,Tangents", 0.3f);
#endif

        //修改完高度以后重新运算法线
        terrainMesh.RecalculateNormals(); 
        //修改完高度以后重新运算切线
        terrainMesh.RecalculateTangents();

#if UNITY_EDITOR
        EditorUtility.DisplayProgressBar("Generate...", "save Recalculate Normals,Tangents", 0.35f);
#endif
        normal = new Vector3[meshx + 1, meshz + 1];
        tangent = new Vector4[meshx + 1, meshz + 1];

        for (int i = 0; i < terrainMesh.vertices.Length; i++)
        {
            Vector3 vertice = terrainMesh.vertices[i];
            int x = (int)vertice.x;
            int z = (int)vertice.z;
            normal[x, z]  = terrainMesh.normals[i];
            tangent[x, z] = terrainMesh.tangents[i];
        }
        //HeightMapDic.Add(v2, vertice.y);

        int heightmapWidth = terrain.terrainData.heightmapWidth;
        int heightmapHeight = terrain.terrainData.heightmapHeight;

        float heightmapWidth_size_x = heightmapWidth / size.x;
        float heightmapHeight_size_z = heightmapHeight / size.z;
        //单个地图块的尺寸
        int cellxSize = Mathf.RoundToInt(size.x / cellxNum);
        int cellzSize = Mathf.RoundToInt(size.z / cellzNum);

        int heightmapXSize = Mathf.RoundToInt(cellxSize * heightmapWidth_size_x);
        int heightmapZSize = Mathf.RoundToInt(cellzSize * heightmapHeight_size_z);

        float uOffset = 0;
        float vOffset = 0;
        float uSize = 1f / cellxNum;
        float vSize = 1f / cellzNum;

        Vector2 key = new Vector2();
#if UNITY_EDITOR
        EditorUtility.DisplayProgressBar("Generate...", "terrainMesh Cell", 0.38f);
#endif
        for (int i = 0; i < cellxNum; i++)
        {
#if UNITY_EDITOR
            EditorUtility.DisplayProgressBar("Generate...", "terrainMesh Cell", 0.38f + 0.6f*(float)i/(float)cellxNum);
#endif
            for (int j = 0; j < cellzNum; j++)
            {
                uOffset = i * uSize;
                vOffset = j * vSize;

                Mesh mesh = GetCellXZRect(cellxSize , cellzSize , uOffset, vOffset, uSize, vSize);
                
                Vector3[] vertices = mesh.vertices;
                Vector3[] normals = new Vector3[vertices.Length];
                Vector4[] tangents = new Vector4[vertices.Length];

                for (int k = 0;k< vertices.Length;k++) {
                    Vector3 p = vertices[k];
                 
                    int x = (int)p.x + i * cellxSize;   
                    int z = (int)p.z + j * cellzSize;

                    key.x = x;
                    key.y = z;
                    //Vector2 v2 = new Vector2(vertice.x, vertice.z);
                    p.y = HeightMapDic[x, z];

                    Debug.LogWarning("key = >>" + key + ",HeightMapDic = " + HeightMapDic[x, z]);

                    vertices[k] = p;

                    normals[k] = normal[x, z];
                    tangents[k] = tangent[x, z];
                }

                mesh.vertices = vertices;
                //修改完高度以后重新运算法线
                mesh.normals = normals;
                //修改完高度以后重新运算切线
                mesh.tangents = tangents;
                //修改完高度以后重新运算包围盒子
                mesh.RecalculateBounds();
                /*
                heightmapXOffset = i * Mathf.RoundToInt(cellxSize * heightmapWidth_size_x);
                heightmapZOffset = j * Mathf.RoundToInt(cellzSize * heightmapHeight_size_z);

                float sy = terrain.terrainData.size.y;
                float[,] heightmap = terrain.terrainData.GetHeights(heightmapXOffset, heightmapZOffset, heightmapXSize, heightmapZSize);

                SetHeightMap(mesh, cellxSize + 1, cellzSize + 1, heightmap, heightmapXSize, heightmapZSize, sy);
                */
                list.Add(new MeshCell(i, j, mesh));
            }
        }
#if UNITY_EDITOR
        EditorUtility.DisplayProgressBar("Generate...", "terrainMesh Cell", 0.38f + 0.6f + 0.02f);
#endif
    }
}

