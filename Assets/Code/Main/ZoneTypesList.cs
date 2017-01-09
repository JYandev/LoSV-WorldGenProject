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
     * When comparing ZoneTypes - especially to check if they equal - compare instead the ZoneType.name.
     */
    public string name = "Unspecified"; //Unspecified by default. This is so that we can determine which zones have been iterated over during world-gen.
    public float priority = 0.5f; //ZoneType's Priority.

    public override bool Equals (object obj) {
        if (obj == null || !(obj is ZoneType)) {
            return false;
        }
        else {
            return this.name == ((ZoneType)obj).name;
        }
    }

    public override int GetHashCode () {
        return this.name.GetHashCode();
    }
}
