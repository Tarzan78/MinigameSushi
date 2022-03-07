using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinjaGameManager : MonoBehaviour
{
    //NOTES
    //Put the speed equal for all by using delta time

    //Aux class 
    /// <summary>
    /// scriptable obj the best way
    /// </summary>
    //public class IngredientsPack : MonoBehaviour
    //{
    //    public int id;
    //    public GameObject mainIngredient;
    //    public GameObject leftSideIngredient;
    //    public GameObject rigthSideIngredient;
    //}

    //Variables 
    private bool inDebug = true;
    private bool testingCorners = false;
    private bool testingCornersMarginOfError = false;
    [SerializeField] GameObject ingredientSalmonUI;
    [SerializeField] GameObject ingredientWasabiUI;
    [SerializeField] List<GameObject> ingredientsPrefab;
    private RectTransform rectTransform;
    public Vector3[] panelCorners = new Vector3[4];
    public Vector3[] panelCornersMarginOfError = new Vector3[4];
    private List<GameObject> activeIngredients = new List<GameObject>();
    private List<GameObject> slashedIngredients = new List<GameObject>();
    [SerializeField] List<GameObject> slashedIngredientsPrefabs = new List<GameObject>();
    private float speed = 150f;
    public float marginOfError; //0.5f is the center
    private int score = 0;



    // Start is called before the first frame update
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        panelCorners = GetAllPanelCorners(rectTransform);
        panelCornersMarginOfError = GetAllPanelCornersMarginOfError(rectTransform);
        //SpawnnIngredients();
        SpawnIngredientsManager();
    }

    // Update is called once per frame
    void Update()
    {
        //SpawnIngredientsManager();
        
    }

    public  void StratNinjaGame()
    {

    } 

    private void SpawnIngredientsManager()
    {
        //for (int i = 0; i < 4; i++)
        //{
        //    StartCoroutine(spawnIngredientDelayed(2f));
        //}

        InvokeRepeating("SpawnnIngredients", 0f, 0.5f);
       
    }

    //private IEnumerator spawnIngredientDelayed(float spawnDelay)
    //{
    //    while (true)
    //    {
    //        yield return new WaitForSeconds(spawnDelay);
    //
    //        SpawnnIngredients();
    //    }
    //}

    private void SpawnnIngredients()
    {
        (Vector3 spawnPoint, Vector3 direction) spawnInfo;
        GameObject tempOBJ;
        bool slashable;

        spawnInfo = RandomSpawnAndDirectionInPanel(panelCornersMarginOfError);

        if (Random.Range(0,2) % 2 == 0)
        {
            tempOBJ = ingredientSalmonUI;
            slashable = true;
        }
        else
        {
            tempOBJ = ingredientWasabiUI;
            slashable = false;
        }
        
        Debug.Log("Spawning ");
        GameObject spawnObj = Instantiate(tempOBJ, spawnInfo.spawnPoint, Quaternion.identity, this.transform);
        spawnObj.GetComponent<DragAndDropUI>().direction = spawnInfo.direction * speed;
        spawnObj.GetComponent<DragAndDropUI>().ninjaGameManager = this;
        spawnObj.GetComponent<DragAndDropUI>().slashable = slashable;

        activeIngredients.Add(spawnObj);


        if (testingCorners)
        {
            for (int i = 0; i < 4; i++)
            {
                spawnInfo = RandomSpawnAndDirectionInPanel(panelCornersMarginOfError);

                Debug.Log("Spawning ");
                GameObject spawnObjTest = Instantiate(ingredientSalmonUI, spawnInfo.spawnPoint, Quaternion.identity, this.transform);
                spawnObjTest.GetComponent<DragAndDropUI>().direction = spawnInfo.direction * speed;
                spawnObjTest.GetComponent<DragAndDropUI>().ninjaGameManager = this;

                //GameObject.Destroy(spawnObj);

                activeIngredients.Add(spawnObjTest);
            }
        }
    }

    private void SpawnSlashedIngredients(GameObject ingredientSlashed)
    {
        (Vector3 spawnPoint, Vector3 direction) spawnInfo;
        Vector3 rotation = new Vector3(0,0,0);
        GameObject tempOBJ;

        //(GameObject leftSide, GameObject rightSide) slashedIngredient = GetSlashedIngredient(ingredientSlashed);

        for (int i = 0; i < 2; i++)
        {
            if (i == 0)
            {
                tempOBJ = slashedIngredientsPrefabs[0];
                spawnInfo.direction = new Vector3(-0.1f, -1, 0);
                rotation = new Vector3(0, 0, 0.1f);
            }
            else
            {
                tempOBJ = slashedIngredientsPrefabs[1];
                spawnInfo.direction = new Vector3(0.1f, -1, 0);
                rotation = new Vector3(0, 0, -0.1f);
            }

            spawnInfo.spawnPoint = ingredientSlashed.transform.position;
            

            GameObject spawnObjTest = Instantiate(tempOBJ, spawnInfo.spawnPoint, Quaternion.identity, this.transform);
            spawnObjTest.GetComponent<DragAndDropUI>().direction = spawnInfo.direction * speed;
            spawnObjTest.GetComponent<DragAndDropUI>().ninjaGameManager = this;
            spawnObjTest.GetComponent<DragAndDropUI>().slashedIngredient = true;
            spawnObjTest.GetComponent<DragAndDropUI>().rotation = rotation * speed;

            slashedIngredients.Add(spawnObjTest);
        }
        
    }

    //Get the plane corners vectors in world from ninja game
    //0 -> Down Left
    //1 -> Up Left
    //2 -> Up Right
    //3 -> Down Right
    private Vector3[] GetAllPanelCorners(RectTransform rect)
    {
        Vector3[] corners = new Vector3[4];

        rect.GetWorldCorners(corners);

        if (inDebug)
        {
            Debug.Log("Panel corners");

            for (int i = 0; i < 4; i++)
            {
                Debug.Log("Index : " + i + "Pos " + corners[i]);
            }
        }

        return corners;
    }

    //Get the plane corners vectors in world from ninja game with margin of error
    //0 -> Down Left
    //1 -> Up Left
    //2 -> Up Right
    //3 -> Down Right
    private Vector3[] GetAllPanelCornersMarginOfError(RectTransform rect)
    {
        Vector3[] corners = new Vector3[4];
        //bool check = false;
        float marginHSize = 0;
        float marginWSize = 0;
        rect.GetWorldCorners(corners);

        float height = Mathf.Abs(corners[2].y - corners[0].y);
        float width = Mathf.Abs(corners[2].x - corners[0].x);

        marginHSize = height * marginOfError;
        marginWSize = width * marginOfError;

        corners[0].x += marginWSize;//min
        corners[0].y += marginHSize;
        corners[1].x += marginWSize;
        corners[1].y -= marginHSize;
        corners[2].x -= marginWSize;//max
        corners[2].y -= marginHSize;
        corners[3].x -= marginWSize;
        corners[3].y += marginHSize;

        if (testingCornersMarginOfError)
        {
            Debug.Log("h size -> " + height);
            Debug.Log("w size-> " + width);
            Debug.Log("margin h size -> " + marginHSize);
            Debug.Log("margin w size-> " + marginWSize);

            for (int i = 0; i < 4; i++)
            {
                Debug.Log("Spawning ");
                GameObject spawnObjTest = Instantiate(ingredientSalmonUI, corners[i], Quaternion.identity, this.transform);
                spawnObjTest.GetComponent<DragAndDropUI>().ninjaGameManager = this;
            }
        }

        if (inDebug)
        {
            Debug.Log("Panel corners with margin of error");
            for (int i = 0; i < 4; i++)
            {
                Debug.Log("Index : " + i + "Pos " + corners[i]);
            }
        }

        return corners;
    }

    private (Vector3 spawnPoint, Vector3 direction) RandomSpawnAndDirectionInPanel(Vector3[] panelCornersTemp)
    {
        //Select the edge from the panel 1 of 4
        int randomValue;
        Vector3 spawnPoint = new Vector3(); //HMMM n sei se vai funfar instancear o mesmo várias vezes sunno vamos ver o que dá
        Vector3 direction = new Vector3();
        (Vector3 spawnPoint, Vector3 direction) spawnInfo;

        randomValue = Random.Range(0, 3);

        //randomValue = 3;
        //- > x
        //^
        //| y

        //Up
        if (randomValue == 0)
        {
            //same y
            float leftX = panelCornersTemp[1].x;
            float rightX = panelCornersTemp[2].x;
            float randomX = Random.Range(leftX, rightX);

            //Spawn Point 
            spawnPoint = new Vector3(randomX, panelCornersTemp[1].y, 0f);

            //Direction
            direction = new Vector3(0, -1, 0);
        }
        //Down
        else if (randomValue == 1)  
        {
            //same y
            float leftX  = panelCornersTemp[0].x;
            float rightX = panelCornersTemp[3].x;
            float randomX = Random.Range(leftX, rightX);

            //Spawn Point 
            spawnPoint = new Vector3(randomX, panelCornersTemp[0].y, 0f);

            //Direction
            direction = new Vector3(0, 1, 0);
        }
        //Left
        else if (randomValue == 2)
        {
            //same x
            float UpY = panelCornersTemp[1].y;
            float DownY = panelCornersTemp[0].y;
            float randomY = Random.Range(DownY, UpY);

            //Spawn Point 
            spawnPoint = new Vector3(panelCornersTemp[0].x, randomY, 0f);

            //Direction
            direction = new Vector3(1, 0, 0);
        }
        //Right
        else if (randomValue == 3)
        {
            //same x
            float UpY = panelCornersTemp[2].y;
            float DownY = panelCornersTemp[3].y;
            float randomY = Random.Range(DownY, UpY);

            //Spawn Point 
            spawnPoint = new Vector3(panelCornersTemp[2].x, randomY, 0f);

            //Direction
            direction = new Vector3(-1 , 0, 0);
        }
      
        spawnInfo.spawnPoint = spawnPoint;
        spawnInfo.direction = direction;

        return spawnInfo;
    }

    public void DestroyIngredient(Transform ingredientT)
    {
        //activeIngredients.FindIndex(o => o == ingredient);
        //activeIngredients.Remove(o => o.transforme == ingredientT);
        //GameObject tempObj = new GameObject();

        //int index = 0;

        if (inDebug)
        {
            Debug.Log("Destroying ingridient called");
        }

        for (int i = 0; i < activeIngredients.Count; i++)
        {
            if (activeIngredients[i].transform == ingredientT)
            {
                if (inDebug)
                {
                    Debug.Log("Destroying ingridient");
                }

                if (inDebug)
                {
                    foreach (GameObject item in activeIngredients)
                    {
                        Debug.Log("info pos" + item.transform.position);
                    }
                }

                //activeIngredients[i].SetActive(false);
                GameObject.Destroy(activeIngredients[i]);
                activeIngredients.RemoveAt(i);

                break;
            }
        }

        //foreach (var i in activeIngredients)
        //{
        //    if (i.transform == ingredientT)
        //    {
        //        //if (inDebug)
        //        //{
        //        //    Debug.Log("Destroying ingridient");
        //        //}
        //
        //        GameObject.Destroy(i);
        //        activeIngredients.Remove(i);
        //
        //        if (inDebug)
        //        {
        //            foreach (GameObject item in activeIngredients)
        //            {
        //                Debug.Log("info " + item.transform.position);
        //            }
        //        }
        //
        //        break;
        //    }
        //
        //    index++;
        //}
    }

    public void DestroyIngredientByClick(Transform ingredientT)
    {
        //activeIngredients.FindIndex(o => o == ingredient);
        //activeIngredients.Remove(o => o.transforme == ingredientT);
        //GameObject tempObj = new GameObject();
        int index = 0;

        //GameObject.Destroy(activeIngredients[0]); //it destroy and create new empty obj

        //for (int i = 0; i < activeIngredients.Count; i++)
        //{
        //    GameObject.Destroy(activeIngredients[i]);
        //}
        //activeIngredients.Clear();

        foreach (var i in activeIngredients)
        {
            if (i.transform == ingredientT)
            {
                //tempObj = i;

                if (inDebug)
                {
                    Debug.Log("Destroying ingridient");
                }

                //Spawn Slashed Ingredients
                SpawnSlashedIngredients(i);

                GameObject.Destroy(i);
                activeIngredients.Remove(i);


                if (inDebug)
                {
                    foreach (GameObject item in activeIngredients)
                    {
                        Debug.Log("info " + item.transform.position);
                    }

                }

                //activeIngredients.RemoveAt(index);

                break;
            }

            index++;
        }

        if (inDebug)
        {
            foreach (GameObject item in activeIngredients)
            {
                Debug.Log("info " + item.transform.position);
            }

        }

        //activeIngredients.RemoveAt(index);
        //GameObject.Destroy(tempObj);

        //GameObject.Destroy(activeIngredients[index]);
    }

    public void DestroySlashedIngredient(Vector3 ingredientSlashedT)
    {
        //activeIngredients.FindIndex(o => o == ingredient);
        //activeIngredients.Remove(o => o.transforme == ingredientT);
        //GameObject tempObj = new GameObject();
        //int index = 0;

        //GameObject.Destroy(activeIngredients[0]); //it destroy and create new empty obj

        //for (int i = 0; i < activeIngredients.Count; i++)
        //{
        //    GameObject.Destroy(activeIngredients[i]);
        //}
        //activeIngredients.Clear();

        foreach (var i in slashedIngredients)
        {
            if (i.transform.position == ingredientSlashedT)
            {
                //tempObj = i;

                if (inDebug)
                {
                    Debug.Log("Destroying Slashed ingridient");
                }

                GameObject.Destroy(i);
                slashedIngredients.Remove(i);
                //activeIngredients.RemoveAt(index);

                break;
            }

            //index++;
        }

        //if (inDebug)
        //{
        //    foreach (GameObject item in activeIngredients)
        //    {
        //        Debug.Log("info " + item.transform.position);
        //    }
        //
        //}

        //activeIngredients.RemoveAt(index);
        //GameObject.Destroy(tempObj);

        //GameObject.Destroy(activeIngredients[index]);
    }


    /// <summary>
    /// This method is to verify if a pos is inside the panel area
    /// </summary>
    /// <param name="targetPos"> Pos to check </param>
    /// <returns>True -> inside False -> not inside</returns>
    public bool VerifyIfIsInPanel(Vector3 targetPos, Vector3[] corners)
    {
        //variables 
        float minX = corners[0].x;
        float minY = corners[0].y;
        float maxX = corners[2].x;
        float maxY = corners[2].y;
        bool check = false;

        //check if it´s inside 
        if (minX <= targetPos.x && minY <= targetPos.y && maxX >= targetPos.x && maxY >= targetPos.y)
        {
            check = true;
        }

        return check;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="targetPos">Pos to check</param>
    /// <param name="marginOfError"> [0:1] this value is to adjust the borders from panel</param>
    /// <returns>True -> inside False -> not inside</returns>
    //public bool VerifyIfIsInPanel(Vector3 targetPos, float marginOfError)
    //{
    //    //variables 
    //    float minX = panelCorners[0].x;
    //    float minY = panelCorners[0].y;
    //    float maxX = panelCorners[2].x;
    //    float maxY = panelCorners[2].y;
    //    bool check = false;
    //    float marginHSize = 0;
    //    float marginWSize = 0;
    //
    //    float height = Mathf.Abs(maxY - minY);
    //    float width = Mathf.Abs(maxX - minX);
    //
    //    marginHSize = height * marginOfError;
    //    marginWSize = width * marginOfError;
    //
    //    minX += marginWSize;
    //    minY += marginHSize;
    //    maxX -= marginWSize;
    //    maxY -= marginHSize;
    //
    //    //check if it´s inside 
    //    if (minX <= targetPos.x && minY <= targetPos.y && maxX >= targetPos.x && maxY >= targetPos.y)
    //    {
    //        check = true;
    //    }
    //
    //    return check;
    //}

    /// <returns> null -> not fund</returns>
    private (GameObject leftSide, GameObject rightSide) GetSlashedIngredient (GameObject ingredient)
    {
        (GameObject leftSide, GameObject rightSide) slashedIngredient = (null, null);

        for (int i = 0; i < ingredientsPrefab.Count; i++)
        {
            if (ingredientsPrefab[i].transform.position == ingredient.transform.position)
            {
                Debug.Log("find ingredient");

                slashedIngredient = (slashedIngredients[i * 2], slashedIngredients[i * 2 + 1]);
            }
        }

        return slashedIngredient;
    }

    public void ScoreUP()
    {
        score++;

        Debug.Log("Score -> " + score);
    }


    public void ScoreDown()
    {
        
        score--;

        Debug.Log("Score -> " + score);
    }

}
