﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding.Serialization.JsonFx;
using System.IO;

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
    [SerializeField]
    private ProgressBar progressBar;
    [SerializeField]
    private Transform tileContainer;

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
        StartCoroutine(GenerateWorld());
    }
    public void OnButton_SaveWorld () {
        if (currentWorldData != null) {
            string newPath = UnityEditor.EditorUtility.SaveFilePanel("Save World to .JSON file", Application.dataPath, "World", "JSON"); //Only works in editor.
            string data = JsonWriter.Serialize(currentWorldData);
            StreamWriter streamWriter = new StreamWriter(newPath);
            streamWriter.Write(data);
            streamWriter.Close();
        }
        else {
            throw new System.Exception("World must be generated in order to save");
        }
    }
    public void OnButton_LoadWorld () {
        string newPath = UnityEditor.EditorUtility.OpenFilePanel("Open WorldData .JSON file", Application.dataPath, "JSON"); //Only works in editor.
        StreamReader streamReader = new StreamReader(newPath);
        string data = streamReader.ReadToEnd();
        streamReader.Close();
        currentWorldData = JsonReader.Deserialize<WorldData>(data);
        DisplayWorld();
    }
    #endregion

    #region --- [Utilities] ---
    void SaveCurrentWorldDataToFile () {
        /* Saves the currentWorldData to a file in Application.dataPath.
         */
        
    }
    #endregion

    #region --- [Main Example Functionality] ---
    private IEnumerator GenerateWorld () {
        yield return WorldManager.GenerateWorld(zoneTypesList, mandatoryZoneList, fillerZoneList, uniqueZoneList, progressBar);
        currentWorldData = WorldManager.getWorld();
        DisplayWorld();
    }

    private void DisplayWorld () {
        if (tileContainer.childCount > 0) {
            foreach (Transform child in tileContainer) {
                Destroy(child.gameObject); //Cleanup any old tiles.
            }
        }

        foreach (ZoneData zoneData in currentWorldData.w_Zones) {
            if (zoneData.z_ZoneFunction == ZoneFunction.Filler) {
                Instantiate(fillerZoneList[zoneData.z_ZoneType].zonePrefabList[zoneData.z_ZonePrefabIndex], new Vector3(zoneData.z_ZonePosition.x * zoneTileSize, 0, zoneData.z_ZonePosition.y * zoneTileSize), Quaternion.identity, tileContainer);
            }
            else if (zoneData.z_ZoneFunction == ZoneFunction.Mandatory) {
                Instantiate(mandatoryZoneList[zoneData.z_ZoneType].zonePrefabList[zoneData.z_ZonePrefabIndex], new Vector3(zoneData.z_ZonePosition.x * zoneTileSize, 0, zoneData.z_ZonePosition.y * zoneTileSize), Quaternion.identity, tileContainer);
            }
            else if (zoneData.z_ZoneFunction == ZoneFunction.Unique) {
                Instantiate(uniqueZoneList[zoneData.z_ZoneType].zonePrefabList[zoneData.z_ZonePrefabIndex], new Vector3(zoneData.z_ZonePosition.x * zoneTileSize, 0, zoneData.z_ZonePosition.y * zoneTileSize), Quaternion.identity, tileContainer);
            }
            else {
                //Debug.Log("Cannot display zones type of: " + zoneData.z_ZoneFunction);
            }
        }
    }
    #endregion
}
