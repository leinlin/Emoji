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
* Filename: MikuAtlasInspector
* Created:  2017/11/26 1:26:14
* Author:   HaYaShi ToShiTaKa
* Purpose:  解析 TextruePacker的uv 信息
* ==============================================================================
*/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MikuAtlas))]
public class MikuAtlasInspector : Editor {

    MikuAtlas mAtlas;

    public override void OnInspectorGUI() {
        mAtlas = target as MikuAtlas;

        mAtlas.spriteMaterial = EditorGUILayout.ObjectField("material", mAtlas.spriteMaterial, typeof(Material), true) as Material;
        TextAsset ta = EditorGUILayout.ObjectField("TP Import", null, typeof(TextAsset), false) as TextAsset;
        if (ta != null) {
            LoadSpriteData(mAtlas, ta);
        }
    }

     static public void LoadSpriteData(MikuAtlas atlas, TextAsset asset) {
        if (asset == null || atlas == null) return;

        string jsonString = asset.text;
        Hashtable decodedHash = MikuJson.jsonDecode(jsonString) as Hashtable;

        if (decodedHash == null) {
            Debug.LogWarning("Unable to parse Json file: " + asset.name);
        }
        else {
            LoadSpriteData(atlas, decodedHash);
        }

        asset = null;
        Resources.UnloadUnusedAssets();
    }

    /// <summary>
    /// Parse the specified JSon file, loading sprite information for the specified atlas.
    /// </summary>

    static void LoadSpriteData(MikuAtlas atlas, Hashtable decodedHash) {
        if (decodedHash == null || atlas == null) return;
        List<MikuSpriteData> oldSprites = atlas.spriteList;
        atlas.spriteList = new List<MikuSpriteData>();

        Hashtable frames = (Hashtable)decodedHash["frames"];

        foreach (DictionaryEntry item in frames) {
            MikuSpriteData newSprite = new MikuSpriteData();
            newSprite.name = item.Key.ToString();

            bool exists = false;

            // Check to see if this sprite exists
            foreach (MikuSpriteData oldSprite in oldSprites) {
                if (oldSprite.name.Equals(newSprite.name, StringComparison.OrdinalIgnoreCase)) {
                    exists = true;
                    break;
                }
            }

            // Get rid of the extension if the sprite doesn't exist
            // The extension is kept for backwards compatibility so it's still possible to update older atlases.
            if (!exists) {
                newSprite.name = newSprite.name.Replace(".png", "");
                newSprite.name = newSprite.name.Replace(".tga", "");
            }

            // Extract the info we need from the TexturePacker json file, mainly uvRect and size
            Hashtable table = (Hashtable)item.Value;
            Hashtable frame = (Hashtable)table["frame"];

            int frameX = int.Parse(frame["x"].ToString());
            int frameY = int.Parse(frame["y"].ToString());
            int frameW = int.Parse(frame["w"].ToString());
            int frameH = int.Parse(frame["h"].ToString());

            // [Modify] by maosongliang, begin
            // Read the rotation value
            newSprite.rotated = (bool)table["rotated"];
            // [Modify] by maosongliang, end

            newSprite.x = frameX;
            newSprite.y = frameY;
            newSprite.width = frameW;
            newSprite.height = frameH;

            // Support for trimmed sprites
            Hashtable sourceSize = (Hashtable)table["sourceSize"];
            Hashtable spriteSize = (Hashtable)table["spriteSourceSize"];

            if (spriteSize != null && sourceSize != null) {
                // TODO: Account for rotated sprites
                if (frameW > 0) {
                    int spriteX = int.Parse(spriteSize["x"].ToString());
                    int spriteW = int.Parse(spriteSize["w"].ToString());
                    int sourceW = int.Parse(sourceSize["w"].ToString());

                    newSprite.paddingLeft = spriteX;
                    newSprite.paddingRight = sourceW - (spriteX + spriteW);
                }

                if (frameH > 0) {
                    int spriteY = int.Parse(spriteSize["y"].ToString());
                    int spriteH = int.Parse(spriteSize["h"].ToString());
                    int sourceH = int.Parse(sourceSize["h"].ToString());

                    newSprite.paddingTop = spriteY;
                    newSprite.paddingBottom = sourceH - (spriteY + spriteH);
                }
            }

            // [Modify] by maosongliang, begin
            if (newSprite.rotated) {
                int temp = newSprite.width;
                newSprite.width = newSprite.height;
                newSprite.height = temp;

                temp = newSprite.paddingLeft;
                newSprite.paddingLeft = newSprite.paddingTop;
                newSprite.paddingTop = temp;

                temp = newSprite.paddingRight;
                newSprite.paddingRight = newSprite.paddingBottom;
                newSprite.paddingBottom = temp;
            }
            // [Modify] by maosongliang, end

            // If the sprite was present before, see if we can copy its inner rect
            foreach (MikuSpriteData oldSprite in oldSprites) {
                if (oldSprite.name.Equals(newSprite.name, StringComparison.OrdinalIgnoreCase)) {
                    if (oldSprite.rotated != newSprite.rotated) {
                        // ---modify=> by zengyi

                        // left top changed
                        if (oldSprite.rotated) {
                            newSprite.borderLeft = oldSprite.borderTop;
                            newSprite.borderTop = oldSprite.borderRight;
                            newSprite.borderBottom = oldSprite.borderLeft;
                            newSprite.borderRight = oldSprite.borderBottom;
                        }
                        else {
                            newSprite.borderLeft = oldSprite.borderTop;
                            newSprite.borderTop = oldSprite.borderLeft;
                            // right bottom changed
                            newSprite.borderRight = oldSprite.borderBottom;
                            newSprite.borderBottom = oldSprite.borderRight;
                        }

                        //-----------------------
                    }
                    else {
                        newSprite.borderLeft = oldSprite.borderLeft;
                        newSprite.borderRight = oldSprite.borderRight;
                        newSprite.borderBottom = oldSprite.borderBottom;
                        newSprite.borderTop = oldSprite.borderTop;
                    }

                    break;
                }
            }

            // Add this new sprite
            atlas.spriteList.Add(newSprite);
        }

        // Sort imported sprites alphabetically
        atlas.spriteList.Sort(CompareSprites);
        Debug.Log("Imported " + atlas.spriteList.Count + " sprites");
    }

    /// <summary>
    /// Sprite comparison function for sorting.
    /// </summary>

    static int CompareSprites(MikuSpriteData a, MikuSpriteData b) { return a.name.CompareTo(b.name); }

}