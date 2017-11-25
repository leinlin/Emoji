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
* Filename: MikuAtlas
* Created:  2017/11/26 1:29:36
* Author:   HaYaShi ToShiTaKa
* Purpose:  
* ==============================================================================
*/
using UnityEngine;
using System.Collections.Generic;

public class MikuAtlas : MonoBehaviour {
    [SerializeField]
    public Material spriteMaterial;
    [SerializeField]
    // 很不幸,Unity 不能序列化保存字典,搞个List保存一下，然后在运行的的初始化的时候搞成字典吧
    private List<MikuSpriteData> m_sprites = new List<MikuSpriteData>();
    [System.NonSerialized]
    private Dictionary<string, MikuSpriteData> m_spritesDict = new Dictionary<string, MikuSpriteData>();
    [System.NonSerialized]
    private bool m_inited = false;

    public List<MikuSpriteData> spriteList {
        get {
            return m_sprites;
        }
        set {
            m_sprites = value;
        }
    }

    public MikuSpriteData GetSpriteDataByName(string name) {
        MikuSpriteData result = null;

        if (!m_inited) {
            SlotDictData();
        }
        m_spritesDict.TryGetValue(name, out result);
        return result;
    }

    public void SlotDictData() {
        if (m_inited) return;

        for (int i = 0, imax = m_sprites.Count; i < imax; i++) {
            MikuSpriteData data = m_sprites[i];
            m_spritesDict[data.name] = data;
        }

        m_inited = true;
    }

}