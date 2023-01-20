using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AppIDInfo", menuName = "AppIDInfo", order = 0)]
public class AppInfoObject : ScriptableObject {
    public string appID, token, screenShareToken;
}


