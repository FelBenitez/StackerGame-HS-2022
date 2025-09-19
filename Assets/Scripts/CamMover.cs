using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMover : MonoBehaviour
{
    public bool start = false;
    public float duration = 0.5f;
    public AnimationCurve curve;
    Vector3 endPan = new Vector3(0.50f, 20f, 30f);
    //public Transform target;
    Vector3 goToPan;

    void Update()
    {
        if(start){
            start=false;
            StartCoroutine(Shaking());
        }
    }

    public void updateCam1()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y+0.535f, transform.position.z);
        //anytime you update the camera, shake it
        start = true;
        goToPan = new Vector3(18.46f, transform.position.y, 9.06f);
    }

    IEnumerator Shaking()
    {
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        while(elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float strength = curve.Evaluate(elapsedTime / duration);
            transform.position = startPosition + Random.insideUnitSphere * strength;
            yield return null;
        }
        transform.position = startPosition;
        
    }

    public void panOut()
    {
        transform.position = Vector3.Lerp(transform.position, goToPan, 0.5f * Time.deltaTime);
    }

}
