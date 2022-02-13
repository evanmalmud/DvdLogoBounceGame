using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogoMover : MonoBehaviour
{

    public BoxCollider2D dvdLogoCollider;

    public float minX, maxX, minY, maxY;
    public Vector2 initialDirection;

    public Vector2 currentDirection;
    public float speed;

    public float xBuffer, yBuffer;

    public SpriteRenderer spriteRenderer;
    public float colorSpeed;

    Vector3 scale;

    public Vector2 minScreenSize;


    public GameObject easyModeObj;

    private void Start()
    {
        dvdLogoCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentDirection = initialDirection;
        xBuffer = dvdLogoCollider.bounds.size.x / 2;
        yBuffer = dvdLogoCollider.bounds.size.y / 2;
        scale = this.transform.localScale;
        easyModeObj.SetActive(false);
    }

    // Start is called before the first frame update
    void LateUpdate()
    {
        if(Input.GetKeyDown(KeyCode.E)) {
            easyModeObj.SetActive(!easyModeObj.activeSelf);
        }

        //Check minimum screen size
        if(Screen.width < minScreenSize.x || Screen.height < minScreenSize.y) {

            Screen.SetResolution((int)Mathf.Max(Screen.width, minScreenSize.x), (int)Mathf.Max(Screen.height, minScreenSize.y), Screen.fullScreenMode, 60);
            Debug.Log("Set Min Screen size");
        }

        //Get Camera Bounds
        float vertExtent = Camera.main.orthographicSize;
        float horzExtent = vertExtent * Screen.width / Screen.height;
        // Calculations assume map is position at the origin
        if(maxX == horzExtent && minX == -horzExtent && maxY == vertExtent && minY == -vertExtent) {
            //Screen is the same!
        } else {
            maxX = horzExtent;
            minX = 0 - horzExtent;
            maxY = vertExtent;
            minY = 0 - vertExtent;
            return;
        }

        Vector2 currentPosition = transform.position;
        //Debug.Log("currentPosition Before - " + currentPosition);

        Vector4 numberSidesHitThisFrame = new Vector4();
        //Change direction
        if (currentPosition.x + xBuffer > maxX) {
            //To far right
            //Debug.LogError("maxX");
           currentDirection.x = Mathf.Abs(initialDirection.x) * -1;
            numberSidesHitThisFrame.x = 1;
        }
        if (currentPosition.x - xBuffer < minX) {
            //To far left
            //Debug.LogError("minX");
            currentDirection.x = Mathf.Abs(initialDirection.x);
            numberSidesHitThisFrame.y = 1;
        }
        if (currentPosition.y + yBuffer > maxY) {
            //To far up
            //Debug.LogError("maxY");
            currentDirection.y = Mathf.Abs(initialDirection.y) * -1;
            numberSidesHitThisFrame.z = 1;
        }
        if (currentPosition.y - yBuffer < minY) {
            //To far down
            //Debug.LogError("minY");
            currentDirection.y = Mathf.Abs(initialDirection.y);
            numberSidesHitThisFrame.w = 1;
        }

        //Move
        //Debug.Log("currentPosition Before - " + currentPosition);
        //Debug.Log("currentDirection - " + currentDirection);
        currentPosition += speed * currentDirection * Time.deltaTime;
        //Debug.Log("currentPosition After - " + currentPosition);
        transform.position = currentPosition;


        //Change Color
        //spriteRenderer.material.SetColor("_Color", HSBColor.ToColor(new HSBColor(Mathf.PingPong(Time.time * colorSpeed, 1), 1, 1)));
        spriteRenderer.color = HSBColor.ToColor(new HSBColor(Mathf.PingPong(Time.time * colorSpeed, 1), 1, 1));

        //Score
        bool corenerHit = false;
        if (numberSidesHitThisFrame.x > 0 && numberSidesHitThisFrame.z > 0) {
            corenerHit = true;
        } else if (numberSidesHitThisFrame.x > 0 && numberSidesHitThisFrame.w > 0) {
            corenerHit = true;
        } else if (numberSidesHitThisFrame.y > 0 && numberSidesHitThisFrame.z > 0) {
            corenerHit = true;
        } else if (numberSidesHitThisFrame.y > 0 && numberSidesHitThisFrame.w > 0) {
            corenerHit = true;
        }
        if(corenerHit) {
            Debug.Log("We hit a corner!");
        }
    }

}
