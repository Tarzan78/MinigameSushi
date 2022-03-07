using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDropUI : MonoBehaviour, IPointerDownHandler/*, IBeginDragHandler, IEndDragHandler, IDragHandler*/
{
    //Variables 
    private bool inDebug = true;
    bool isInsideCanvas;
    private RectTransform rectTransform;
    public Vector3 direction;
    public Vector3 rotation = new Vector3(0,0,0);
    public NinjaGameManager ninjaGameManager;
    public bool slashable = false;
    [SerializeField] private Canvas mainCanvas;
    public bool slashedIngredient;


    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    //Start detecting the drag by a delta 
    //public void OnBeginDrag(PointerEventData eventData)
    //{
    //    if (inDebug)
    //    {
    //        Debug.Log("On Begin Drag");
    //    }
    //}

    //its the drag update func exc every frame 
    //public void OnDrag(PointerEventData eventData)
    //{
    //    if (inDebug)
    //    {
    //        Debug.Log("Draging");
    //    }
    //
    //    //This is needed because the delta is in scale 1:1 probably normalized dunno  
    //    rectTransform.anchoredPosition += eventData.delta / mainCanvas.scaleFactor;
    //}

    //detect when it stop 
    //public void OnEndDrag(PointerEventData eventData)
    //{
    //    if (inDebug)
    //    {
    //        Debug.Log("End Drag");
    //    }
    //}

    //This method detect when ur mouse is clicking in the obj
    public void OnPointerDown(PointerEventData eventData)
    {
        if (inDebug)
        {
            Debug.Log("On pointer down ");
        }

        if (slashable)
        {

            if (!slashedIngredient)
            {
                //houston we may have a problem, this method is called twice per update so is trying to destroy obj that don't exist, now we will use some true or false variable, and will check it every time.
                ninjaGameManager.DestroyIngredientByClick(this.transform);
                ninjaGameManager.ScoreUP();
            }
            //else
            //{
            //    ninjaGameManager.DestroySlashedIngredient(this.transform.position);
            //}
        }
        else
        {
            ninjaGameManager.ScoreDown();
        }


    }

    private void Update()
    {
        rectTransform.anchoredPosition += (Vector2)direction * Time.deltaTime;

        rectTransform.eulerAngles += rotation * Time.deltaTime;

        //isInsideCanvas = ninjaGameManager.VerifyIfIsInPanel(this.transform.position);
        isInsideCanvas = ninjaGameManager.VerifyIfIsInPanel(this.transform.position, ninjaGameManager.panelCornersMarginOfError);

        if (!isInsideCanvas)
        {
            if (!slashedIngredient)
            {
                //GameObject.Destroy(this);
                ninjaGameManager.DestroyIngredient(this.transform);
            }
            else
            {
                if (inDebug)
                {
                    Debug.Log("Destroy slashed ingredient ");
                }
        
                ninjaGameManager.DestroySlashedIngredient(this.transform.position);
            }
            
        }
    }
}
