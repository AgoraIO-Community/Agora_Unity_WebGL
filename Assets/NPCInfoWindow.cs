using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class NPCInfoWindow : MonoBehaviour
{
    public List<NPCInfo> NPCInfos;
    public NPCInfo playerInfo;
    public List<GameObject> npcs;
    public List<SpatialAudioDemoManager.NPCEffectParams> effectParams;
    public static NPCInfoWindow instance;
    public GameObject panel;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && panel.activeInHierarchy)
        {
            
        }
    }

    public void updateNPC(SpatialAudioDemoManager.NPCEffectParams npcParams, Transform t)
    {
        for (int x = 0; x < NPCInfos.Count; x++)
        {
            if (NPCInfos[x].info.name == t.gameObject.name)
            {
                NPCInfos[x].info.name = t.gameObject.name;
                NPCInfos[x].info.attenuation = (float)npcParams.attenuation;
                NPCInfos[x].info.position = t.position;
                NPCInfos[x].info.forward = t.forward;
                NPCInfos[x].info.right = t.right;
                NPCInfos[x].info.top = t.up;
            }
        }
    }

}
