using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(Terrain))]
public class TerrainTextureRandomizer : MonoBehaviour
{
    [Header("Tile size settings (in meters)")]
    public float minTileSize = 8f;   // smallest tile size
    public float maxTileSize = 20f;  // largest tile size

    [Header("Random offset toggle")]
    public bool randomOffset = true;

    private Terrain terrain;

    void Start()
    {
        ApplyRandomization();
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        ApplyRandomization();
    }
#endif

    void ApplyRandomization()
    {
        terrain = GetComponent<Terrain>();
        if (terrain == null || terrain.terrainData == null) return;

        TerrainLayer[] layers = terrain.terrainData.terrainLayers;
        if (layers == null) return;

        foreach (TerrainLayer layer in layers)
        {
            // Randomize tile size between min and max
            float tileSize = Random.Range(minTileSize, maxTileSize);
            layer.tileSize = new Vector2(tileSize, tileSize);

            // Optionally randomize offset to break repetition
            if (randomOffset)
            {
                layer.tileOffset = new Vector2(Random.value, Random.value);
            }
        }

        // Reassign layers to force update
        terrain.terrainData.terrainLayers = layers;
    }
}
