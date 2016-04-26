using UnityEngine;
using System.Collections;
using System;

public class MapMaker : MonoBehaviour {

    public int width;
    public int height;

    public string seed;
    public bool useRandomSeed;

    [Range(1,10)] public int octaveCount;

    [Range(0.1F,0.9F)] public float persistance;

    Color[,] coloredMap;
    float[,] baseNoise;
    float[,] perlinNoise;

    int[,] typeMap;

    //types
    int waterType = 0;
    int sandType = 1;
    int grassType = 2;
    int mountainType = 3;

    //** Terrain Options **//
    public bool haveMountains;
    public bool haveTrees;
    [Range(0,10)] public int oceanLvl;

    void Start() {
        octaveCount = 4;
        persistance = 0.5F;
        MapMap();
    }

    void MapMap(){
        MakePerlinMap();
        MakeTypeMap();
        MakeColorMap();
    }

    public void generateButton() {
        MapMap();
    }

    void MakeTypeMap(){
        typeMap = new int[width,height];
        float oceanfloat = oceanLvl / 10.0F;


        //Mark all the spots that are water
        for (int i = 0; i < width; i++){
            for (int j = 0; j < height; j++){
                if (perlinNoise[i,j] <= oceanfloat){
                    typeMap[i,j] = waterType;
                }
                else{
                    typeMap[i,j] = grassType; 
                }
            }
        }

        //Mark all the spots that can be sand
        for (int i = 0; i < width; i++){
            for (int j = 0; j < height; j++){
                if ( (typeMap[i,j] == grassType ) && (neighborType(waterType, i, j)) ){
                    typeMap[i,j] = sandType;
                }
            }
        }


    }

    bool neighborType(int type, int x, int y){
        if ( ( ( x!= (width-1) ) && (typeMap[x+1,y] == type) ) || 
            ( ( y!=(height-1) ) && (typeMap[x,y+1] == type) ) || 
            ( ( x!=(width-1) && y!=(height-1) ) && (typeMap[x+1,y+1] == type) ) || 
            ( ( x!=(width-1) && y!=0 ) && (typeMap[x+1,y-1] == type) ) || 
            ( ( x!=0 && y!=(height-1) ) && (typeMap[x-1,y+1] == type) ) || 
            ( ( x!=0 ) && (typeMap[x-1,y] == type) ) || 
            ( ( y!=0 ) && (typeMap[x,y-1] == type) ) || 
            ( ( x!=0 && y!=0 ) && (typeMap[x-1,y-1] == type) ) ){
            return true;
        }
        else {
            return false;
        }
    }


    void MakePerlinMap() {
        baseNoise = GenerateWhiteNoise ();
        perlinNoise = GeneratePerlinNoise(baseNoise);
    }

    void MakeColorMap(){
        //coloredMap = MapGradient(Color.white, Color.black,perlinNoise); //colored map based on perlin noise values
        coloredMap = MapColorType(); // colored map based on type
    }

    Color[,] MapColorType(){
        Color[,] map = new Color[width,height];

        for (int i = 0; i < width; i++){
            for (int j = 0; j < height; j++){
                if (typeMap[i,j] == waterType){
                    map[i,j] = Color.blue;
                }
                else if (typeMap[i,j] == sandType){
                    map[i,j] = Color.yellow;
                }
                else if (typeMap[i,j] == grassType){
                    map[i,j] = Color.green;
                }
            }
        }

        return map;
    }

    void OnDrawGizmos() {
        if (coloredMap != null) {
            for (int x = 0; x < width; x ++) {
                for (int y = 0; y < height; y ++) {
                    Gizmos.color = coloredMap[x,y];
                    Vector3 pos = new Vector3(-width/2 + x + .5f, -height/2 + y+.5f,0);
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
