using UnityEngine;
using System.Collections;
using System;

public class MapMaker : MonoBehaviour {

    //* User Inputs *//
    public int width;
    public int height;

    public float seaLevel;
    public int elevationLvl;
    public float treePercFill;

    private int newWidth;
    private int newHeight;

    /*Debugging Inputs*/
    [Range(1,10)] public int octaveCount;
    [Range(0.1F,0.9F)] public float persistance;

    /*Data*/
    Color[,] coloredMap; //debugging purposes
    public float[,] perlinMap;
    public int[,] typeMap;
    public int[,] treeMap;
  
    private int[,] treePercMap;

    /*Types of Terrian*/
    int waterTile = 0;
    int sandTile = 1;
    int grassTile = 2;

    void DefaultSettings(){
        elevationLvl = 100;
        seaLevel = 50;
        treePercFill = 20;
        newWidth = 5;
        newHeight = 5;
    }

    void Awake() {
        DefaultSettings();
        MakeMap();
    }

    /*UI functions start*/
    public void ApplyChanges(){
        MakeTypeMap();
        MakeTreeMap();
        MakeColorMap();
    }

    public void MakeNewMap(){
        width = newWidth;
        height = newHeight;
        MakeMap();
    }

    public void WidthChanged(string newWidthtext){
        newWidth = int.Parse(newWidthtext);
    }

    public void HeightChanged(string newHeighttext){
        newHeight = int.Parse(newHeighttext);
    }

    public void SeaLevelChanged(float newSeaLevel){
        seaLevel = (float)Math.Floor(newSeaLevel);
    }

    public void TreeFillChanged(float newTreePercFill){
        treePercFill = (float)newTreePercFill;
    }

    public void ElevationChanged(string newElevationtext){
        elevationLvl = int.Parse(newElevationtext);
        /*ARIEL'S PART HERE*/
    }

    /*UI functions end*/

    void MakeMap(){
        MakePerlinMap(); MakeTypeMap();
        MakeTreePercMap(); MakeTreeMap();
        MakeColorMap();
    }

    void MakeTypeMap(){
        typeMap = new int[width,height];
        float seaDecimal = seaLevel / 100.0F;

        //Mark all the spots that are water/land
        for (int i = 0; i < width; i++){
            for (int j = 0; j < height; j++){
                if (perlinMap[i,j] <= seaDecimal){
                    typeMap[i,j] = waterTile;
                }
                else{
                    typeMap[i,j] = grassTile; 
                }
            }
        }

        //Mark all the spots that can be sand
        for (int i = 0; i < width; i++){
            for (int j = 0; j < height; j++){
                if ( (typeMap[i,j] == grassTile ) && (neighborType(waterTile, i, j)) ){
                    typeMap[i,j] = sandTile;
                }
            }
        }


    }

    void MakeTreeMap(){
        treeMap = new int[width,height];
        for (int i = 0; i < width; i ++) {
                for (int j = 0; j < height; j ++) {
                    treeMap[i,j] = ((treePercMap[i,j] < treePercFill) && (typeMap[i,j] == grassTile))? 1: 0;
                }
            }
        }

    void MakeTreePercMap(){
        treePercMap = new int[width,height];
        string seed = Time.time.ToString();
        System.Random pseudoRandom = new System.Random(seed.GetHashCode());

        for (int i = 0; i < width; i ++) {
            for (int j = 0; j < height; j ++) {
                //treeMap[i,j] = ((typeMap[i,j] == grassTile) && (pseudoRandom.Next(0,100) < treePercFill))? 1: 0;
                treePercMap[i,j] = pseudoRandom.Next(0,100);
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
        float[,] baseNoise = GenerateWhiteNoise ();
        GeneratePerlinNoise(baseNoise);
    }

    void MakeColorMap(){
        //MapGradient(Color.white, Color.black); //colored map based on perlin noise values
        MapColorType(); // colored map based on type
    }

    void MapColorType(){
        coloredMap = new Color[width,height];

        for (int i = 0; i < width; i++){
            for (int j = 0; j < height; j++){
                if (typeMap[i,j] == waterTile){
                    coloredMap[i,j] = Color.blue;
                }
                else if (typeMap[i,j] == sandTile){
                    coloredMap[i,j] = Color.yellow;
                }
                else if (typeMap[i,j] == grassTile){
                    coloredMap[i,j] = Color.green;
                }
            }
        }
    }

    /*
    void OnDrawGizmos() {
        if (coloredMap != null) {
            for (int i = 0; i < width; i ++) {
                for (int j = 0; j < height; j ++) {
                    Gizmos.color = coloredMap[i,j];
                    Vector3 pos = new Vector3(-width/2 + i + .5f, -height/2 + j+.5f,0);
                    Gizmos.DrawCube(pos,Vector3.one);
                    if (treeMap != null){
                        if (treeMap[i,j] == 1){
                            Gizmos.color = new Color(0.00F,0.27F,0.00F,1F);
                            Vector3 half = new Vector3(.5F,.5F,.5F);
                            Gizmos.DrawCube(pos,half);
                        }
                    }
                }
            }
        }
    }
    */

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

    //Perlin Noise Algorithm is from Herman Tulleken's code found here:
    //http://devmag.org.za/2009/04/25/perlin-noise/

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

    void GeneratePerlinNoise(float[,] baseNoise){
        int w = baseNoise.GetLength(0);
        int h = baseNoise.GetLength(1);

        float[][,] smoothNoise = new float [octaveCount][,];
        float persistance = 0.5F;

        //generate smooth noise
        for (int i = 0; i < octaveCount; i++){
            smoothNoise[i] = GenerateSmoothNoise(baseNoise,i);
        }

        perlinMap = new float[w,h];
        float amplitude = 1.0F;
        float totalAmplitude = 0.0F;

        //blend noise together
        for (int octave = octaveCount - 1; octave >= 0; octave--){
            amplitude *= persistance;
            totalAmplitude += amplitude;

            for (int i = 0; i < width; i++){
                for (int j = 0; j < height; j++){
                    perlinMap[i,j] += smoothNoise[octave][i,j] * amplitude;
                }
            }
        }

        //normalisation
        for (int i = 0; i < width; i++){
            for (int j = 0; j < height; j++){
                perlinMap[i,j] /= totalAmplitude;
            }
        }

    }


    void MapGradient(Color gradientStart, Color gradientEnd){
        coloredMap = new Color[width,height];
        for (int i = 0; i < width; i++){
            for (int j = 0; j < height; j++){
                coloredMap[i,j] = Color.Lerp(gradientStart,gradientEnd,perlinMap[i,j]);
            }
        }
    }

}
