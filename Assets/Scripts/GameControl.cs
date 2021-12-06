using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class GameControl : MonoBehaviour
{
    [SerializeField] private ChessBoard chessBoard;
    [SerializeField] private GraphUtils graphUtils;
    [SerializeField] private AnimationSequence manChess;
    [SerializeField] private AnimationSequence girlChess;
    [SerializeField] private GameObject optionPrefab;

    private enum State
    {
        WaitInput,
        AnimateManChess,
        AnimateGirlChess,
        TurnFinished
    }

    private State _state = State.WaitInput;
    private int _sceneIndex = 0;
    private readonly Dictionary<Vector2, List<Vector2>> _graph = new Dictionary<Vector2, List<Vector2>>{
        {new Vector2(0, 0), new List<Vector2>{new Vector2(1, 0), new Vector2(0, 1)}},
        {new Vector2(0, 1), new List<Vector2>{new Vector2(1, 1), new Vector2(0, 2), new Vector2(0, 0)}},
        {new Vector2(0, 2), new List<Vector2>{new Vector2(1, 2), new Vector2(0, 3), new Vector2(0, 1)}},
        {new Vector2(0, 3), new List<Vector2>{new Vector2(1, 3), new Vector2(0, 4), new Vector2(0, 2)}},
        {new Vector2(0, 4), new List<Vector2>{new Vector2(1, 4), new Vector2(0, 3)}},
        {new Vector2(1, 0), new List<Vector2>{new Vector2(2, 0), new Vector2(0, 0), new Vector2(1, 1)}},
        {new Vector2(1, 1), new List<Vector2>{new Vector2(2, 1), new Vector2(0, 1), new Vector2(1, 2), new Vector2(1, 0)}},
        {new Vector2(1, 2), new List<Vector2>{new Vector2(2, 2), new Vector2(0, 2), new Vector2(1, 3), new Vector2(1, 1)}},
        {new Vector2(1, 3), new List<Vector2>{new Vector2(2, 3), new Vector2(0, 3), new Vector2(1, 4), new Vector2(1, 2)}},
        {new Vector2(1, 4), new List<Vector2>{new Vector2(2, 4), new Vector2(0, 4), new Vector2(1, 3)}},
        {new Vector2(2, 0), new List<Vector2>{new Vector2(3, 0), new Vector2(1, 0), new Vector2(2, 1)}},
        {new Vector2(2, 1), new List<Vector2>{new Vector2(3, 1), new Vector2(1, 1), new Vector2(2, 2), new Vector2(2, 0)}},
        {new Vector2(2, 2), new List<Vector2>{new Vector2(3, 2), new Vector2(1, 2), new Vector2(2, 3), new Vector2(2, 1)}},
        {new Vector2(2, 3), new List<Vector2>{new Vector2(3, 3), new Vector2(1, 3), new Vector2(2, 4), new Vector2(2, 2)}},
        {new Vector2(2, 4), new List<Vector2>{new Vector2(3, 4), new Vector2(1, 4), new Vector2(2, 3)}},
        {new Vector2(3, 0), new List<Vector2>{new Vector2(4, 0), new Vector2(2, 0), new Vector2(3, 1)}},
        {new Vector2(3, 1), new List<Vector2>{new Vector2(4, 1), new Vector2(2, 1), new Vector2(3, 2), new Vector2(3, 0)}},
        {new Vector2(3, 2), new List<Vector2>{new Vector2(4, 2), new Vector2(2, 2), new Vector2(3, 3), new Vector2(3, 1)}},
        {new Vector2(3, 3), new List<Vector2>{new Vector2(4, 3), new Vector2(2, 3), new Vector2(3, 4), new Vector2(3, 2)}},
        {new Vector2(3, 4), new List<Vector2>{new Vector2(4, 4), new Vector2(2, 4), new Vector2(3, 3)}},
        {new Vector2(4, 0), new List<Vector2>{new Vector2(3, 0), new Vector2(4, 1)}},
        {new Vector2(4, 1), new List<Vector2>{new Vector2(3, 1), new Vector2(4, 2), new Vector2(4, 0)}},
        {new Vector2(4, 2), new List<Vector2>{new Vector2(3, 2), new Vector2(4, 3), new Vector2(4, 1)}},
        {new Vector2(4, 3), new List<Vector2>{new Vector2(3, 3), new Vector2(4, 4), new Vector2(4, 2)}},
        {new Vector2(4, 4), new List<Vector2>{new Vector2(3, 4), new Vector2(4, 3)}}
    };
    private readonly List<GameObject> _optionObjs = new List<GameObject>();
    private Vector2 _manChessPos;
    private Vector2 _manChessRot;
    private Vector2 _girlChessPos;
    private Vector2 _girlChessRot;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize man and girl chess
        _manChessPos = new Vector2(0, 0);
        _girlChessPos = new Vector2(1, 1);
        _manChessRot = new Vector2(1, 0);
        _girlChessRot = new Vector2(0, 1);
        // Apply transform
        manChess.transform.position = chessBoard.ChessVecToRealVec(_manChessPos);
        girlChess.transform.position = chessBoard.ChessVecToRealVec(_girlChessPos);
        manChess.transform.rotation = Quaternion.LookRotation(chessBoard.ChessVecToRealVec(_manChessRot));
        girlChess.transform.rotation = Quaternion.LookRotation(chessBoard.ChessVecToRealVec(_girlChessRot));
        // Show options for the first time
        ShowOptions();
    }

    // Update is called once per frame
    void Update()
    {
        switch (_state)
        {
            case State.WaitInput:
            {
                break;
            }
            case State.AnimateManChess:
            {
                if (!manChess.IsAnimating)
                {
                    AutoMoveGirlChess();
                    _state = State.AnimateGirlChess;
                }
                break;
            }
            case State.AnimateGirlChess:
            {
                if (!girlChess.IsAnimating)
                {
                    _state = State.TurnFinished;
                }
                break;
            }
            case State.TurnFinished:
            {
                switch (_sceneIndex)
                {
                    case 0:
                    {
                        if (_manChessPos == new Vector2(4, 4))
                        {
                            
                        }
                        break;
                    }
                    case 1:
                    {
                        break;
                    }
                }
                ShowOptions();
                _state = State.WaitInput;
                break;
            }
        }

    }

    private void ShowOptions()
    {
        var optionCords = graphUtils.GetNeighbors(_graph, _manChessPos);
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
                    _state = State.AnimateManChess;
                });
            }
        }
    }

    private void MoveManChess(Vector2 dst)
    {
        var faceTowards = dst - _manChessPos;
        if (_manChessRot != faceTowards)
        {
            manChess.AddToSequence(AnimationSequence.Subject.Rotation, chessBoard.ChessVecToRealVec(_manChessRot),
                chessBoard.ChessVecToRealVec(faceTowards), 0.25f, AnimationSequence.Curve.Linear);
            _manChessRot = faceTowards;
        }
        manChess.AddToSequence(AnimationSequence.Subject.Position, chessBoard.ChessVecToRealVec(_manChessPos),
            chessBoard.ChessVecToRealVec(dst), 1, AnimationSequence.Curve.Quadratic);
        _manChessPos = dst;
    }

    private void AutoMoveGirlChess()
    {
        var dst = graphUtils.GetNextNodeFurthestTo(_graph, _girlChessPos, _manChessPos);
        if (dst == new Vector2(-1, -1))
            return;
        var faceTowards = dst - _girlChessPos;
        if (_girlChessPos != faceTowards)
        {
            girlChess.AddToSequence(AnimationSequence.Subject.Rotation, chessBoard.ChessVecToRealVec(_girlChessRot),
                chessBoard.ChessVecToRealVec(faceTowards), 0.25f, AnimationSequence.Curve.Linear);
            _girlChessRot = faceTowards;
        }
        girlChess.AddToSequence(AnimationSequence.Subject.Position, chessBoard.ChessVecToRealVec(_girlChessPos),
            chessBoard.ChessVecToRealVec(dst), 1, AnimationSequence.Curve.Quadratic);
        _girlChessPos = dst;
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
