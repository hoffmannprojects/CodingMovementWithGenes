using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using System.Collections;

[RequireComponent(typeof(ThirdPersonCharacter))]

public class Brain : MonoBehaviour
{
    #region Properties
    public Dna Dna { get; private set; }
    public float TimeAlive { get; private set; }
    #endregion

    #region Fields
    private int DnaLength = 1;

    private ThirdPersonCharacter thirdPersonCharacter;
    private Vector3 moveDirection;
    private bool isJumping;
    private bool isAlive = true;
    #endregion

    #region public methods
    public void Init ()
    {
        // Initialize DNA
        // 0 forward
        // 1 back
        // 2 left
        // 3 right
        // 4 jump
        // 5 crouch
        Dna = new Dna(DnaLength, 6);
        thirdPersonCharacter = GetComponent<ThirdPersonCharacter>();
        TimeAlive = 0;
        isAlive = true;
    }
    #endregion

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

    // FixedUpdate is called in sync with physics.
    private void FixedUpdate ()
    {
        // Read DNA
        float horizontal = 0f;
        float vertical = 0f;
        bool isCrouching = false;

        if (Dna.GetGene(0) == 0)
        {
            vertical = 1;
        }
        else if (Dna.GetGene(0) == 1)
        {
            vertical = -1;
        }
        else if (Dna.GetGene(0) == 2)
        {
            horizontal = -1;
        }
        else if (Dna.GetGene(0) == 3)
        {
            horizontal = 1;
        }
        else if (Dna.GetGene(0) == 4)
        {
            isJumping = true;
        }
        else if (Dna.GetGene(0) == 5)
        {
            isCrouching = true;
        }

        moveDirection = vertical * Vector3.forward + horizontal * Vector3.right;
        thirdPersonCharacter.Move(moveDirection, isCrouching, isJumping);
        isJumping = false;

        if (isAlive)
        {
            TimeAlive += Time.deltaTime;
        }
    }
}
