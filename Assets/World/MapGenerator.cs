using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapGenerator : MonoBehaviour {

	public int mapSize;
	private float noiseScale;

	[SerializeField] private int octaves;
	[Range(0,1)]
	[SerializeField] private float persistance;
	[SerializeField] private float lacunarity;
	[SerializeField] private int seed;
	[SerializeField] private Vector2 offset;

	public bool autoUpdate;

	public void GenerateMap() {
        noiseScale = mapSize / 5f;

        int noiseMapSize = mapSize * 2 + 1;

		List<List<float>> noiseMap = Noise.GenerateNoiseMap(noiseMapSize, seed, noiseScale, octaves, persistance, lacunarity, offset);
        List<List<float>> falloffMap = Noise.GenerateFalloffMap(noiseMapSize);

        ProceduralGeneration proceduralGeneration = GetComponent<ProceduralGeneration>();
        proceduralGeneration.ClearMap();
        proceduralGeneration.GenerateMap(mapSize, noiseMap, falloffMap);
	}

	void OnValidate() {
		if (mapSize < 1) {
			mapSize = 1;
		}
		if (lacunarity < 1) {
			lacunarity = 1;
		}
		if (octaves < 0) {
			octaves = 0;
		}
        if (octaves > 15) {
            octaves = 15;
        }
	}

    // void Start() {
    //     GenerateMap();
    // }
}
