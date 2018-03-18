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
* Author  :To Harden The Mind
* Time    :2018/3/18 15:29:52
* FileName:TestEmoji
* Purpose :
* ==============================================================================		   
*/
using System;
using System.Collections.Generic;
using UnityEngine;

public class TestEmoji : MonoBehaviour
{
    public MikuAtlas atlas;
    public Font font;
    public string str = "He#000llo#001World";
    public int fontSize = 40;
    public Color fontColor = Color.white;
    public GameObject picGo;

    Mesh mesh;
    Mesh picMesh;

    void OnFontTextureRebuilt(Font changedFont)
    {
        if (changedFont != font)
            return;

        RebuildMesh();
    }

    void OnValidate()
    {
        if (!Application.isPlaying) return;
        RebuildMesh();
    }

    void RebuildMesh()
    {
        if (mesh == null) return;

        font.RequestCharactersInTexture(str, fontSize);
        mesh.Clear();
        picMesh.Clear();

        // Generate a mesh for the characters we want to print.
        List<Vector3> fontVertices = new List<Vector3>();
        List<int> fontTriangles = new List<int>();
        List<Vector2> fontUV = new List<Vector2>();
        List<Color> fontColors = new List<Color>();

        List<Vector3> picVertices = new List<Vector3>();
        List<Vector2> picUV = new List<Vector2>();
        List<int> picTriangles = new List<int>();

        //这里是真正的字
        DrawText(fontVertices, fontTriangles, fontUV, fontColors, fontColor, picVertices, picUV, picTriangles);

        mesh.vertices = fontVertices.ToArray();
        mesh.triangles = fontTriangles.ToArray();
        mesh.uv = fontUV.ToArray();
        mesh.colors = fontColors.ToArray();

        picMesh.vertices = picVertices.ToArray();
        picMesh.triangles = picTriangles.ToArray();
        picMesh.uv = picUV.ToArray();
    }

    void DrawText(List<Vector3> fontVertices, List<int> fontTriangles, List<Vector2> fontUV, List<Color> fontColorList, Color fontColor,
        List<Vector3> picVertices, List<Vector2> picUV, List<int> picTriangles)
    {

        Vector3 pos = Vector3.zero;
        int index = 0;
        int picIndex = 0;
        for (int i = 0; i < str.Length;)
        {
            char c = str[i];
            if (c == '#'
                && Char.IsNumber(str[i + 1])
                && Char.IsNumber(str[i + 2])
                && Char.IsNumber(str[i + 3])
                )
            {
                string name = str.Substring(i+1, 3);
                if (atlas != null)
                {
                    MikuSpriteData spriteInfo = atlas.GetSpriteDataByName(name);
                    Rect outer = new Rect(spriteInfo.x, spriteInfo.y, spriteInfo.width, spriteInfo.height);
                    Texture tex = atlas.spriteMaterial.mainTexture;
                    Rect outerUV = MikuSpriteData.ConvertToTexCoords(outer, tex.width, tex.height);

                    picVertices.Add(pos + new Vector3(0, spriteInfo.height, 0));
                    picVertices.Add(pos + new Vector3(spriteInfo.width, spriteInfo.height, 0));
                    picVertices.Add(pos + new Vector3(spriteInfo.width, 0, 0));
                    picVertices.Add(pos + new Vector3(0, 0, 0));

                    picUV.Add(new Vector2(outerUV.xMin, outerUV.yMax));
                    picUV.Add(new Vector2(outerUV.xMax, outerUV.yMax));
                    picUV.Add(new Vector2(outerUV.xMax, outerUV.yMin));// ch.uvBottomRight);
                    picUV.Add(new Vector2(outerUV.xMin, outerUV.yMin));//ch.uvBottomLeft);

                    picTriangles.Add(4 * picIndex + 0);
                    picTriangles.Add(4 * picIndex + 1);
                    picTriangles.Add(4 * picIndex + 2);
                    picTriangles.Add(4 * picIndex + 0);
                    picTriangles.Add(4 * picIndex + 2);
                    picTriangles.Add(4 * picIndex + 3);

                    // Advance character position
                    pos += new Vector3(spriteInfo.width, 0, 0);
                }
               
                i = i + 4;
                picIndex++;
            }
            else
            {
                // Get character rendering information from the font
                CharacterInfo ch;
                font.GetCharacterInfo(c, out ch, fontSize);

                fontVertices.Add(pos + new Vector3(ch.minX, ch.maxY, 0));
                fontVertices.Add(pos + new Vector3(ch.maxX, ch.maxY, 0));
                fontVertices.Add(pos + new Vector3(ch.maxX, 0, 0));
                fontVertices.Add(pos + new Vector3(ch.minX, 0, 0));

                fontUV.Add(ch.uvTopLeft);
                fontUV.Add(ch.uvTopRight);
                fontUV.Add(ch.uvBottomRight);
                fontUV.Add(ch.uvBottomLeft);

                fontColorList.Add(fontColor);
                fontColorList.Add(fontColor);
                fontColorList.Add(fontColor);
                fontColorList.Add(fontColor);

                fontTriangles.Add(4 * index + 0);
                fontTriangles.Add(4 * index + 1);
                fontTriangles.Add(4 * index + 2);
                fontTriangles.Add(4 * index + 0);
                fontTriangles.Add(4 * index + 2);
                fontTriangles.Add(4 * index + 3);

                // Advance character position
                pos += new Vector3(ch.advance, 0, 0);
                index++;
                i++;
            }
            
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

        picMesh = new Mesh();
        picGo.GetComponent<MeshFilter>().mesh = picMesh;

        // Generate font mesh.
        RebuildMesh();
    }

    void OnDestroy()
    {
        Font.textureRebuilt -= OnFontTextureRebuilt;
    }

}

