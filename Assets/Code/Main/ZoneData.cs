[System.Serializable]
public class ZoneData {
    /* Data storage container for a Zone.
	 * A Zone is simply one piece - or tile - in the grid based world; WorldData will hold a List of all Zones.
     */

    public ZoneType z_ZoneType; //Zone's Type
    public int z_ZonePrefabIndex; //The index of the prefab in a ZoneList representing the ZoneType from z_ZoneType.
}
