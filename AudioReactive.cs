using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioReactive : MonoBehaviour
{
    GameObject[] cubes;
    static int numCube = 250; 
    float time = 0f;
    Vector3[] initPos;
    Vector3[] startPosition, endPosition;
    float lerpFraction;
    float t;
    void Start()
    {
        //list of cubes set up
        cubes = new GameObject[numCube];
        initPos = new Vector3[numCube];
        startPosition = new Vector3[numCube]; 
        endPosition = new Vector3[numCube]; 
        
        for (int i =0; i < numCube; i++){

            //randomizes each cube's starting position
            float r = 10f;
            startPosition[i] = new Vector3(r * Random.Range(-1f, 1f), r * Random.Range(-1f, 1f), r * Random.Range(-1f, 1f));        

            //makes cubes expand down two separate lines and contract towards the camera
            if(i % 2 == 0)
            {
                endPosition[i] = new Vector3(-2 * i, 1, 2 * i);
            }
            if(i % 2 != 0)
            {
                endPosition[i] = new Vector3(2 * i, 1, 2 * i);
            }

        }
        //actually creates the cubes
        for (int i =0; i < numCube; i++){
            cubes[i] = GameObject.CreatePrimitive(PrimitiveType.Cube); 

            initPos[i] = startPosition[i];
            cubes[i].transform.position = initPos[i];

        //sets up color renderer
            Renderer cubeRenderer = cubes[i].GetComponent<Renderer>();
            float hue = (float)i / numCube; 
            Color color = Color.HSVToRGB(hue, 1f, 1f); 
            cubeRenderer.material.color = color;
        }
    }

    void Update()
    {
        time += Time.deltaTime * AudioSpectrum.audioAmp; 

        //lerps from each cube's start position to end position, back and forth
        for (int i =0; i < numCube; i++){
            lerpFraction = Mathf.Sin(time) * 0.5f + 0.5f;
      
            t = i* 2 * Mathf.PI / numCube;
            cubes[i].transform.position = Vector3.Lerp(startPosition[i], endPosition[i], lerpFraction);
            float scale = 1f + AudioSpectrum.audioAmp;
            cubes[i].transform.localScale = new Vector3(scale, 1f, 1f);
            cubes[i].transform.Rotate(AudioSpectrum.audioAmp, 1f, 1f);

            //lerps color by cycling through HSV value
            Renderer cubeRenderer = cubes[i].GetComponent<Renderer>();
            float hue = (float)i / numCube;
            Color color = Color.HSVToRGB(Mathf.Abs(hue * Mathf.Cos(time)), Mathf.Cos(AudioSpectrum.audioAmp / 10f), 2f + Mathf.Cos(time)); // Full saturation and brightness
            cubeRenderer.material.color = color;
        }
    }
}
