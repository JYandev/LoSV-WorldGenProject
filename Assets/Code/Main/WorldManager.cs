using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class WorldManager : Singleton<WorldManager> {

    protected WorldManager () {} 

    public static IEnumerator GenerateWorld (ZoneTypesList zoneTypes, Dictionary<ZoneType, ZoneList> mandatoryZoneLibrary, Dictionary<ZoneType, ZoneList> fillerZoneLibrary, Dictionary<ZoneType, ZoneList> uniqueZoneLibrary, ProgressBar progressBar, float uniqueZoneChance = 0.5f, int length = 10, int width = 10) {
        yield return Instance.StartCoroutine(Instance.Generate(zoneTypes, mandatoryZoneLibrary, fillerZoneLibrary, uniqueZoneLibrary, progressBar, uniqueZoneChance, length, width));
    }

    public static WorldData getWorld () {
        return Instance.currentWorld;
    }

    #region --- [World-Gen Functions] ---
    private WorldData currentWorld;
    private ZoneData[,] currentZoneDataProcess;
    private ProgressBar currentProgressBar;
    private IEnumerator Generate (ZoneTypesList zoneTypes, Dictionary<ZoneType, ZoneList> mandatoryZoneLibrary, Dictionary<ZoneType, ZoneList> fillerZoneLibrary, Dictionary<ZoneType, ZoneList> uniqueZoneLibrary, ProgressBar progressBar, float uniqueZoneChance, int length, int width) {
        currentWorld = new WorldData();
        currentProgressBar = progressBar;



        // Create a 2D Array of blank zones, initialize starting points for each zoneType:
        if (currentProgressBar) {
            currentProgressBar.gameObject.SetActive(true);
            currentProgressBar.resetProgress();
            currentProgressBar.setText("...Initializing...");
        }
        currentZoneDataProcess = new ZoneData[length, width];
        yield return initZones(currentZoneDataProcess, zoneTypes);

        //Grow each zone type until no more moves can be made:
        if (currentProgressBar) {
            currentProgressBar.setProgress(1, 0);
            currentProgressBar.setText("...Growing ZoneTypes...");
        }
        yield return growZones(currentZoneDataProcess, zoneTypes);

        //Apply a randomized cutting of zones at the edges of the world to create a continental look:
        if (currentProgressBar) {
            currentProgressBar.setProgress(1, 1);
            currentProgressBar.setText("...Applying Continent Filter...");
        }
        yield return applyContinentFilter(currentZoneDataProcess, 1, 0.5f);

        //Assign MandatoryZones in their respective ZoneTypes, and if they do not exist, create them randomly.
        if (currentProgressBar) {
            currentProgressBar.setProgress(1, 2);
            currentProgressBar.setText("...Assigning Mandatory Zones...");
        }
        yield return assignMandatoryZones(currentZoneDataProcess, mandatoryZoneLibrary);

        //Fill in the rest of the Zones with FillerZones or UniqueZones based on their ZoneType while assigning their prefabIndex. 
        if (currentProgressBar) {
            currentProgressBar.setProgress(1, 3);
            currentProgressBar.setText("...Finishing...");
        }
        yield return indexZones(currentZoneDataProcess, uniqueZoneLibrary, fillerZoneLibrary, uniqueZoneChance); //This function automatically finishes and sets the currentWorld's zonedata

        if (currentProgressBar) {
            currentProgressBar.setProgress(1, 4);
            currentProgressBar.setText("Done!");
        }
        yield return new WaitForSeconds(0.5f);
        if (currentProgressBar) {
            currentProgressBar.gameObject.SetActive(false);
        }
    }

    private IEnumerator initZones (ZoneData[,] zdArray, ZoneTypesList zoneTypes) {
        /* Sets initial zoneType growth positions.
         * This function is worth .2 of a load session.
         */
        float subProgress = 0.0f;

        ZoneData[,] processedZDArray = zdArray;
        for (int yIndex = 0; yIndex < zdArray.GetLength(0); yIndex++) {
            for (int xIndex = 0; xIndex < zdArray.GetLength(1); xIndex++) {
                processedZDArray[yIndex, xIndex] = new ZoneData();
                processedZDArray[yIndex, xIndex].z_ZonePosition = new Vector2(xIndex, yIndex);
                subProgress += 0.5f * (1.0f / (zdArray.GetLength(0) * zdArray.GetLength(1)));
                updateProgress(subProgress, 0);
                yield return new WaitForEndOfFrame();
            }
        }

        // Pick random starting points for each zone type:
        float index = 0.0f;
        foreach (ZoneType currentType in zoneTypes.zoneTypes) {
            Vector2 chosenPoint = pickRandomUnchosenPoint(processedZDArray);
            processedZDArray[(int)chosenPoint.x, (int)chosenPoint.y].z_ZoneType = currentType;
            subProgress += 0.5f*(1.0f / zoneTypes.zoneTypes.Count);
            updateProgress(subProgress, 0);
            index++;
            yield return null;
        }
        currentZoneDataProcess = processedZDArray;
    }
    private IEnumerator growZones (ZoneData[,] zdArray, ZoneTypesList zoneTypes) {
        float subProgress = 0.0f;

        ZoneData[,] processedZDArray = zdArray;
        int zonesFinished = zoneTypes.zoneTypes.Count;
        Vector2[] adjacentArray = new Vector2[8] { new Vector2(0, 1), new Vector2(0, -1), new Vector2(1, 0), new Vector2(1, 1), new Vector2(1, -1), new Vector2(-1, 0), new Vector2(-1, 1), new Vector2(-1, -1) };
        while (zonesFinished < zdArray.GetLength(0) * zdArray.GetLength(1)) {
            for (int yIndex = 0; yIndex < zdArray.GetLength(0); yIndex++) {
                for (int xIndex = 0; xIndex < zdArray.GetLength(1); xIndex++) {
                    //For every zone in our newWorldZones array.
                    if (processedZDArray[yIndex, xIndex].z_ZoneType.name != "Unspecified") { //If the zonetype has been set.
                        //Check all adjacent tiles and spread to them with priority zoneType.Priority if possible.
                        foreach (Vector2 adjacentTilePos in adjacentArray) {
                            try {
                                if (processedZDArray[(int)yIndex + (int)adjacentTilePos.x, (int)xIndex + (int)adjacentTilePos.y].isJustSet()) {
                                    processedZDArray[(int)yIndex + (int)adjacentTilePos.x, (int)xIndex + (int)adjacentTilePos.y].unJustSet();
                                    continue;
                                }
                                if (!processedZDArray[(int)yIndex + (int)adjacentTilePos.x, (int)xIndex + (int)adjacentTilePos.y].alreadySet()) {
                                    if (processedZDArray[(int)yIndex + (int)adjacentTilePos.x, (int)xIndex + (int)adjacentTilePos.y].setConversionPercentage(processedZDArray[yIndex, xIndex].z_ZoneType)) {
                                        zonesFinished += 1; // A new zone has been finished
                                    }
                                }
                            }
                            catch (System.IndexOutOfRangeException) {
                                continue;
                            }
                        }
                    }
                }
                subProgress = (float)zonesFinished / (zdArray.GetLength(0) * zdArray.GetLength(1));
                updateProgress(subProgress, 1);
                yield return null;
            }
        }
        currentZoneDataProcess = processedZDArray;
    }
    private IEnumerator assignMandatoryZones (ZoneData[,] zdArray, Dictionary<ZoneType, ZoneList> mandatoryZoneLib) {
        /* Ensures that every mandatory zone is assigned to a unique tile.
         * Zone positions of a matching zonetype are chosen first.
         * If no empty zones of a matching zonetype are found, then for each tile that matches in zonetype, we check adjacent (even in other zonetypes) and try to place one there.
         * If none of the above works or is applicable, we pick random empty zone.
         */
        float subProgress = 0.0f;

        ZoneData[,] processedZDArray = zdArray;
        foreach (ZoneType currentZType in mandatoryZoneLib.Keys) {
            List<int> prefabIndexQueue = new List<int>();
            for (int i = 0; i < mandatoryZoneLib[currentZType].zonePrefabList.Count; i++) {
                prefabIndexQueue.Add(i);
            }
            while (prefabIndexQueue.Count > 0) {
                Vector2 newZonePos = getRandomUnchosenPoint_MandatoryZone(processedZDArray, currentZType);
                if (newZonePos.x >= 0 || newZonePos.y >= 0) { //If we have a valid zonePosition
                    processedZDArray[(int)newZonePos.x, (int)newZonePos.y].z_ZoneType = currentZType;
                    processedZDArray[(int)newZonePos.x, (int)newZonePos.y].z_ZoneFunction = ZoneFunction.Mandatory;
                    int prefabIndex = prefabIndexQueue[Random.Range(0, prefabIndexQueue.Count - 1)];
                    processedZDArray[(int)newZonePos.x, (int)newZonePos.y].z_ZonePrefabIndex = prefabIndex;
                    prefabIndexQueue.Remove(prefabIndex);
                }
                else {
                    newZonePos = getRandomAdjacentPoint_MandatoryZone(processedZDArray, currentZType);
                    if (newZonePos.x >= 0 || newZonePos.y >= 0) { //If we have a valid zonePosition
                        processedZDArray[(int)newZonePos.x, (int)newZonePos.y].z_ZoneType = currentZType;
                        processedZDArray[(int)newZonePos.x, (int)newZonePos.y].z_ZoneFunction = ZoneFunction.Mandatory;
                        int prefabIndex = prefabIndexQueue[Random.Range(0, prefabIndexQueue.Count - 1)];
                        processedZDArray[(int)newZonePos.x, (int)newZonePos.y].z_ZonePrefabIndex = prefabIndex;
                        prefabIndexQueue.Remove(prefabIndex);
                    }
                    else {
                        newZonePos = getRandomEmptyOrFillerPoint_MandatoryZone(processedZDArray);
                        processedZDArray[(int)newZonePos.x, (int)newZonePos.y].z_ZoneType = currentZType;
                        processedZDArray[(int)newZonePos.x, (int)newZonePos.y].z_ZoneFunction = ZoneFunction.Mandatory;
                        int prefabIndex = prefabIndexQueue[Random.Range(0, prefabIndexQueue.Count - 1)];
                        processedZDArray[(int)newZonePos.x, (int)newZonePos.y].z_ZonePrefabIndex = prefabIndex;
                        prefabIndexQueue.Remove(prefabIndex);
                    }
                }
            }
            subProgress += (1.0f / mandatoryZoneLib.Keys.Count);
            updateProgress(subProgress, 3);
            yield return null;
        }

        currentZoneDataProcess = processedZDArray;
    }
    private IEnumerator applyContinentFilter (ZoneData[,] zdArray, int iterations, float strength) {
        float subProgress = 0.0f;
        int currentIterations = 0;
        ZoneData[,] processedZDArray = zdArray;
        while (currentIterations < iterations) {
            List<Vector2> toFilter = new List<Vector2>();
            for (int yIndex = 0; yIndex < processedZDArray.GetLength(0) - currentIterations; yIndex++) {
                for (int xIndex = 0; xIndex < processedZDArray.GetLength(1) - currentIterations; xIndex++) {
                    if (yIndex == currentIterations || xIndex == currentIterations || yIndex == processedZDArray.GetLength(0) - currentIterations - 1 || xIndex == processedZDArray.GetLength(1) - currentIterations - 1) {
                        toFilter.Add(new Vector2(yIndex, xIndex));
                    }
                }
            }
            foreach (Vector2 zDataPos in toFilter) {
                if (strength >= Random.value) {
                    processedZDArray[(int)zDataPos.x, (int)zDataPos.y].z_ZoneFunction = ZoneFunction.Empty;
                }

                subProgress += (1.0f/toFilter.Count) * (1.0f / iterations);
                updateProgress(subProgress, 2);
                yield return null;
            }
            currentIterations++;
        }

        currentZoneDataProcess = processedZDArray;
    }
    private IEnumerator indexZones (ZoneData[,] zdArray, Dictionary<ZoneType, ZoneList> uniqueZoneLibrary, Dictionary<ZoneType, ZoneList> fillerZoneLibrary, float uniqueChance) {
        float subProgress = 0.0f;
        ZoneData[,] zoneDataArray = zdArray;
        List<ZoneData> processedZoneList = new List<ZoneData>();
        Dictionary<ZoneType, List<int>> uniqueIndexPool = new Dictionary<ZoneType, List<int>>();
        //Initialize our prefabIndex-by-zoneType pool:
        foreach (ZoneType zt in uniqueZoneLibrary.Keys) {
            List<int> newList = new List<int>();
            for (int i = 0; i < uniqueZoneLibrary[zt].zonePrefabList.Count; i++) {
                newList.Add(i);
            }
            uniqueIndexPool.Add(zt, newList);
        }
        foreach (ZoneData z in zoneDataArray) {
            //If it is not a story zone, determine and set whether it is unique or filler --- (To Add)
            if (z.z_ZoneFunction == ZoneFunction.Unset) {
                if (uniqueZoneLibrary.ContainsKey(z.z_ZoneType)) { //Exception may be found here --- (To Review/Foolproof)
                    //Chance for a unique zone
                    if (uniqueIndexPool[z.z_ZoneType].Count > 0) {
                        if (Random.value >= 1 - uniqueChance) {
                            //UniqueZone()
                            z.z_ZoneFunction = ZoneFunction.Unique;
                            z.z_ZonePrefabIndex = (int)Random.Range(0, uniqueZoneLibrary[z.z_ZoneType].zonePrefabList.Count);
                            uniqueIndexPool[z.z_ZoneType].Remove(z.z_ZonePrefabIndex);
                        }
                        else {
                            //Filler zone()
                            z.z_ZoneFunction = ZoneFunction.Filler;
                            if (fillerZoneLibrary.ContainsKey(z.z_ZoneType)) { //Exception may be found here --- (To Review/Foolproof)
                                z.z_ZonePrefabIndex = (int)Random.Range(0, fillerZoneLibrary[z.z_ZoneType].zonePrefabList.Count);
                            }
                        }
                    }
                    else {
                        //Filler zone()
                        z.z_ZoneFunction = ZoneFunction.Filler;
                        if (fillerZoneLibrary.ContainsKey(z.z_ZoneType)) { //Exception may be found here --- (To Review/Foolproof)
                            z.z_ZonePrefabIndex = (int)Random.Range(0, fillerZoneLibrary[z.z_ZoneType].zonePrefabList.Count);
                        }
                    }
                }
                else {
                    //Filler zone()
                    z.z_ZoneFunction = ZoneFunction.Filler;
                    if (fillerZoneLibrary.ContainsKey(z.z_ZoneType)) { //Exception may be found here --- (To Review/Foolproof)
                        z.z_ZonePrefabIndex = (int)Random.Range(0, fillerZoneLibrary[z.z_ZoneType].zonePrefabList.Count);
                    }
                }
            }
            processedZoneList.Add(z); //Perhaps check for empty, deleted zones? --- (To Add/Review)
            subProgress += 1.0f / ((float)zoneDataArray.GetLength(0)*zoneDataArray.GetLength(1));
            updateProgress(subProgress, 4);
            yield return null;
        }
        currentWorld.w_Zones = processedZoneList;
    }

    // --- [Helpers] ---
    private void updateProgress (float newProgress, int step = 0) {
        /* Updates the progress bar.
         * Step is the current step in the world gen process in the form of an index [0-max] (e.g. step 0 out of 4)
         */
        float progress = newProgress;
        currentProgressBar.setProgress(progress, step);
    }
    private Vector2 getRandomEmptyOrFillerPoint_MandatoryZone (ZoneData[,] zones) {
        List<ZoneData> possibleZones = new List<ZoneData>();
        foreach (ZoneData zd in zones) {
            if (zd.z_ZoneFunction == ZoneFunction.Unset || zd.z_ZoneFunction == ZoneFunction.Empty || zd.z_ZoneFunction == ZoneFunction.Filler) {
                possibleZones.Add(zd);
            }
        }
        if (possibleZones.Count > 0) {
            return possibleZones[Random.Range(0, possibleZones.Count)].z_ZonePosition;
        }
        else {
            throw new System.Exception("No possible zone for current mandatory zone!"); //This should never happen if proper world size minimums are in place.
        }
    }
    private Vector2 getRandomUnchosenPoint_MandatoryZone (ZoneData[,] zones, ZoneType zType) {
        /* Gets a random, unchosen point from a list of all empty zones of the same zoneType as zType.
         * Otherwise, returns a new Vector2(-1,-1) - which will be interpreted as invalid.
         */
        List<ZoneData> possibleZones = new List<ZoneData>();
        foreach (ZoneData zd in zones) {
            if (zd.z_ZoneFunction == ZoneFunction.Unset && zd.z_ZoneType.name == zType.name) {
                possibleZones.Add(zd);
            }
        }

        if (possibleZones.Count > 0) {
            return possibleZones[Random.Range(0, possibleZones.Count)].z_ZonePosition;
        }
        return new Vector2(-1, -1);
    }
    private Vector2 getRandomAdjacentPoint_MandatoryZone (ZoneData[,] zones, ZoneType zType) {
        List<ZoneData> filteredZones = new List<ZoneData>();
        foreach (ZoneData zd in zones) {
            if (zd.z_ZoneFunction == ZoneFunction.Unset && zd.z_ZoneType.name == zType.name) {
                filteredZones.Add(zd);
            }
        }
        List<ZoneData> possibleZones = new List<ZoneData>();
        Vector2[] adjacentArray = new Vector2[8] { new Vector2(0, 1), new Vector2(0, -1), new Vector2(1, 0), new Vector2(1, 1), new Vector2(1, -1), new Vector2(-1, 0), new Vector2(-1, 1), new Vector2(-1, -1) };
        foreach (ZoneData currentCenterZone in filteredZones) {
            foreach (Vector2 adjacentPos in adjacentArray) {
                try {
                    if (zones[(int)currentCenterZone.z_ZonePosition.x + (int)adjacentPos.x, (int)currentCenterZone.z_ZonePosition.y + (int)adjacentPos.y] != null) {
                        possibleZones.Add(zones[(int)currentCenterZone.z_ZonePosition.x + (int)adjacentPos.x, (int)currentCenterZone.z_ZonePosition.y + (int)adjacentPos.y]);
                    }
                }
                catch (System.IndexOutOfRangeException) {
                    continue;
                }
            }
        }

        if (possibleZones.Count > 0) {
            return possibleZones[Random.Range(0, possibleZones.Count)].z_ZonePosition;
        }
        return new Vector2(-1, -1);
    }
    private Vector2 pickRandomUnchosenPoint (ZoneData[,] zones) {
        var chosen = false;
        Vector2 randomVector = Vector2.zero;
        while (chosen == false) {
            randomVector = new Vector2((int)Random.Range(0, zones.GetLength(0)), (int)Random.Range(0, zones.GetLength(1)));
            if (zones[(int)randomVector.x, (int)randomVector.y].z_ZoneType.name == "Unspecified") {
                chosen = true;
            }
        }
        return randomVector;
    }
    #endregion
}
