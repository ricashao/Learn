using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

public enum C{
	r,
	g,
	b,
	a
}

public class TerrainTools : Editor { 

	public static string Path = "C:\\U3D_PRJ\\SimpleFramework_UGUI-0.4.1\\Assets\\Z!M.O.B.A Environment Art Pack\\Demo\\Terrain4Tex\\SplatAlphaMap_";

    [MenuItem(@"TerrainTools/SeveToMesh")]
    public static void SeveToMesh()
    {
        Transform[] transforms = Selection.transforms;
        foreach (Transform transform in transforms)
        {
            EditorUtility.DisplayProgressBar("Generate...", "heightmap", 0f);
            Terrain terrain = transform.GetComponent<Terrain>();
            Vector3 size = terrain.terrainData.size;
            int w = terrain.terrainData.heightmapWidth;
            int h = terrain.terrainData.heightmapHeight;
            int resolution = terrain.terrainData.heightmapResolution;
            float[,] heightmap = terrain.terrainData.GetHeights(0,0,w,h);

            EditorUtility.DisplayProgressBar("Generate...", "MeshRect", 0.1f);
            //网格密度
            int meshx = 32;
            int meshz = 32;

            Mesh mesh = CodeMesh.GetCellXZRect(meshx, meshz, 0, 0,1f,1f, size.x/(float) meshx, size.z / (float)meshz );

            EditorUtility.DisplayProgressBar("Generate...", "SetHeight", 0.6f);

            CodeMesh.SetHeight(terrain, mesh,(int) size.x, (int)size.z);

            EditorUtility.DisplayProgressBar("Generate...", "Recalculate", 0.9f);
            //修改完高度以后重新运算法线
            mesh.RecalculateNormals();
            //修改完高度以后重新运算切线
            mesh.RecalculateTangents();
            //修改完高度以后重新运算包围盒子
            mesh.RecalculateBounds();

            EditorUtility.DisplayProgressBar("Generate...", "SaveMesh", 0.9f);

            AssetDatabase.CreateAsset(mesh,"Assets/T4MOBJ/" + terrain.name + ".asset");   
            AssetDatabase.Refresh();

            EditorUtility.ClearProgressBar();
        }
    }

    [MenuItem(@"TerrainTools/SeveToMeshCell")]
    public static void SeveToMeshCell()
    {
        Transform[] transforms = Selection.transforms;
        foreach (Transform transform in transforms)
        {
            EditorUtility.DisplayProgressBar("Generate...", "Heightmap", 0f);

            Terrain terrain = transform.GetComponent<Terrain>();
            Vector3 size = terrain.terrainData.size;
            int w = terrain.terrainData.heightmapWidth;
            int h = terrain.terrainData.heightmapHeight;
            int resolution = terrain.terrainData.heightmapResolution;
            float[,] heightmap = terrain.terrainData.GetHeights(0, 0, w, h);

            EditorUtility.DisplayProgressBar("Generate...", "Cutting", 0.1f);
            /*
            int meshx = 32;
            int meshz = 32;

            Mesh mesh = CodeMesh.GetCellXZRect(meshx, meshz);
            CodeMesh.SetHeight(terrain, mesh, meshx, meshz);
            */

            List<MeshCell> list = new List<MeshCell>();
            //地图块密度
            int cellxNum = 8;
            int cellzNum = 8; 
            CodeMesh.Cutting(terrain, cellxNum, cellzNum, list);

            EditorUtility.DisplayProgressBar("Generate...", "TerrainMeshCell", 0.7f);

            GameObject terrainMeshCellRoot = new GameObject("TerrainMeshCellRoot");
            if(terrain.gameObject.transform.parent != null)
                terrainMeshCellRoot.transform.parent = terrain.gameObject.transform.parent;
            terrainMeshCellRoot.transform.position = terrain.gameObject.transform.position;
            
            for (int i = 0; i < list.Count; i++)
            {
                MeshCell meshCell = list[i];

                Mesh mesh = meshCell.mesh;
                //修改完高度以后重新运算法线
                //mesh.RecalculateNormals();
                //修改完高度以后重新运算切线
                //mesh.RecalculateTangents();
                //修改完高度以后重新运算包围盒子
                //mesh.RecalculateBounds();

                GameObject terrainMeshCell = new GameObject("TerrainMeshCell_" + mesh.name);
                terrainMeshCell.transform.parent = terrainMeshCellRoot.transform;
                terrainMeshCell.transform.localPosition = new Vector3(meshCell.xIndex * size.x/cellxNum,0, meshCell.zIndex * size.z/cellzNum);

                MeshFilter meshFilter = terrainMeshCell.AddComponent<MeshFilter>();
                meshFilter.sharedMesh = mesh;
                terrainMeshCell.AddComponent<MeshRenderer>();
                terrainMeshCell.AddComponent<MeshCollider>();
                //AssetDatabase.CreateAsset(mesh, "Assets/T4MOBJ/" + terrain.name +"_"+ mesh.name + ".asset");
                //AssetDatabase.Refresh();
            }
          
            if(true){
                Vector3 terrainSize = terrain.terrainData.size;
                int meshx = (int)terrainSize.x;
                int meshz = (int)terrainSize.z;

                Mesh terrainMesh = CodeMesh.GetCellXZRect(meshx, meshz);
                CodeMesh.SetHeight(terrain, terrainMesh, meshx, meshz);  
                //修改完高度以后重新运算法线
                terrainMesh.RecalculateNormals();
                //修改完高度以后重新运算切线
                terrainMesh.RecalculateTangents();
                //修改完高度以后重新运算包围盒子
                terrainMesh.RecalculateBounds();

                GameObject terrainMeshCell = new GameObject("TerrainMesh");
                terrainMeshCell.transform.parent = terrainMeshCellRoot.transform;
                terrainMeshCell.transform.localPosition = new Vector3(0, 0, 0);

                MeshFilter meshFilter = terrainMeshCell.AddComponent<MeshFilter>();
                meshFilter.sharedMesh = terrainMesh;
                MeshRenderer meshRenderer = terrainMeshCell.AddComponent<MeshRenderer>();
            }
        }

        EditorUtility.ClearProgressBar();
    }

