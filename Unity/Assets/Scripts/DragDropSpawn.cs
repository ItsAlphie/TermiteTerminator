using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDropSpawn : MonoBehaviour
{
    private bool isDragging = false;

    private void Update(){
        if(isDragging){
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            transform.Translate(mousePosition);
        }
    }

    private void OnMouseDown() {
        Debug.Log("Start Dragging");
        isDragging = true;
    }

    private void OnMouseUp(){
        Debug.Log("Stop Dragging");
        isDragging = false;
    }
    
}


