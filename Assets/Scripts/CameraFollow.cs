using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Character characterScript;
    
        public Object wallLeft;
        public Object wallRight;
        public Object wallUp;
        public Object wallDown;
    
        //for Mathf.Clamp, setting the boundary for Camera
        public float leftModify;
        public float rightModify;
        public float upModify;
        public float downModify;
        private float leftBoundary;
        private float rightBoundary;
        private float upBoundary;
        private float downBoundary;
    
        void Awake()
        {
            leftBoundary = wallLeft.GetComponent<Transform>().position.x +
                           wallLeft.GetComponent<BoxCollider2D>().size.x / 2 + 
                           gameObject.GetComponent<Camera>().orthographicSize * Screen.width / Screen.height + leftModify;
            rightBoundary = wallRight.GetComponent<Transform>().position.x -
                           wallRight.GetComponent<BoxCollider2D>().size.x / 2 - 
                           gameObject.GetComponent<Camera>().orthographicSize * Screen.width / Screen.height + rightModify;
            upBoundary = wallUp.GetComponent<Transform>().position.y - wallUp.GetComponent<BoxCollider2D>().size.y / 2 - 
                gameObject.GetComponent<Camera>().orthographicSize + upModify;
            downBoundary = wallDown.GetComponent<Transform>().position.y + wallDown.GetComponent<BoxCollider2D>().size.y / 2 + 
                           gameObject.GetComponent<Camera>().orthographicSize + downModify;
        }

    // Start is called before the first frame update
    void Start()
    {
        characterScript.characterPosChanged = (Transform characterTransform) =>
        {
            Vector3 newPos = new(Mathf.Clamp(characterTransform.position.x, leftBoundary, rightBoundary), Mathf.Clamp(characterTransform.position.y, downBoundary, upBoundary), transform.position.z);
            transform.position = newPos;
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
