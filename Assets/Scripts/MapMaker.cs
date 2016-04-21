using UnityEngine;
using System.Collections;
using System;

public class MapMaker : MonoBehaviour {

    public int width;
    public int height;

    public string seed;
    public bool useRandomSeed;

    [Range(1,10)]
    public int octaveCount;

    [Range(0.1F,0.9F)]
    public float persistance;



    Color[,] coloredMap;
    float[,] baseNoise;
    float[,] perlinNoise;


    void Start() {
        octaveCount = 4;
        persistance = 0.5F;
        MakeMap();
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            MakeMap();
        }
    }

    void MakeMap() {
        baseNoise = GenerateWhiteNoise ();
        perlinNoise = GeneratePerlinNoise(baseNoise);
        Color orange = new Color(255, 127, 0);
        coloredMap = MapGradient(Color.green, Color.blue,perlinNoise);
    }

    void OnDrawGizmos() {
        if (coloredMap != null) {
            for (int x = 0; x < width; x ++) {
                for (int y = 0; y < height; y ++) {
                    Gizmos.color = coloredMap[x,y];
                    Vector3 pos = new Vector3(-width/2 + x + .5f,0, -height/2 + y+.5f);
                    Gizmos.DrawCube(pos,Vector3.one);
                }
            }
        }
    }

    //GENERATE MAP WITH PERLIN NOISE //
    float[,] GenerateWhiteNoise(){
        float[,] noise = new float[width,height];
        for (int i = 0; i < width; i++){
            for (int j = 0; j < height; j++){
                noise[i,j] = UnityEngine.Random.value;
            }
        }

        return noise;
    }

    float Interpolate(float x0, float x1, float alpha){
        return x0 * (1-alpha) + alpha * x1;
    }

	float[,] GenerateSmoothNoise(float[,] baseNoise, int octave){
		int w = baseNoise.GetLength (0);
		int h = baseNoise.GetLength (1);

		float[,] smoothNoise = new float[width, height];
		int samplePeriod = 1 << octave;

		float sampleFrequency = 1.0F / samplePeriod;

		for (int i = 0; i < width; i++) {
			//calculate the horizontal sampling indices
			int sample_i0 = (i / samplePeriod) * samplePeriod;
			int sample_i1 = (sample_i0 + samplePeriod) % w;
			float horizontal_blend = (i - sample_i0) * sampleFrequency;
			
			for (int j = 0; j < height; j++) {
                //calculate the vertical sampling indices
                int sample_j0 = (j / samplePeriod) * samplePeriod;
                int sample_j1 = (sample_j0 + samplePeriod) % h;
                float vertical_blend = (j - sample_j0) * sampleFrequency;

                //blend the top two corners
                float top = Interpolate(baseNoise[sample_i0,sample_j0], baseNoise[sample_i1,sample_j0], horizontal_blend);
                float bottom = Interpolate(baseNoise[sample_i0,sample_j1], baseNoise[sample_i1,sample_j1], horizontal_blend);

                smoothNoise[i,j] = Interpolate(top,bottom,vertical_blend);
			}
		}

        return smoothNoise;
	}

    float[,] GeneratePerlinNoise(float[,] baseNoise){
        int w = baseNoise.GetLength(0);
        int h = baseNoise.GetLength(1);

        float[][,] smoothNoise = new float [octaveCount][,];
        float persistance = 0.5F;

        //generate smooth noise
        for (int i = 0; i < octaveCount; i++){
            smoothNoise[i] = GenerateSmoothNoise(baseNoise,i);
        }

        float[,] perlinNoise = new float[w,h];
        float amplitude = 1.0F;
        float totalAmplitude = 0.0F;

        //blend noise together
        for (int octave = octaveCount - 1; octave >= 0; octave--){
            amplitude *= persistance;
            totalAmplitude += amplitude;

            for (int i = 0; i < width; i++){
                for (int j = 0; j < height; j++){
                    perlinNoise[i,j] += smoothNoise[octave][i,j] * amplitude;
                }
            }
        }

        //normalisation
        for (int i = 0; i < width; i++){
            for (int j = 0; j < height; j++){
                perlinNoise[i,j] /= totalAmplitude;
            }
        }

        return perlinNoise;
    }


    Color[,] MapGradient(Color gradientStart, Color gradientEnd, float[,] perlinNoise){
        Color[,] image = new Color[width,height];
        for (int i = 0; i < width; i++){
            for (int j = 0; j < height; j++){
                image[i,j] = Color.Lerp(gradientStart,gradientEnd,perlinNoise[i,j]);
            }
        }

        return image;
    }

}