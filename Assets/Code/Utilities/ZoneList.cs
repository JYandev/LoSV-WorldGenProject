using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewZoneList", menuName = "ZoneList", order = 1)]
[System.Serializable]
public class ZoneList : ScriptableObject {
    /* This scriptable object is made to store a chunk of prefab references to be loaded whenever this object is loaded.
     * This is used mainly for organization purposes and to keep the file structure clean.
     * We will also use our reference list as the container to iterate on when searching through all Possible zones for a ZoneType.
     * In order to be used in world gen, the ZoneList must be given a special name. The prefix consists of the ZoneType name, which is then connected to the suffix by an "_". 
     * - The suffix has to be called either "Unique", "Mandatory", or "Filler"; the actual name will vary depending on purpose and ZoneType, but will closely resemble something like "Grasslands_Unique".
     */

    public List<GameObject> zonePrefabList;
}
