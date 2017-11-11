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
* Filename: Black.cs
* Created:  2017/11/11 15:51:28
* Author:   To Hard The Mind
* Purpose:  
* ==============================================================================
*/
using System;
using System.Collections.Generic;
using UnityEngine;

public class Black : MonoBehaviour {
    public int width = 512;
    public int height = 512;
    public Color color = Color.black;

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    void Start() {
        Shader s = Shader.Find("Unlit/Transparent Colored");
        Material spriteMaterial = new Material(s);

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
        colors[0] = color;
        colors[1] = color;
        colors[2] = color;
        colors[3] = color;

        //绑定贴图UV//
        uv[0] = new Vector2(0, 0);
        uv[1] = new Vector2(0, 1);
        uv[2] = new Vector2(1, 0);
        uv[3] = new Vector2(1, 1);

        //给mesh赋值//
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.colors = colors;
        mesh.uv = uv;


    }
}