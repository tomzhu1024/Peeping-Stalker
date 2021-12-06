using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour
{
    [SerializeField] private ChessBoard chessBoard;
    [SerializeField] private GraphUtils graphUtils;
    [SerializeField] private AnimationSequence manChess;
    [SerializeField] private AnimationSequence girlChess;
    [SerializeField] private GameObject optionPrefab;
    [SerializeField] private AnimationSequence chessGroup;
    [SerializeField] private AnimationSequence dayScene;
    [SerializeField] private AnimationSequence nightScene;

    private enum State
    {
        WaitInput,
        AnimateManChess,
        AnimateGirlChess,
        TurnFinished,
        SwitchToNight,
        SwitchToDay
    }

    private State _state = State.WaitInput;
    private int _sceneIndex = 0;
    private int _stepCount = 0;

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
        _girlChessPos = new Vector2(2, 2);
        _manChessRot = new Vector2(1, 0);
        _girlChessRot = new Vector2(0, 1);
        // Apply transform
        manChess.transform.position = chessBoard.ChessVecToRealVec(_manChessPos);
        girlChess.transform.position = chessBoard.ChessVecToRealVec(_girlChessPos);
        manChess.transform.rotation = Quaternion.LookRotation(chessBoard.ChessVecToRealVec(_manChessRot));
        girlChess.transform.rotation = Quaternion.LookRotation(chessBoard.ChessVecToRealVec(_girlChessRot));
        // Show options for the first time
        ShowOptions();
        nightScene.AddToSequence(AnimationSequence.Subject.Disable, Vector3.zero, Vector3.zero, 0, AnimationSequence.Curve.Linear);
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
                // Check win/lose
                switch (_sceneIndex)
                {
                    case 0:
                    {
                        if (graphUtils.GetDistance(_graph, _manChessPos, _girlChessPos) <= 2)
                        {
                            StartSwitchToNight();
                            _stepCount = 0;
                            _sceneIndex = 1;
                            _state = State.SwitchToNight;
                        } else if (_stepCount > 6)
                        {
                            SetFail();
                        }
                        break;
                    }
                    case 1:
                    {
                        if (_manChessPos == new Vector2(0, 0))
                        {
                            StartSwitchToDay();
                            _stepCount = 0;
                            _sceneIndex = 2;
                            _state = State.SwitchToDay;
                        } else if (_manChessPos == _girlChessPos)
                        {
                            SetFail();
                        }
                        break;
                    }
                    case 2:
                    {
                        if (graphUtils.GetDistance(_graph, _manChessPos, _girlChessPos) <= 1)
                        {
                            SetWin();
                        }
                        break;
                    }
                }
                // Prevent going into a new turn if scene needs switching
                if (_state == State.TurnFinished)
                {
                    ShowOptions();
                    _state = State.WaitInput;
                }
                break;
            }
            case State.SwitchToNight:
            {
                if (!nightScene.IsAnimating)
                {   
                    EndSwitchToNight();
                    ShowOptions();
                    _state = State.WaitInput;
                }
                break;
            }
            case State.SwitchToDay:
            {
                if (!dayScene.IsAnimating)
                {   
                    print("Switch to day done!");
                    EndSwitchToDay();
                    ShowOptions();
                    _state = State.WaitInput;
                }
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
            manChess.AddToSequence(AnimationSequence.Subject.RotationTowards, chessBoard.ChessVecToRealVec(_manChessRot),
                chessBoard.ChessVecToRealVec(faceTowards), 0.25f, AnimationSequence.Curve.Linear);
            _manChessRot = faceTowards;
        }
        manChess.AddToSequence(AnimationSequence.Subject.Position, chessBoard.ChessVecToRealVec(_manChessPos),
            chessBoard.ChessVecToRealVec(dst), 1, AnimationSequence.Curve.Quadratic);
        _manChessPos = dst;
        _stepCount++;
    }

    private void AutoMoveGirlChess()
    {
        Vector2 dst;
        switch (_sceneIndex)
        {
            case 0:
            {
                dst = graphUtils.GetNextNodeFurthestTo(_graph, _girlChessPos, _manChessPos);
                break;
            }
            case 1:
            {
                dst = graphUtils.GetNextNodeClosestTo(_graph, _girlChessPos, _manChessPos);
                break;
            }
            case 2:
            {
                dst = graphUtils.GetNextNodeFurthestTo(_graph, _girlChessPos, _manChessPos);
                break;
            }
            default:
            {
                return;
            }
        }
        if (dst == new Vector2(-1, -1))
            return;
        var faceTowards = dst - _girlChessPos;
        if (_girlChessPos != faceTowards)
        {
            girlChess.AddToSequence(AnimationSequence.Subject.RotationTowards, chessBoard.ChessVecToRealVec(_girlChessRot),
                chessBoard.ChessVecToRealVec(faceTowards), 0.25f, AnimationSequence.Curve.Linear);
            _girlChessRot = faceTowards;
        }
        girlChess.AddToSequence(AnimationSequence.Subject.Position, chessBoard.ChessVecToRealVec(_girlChessPos),
            chessBoard.ChessVecToRealVec(dst), 1, AnimationSequence.Curve.Quadratic);
        _girlChessPos = dst;
    }

    private void StartSwitchToNight()
    {
        chessGroup.AddToSequence(AnimationSequence.Subject.Disable, Vector3.zero, Vector3.zero, 0, AnimationSequence.Curve.Linear);
        nightScene.AddToSequence(AnimationSequence.Subject.Enable, Vector3.zero, Vector3.zero, 0, AnimationSequence.Curve.Linear);
        nightScene.AddToSequence(AnimationSequence.Subject.RotationEuler, new Vector3(0, 0, 180), new Vector3(0, 0, 360), 3,
            AnimationSequence.Curve.Quadratic);
        dayScene.AddToSequence(AnimationSequence.Subject.RotationEuler, new Vector3(0, 0, 0), new Vector3(0, 0, 180), 3,
            AnimationSequence.Curve.Quadratic);
    }

    private void EndSwitchToNight()
    {
        dayScene.AddToSequence(AnimationSequence.Subject.Disable, Vector3.zero, Vector3.zero, 0, AnimationSequence.Curve.Linear);
        chessGroup.AddToSequence(AnimationSequence.Subject.Enable, Vector3.zero, Vector3.zero, 0, AnimationSequence.Curve.Linear);
    }

    private void StartSwitchToDay()
    {
        chessGroup.AddToSequence(AnimationSequence.Subject.Disable, Vector3.zero, Vector3.zero, 0, AnimationSequence.Curve.Linear);
        dayScene.AddToSequence(AnimationSequence.Subject.Enable, Vector3.zero, Vector3.zero, 0, AnimationSequence.Curve.Linear);
        dayScene.AddToSequence(AnimationSequence.Subject.RotationEuler, new Vector3(0, 0, 180), new Vector3(0, 0, 360), 3,
            AnimationSequence.Curve.Quadratic);
        nightScene.AddToSequence(AnimationSequence.Subject.RotationEuler, new Vector3(0, 0, 0), new Vector3(0, 0, 180), 3,
            AnimationSequence.Curve.Quadratic);
    }
    
    private void EndSwitchToDay()
    {
        nightScene.AddToSequence(AnimationSequence.Subject.Disable, Vector3.zero, Vector3.zero, 0, AnimationSequence.Curve.Linear);
        chessGroup.AddToSequence(AnimationSequence.Subject.Enable, Vector3.zero, Vector3.zero, 0, AnimationSequence.Curve.Linear);
    }

    private void SetWin()
    {
        SceneManager.LoadScene("Victory");
    }

    private void SetFail()
    {
        SceneManager.LoadScene("GameOver");
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
