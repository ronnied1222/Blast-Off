using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour {

    Vector3 movementVector = new Vector3(10f, 0, 0);

    // todo remvoe from inspector later
    [Range(0, 1)]float movementFactor; // 0 for not moved 1 for fully moved.
    private Vector3 startingPos;
    [SerializeField] float period = 2f;

	// Use this for initialization
	void Start () {
        startingPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        //set movement factor
        if(period <= Mathf.Epsilon){
            period = 2f;
        }

        float cycles = Time.time / period; //grows continually from 0

        const float tau = Mathf.PI * 2;
        float rawSinWave = Mathf.Sin(tau * cycles);

        movementFactor = rawSinWave / 2f + 0.5f;
        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPos + offset;
	}
}
