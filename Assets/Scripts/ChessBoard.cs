using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessBoard : MonoBehaviour
{
    public Transform xUnitPoint;
    public Transform yUnitPoint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public Vector3 ChessLocToRealLoc(Vector2 chessLoc)
    {
        var xUnitVec = xUnitPoint.position - transform.position;
        var yUnitVec = yUnitPoint.position - transform.position;
        var realLoc = transform.position + chessLoc.x * xUnitVec + chessLoc.y * yUnitVec;
        return realLoc;
    }

    public Vector2 RealLocToChessLoc(Vector3 realLoc)
    {
        var xUnitVec = xUnitPoint.position - transform.position;
        var yUnitVec = yUnitPoint.position - transform.position;
        var realLocVector = realLoc - transform.position;
        var xMag = Vector3.Project(realLocVector, xUnitVec).magnitude;
        var yMag = Vector3.Project(realLocVector, yUnitVec).magnitude;
        return new Vector2(xMag, yMag);
    }
}
