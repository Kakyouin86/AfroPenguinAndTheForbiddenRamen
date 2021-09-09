using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))] //whenever we add this item, whenever at the script to an object it's going to add a Box Collider.
//By default.
public class Item : MonoBehaviour
{    
    public enum InteractionType { NONE, PickUp, Examine,GrabDrop } //List of alphanumerical objects, and each of them has an identifier (like an array, 0,1,2,3). Best practice: always "NONE" at the beginning.
    public enum ItemType { Staic, Consumables}
    [Header("Attributes")]
    public InteractionType interactType;
    public ItemType type;
    [Header("Exmaine")]
    public string descriptionText;
    [Header("Custom Events")]
    public UnityEvent customEvent;
    public UnityEvent consumeEvent;

    private void Reset()
    {
        GetComponent<Collider2D>().isTrigger = true; //Any collider will trigger.
        gameObject.layer = 10;
    }

    public void Interact()
    {
        switch(interactType)
        {
            case InteractionType.PickUp:
                //Add the object to the PickedUpItems list
                FindObjectOfType<InventorySystem>().PickUp(gameObject); //It looks all over the scene for an object that has this specific script.
                //Disable
                gameObject.SetActive(false);
                break;
            case InteractionType.Examine:
                //Call the Examine item in the interaction system
                FindObjectOfType<InteractionSystem>().ExamineItem(this);                
                break;
            case InteractionType.GrabDrop:
                //Grab interaction
                FindObjectOfType<InteractionSystem>().GrabDrop();
                break;
            default:
                Debug.Log("NULL ITEM");
                break;
        }

        //Invoke (call) the custom event(s)
        customEvent.Invoke();
    }
}
