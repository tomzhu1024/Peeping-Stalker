using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessBoard : MonoBehaviour
{
    [SerializeField] private float gridSize = 2;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public Vector3 ChessVecToRealVec(Vector2 chessVec)
    {
        var xUnitVec = Vector3.right * gridSize;
        var yUnitVec = Vector3.forward * gridSize;
        var realLoc = transform.position + chessVec.x * xUnitVec + chessVec.y * yUnitVec;
        return realLoc;
    }

    public Vector2 RealVecToChessVec(Vector3 realVec)
    {
        var xUnitVec = Vector2.right * gridSize;
        var yUnitVec = Vector2.up * gridSize;
        var realLocVector = realVec - transform.position;
        var xMag = Vector3.Project(realLocVector, xUnitVec).magnitude;
        var yMag = Vector3.Project(realLocVector, yUnitVec).magnitude;
        return new Vector2(xMag, yMag);
    }
}
