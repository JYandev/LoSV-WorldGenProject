﻿using UnityEngine;

[CreateAssetMenu(fileName = "NewZoneList", menuName = "ResourceList", order = 1)]
[System.Serializable]
public class ZoneList : ScriptableObject {
    /* This scriptable object is made to store a chunk of prefab references to be loaded whenever this object is loaded.
     * This is used mainly for organization purposes and to keep the file structure clean.
     * We will also use our reference list as the container to iterate on when searching through all Possible zones for a ZoneType.
     */

    public GameObject[] referenceList;
}