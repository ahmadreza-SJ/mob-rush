using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CannonMovementBehaviour))]
public class CannonController : MonoBehaviour
{ 
    private CannonMovementBehaviour _movementBehaviour;
    
    public CannonMovementBehaviour MovementBehaviour => _movementBehaviour; 
    
    // Start is called before the first frame update
    void Start()
    {
        _movementBehaviour = GetComponent<CannonMovementBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
