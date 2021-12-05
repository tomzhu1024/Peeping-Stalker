using UnityEngine;
using UnityEngine.EventSystems;

public class ClickableObject : MonoBehaviour, IPointerClickHandler
{
    public delegate void DelCallback();
    private DelCallback _cb;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCallback(DelCallback cb)
    {
        _cb = cb;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _cb();
    }
}
