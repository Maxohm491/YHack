using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplanetOrbiter : Orbiter
{
    protected GameObject[] allPlanets = Array.Empty<GameObject>();

    protected void Awake() {
        allPlanets = GameObject.FindGameObjectsWithTag("Planet");
        StartCoroutine(UpdatePlanet());
    }

    IEnumerator UpdatePlanet() {
        while (true){
            planet = allPlanets[0];
            double closest = ((Vector2) allPlanets[0].transform.position - (Vector2) transform.position).sqrMagnitude;
            planetRadius = allPlanets[0].GetComponent<PlanetManager>().radius;

            foreach(GameObject i in allPlanets) {
                double squareDistance = ((Vector2) i.transform.position - (Vector2) transform.position).sqrMagnitude;   
                if(squareDistance < closest) {
                    planet = i;
                    closest = squareDistance;
                    planetRadius = i.GetComponent<PlanetManager>().radius;
                } 
            }

            yield return new WaitForSeconds(0.2f);
        }
    }


}
