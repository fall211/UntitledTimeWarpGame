using UnityEngine;
using System.Collections.Generic;

public static class Noise {

	public static List<List<float>> GenerateNoiseMap(int size, int seed, float scale, int octaves, float persistance, float lacunarity, Vector2 offset) {
		List<List<float>> noiseMap = new List<List<float>>();

		System.Random prng = new System.Random (seed);
		Vector2[] octaveOffsets = new Vector2[octaves];
		for (int i = 0; i < octaves; i++) {
			float offsetX = prng.Next (-100000, 100000) + offset.x;
			float offsetY = prng.Next (-100000, 100000) + offset.y;
			octaveOffsets [i] = new Vector2 (offsetX, offsetY);
		}

		if (scale <= 0) {
			scale = 0.0001f;
		}

		float maxNoiseHeight = float.MinValue;
		float minNoiseHeight = float.MaxValue;

		float halfWidth = size / 2f;
		float halfHeight = size / 2f;


		for (int y = 0; y <= size; y++) {
            List<float> row = new List<float>();

			for (int x = 0; x <= size; x++) {
		
				float amplitude = 1;
				float frequency = 1;
				float noiseHeight = 0;

				for (int i = 0; i < octaves; i++) {
					float sampleX = (x-halfWidth) / scale * frequency + octaveOffsets[i].x;
					float sampleY = (y-halfHeight) / scale * frequency + octaveOffsets[i].y;

					float perlinValue = Mathf.PerlinNoise (sampleX, sampleY) * 2 - 1;
					noiseHeight += perlinValue * amplitude;

					amplitude *= persistance;
					frequency *= lacunarity;
				}

				if (noiseHeight > maxNoiseHeight) {
					maxNoiseHeight = noiseHeight;
				} else if (noiseHeight < minNoiseHeight) {
					minNoiseHeight = noiseHeight;
				}
                row.Add(noiseHeight);
			}
            noiseMap.Add(row);
		}

		for (int y = 0; y < size; y++) {
			for (int x = 0; x < size; x++) {
				noiseMap[x][y] = Mathf.InverseLerp (minNoiseHeight, maxNoiseHeight, noiseMap[x][y]);
			}
		}

		return noiseMap;
	}
    public static List<List<float>> GenerateFalloffMap(int size) {
        List<List<float>> falloffMap = new List<List<float>>();

        int size_half = size / 2;
        int scale = (3*size_half)/6;
        int boundBoxSizeUpper = size_half + scale;
        int boundBoxSizeLower = size_half - scale;

        for (int y = 0; y <= size; y++) {
            List<float> row = new List<float>();
            for (int x = 0; x <= size; x++) {
                float distance = DistToRect(boundBoxSizeLower, boundBoxSizeUpper, x, y);

                float intensity = distance / scale;
                
                row.Add(Eval(intensity));
            }
            falloffMap.Add(row);
        }
        return falloffMap;

    }
    public static List<List<float>> GenerateHeightMap(int size, int seed, float scale, int octaves, float persistance, float lacunarity, Vector2 offset) {
        List<List<float>> noiseMap = GenerateNoiseMap(size, seed, scale, octaves, persistance, lacunarity, offset);
        List<List<float>> falloffMap = GenerateFalloffMap(size);
        List<List<float>> heightMap = new List<List<float>>();
        for (int y = 0; y < noiseMap.Count; y++) {
            List<float> row = new List<float>();
            for (int x = 0; x < noiseMap[y].Count; x++) {
                row.Add(noiseMap[x][y] - falloffMap[x][y]);
            }
            heightMap.Add(row);
        }
        return heightMap;
    }
    private static float Eval(float n) {
        if (n > 1) {
            return 1;
        } else {
            return ((3 * n * n )- (2 * n * n * n));
        }
    }
    private static float DistToRect(int lower, int upper, int x, int y) {
        if (y > upper) {
            if (x < lower) {
                return Vector2.Distance( new Vector2(lower, upper), new Vector2(x, y));
            } else if ( x > upper) {
                return Vector2.Distance( new Vector2(upper, upper), new Vector2(x, y));
            } else {
                return y - upper;
            }
        } else if (y < lower) {
            if (x < lower) {
                return Vector2.Distance( new Vector2(lower, lower), new Vector2(x, y));
            } else if ( x > upper) {
                return Vector2.Distance( new Vector2(upper, lower), new Vector2(x, y));
            } else {
                return lower - y;
            } 
        } else {
            if (x < lower) {
                return lower - x;
            } else if (x > upper) {
                return x - upper;
            } else {
                return 0;
            }
        }
    }

}