using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dna {

    List<int> genes = new List<int>();
    int dnaLength = 0;
    int maxValues = 0;

    public Dna (int length, int maxValues)
    {
        dnaLength = length;
        this.maxValues = maxValues;
        SetRandom();
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetRandom ()
    {
        genes.Clear();
        
        for(var i = 0; i < dnaLength; i++)
        {
            genes.Add(Random.Range(0, maxValues));
        }
    }

    public void SetInt(int position, int value)
    {
        genes[position] = value;
    }

    public void Combine(Dna parent1, Dna parent2)
    {
        // Split gene sequence of parents.
        for (var i = 0; i < dnaLength; i++)
        {
            // Use first half of parent1
            if (i < dnaLength / 2.0f)
            {
                int chromosome = parent1.genes[i];
                genes[i] = chromosome;
            }
            // and second half of parent2.
            else
            {
                int chromosome = parent2.genes[i];
                genes[i] = chromosome;
            }
        }
    }

    public void Mutate()
    {
        genes[Random.Range(0, dnaLength)] = Random.Range(0, maxValues); 
    }

    public int GetGene(int position)
    {
        return genes[position];
    }
}
