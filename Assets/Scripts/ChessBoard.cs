using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessBoard : MonoBehaviour
{
    public Transform refPoint;
    public Transform xUnitPoint;
    public Transform yUnitPoint;
    public Transform zUnitPoint;

    // Start is called before the first frame update
    void Start()
    {
        print(Quaternion.LookRotation(new Vector3(1, 0, 0), new Vector3(0, 1, 0)).eulerAngles.ToString());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public Vector3 ChessVecToRealVec(Vector2 chessVec)
    {
        var xUnitVec = xUnitPoint.position - refPoint.position;
        var yUnitVec = yUnitPoint.position - refPoint.position;
        var realLoc = refPoint.position + chessVec.x * xUnitVec + chessVec.y * yUnitVec;
        return realLoc;
    }

    public Vector2 RealVecToChessVec(Vector3 realVec)
    {
        var xUnitVec = xUnitPoint.position - refPoint.position;
        var yUnitVec = yUnitPoint.position - refPoint.position;
        var realLocVector = realVec - refPoint.position;
        var xMag = Vector3.Project(realLocVector, xUnitVec).magnitude;
        var yMag = Vector3.Project(realLocVector, yUnitVec).magnitude;
        return new Vector2(xMag, yMag);
    }

    public Vector3 GetUpVector()
    {
        return (zUnitPoint.position - refPoint.position).normalized;
    }
}
