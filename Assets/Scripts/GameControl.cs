using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour
{
    public int GridSize = 2;
    public Transform ChessRef;
    public Transform ManChess;
    public Transform GirlChess;
    public GameObject OptionPrefab;

    private List<GameObject> _options = new List<GameObject>();
    private SmoothTransition _manChessXTransition = new SmoothTransition();
    private SmoothTransition _manChessYTransition = new SmoothTransition();
    private SmoothTransition _girlChessXTransition = new SmoothTransition();
    private SmoothTransition _girlChessYTransition = new SmoothTransition();
    private Point _manChessPos = new Point(2, 3);
    private Point _girlChessPos = new Point(3, 2);

    private Dictionary<Point, List<List<Point>>> _pathGraph = new Dictionary<Point, List<List<Point>>>{
        {
            new Point(2, 3), new List<List<Point>>{
                new List<Point>{new Point(2, 2)},
            }
        },{
            new Point(2, 2), new List<List<Point>>{
                new List<Point>{new Point(3, 2)},
            }
        },{
            new Point(3, 2), new List<List<Point>>{
                new List<Point>{new Point(4, 2)},
            }
        },{
            new Point(4, 2), new List<List<Point>>{
                new List<Point>{new Point(5, 2)},
            }
        },{
            new Point(5, 2), new List<List<Point>>{
                new List<Point>{new Point(5, 3)},
            }
        },{
            new Point(5, 3), new List<List<Point>>{
                new List<Point>{new Point(5, 4)},
            }
        },{
            new Point(5, 4), new List<List<Point>>{
                new List<Point>{new Point(4, 4)},
            }
        },{
            new Point(4, 4), new List<List<Point>>{
                new List<Point>{new Point(4, 5)},
            }
        },{
            new Point(4, 5), new List<List<Point>>{
                new List<Point>{new Point(3, 5)},
            }
        },{
            new Point(3, 5), new List<List<Point>>{
                new List<Point>{new Point(2, 5)},
            }
        },{
            new Point(2, 5), new List<List<Point>>{
                new List<Point>{new Point(2, 4)},
            }
        },{
            new Point(2, 4), new List<List<Point>>{
                new List<Point>{new Point(2, 3)},
            }
        }
    };

    // Start is called before the first frame update
    private void Start()
    {
        ShowOptions();
        ManChess.position = new Vector3(_manChessPos.X, 0, _manChessPos.Y) * GridSize + ChessRef.position;
        GirlChess.position = new Vector3(_girlChessPos.X, 0, _girlChessPos.Y) * GridSize + ChessRef.position;
    }

    // Update is called once per frame
    private void Update()
    {
        var prevIsManChessTransiting = _manChessXTransition.IsTransiting || _manChessYTransition.IsTransiting;
        _manChessXTransition.Update();
        _manChessYTransition.Update();
        var isManChessTransiting = _manChessXTransition.IsTransiting || _manChessYTransition.IsTransiting;
        if (isManChessTransiting)
        {
            ManChess.position = new Vector3(_manChessXTransition.CurrentValue, 0, _manChessYTransition.CurrentValue) * GridSize +
                                ChessRef.position;
        }
        if (!isManChessTransiting && prevIsManChessTransiting)
        {
            MoveGirl();
        }

        var prevIsGirlChessTransiting = _girlChessXTransition.IsTransiting || _girlChessYTransition.IsTransiting;
        _girlChessXTransition.Update();
        _girlChessYTransition.Update();
        var isGirlChessTransiting = _girlChessXTransition.IsTransiting || _girlChessYTransition.IsTransiting;
        if (isGirlChessTransiting)
        {
            GirlChess.position = new Vector3(_girlChessXTransition.CurrentValue, 0, _girlChessYTransition.CurrentValue) * GridSize +
                                 ChessRef.position;
        }
        if (!isGirlChessTransiting && prevIsGirlChessTransiting)
        {
            ShowOptions();
        }
    }

    private void ShowOptions()
    {
        foreach (var path in _pathGraph[_manChessPos])
        {
            var lastPoint = path[path.Count - 1];
            var pos = new Vector3(lastPoint.X, 0, lastPoint.Y) * GridSize + ChessRef.position;
            var go = Instantiate(OptionPrefab, pos, Quaternion.identity);
            ClickableObject co = go.GetComponent<ClickableObject>();
            if (co != null)
            {
                _options.Add(go);
                co.OnClicked = () => OnOptionClicked(path);
            }
        }
    }

    private void OnOptionClicked(List<Point> path)
    {
        ClearOptions();
        AnimateManChess(path);
    }

    private void ClearOptions()
    {
        foreach (var option in _options)
        {
            Destroy(option);
        }
        _options.Clear();
    }

    private void AnimateManChess(List<Point> path)
    {
        foreach (var point in path)
        {
            _manChessXTransition.AddAction(1f, _manChessPos.X, point.X, SmoothTransition.Quadratic);
            _manChessYTransition.AddAction(1f, _manChessPos.Y, point.Y, SmoothTransition.Quadratic);
            _manChessPos = point;
        }
    }

    private void MoveGirl()
    {
        AnimateGirlChess(_pathGraph[_girlChessPos][0]);
    }

    private void AnimateGirlChess(List<Point> path)
    {
        foreach (var point in path)
        {
            _girlChessXTransition.AddAction(1f, _girlChessPos.X, point.X, SmoothTransition.Quadratic);
            _girlChessYTransition.AddAction(1f, _girlChessPos.Y, point.Y, SmoothTransition.Quadratic);
            _girlChessPos = point;
        }
    }
}
