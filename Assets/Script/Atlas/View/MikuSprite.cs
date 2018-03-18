/*
               #########                       
              ############                     
              #############                    
             ##  ###########                   
            ###  ###### #####                  
            ### #######   ####                 
           ###  ########## ####                
          ####  ########### ####               
         ####   ###########  #####             
        #####   ### ########   #####           
       #####   ###   ########   ######         
      ######   ###  ###########   ######       
     ######   #### ##############  ######      
    #######  #####################  ######     
    #######  ######################  ######    
   #######  ###### #################  ######   
   #######  ###### ###### #########   ######   
   #######    ##  ######   ######     ######   
   #######        ######    #####     #####    
    ######        #####     #####     ####     
     #####        ####      #####     ###      
      #####       ###        ###      #        
        ###       ###        ###               
         ##       ###        ###               
__________#_______####_______####______________

                我们的未来没有BUG              
* ==============================================================================
* Filename: MikuSprite
* Created:  2017/11/26 1:22:54
* Author:   HaYaShi ToShiTaKa
* Purpose:  
* ==============================================================================
*/
using System.Collections.Generic;
using UnityEngine;

public class MikuSprite : MonoBehaviour {
    public int width = 220;
    public int height = 102;

    public MikuAtlas atlas;
    public string spriteName = "cancel";
    private MikuSpriteData m_sprite;

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    Rect m_outerUV = new Rect();
    const int maxIndexBufferCache = 10;
    static List<int[]> mCache = new List<int[]>(maxIndexBufferCache);

    List<Vector3> vertices = new List<Vector3>();
    List<Vector2> uv = new List<Vector2>();

    Vector4 drawingUVs {
        get {
            return new Vector4(m_outerUV.xMin, m_outerUV.yMin, m_outerUV.xMax, m_outerUV.yMax);
        }
    }
    Vector4 drawingDimensions {
        get {
            float halfWidth = 0.5f * width;
            float halfHeight = 0.5f * height;

            return new Vector4(-halfWidth, -halfHeight, halfWidth, halfHeight);
        }
    }

    void Start() {
        //得到MeshFilter对象//  
        meshFilter = gameObject.GetComponent<MeshFilter>();
        meshRenderer = null;
        if (meshFilter == null) {
            //为null时，自动添加//  
            meshFilter = gameObject.AddComponent<MeshFilter>();
            meshRenderer = gameObject.AddComponent<MeshRenderer>();
            if (atlas != null) {
                meshRenderer.sharedMaterial = atlas.spriteMaterial;
            }
        }
        Fill();
    }

    void OnValidate() {
        if (meshFilter && Application.isPlaying) {
            Fill();
        }
    }

    void Fill() {
        if (atlas == null) return;
        if (string.IsNullOrEmpty(spriteName)) return;
        m_sprite = atlas.GetSpriteDataByName(spriteName);

        CalUV();

        Vector4 u = drawingUVs;
        Vector4 v = drawingDimensions;

        //得到对应的网格对象//  
        Mesh mesh = new Mesh();
        meshFilter.mesh = mesh;

        //三角形顶点的坐标数组//  
        vertices.Clear();
        //uv贴图坐标//
        uv.Clear();

        //三角形顶点数组//  
        int[] triangles = GenerateCachedIndexBuffer(4, 6);
        //颜色数组//
        Color[] colors = new Color[4];

        vertices.Add(new Vector3(v.x, v.y, 0));
        vertices.Add(new Vector3(v.x, v.w, 0));
        vertices.Add(new Vector3(v.z, v.w, 0));
        vertices.Add(new Vector3(v.z, v.y, 0));

        //设置顶点颜色//
        colors[0] = Color.white;
        colors[1] = Color.white;
        colors[2] = Color.white;
        colors[3] = Color.white;

        //绑定贴图UV//
        uv.Add(new Vector2(u.x, u.y));
        uv.Add(new Vector2(u.x, u.w));
        uv.Add(new Vector2(u.z, u.w));
        uv.Add(new Vector2(u.z, u.y));


        //给mesh赋值//
        mesh.SetVertices(vertices);
        mesh.triangles = triangles;
        mesh.colors = colors;
        mesh.SetUVs(0, uv);
    }

    protected int[] GenerateCachedIndexBuffer(int vertexCount, int indexCount) {
        for (int i = 0, imax = mCache.Count; i < imax; ++i) {
            int[] ids = mCache[i];
            if (ids != null && ids.Length == indexCount)
                return ids;
        }

        int[] rv = new int[indexCount];
        int index = 0;

        for (int i = 0; i < vertexCount; i += 4) {
            rv[index++] = i;
            rv[index++] = i + 1;
            rv[index++] = i + 2;

            rv[index++] = i + 2;
            rv[index++] = i + 3;
            rv[index++] = i + 0;
        }

        if (mCache.Count > maxIndexBufferCache) mCache.RemoveAt(0);
        mCache.Add(rv);
        return rv;
    }


    private void CalUV() {
        Rect outer = new Rect(m_sprite.x, m_sprite.y, m_sprite.width, m_sprite.height);
        Texture tex = atlas.spriteMaterial.mainTexture;
        m_outerUV = MikuSpriteData.ConvertToTexCoords(outer, tex.width, tex.height);
    }

}