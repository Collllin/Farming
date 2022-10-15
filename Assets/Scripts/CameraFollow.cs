using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Character characterScript;

    // Start is called before the first frame update
    void Start()
    {
        characterScript.characterPosChanged = (Transform characterTransform) =>
        {
            Vector3 newPos = new(characterTransform.position.x, characterTransform.position.y, transform.position.z);
            transform.position = newPos;
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
