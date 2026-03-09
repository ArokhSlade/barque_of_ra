using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System;
public class UISystem : MonoBehaviour
{
    [SerializeField]GameObject OnHover;
    [SerializeField] GameObject OnClickButton;

    GraphicRaycaster UIGraphicRaycast;
    EventSystem eventSys;
    List<RaycastResult> Result = new List<RaycastResult>();


    public enum ButtonStates
    {
        Clicked, Hover , Holding

    }
    [SerializeField]ButtonStates state;
    private void Awake()
    {
        UIGraphicRaycast = GetComponent<GraphicRaycaster>();
        eventSys = EventSystem.current;
    }

    private void Update()
    {
        
        PointerEventData eventData = new PointerEventData(eventSys);
        eventData.position = Input.mousePosition;




        if(eventSys.IsPointerOverGameObject())
        {
           
            eventData.position = Input.mousePosition;
            UIGraphicRaycast.Raycast(eventData, Result);
            if(Result.Count > 0 && Result[Result.Count - 1].gameObject.GetComponent<Button>())
            {
                OnHover = Result[Result.Count - 1].gameObject; OnHover.GetComponent<Animator>().SetBool("IsHovering", true);
            }
          

        }
        else if(OnHover)
        {
            OnHover.GetComponent<Animator>().SetBool("IsHovering", false);
        }
        if(eventSys.currentSelectedGameObject)
        {
            OnClickButton = eventSys.currentSelectedGameObject;
        }


    }
    public void OnPressingMouse(string Action)
    {

        state = ButtonStates.Clicked;
        OnClickButton.GetComponent<Animator>().SetBool("OnClicked",true);
        Invoke(Action, OnClickButton.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);

       Debug.Log(OnClickButton.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);

    }
    void OpenNewGame()
    {
        Debug.Log("Opening new game session");
    }
    void OnHoldingMouse()
    {
        state = ButtonStates.Holding;
        OnHover.GetComponent<Animator>().SetTrigger("OnHolding");
    }
    void OnHovering()
    {

        state = ButtonStates.Hover;
        OnHover.GetComponent<Animator>().SetTrigger("OnHover");
    }

    //Sceneloader and Changer 
}
