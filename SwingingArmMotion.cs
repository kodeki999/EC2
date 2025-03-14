using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingingArmMotion : MonoBehaviour
{

    private Transform player;      // Riferimento al Transform del player
    public LayerMask terrainLayer; // Layer del terreno per il raycast
    public float heightOffset = 1.75f; // Altezza da mantenere sopra il terreno

    public static SwingingArmMotion Instance;

    private void Awake()
    {

        Instance = this;

    }

    // Game Objects
    [SerializeField] private GameObject LeftHand;
    [SerializeField] private GameObject RightHand;
    [SerializeField] private GameObject MainCamera;
    [SerializeField] private GameObject ForwardDirection;

    //Vector3 Positions
    [SerializeField] private Vector3 PositionPreviousFrameLeftHand;
    [SerializeField] private Vector3 PositionPreviousFrameRightHand;
    [SerializeField] private Vector3 PlayerPositionPreviousFrame;
    [SerializeField] private Vector3 PlayerPositionCurrentFrame;
    [SerializeField] private Vector3 PositionCurrentFrameLeftHand;
    [SerializeField] private Vector3 PositionCurrentFrameRightHand;

    //Speed
    [SerializeField] private float Speed = 70;
    [SerializeField] private float HandSpeed;
    public float bound_q = 1;


    void Start()
    {
        PlayerPositionPreviousFrame = transform.position; //set current positions
        PositionPreviousFrameLeftHand = LeftHand.transform.position; //set previous positions
        PositionPreviousFrameRightHand = RightHand.transform.position;
        player = transform;


        InvokeRepeating("bug", 0.0f, 2.0f);
    }

    // Update is called once per frame
    void Update()
    {
        // get forward direction from the center eye camera and set it to the forward direction object
        float yRotation = MainCamera.transform.eulerAngles.y;
        ForwardDirection.transform.eulerAngles = new Vector3(0, yRotation, 0);

        // get positons of hands
        PositionCurrentFrameLeftHand = LeftHand.transform.position;
        PositionCurrentFrameRightHand = RightHand.transform.position;

        // position of player
        PlayerPositionCurrentFrame = transform.position;

        // get distance the hands and player has moved from last frame
        var playerDistanceMoved = Vector3.Distance(PlayerPositionCurrentFrame, PlayerPositionPreviousFrame);
        var leftHandDistanceMoved = Vector3.Distance(PositionPreviousFrameLeftHand, PositionCurrentFrameLeftHand);
        var rightHandDistanceMoved = Vector3.Distance(PositionPreviousFrameRightHand, PositionCurrentFrameRightHand);

        // aggregate to get hand speed
        HandSpeed = ((leftHandDistanceMoved - playerDistanceMoved) + (rightHandDistanceMoved - playerDistanceMoved));
        if(Time.timeSinceLevelLoad > 1f)
        {
            transform.position += ForwardDirection.transform.forward * HandSpeed * Speed * Time.deltaTime * bound_q;
        }



        // set previous position of hands for next frame
        PositionPreviousFrameLeftHand = PositionCurrentFrameLeftHand;
        PositionPreviousFrameRightHand = PositionCurrentFrameRightHand;
        // set player position previous frame
        PlayerPositionPreviousFrame = PlayerPositionCurrentFrame;

        AdjustHeightToTerrain();

        
    }

    void AdjustHeightToTerrain()
    {
        Ray ray = new Ray(player.position + Vector3.up, Vector3.down); // Raycast verso il basso
        RaycastHit hit;

        // Esegui il raycast
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, terrainLayer))
        {
            // Calcola la nuova altezza basata sul terreno
            Vector3 playerPosition = player.position;
            playerPosition.y = hit.point.y + heightOffset; // Altezza del terreno + offset

            // Aggiorna la posizione del player
            player.position = playerPosition;
        }
    }


    //private void bug()
    //{
    //    Debug.Log($"{player.position.y}");
    //}

}
