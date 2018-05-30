using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PopulationManager : MonoBehaviour {

    #region Fields
    [SerializeField] GameObject botPrefab;
    [SerializeField] int populationSize = 50;
    [SerializeField] float trialTime = 5;

    float timeElapsed = 0;
    int generation = 1;

    List<GameObject> population = new List<GameObject>();

    GUIStyle guiStyle = new GUIStyle();
    #endregion

    // Use this for initialization
    void Start () {
        InitializePopulation();
	}
	
	// Update is called once per frame
	void Update () {
        timeElapsed += Time.deltaTime;

        if(timeElapsed >= trialTime)
        {
            BreedNewPopulation();
            timeElapsed = 0;
        }
	}

    private void OnGUI ()
    {
        guiStyle.fontSize = 25;
        guiStyle.normal.textColor = Color.white;
        GUI.BeginGroup(new Rect(10, 10, 250, 150));
        GUI.Box(new Rect(0, 0, 140, 140), "Stats", guiStyle);
        GUI.Label(new Rect(10, 25, 200, 30), "Gen: " + generation, guiStyle);
        GUI.Label(new Rect(10, 50, 200, 30), string.Format("Time: {0:0.00}", timeElapsed), guiStyle);
        GUI.Label(new Rect(10, 75, 200, 30), "Population: " + population.Count, guiStyle);
        GUI.EndGroup();
    }

    private void InitializePopulation ()
    {
        for(var i = 0; i < populationSize; i++)
        {
            Vector3 startingPosition = new Vector3(this.transform.position.x + Random.Range(-2, 2), this.transform.position.y, this.transform.position.z + Random.Range(-2, 2));

            GameObject bot = Instantiate(botPrefab, startingPosition, Quaternion.identity);
            bot.GetComponent<Brain>().Init();
            population.Add(bot);
        }
    }

    private GameObject Breed(GameObject parent1, GameObject parent2)
    {
        Vector3 startingPosition = new Vector3(this.transform.position.x + Random.Range(-2, 2), this.transform.position.y, this.transform.position.z + Random.Range(-2, 2));

        GameObject offspring = Instantiate(botPrefab, startingPosition, Quaternion.identity);
        Brain offspringBrain = offspring.GetComponent<Brain>();

        if(Random.Range(0, 100) == 1)
        // Mutate 1 of 100.
        {
            offspringBrain.Init();
            offspringBrain.Dna.Mutate();
        }
        else
        // Combine parent Dna.
        {
            offspringBrain.Init();
            offspringBrain.Dna.Combine(parent1.GetComponent<Brain>().Dna, parent2.GetComponent<Brain>().Dna);
        }
        return offspring;
    }

    private void BreedNewPopulation()
    {
        List<GameObject> sortedPopulation = population.OrderBy(g => g.GetComponent<Brain>().DistanceTravelled).ToList();
        population.Clear();

        // Breed second half of sortedPopulation.
        // TODO: What happens for uneven population counts (float vs int)?
        for(var i = (sortedPopulation.Count / 2) - 1; i < sortedPopulation.Count - 1; i++)
        {
            // Breed two offsprings from a pair of parents:
            // One with parent1's first half and parent2's second half of DNA
            // and one the other way around.
            population.Add(Breed(sortedPopulation[i], sortedPopulation[i + 1]));
            population.Add(Breed(sortedPopulation[i + 1], sortedPopulation[i]));
        }
        // Destroy all parents and previous population.
        for(var i = 0; i < sortedPopulation.Count; i++)
        {
            Destroy(sortedPopulation[i]);
        }
        generation++;
    }
}
