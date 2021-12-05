using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class GameControlL1 : MonoBehaviour
{
    public ChessBoard chessBoard;
    public GraphUtils graphUtils;
    public AnimationSequence manChess;
    public AnimationSequence girlChess;
    public GameObject optionPrefab;

    private readonly Dictionary<Vector2, List<Vector2>> _graph = new Dictionary<Vector2, List<Vector2>>{
        {new Vector2(0, 0), new List<Vector2>{new Vector2(1, 0), new Vector2(0, 1)}},
        {new Vector2(0, 1), new List<Vector2>{new Vector2(1, 1), new Vector2(0, 2)}},
        {new Vector2(0, 2), new List<Vector2>{new Vector2(1, 2), new Vector2(0, 3)}},
        {new Vector2(0, 3), new List<Vector2>{new Vector2(1, 3), new Vector2(0, 4)}},
        {new Vector2(0, 4), new List<Vector2>{new Vector2(1, 4)}},
        {new Vector2(1, 0), new List<Vector2>{new Vector2(2, 0), new Vector2(1, 1)}},
        {new Vector2(1, 1), new List<Vector2>{new Vector2(2, 1), new Vector2(1, 2)}},
        {new Vector2(1, 2), new List<Vector2>{new Vector2(2, 2), new Vector2(1, 3)}},
        {new Vector2(1, 3), new List<Vector2>{new Vector2(2, 3), new Vector2(1, 4)}},
        {new Vector2(1, 4), new List<Vector2>{new Vector2(2, 4)}},
        {new Vector2(2, 0), new List<Vector2>{new Vector2(3, 0), new Vector2(2, 1)}},
        {new Vector2(2, 1), new List<Vector2>{new Vector2(3, 1), new Vector2(2, 2)}},
        {new Vector2(2, 2), new List<Vector2>{new Vector2(3, 2), new Vector2(2, 3)}},
        {new Vector2(2, 3), new List<Vector2>{new Vector2(3, 3), new Vector2(2, 4)}},
        {new Vector2(2, 4), new List<Vector2>{new Vector2(3, 4)}},
        {new Vector2(3, 0), new List<Vector2>{new Vector2(4, 0), new Vector2(3, 1)}},
        {new Vector2(3, 1), new List<Vector2>{new Vector2(4, 1), new Vector2(3, 2)}},
        {new Vector2(3, 2), new List<Vector2>{new Vector2(4, 2), new Vector2(3, 3)}},
        {new Vector2(3, 3), new List<Vector2>{new Vector2(4, 3), new Vector2(3, 4)}},
        {new Vector2(3, 4), new List<Vector2>{new Vector2(4, 4)}},
        {new Vector2(4, 0), new List<Vector2>{new Vector2(4, 1)}},
        {new Vector2(4, 1), new List<Vector2>{new Vector2(4, 2)}},
        {new Vector2(4, 2), new List<Vector2>{new Vector2(4, 3)}},
        {new Vector2(4, 3), new List<Vector2>{new Vector2(4, 4)}},
        {new Vector2(4, 4), new List<Vector2>()}
    };

    private readonly List<GameObject> _optionObjs = new List<GameObject>();

    private Vector2 manChessPos;
    private Vector2 manChessRot;
    private Vector2 girlChessPos;
    private Vector2 girlChessRot;

    private enum State
    {
        WaitUserInput,
        ManAnimating,
        GirlAnimating,
        ShowOptions
    }

    private State _state = State.WaitUserInput;

    // Start is called before the first frame update
    void Start()
    {
        manChessPos = new Vector2(0, 0);
        girlChessPos = new Vector2(1, 1);
        manChessRot = new Vector2(1, 0);
        girlChessRot = new Vector2(0, 1);
        manChess.transform.position = chessBoard.ChessVecToRealVec(manChessPos);
        girlChess.transform.position = chessBoard.ChessVecToRealVec(girlChessPos);
        manChess.transform.rotation = Quaternion.LookRotation(chessBoard.ChessVecToRealVec(manChessRot), chessBoard.GetUpVector());
        girlChess.transform.rotation = Quaternion.LookRotation(chessBoard.ChessVecToRealVec(girlChessRot), chessBoard.GetUpVector());
        ShowOptions();
    }

    // Update is called once per frame
    void Update()
    {
        switch (_state)
        {
            case State.WaitUserInput:
            {
                break;
            }
            case State.ManAnimating:
            {
                print("man animating" + manChess.IsAnimating);
                if (!manChess.IsAnimating)
                {
                    AutoMoveGirlChess();
                    _state = State.GirlAnimating;
                }
                break;
            }
            case State.GirlAnimating:
            {
                print("girl animating" + girlChess.IsAnimating);
                if (!girlChess.IsAnimating)
                {
                    _state = State.ShowOptions;
                }
                break;
            }
            case State.ShowOptions:
            {
                ShowOptions();
                _state = State.WaitUserInput;
                break;
            }
        }

    }

    private void ShowOptions()
    {
        var optionCords = graphUtils.GetNeighbors(_graph, manChessPos);
        foreach (var optionCord in optionCords)
        {
            var go = Instantiate(optionPrefab, chessBoard.ChessVecToRealVec(optionCord), Quaternion.identity);
            var co = go.GetComponent<ClickableObject>();
            if (co != null)
            {
                _optionObjs.Add(go);
                co.SetCallback(() =>
                {
                    MoveManChess(optionCord);
                    // Remove all option objects
                    foreach (var optionObj in _optionObjs)
                    {
                        Destroy(optionObj);
                    }
                    _optionObjs.Clear();
                    _state = State.ManAnimating;
                });
            }
        }
    }

    private void MoveManChess(Vector2 dst)
    {
        var faceTowards = dst - manChessPos;
        if (manChessRot != faceTowards)
        {
            manChess.AddToSequence(AnimationSequence.Subject.Rotation, chessBoard.ChessVecToRealVec(manChessRot),
                chessBoard.ChessVecToRealVec(faceTowards), 0.25f, AnimationSequence.Curve.Linear);
            manChessRot = faceTowards;
        }
        manChess.AddToSequence(AnimationSequence.Subject.Position, chessBoard.ChessVecToRealVec(manChessPos),
            chessBoard.ChessVecToRealVec(dst), 1, AnimationSequence.Curve.Quadratic);
        manChessPos = dst;
    }

    private void AutoMoveGirlChess()
    {
        var dst = graphUtils.GetNextNodeFurthestTo(_graph, girlChessPos, manChessPos);
        if (dst == Vector2.zero)
            return;
        var faceTowards = dst - girlChessPos;
        if (girlChessPos != faceTowards)
        {
            girlChess.AddToSequence(AnimationSequence.Subject.Rotation, chessBoard.ChessVecToRealVec(girlChessRot),
                chessBoard.ChessVecToRealVec(faceTowards), 0.25f, AnimationSequence.Curve.Linear);
            girlChessRot = faceTowards;
        }
        girlChess.AddToSequence(AnimationSequence.Subject.Position, chessBoard.ChessVecToRealVec(girlChessPos),
            chessBoard.ChessVecToRealVec(dst), 1, AnimationSequence.Curve.Quadratic);
        girlChessPos = dst;
    }

    public void OnDrawGizmosSelected()
    {
        // Draw grid dots
        var gridSize = 10;
        var sphereRadius = 0.05f;
        var color = new Color(1, 0, 0, 0.9f);
        Gizmos.color = color;
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                Gizmos.DrawSphere(chessBoard.ChessVecToRealVec(new Vector2(x, y)), sphereRadius);
            }
        }
        // Draw paths
        color = new Color(0, 0, 1, 0.9f);
        Gizmos.color = color;
        foreach (var pair in _graph)
        {
            var node = pair.Key;
            var neighbors = graphUtils.GetNeighbors(_graph, node);
            foreach (var neighbor in neighbors)
            {
                Gizmos.DrawLine(chessBoard.ChessVecToRealVec(node),
                    (chessBoard.ChessVecToRealVec(node) + chessBoard.ChessVecToRealVec(neighbor)) / 2);
            }
        }
    }
}
