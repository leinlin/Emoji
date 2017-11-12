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
* Filename: TestSprite
* Created:  2017/11/12 22:15:43
* Author:   HaYaShi ToShiTaKa
* Purpose:  
* ==============================================================================
*/
using UnityEngine;

public class TestSprite : MonoBehaviour {
    public int width = 220;
    public int height = 102;
    public Material spriteMaterial;
    public Vector2 uv1 = new Vector2(0, 0f);
    public Vector2 uv2 = new Vector2(0, 0.5f);
    public Vector2 uv3 = new Vector2(1, 0);
    public Vector2 uv4 = new Vector2(1, 0.5f);

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    void Start() {
        //得到MeshFilter对象//  
        meshFilter = gameObject.GetComponent<MeshFilter>();
        meshRenderer = null;
        if (meshFilter == null) {
            //为null时，自动添加//  
            meshFilter = gameObject.AddComponent<MeshFilter>();
            meshRenderer = gameObject.AddComponent<MeshRenderer>();
            meshRenderer.sharedMaterial = spriteMaterial;
        }
        Fill();
    }

    void OnValidate() {
        if (meshFilter && Application.isPlaying) {
            Fill();
        }
    }

    void Fill() {
        //得到对应的网格对象//  
        Mesh mesh = new Mesh();
        meshFilter.mesh = mesh;

        //三角形顶点的坐标数组//  
        Vector3[] vertices = new Vector3[4];
        //三角形顶点数组//  
        int[] triangles = new int[6];
        //颜色数组//
        Color[] colors = new Color[4];
        //uv贴图坐标//
        Vector2[] uv = new Vector2[4];

        float glWidth = (float)width / 2;
        float glHeight = (float)height / 2;
        //以当前对象的中心坐标为标准//  
        vertices[0] = new Vector3(-glWidth, -glHeight, 0);
        vertices[1] = new Vector3(-glWidth, glHeight, 0);
        vertices[2] = new Vector3(glWidth, -glHeight, 0);
        vertices[3] = new Vector3(glWidth, glHeight, 0);

        //绑定顶点顺序//  
        triangles[0] = 0;
        triangles[1] = 2;
        triangles[2] = 1;
        triangles[3] = 2;
        triangles[4] = 3;
        triangles[5] = 1;

        //设置顶点颜色//
        colors[0] = Color.white;
        colors[1] = Color.white;
        colors[2] = Color.white;
        colors[3] = Color.white;

        //绑定贴图UV//
        uv[0] = uv1;
        uv[1] = uv2;
        uv[2] = uv3;
        uv[3] = uv4;

        //给mesh赋值//
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.colors = colors;
        mesh.uv = uv;


    }
}