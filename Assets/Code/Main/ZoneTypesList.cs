using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewZoneTypesList.asset", menuName = "ZoneTypesList")]
[System.Serializable]
public class ZoneTypesList : ScriptableObject {
    /* ZoneTypes is a scriptableObject made to be saved and referenced in any implementation needed.
     * It simply a replacement dictionary that allows modification in the inspector.
     */
    public List<ZoneType> zoneTypes;
}

[System.Serializable]
public class ZoneType {
    public string name = "Unspecified"; //Unspecified by default. This is so that we can determine which zones have been iterated over during world-gen.
    public float priority = 1.0; //ZoneType's Priority.
}
