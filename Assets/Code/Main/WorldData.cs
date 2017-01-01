using System.Collections.Generic;

[System.Serializable]
public class WorldData {
    /* WorldData is a container that represents a world.
     * World state, name, zone information, etc. are all stored in this class.
     * In the most basic implementation, this class is to be serialized to JSON
     * - for storage in GameData - Our save data file.
     */

    public List<ZoneData> w_Zones; //World Zone List

    public void Generate (ZoneTypesList zoneTypes, int length = 100, int width = 100) {
        /* Generates a new world and fills the w_Zones property.
         */

        ZoneData[,] newWorldZones = new ZoneData[length, width]; //Create a 2D Array of blank zones.
        throw new System.NotImplementedException();
    }
}
