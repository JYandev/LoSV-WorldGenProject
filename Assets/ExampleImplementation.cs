using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleImplementation : MonoBehaviour {
    /* This is an example implementation of the code structures found in "Assets/Code/".
     * When the Generate World button is pressed, a new WorldData will be created, filled with zones, and then saved to JSON to this project's DataPath.
     * When the Load World button is pressed, this script's currentWorldData will be filled with the deserialized JSON.
     */

    private WorldData currentWorldData;

    // --- [Manually Set References] ---
    [SerializeField]
    private ZoneTypesList zoneTypesList;

    // --- [Cached Zone Prefab References] ---
    private ZoneList mandatoryZoneList;
    private ZoneList fillerZoneList;
    private ZoneList uniqueZoneList;
    //

    #region --- [Button Event Receivers] ---
    public void OnButton_GenerateWorld () {
        throw new System.NotImplementedException();
    }
    public void OnButton_LoadWorld () {
        throw new System.NotImplementedException();
    }
    #endregion

    #region --- [Utilities] ---
    void SaveCurrentWorldDataToFile () {
        /* Saves the currentWorldData to a file in Application.dataPath.
         */
        
    }
    #endregion
}
