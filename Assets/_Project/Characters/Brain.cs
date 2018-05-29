using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using System.Collections;

[RequireComponent(typeof(ThirdPersonCharacter))]

public class Brain : MonoBehaviour
{
    private int DnaLength = 1;
    private float timeAlive;
    private Dna dna;

    private ThirdPersonCharacter thirdPersonCharacter;
    private Vector3 moveDirection;
    private bool isJumping;
    private bool isAlive = true;

    // Use this for initialization
    void Start ()
    {

    }

    private void OnCollisionEnter (Collision collision)
    {
        if (collision.gameObject.tag == "dead")
        {
            isAlive = false;
        }
    }

    private void Init ()
    {
        // Initialize DNA
        // 0 forward
        // 1 back
        // 2 left
        // 3 right
        // 4 jump
        // 5 crouch
        dna = new Dna(DnaLength, 6);
        thirdPersonCharacter = GetComponent<ThirdPersonCharacter>();
        timeAlive = 0;
        isAlive = true;
    }

    // FixedUpdate is called in sync with physics.
    private void FixedUpdate ()
    {
        // Read DNA
        float horizontal = 0f;
        float vertical = 0f;
        bool isCrouching = false;

        if (dna.GetGene(0) == 0)
        {
            vertical = 1;
        }
        else if (dna.GetGene(0) == 1)
        {
            vertical = -1;
        }
        else if (dna.GetGene(0) == 2)
        {
            horizontal = -1;
        }
        else if (dna.GetGene(0) == 3)
        {
            horizontal = 1;
        }
        else if (dna.GetGene(0) == 4)
        {
            isJumping = true;
        }
        else if (dna.GetGene(0) == 5)
        {
            isCrouching = true;
        }

        moveDirection = vertical * Vector3.forward + horizontal * Vector3.right;
        thirdPersonCharacter.Move(moveDirection, isCrouching, isJumping);
        isJumping = false;

        if (isAlive)
        {
            timeAlive += Time.deltaTime;
        }
    }
}
