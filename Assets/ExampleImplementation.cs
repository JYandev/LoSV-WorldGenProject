using System.Collections.Generic;
using UnityEngine;

public class ExampleImplementation : MonoBehaviour {
    /* This is an example implementation of the code structures found in "Assets/Code/".
     * When the Generate World button is pressed, a new WorldData will be created, filled with zones, and then saved to JSON to this project's DataPath.
     * When the Load World button is pressed, this script's currentWorldData will be filled with the deserialized JSON.
     */

    private WorldData currentWorldData;

    // --- [Manually Set References and Options] ---
    [SerializeField]
    private ZoneTypesList zoneTypesList;
    [SerializeField]
    private float zoneTileSize = 1.0f; //Assuming the tile is a square, this is the length or width between tiles.

    // --- [Cached Zone Prefab References] ---
    private Dictionary<ZoneType, ZoneList> mandatoryZoneList;
    private Dictionary<ZoneType, ZoneList> fillerZoneList;
    private Dictionary<ZoneType, ZoneList> uniqueZoneList;

    #region --- [Initialization] ---
    private void Start () {
        mandatoryZoneList = new Dictionary<ZoneType, ZoneList>();
        fillerZoneList = new Dictionary<ZoneType, ZoneList>();
        uniqueZoneList = new Dictionary<ZoneType, ZoneList>();
        foreach (ZoneType zType in zoneTypesList.zoneTypes) {
            mandatoryZoneList.Add(zType, (ZoneList)(Resources.Load(zType.name + "_Mandatory") as ScriptableObject));
            fillerZoneList.Add(zType, (ZoneList)(Resources.Load(zType.name + "_Filler") as ScriptableObject));
            uniqueZoneList.Add(zType, (ZoneList)(Resources.Load(zType.name + "_Unique") as ScriptableObject));
        }
    } 
    #endregion

    #region --- [Button Event Receivers] ---
    public void OnButton_GenerateWorld () {
        GenerateWorld();
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

    #region --- [Main Example Functionality] ---
    private void GenerateWorld () {
        WorldData newWorldData = new WorldData();
        newWorldData.Generate(zoneTypesList, mandatoryZoneList, fillerZoneList, uniqueZoneList);

        currentWorldData = newWorldData;
        DisplayWorld();
    }

    private void DisplayWorld () {
        foreach (ZoneData zoneData in currentWorldData.w_Zones) {
            if (zoneData.z_ZoneFunction == ZoneFunction.Filler) {
                Instantiate(fillerZoneList[zoneData.z_ZoneType].zonePrefabList[zoneData.z_ZonePrefabIndex], new Vector3(zoneData.z_ZonePosition.x * zoneTileSize, 0, zoneData.z_ZonePosition.y * zoneTileSize), Quaternion.identity);
            }
            else if (zoneData.z_ZoneFunction == ZoneFunction.Mandatory) {
                Instantiate(mandatoryZoneList[zoneData.z_ZoneType].zonePrefabList[zoneData.z_ZonePrefabIndex], new Vector3(zoneData.z_ZonePosition.x * zoneTileSize, 0, zoneData.z_ZonePosition.y * zoneTileSize), Quaternion.identity);
            }
            else if (zoneData.z_ZoneFunction == ZoneFunction.Unique) {
                Instantiate(uniqueZoneList[zoneData.z_ZoneType].zonePrefabList[zoneData.z_ZonePrefabIndex], new Vector3(zoneData.z_ZonePosition.x * zoneTileSize, 0, zoneData.z_ZonePosition.y * zoneTileSize), Quaternion.identity);
            }
            else {
                //Debug.Log("Cannot display zones type of: " + zoneData.z_ZoneFunction);
            }
        }
    }
    #endregion
}