    [MenuItem(@"TerrainTools/SaveSplat")]
	public static void SaveSplat () {
		Transform[] transforms = Selection.transforms;
		foreach (Transform transform in transforms) {
			Terrain terrain = transform.GetComponent<Terrain> ();
			saveAlphamaps (terrain,Path);
		}
	}

	public static void saveAlphamaps(Terrain terrain,string fileName = "D:/mySplatAlphaMap_")
	{
		Texture2D [] texture2DArr = terrain.terrainData.alphamapTextures;
		for (int textureIndex=0; textureIndex < texture2DArr.Length; textureIndex++) {
			Texture2D texture2D = texture2DArr[textureIndex];
			//获取png二进制数据
			byte[] pngData = texture2D.EncodeToPNG ();
			//保存泼溅图
			FileStream   fs1 = new FileStream( fileName +terrain.name+"_"+textureIndex+".png",FileMode.Create); 
			BinaryWriter bw  = new BinaryWriter(fs1); 
			bw.Write( pngData,0,pngData.Length);   
			bw.Close();   
			fs1.Close(); 
		}
	}
	/// <summary>
	/// Saves the alphamaps all debug r
	/// r->r000
	/// g->r000
	/// b->r000
	/// a->r000
	/// </summary>
	[MenuItem(@"TerrainTools/saveAlphamapsAllDebugR")]
	public static void saveAlphamapsAllDebugR () {
		Transform[] transforms = Selection.transforms;
		foreach (Transform transform in transforms) {
			Terrain terrain = transform.GetComponent<Terrain> ();
			saveAlphamapsAllDebugRGBA (terrain, true, "D:/mySplatAlphaMapDebugA_");
		}
	}
	/// <summary>
	/// Saves the alphamaps all RGB.
	/// r->r000
	/// g->0g00
	/// b->00b0
	/// a->000a
	/// </summary>
	[MenuItem(@"TerrainTools/saveAlphamapsAllRGBA")]
	public static void saveAlphamapsAllRGBA () {
		Transform[] transforms = Selection.transforms;
		foreach (Transform transform in transforms) {
			Terrain terrain = transform.GetComponent<Terrain> ();
			saveAlphamapsAllDebugRGBA (terrain, false, "D:/mySplatAlphaMap_");
		}
	}

