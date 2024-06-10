using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using DG.Tweening;
using UnityEngine;

public class LockChildRotation : MonoBehaviour
{ 
   // public float rotationOffset = 90f; // Adjust this for desired texture rotation

    private Mesh mesh;

    void Start()
    {
        mesh = GetComponentInChildren<MeshFilter>().mesh; // Get the mesh from child object
        DragController.Instance.OnBallChildRotation += OnBallChildRotation;
    }

    private void OnBallChildRotation()
    {
        Vector2[] originalUVs = mesh.uv; // Get original UV coordinates
       
               // Apply rotation offset to parent's rotation (assuming parent holds the rotation)
               float rotationAngle = transform.parent.rotation.eulerAngles.z/* + rotationOffset*/;
       
               // Simple rotation calculation (adjust based on your needs)
               float radians = rotationAngle * Mathf.Deg2Rad;
               float cosTheta = Mathf.Cos(radians);
               float sinTheta = Mathf.Sin(radians);
       
               for (int i = 0; i < originalUVs.Length; i++)
               {
                   float newX = originalUVs[i].x * cosTheta - originalUVs[i].y * sinTheta;
                   float newY = originalUVs[i].x * sinTheta + originalUVs[i].y * cosTheta;
                   originalUVs[i] = new Vector2(newX, newY);
               }
       
               mesh.uv = originalUVs; // Update mesh UV coordinates
           }
    

  
       
}
