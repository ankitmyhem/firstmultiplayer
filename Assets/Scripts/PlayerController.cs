using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{ 
    // Update is called once per frame
    void Update()
    {

        Vector3 moveDirection = Vector3.zero;
        if (Input.GetKey(KeyCode.LeftArrow)) moveDirection.z = +1;
        if (Input.GetKey(KeyCode.RightArrow)) moveDirection.z = -1;
        if (Input.GetKey(KeyCode.UpArrow)) moveDirection.x = +1;
        if (Input.GetKey(KeyCode.DownArrow)) moveDirection.x = -1;
        float moveSpeed = 3f;
        transform.position += moveSpeed * Time.deltaTime * moveDirection;
    }
}
