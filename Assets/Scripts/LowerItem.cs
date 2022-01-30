using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowerItem : MonoBehaviour
{
    public GameObject itemToLower;
    public float liftSpeed = 10f;
    public float objectHeight = 1f;

    private bool onPlate = false;
    private Vector3 initialPosition;

    // Start is called before the first frame update
    void Start()
    {
        initialPosition = itemToLower.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!onPlate)
        {
            itemToLower.transform.position = Vector3.Lerp(itemToLower.transform.position, initialPosition, Time.deltaTime * liftSpeed);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == Player.instance.transform.tag)
        {
            Vector3 target = new Vector3(itemToLower.transform.position.x, initialPosition.y - objectHeight, itemToLower.transform.position.z);
            itemToLower.transform.position = Vector3.Lerp(itemToLower.transform.position, target, Time.deltaTime * liftSpeed);
            onPlate = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        onPlate = false;
    }
}
