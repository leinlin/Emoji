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
* Filename: MikuSpriteData
* Created:  2017/11/26 1:23:55
* Author:   HaYaShi ToShiTaKa
* Purpose:  
* ==============================================================================
*/

using UnityEngine;

[System.Serializable]
public class MikuSpriteData {
    public string name = "Sprite";
    public int x = 0;
    public int y = 0;
    public int width = 0;
    public int height = 0;

    public int borderLeft = 0;
    public int borderRight = 0;
    public int borderTop = 0;
    public int borderBottom = 0;

    public int paddingLeft = 0;
    public int paddingRight = 0;
    public int paddingTop = 0;
    public int paddingBottom = 0;

    public bool rotated = false;


    /// <summary>
    /// Whether the sprite has a border.
    /// </summary>

    public bool hasBorder { get { return (borderLeft | borderRight | borderTop | borderBottom) != 0; } }

    /// <summary>
    /// Whether the sprite has been offset via padding.
    /// </summary>

    public bool hasPadding { get { return (paddingLeft | paddingRight | paddingTop | paddingBottom) != 0; } }

    /// <summary>
    /// Convenience function -- set the X, Y, width, and height.
    /// </summary>

    public void SetRect(int x, int y, int width, int height) {
        this.x = x;
        this.y = y;
        this.width = width;
        this.height = height;
    }

    /// <summary>
    /// Convenience function -- set the sprite's padding.
    /// </summary>

    public void SetPadding(int left, int bottom, int right, int top) {
        paddingLeft = left;
        paddingBottom = bottom;
        paddingRight = right;
        paddingTop = top;
    }

    /// <summary>
    /// Convenience function -- set the sprite's border.
    /// </summary>

    public void SetBorder(int left, int bottom, int right, int top) {
        borderLeft = left;
        borderBottom = bottom;
        borderRight = right;
        borderTop = top;
    }

    static public Rect ConvertToTexCoords(Rect rect, int width, int height) {
        Rect final = rect;

        if (width != 0f && height != 0f) {
            final.xMin = rect.xMin / width;
            final.xMax = rect.xMax / width;
            final.yMin = 1f - rect.yMax / height;
            final.yMax = 1f - rect.yMin / height;
        }
        return final;
    }
}