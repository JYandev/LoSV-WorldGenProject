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
    /* This class is used during world gen to determine a zone's "Biome" type and how fast that type spreads to other zones during the world gen process.
     */
    public string name = "Unspecified"; //Unspecified by default. This is so that we can determine which zones have been iterated over during world-gen.
    public float priority = 0.5f; //ZoneType's Priority.
}
