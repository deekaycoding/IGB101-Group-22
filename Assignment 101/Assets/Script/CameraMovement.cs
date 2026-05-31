using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour{


    public GameObject[] cameraNodes;
    private int cameraIndex = 0;

    public GameObject[] objects;

    private float proximity = 0.1f;
    public float moveSpeed = 5.0f;
    public float rotSpeed = 5.0f;
    private float adjRotSpeed;
    private Quaternion targetRotation;


    // Start is called before the first frame update
    void Start(){
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update(){

        Movement();
    }



    private void Movement() {

        //Increment or Decrement Camera Position Index
        if (Vector3.Distance(transform.position, cameraNodes[cameraIndex].transform.position) < proximity) {

            if (Input.GetKeyDown("w")) {
                cameraIndex++;

                if (cameraIndex >= cameraNodes.Length - 1)
                    cameraIndex = cameraNodes.Length - 1;
            }
            else if (Input.GetKeyDown("s")) {
                    cameraIndex--;
                    if (cameraIndex <= 0)
                        cameraIndex = 0;
            }
        }

        //Move Camera towards Camera Index and Rotate Towards Object Index
        else {

            //Translation
            transform.position = Vector3.MoveTowards(transform.position, cameraNodes[cameraIndex].transform.position, moveSpeed * Time.deltaTime);

            //Rotation
            if (objects[cameraIndex]) {
                targetRotation = Quaternion.LookRotation(objects[cameraIndex].transform.position - transform.position);
                adjRotSpeed = Mathf.Min(rotSpeed * Time.deltaTime, 1);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, adjRotSpeed);
            }

            //Play Audio if contains Audio Source and is not playing
            if (objects[cameraIndex].GetComponent<AudioSource>() != null){
                if (!objects[cameraIndex].GetComponent<AudioSource>().isPlaying)
                    objects[cameraIndex].GetComponent<AudioSource>().Play();
            }
        }
    }

}
