using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class NPCInfo : MonoBehaviour
{
    [SerializeField]
    public npcInfo info;

    public Text name, attenuation, position, forward, right, top;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        name.text = info.name;
        attenuation.text = info.attenuation.ToString();
        position.text = "(" + (info.position.x).ToString() + ", " + ((int)info.position.y).ToString() + ", " + ((int)info.position.z).ToString() + ")";
        forward.text = "(" + ((int)info.forward.x).ToString() + ", " + ((int)info.forward.y).ToString() + ", " + ((int)info.forward.z).ToString() + ")";
        right.text = "(" + ((int)info.right.x).ToString() + ", " + ((int)info.right.y).ToString() + ", " + ((int)info.right.z).ToString() + ")";
        top.text = "(" + ((int)info.top.x).ToString() + ", " + ((int)info.top.y).ToString() + ", " + ((int)info.top.z).ToString() + ")";
    }

    [Serializable]
    public struct npcInfo {
        public string name;
        public float attenuation;
        public Vector3 position, forward, right, top;
    }
}