	public static void saveAlphamapsAllDebugRGBA(Terrain terrain,bool isDebugR,string fileName = "D:/mySplatAlphaMap_"){
		
		Texture2D [] texture2DArr = terrain.terrainData.alphamapTextures;
		for (int textureIndex=0; textureIndex < texture2DArr.Length; textureIndex++) {
			Texture2D texture2D = texture2DArr[textureIndex];
			savaTexture2D (texture2D,Color.red  ,isDebugR,fileName + textureIndex);
			savaTexture2D (texture2D,Color.green,isDebugR,fileName + textureIndex);
			savaTexture2D (texture2D,Color.blue ,isDebugR,fileName + textureIndex);
			savaTexture2D (texture2D,Color.black,isDebugR,fileName + textureIndex);
		}
	}

	public static void savaTexture2D(Texture2D texture2D ,Color c,bool isDebugR,string fileName = "D:/mySplatAlphaMap_"){
		Color setColor = new Color (0,0,0,isDebugR ? 255f : 0);
		string fileSaveName = fileName;
		Texture2D texture2DSave = new Texture2D (texture2D.width, texture2D.height, TextureFormat.ARGB32,false);
		if (c == Color.red) {
			fileSaveName += "_R";
			for(int i = 0 ; i<texture2D.width ; i++){
				for(int j = 0 ; j<texture2D.width ; j++){
					setColor.r = texture2D.GetPixel (i,j).r;
					texture2DSave.SetPixel (i,j,setColor);
				}
			}
		}
		if (c == Color.green) {
			fileSaveName += "_G";
		
			for(int i = 0 ; i<texture2D.width ; i++){
				for(int j = 0 ; j<texture2D.width ; j++){
					if(isDebugR)
						setColor.r = texture2D.GetPixel (i,j).g;
					else
						setColor.g = texture2D.GetPixel (i,j).g;
					texture2DSave.SetPixel (i,j,setColor);
				}
			}
		}
		if (c == Color.blue) {
			fileSaveName += "_B";

			for(int i = 0 ; i<texture2D.width ; i++){
				for(int j = 0 ; j<texture2D.width ; j++){
					if(isDebugR)
						setColor.r = texture2D.GetPixel (i,j).b;
					else
						setColor.b = texture2D.GetPixel (i,j).b;
					texture2DSave.SetPixel (i,j,setColor);
				}
			}
		}
		if (c == Color.black) {
			fileSaveName += "_A";

			for(int i = 0 ; i<texture2D.width ; i++){
				for(int j = 0 ; j<texture2D.width ; j++){
					if(isDebugR)
						setColor.r = texture2D.GetPixel (i,j).a;
					else
						setColor.a = texture2D.GetPixel (i,j).a;
					texture2DSave.SetPixel (i,j,setColor);
				}
			}
		}
			
		byte[] pngData = texture2DSave.EncodeToPNG ();
		//保存泼溅图
		FileStream   fs1 = new FileStream( fileSaveName + ".png",FileMode.Create); 
		BinaryWriter bw  = new BinaryWriter(fs1); 
		bw.Write( pngData,0,pngData.Length);   
		bw.Close();   
		fs1.Close(); 

	}

