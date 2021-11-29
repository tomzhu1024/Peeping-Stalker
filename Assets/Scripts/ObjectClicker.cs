using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectClicker : MonoBehaviour, IPointerDownHandler
{
    // Start is called before the first frame update
    private void Start()
    {
        AddPhysicsRaycaster();
    }

    // Update is called once per frame
    private void Update()
    {

    }

    private void AddPhysicsRaycaster()
    {
        var physicsRaycaster = FindObjectOfType<PhysicsRaycaster>();
        if (physicsRaycaster == null)
            Camera.main.gameObject.AddComponent<PhysicsRaycaster>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Clicked: " + eventData.pointerCurrentRaycast.gameObject.name);
    }

}
