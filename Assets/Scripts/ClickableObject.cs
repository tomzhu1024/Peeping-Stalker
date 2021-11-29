using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickableObject : MonoBehaviour, IPointerClickHandler
{
    public delegate void DelCallback();
    public DelCallback OnClicked;
    public (int, int) Position;
    
    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {

    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        OnClicked();
    }
}
