using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewZoneTypesList.asset", menuName = "ZoneTypesList")]
[System.Serializable]
public class ZoneTypesList : ScriptableObject {
    /* ZoneTypes is a scriptableObject made to be saved and referenced in any implementation needed.
     * It simply a replacement dictionary that allows modification in the inspector.
     */
    [System.Serializable]
    public class ZoneType {
        public string name;
        public float priority; //ZoneType's Priority.
    }

    public List<ZoneType> zoneTypes;
}