	[MenuItem(@"TerrainTools/combine")]
	public static void SplatMapCombine () {
		/*
		Texture2D texture2D_0r = getTexture2D ("D://0R+0A=0R//mySplatAlphaMap_0_R.png");
		Texture2D texture2D_0a = getTexture2D ("D://0R+0A=0R//mySplatAlphaMap_0_A.png");
	
		CombineTexture (texture2D_0r,texture2D_0a,texture2D_0r,C.r,C.a,C.r); 
		*/

		Texture2D texture2D_0 = getTexture2D ("D://0B+1G=0B//mySplatAlphaMap_0_B.png");
		Texture2D texture2D_1 = getTexture2D ("D://0B+1G=0B//mySplatAlphaMap_1_G.png");

		Texture2D Texture2D_save = CombineTexture (texture2D_0,texture2D_1,texture2D_0,C.b,C.g,C.b); 

		byte[] pngData = Texture2D_save.EncodeToPNG ();
		//保存泼溅图
		FileStream   fs1 = new FileStream("D://0B+1G=0B//0B+1G=0B.png",FileMode.Create); 
		BinaryWriter bw  = new BinaryWriter(fs1); 
		bw.Write( pngData,0,pngData.Length);   
		bw.Close();   
		fs1.Close(); 

		//Texture2D texture2D_b = getTexture2D ();
		//Texture2D texture2D_a = getTexture2D ();
	}
	[MenuItem(@"TerrainTools/SplatMapCombineRGBA")]
	public static void SplatMapCombineRGBA () {
		
		Texture2D texture2DSave = new Texture2D (512,512, TextureFormat.ARGB32,false);
		Texture2D texture2D_R = getTexture2D ("D://CombineRGBA//mySplatAlphaMap_0_R.png");
		Texture2D texture2D_G = getTexture2D ("D://CombineRGBA//mySplatAlphaMap_0_G.png");
		Texture2D texture2D_B = getTexture2D ("D://CombineRGBA//mySplatAlphaMap_0_B.png");
		Texture2D texture2D_A = getTexture2D ("D://CombineRGBA//mySplatAlphaMap_0_A.png");

		//Texture2D_save = CombineTexture (texture2D_0,texture2D_1,texture2D_0,C.b,C.g,C.b); 
		Color setColor = new Color (0,0,0,0);
		for (int i = 0; i < texture2DSave.width; i++) {
			for (int j = 0; j < texture2DSave.width; j++) {
				setColor.r = texture2D_R.GetPixel (i, j).r;
				setColor.g = texture2D_G.GetPixel (i, j).g;
				setColor.b = texture2D_B.GetPixel (i, j).b;
				setColor.a = texture2D_A.GetPixel (i, j).a;
				texture2DSave.SetPixel (i,j,setColor);
			}
		}

		byte[] pngData = texture2DSave.EncodeToPNG ();
		//保存泼溅图
		FileStream   fs1 = new FileStream("D://CombineRGBA//CombineRGBA.png",FileMode.Create); 
		BinaryWriter bw  = new BinaryWriter(fs1); 
		bw.Write( pngData,0,pngData.Length);   
		bw.Close();   
		fs1.Close(); 
	}

	public static Texture2D CombineTexture (Texture2D textureSource0,
									   Texture2D textureSource1,
		                               Texture2D textureTarget,
										C source0, C source1,C target) {
		Color setColor = new Color (0,0,0,0);
		float setSource0 = 0;
		float setSource1 = 0;
		//float setB0 = 0;
		//float setA0 = 0;
		//float setR1 = 0;
		//float setG1 = 0;
		//float setB1 = 0;
		//float setA1 = 0;

		for(int i = 0 ; i<textureSource0.width ; i++){
			for(int j = 0 ; j<textureSource0.width ; j++){
				setColor = textureTarget.GetPixel(i,j);
				switch (source0) {
				case C.r:setSource0 = textureSource0.GetPixel (i, j).r; break;
				case C.g:setSource0 = textureSource0.GetPixel (i, j).g; break;
				case C.b:setSource0 = textureSource0.GetPixel (i, j).b; break;
				case C.a:setSource0 = textureSource0.GetPixel (i, j).a; break;
				}
				switch (source1) {
				case C.r:
					setSource1 = textureSource1.GetPixel (i, j).r;
					break;
				case C.g: setSource1 = textureSource1.GetPixel (i, j).g; break;
				case C.b: setSource1 = textureSource1.GetPixel (i, j).b; break;
				case C.a: setSource1 = textureSource1.GetPixel (i, j).a; break;
					
				}
				switch (target) {
				case C.r:setColor.r = setSource0 + setSource1;
					break;
				case C.g:setColor.g = setSource0 + setSource1; break;
				case C.b:setColor.b = setSource0 + setSource1; break;
				case C.a:setColor.a = setSource0 + setSource1; break;

				}

				textureTarget.SetPixel (i,j,setColor);
			}
		}
		return textureTarget;
	}

	public static Texture2D getTexture2D(string fileName = "D:/mySplatAlphaMap_",int width=512,int height=512){

		FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
		fileStream.Seek(0, SeekOrigin.Begin);
		//创建文件长度缓冲区
		byte[] bytes = new byte[fileStream.Length]; 
		//读取文件
		fileStream.Read(bytes, 0, (int)fileStream.Length);
		//释放文件读取流
		fileStream.Close();
		fileStream.Dispose();
		fileStream = null;
		//创建Texture
		//int width=512;
		//int height=512;
		Texture2D texture = new Texture2D(width, height);
		texture.LoadImage(bytes);

		return texture;
	}
}
