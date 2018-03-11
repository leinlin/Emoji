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
* Filename: TestFont
* Created:  2017/11/12 22:15:43
* Author:   HaYaShi ToShiTaKa
* Purpose:  
* ==============================================================================
*/

using UnityEngine;
using System.Collections.Generic;

public class TestFont : MonoBehaviour
{
    public Font font;
    public string str = "Hello World";
    public int fontSize = 40;
    public float outWidth = 5;
    public Color fontColor = Color.white;
    public Color outColor = Color.black;
    Mesh mesh;

    void OnFontTextureRebuilt(Font changedFont)
    {
        if (changedFont != font)
            return;

        RebuildMesh();
    }

    void OnValidate() {
        if (!Application.isPlaying) return;
        RebuildMesh();
    }

    void RebuildMesh()
    {
        if (mesh == null) return;

        font.RequestCharactersInTexture(str, fontSize);
        mesh.Clear();
        // Generate a mesh for the characters we want to print.
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Vector2> uv = new List<Vector2>();
        List<Color> color = new List<Color>();

        //这里是描边
        DrawText(vertices, triangles, uv, color, outColor, new Vector3(outWidth,0,0), 0);
        DrawText(vertices, triangles, uv, color, outColor, new Vector3(-outWidth,0,0), 1);
        DrawText(vertices, triangles, uv, color, outColor, new Vector3(0, outWidth, 0), 2);
        DrawText(vertices, triangles, uv, color, outColor, new Vector3(0, -outWidth, 0), 3);
        DrawText(vertices, triangles, uv, color, outColor, new Vector3(outWidth, outWidth, 0), 4);
        DrawText(vertices, triangles, uv, color, outColor, new Vector3(outWidth, -outWidth, 0), 5);
        DrawText(vertices, triangles, uv, color, outColor, new Vector3(-outWidth, outWidth, 0), 6);
        DrawText(vertices, triangles, uv, color, outColor, new Vector3(-outWidth, -outWidth, 0), 7);

        //这里是真正的字
        DrawText(vertices, triangles, uv, color, fontColor, Vector3.zero, 8);

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uv.ToArray();
        mesh.colors = color.ToArray();
    }

    void DrawText(List<Vector3> vertices, List<int> triangles, List<Vector2> uv, List<Color> colorList, Color color, Vector3 offset, int index) {

        Vector3 pos = Vector3.zero - offset;
        for (int i = 0; i < str.Length; i++)
        {
            // Get character rendering information from the font
            CharacterInfo ch;
            font.GetCharacterInfo(str[i], out ch, fontSize);

            vertices.Add(pos + new Vector3(ch.minX, ch.maxY, 0));
            vertices.Add(pos + new Vector3(ch.maxX, ch.maxY, 0));
            vertices.Add(pos + new Vector3(ch.maxX, ch.minY, 0));
            vertices.Add(pos + new Vector3(ch.minX, ch.minY, 0));

            colorList.Add(color);
            colorList.Add(color);
            colorList.Add(color);
            colorList.Add(color);

            uv.Add(ch.uvTopLeft);
            uv.Add(ch.uvTopRight);
            uv.Add(ch.uvBottomRight);
            uv.Add(ch.uvBottomLeft);

            triangles.Add(4 * (i + index * str.Length) + 0);
            triangles.Add(4 * (i + index * str.Length) + 1);
            triangles.Add(4 * (i + index * str.Length) + 2);

            triangles.Add(4 * (i + index * str.Length) + 0);
            triangles.Add(4 * (i + index * str.Length) + 2);
            triangles.Add(4 * (i + index * str.Length) + 3);

            // Advance character position
            pos += new Vector3(ch.advance, 0, 0);
        }
    }

    void Start()
    {
        //font = Font.CreateDynamicFontFromOSFont("Helvetica", 16);
        // Set the rebuild callback so that the mesh is regenerated on font changes.
        Font.textureRebuilt += OnFontTextureRebuilt;

        // Set up mesh.
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshRenderer>().material = font.material;

        // Generate font mesh.
        RebuildMesh();
    }

    void OnDestroy()
    {
        Font.textureRebuilt -= OnFontTextureRebuilt;
    }
}