using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//make it faster every 10 or so
//make it chnage colors
//make placing assist
//add music
//add intro screen

public class Stacker : MonoBehaviour
{
    //change this within script, if 0, then x axis, if 1, then z axis
    public float side = 0f;
    public float minX = 0f;
    public float timerdirection = 0f;
    public float minZ = 0f;
    public float timer = 0f;
    public bool slideblock = false; //made false to not start game immediately and to wait for key response.
    public float oldBlockX = 10.22f; //will change these to fit the size of the old block, to start off it's only the size of the base block
    public float oldBlockZ = 0.91f;
    public float BlockY = 16.07f;
    public int scoreCount = 0;
    public float slideSpeed = 3f;
    public int slideCounter = 0;
    public Scorer handleScore;
    public CamMover moveCamera;
    List<GameObject> blocksList;
    public bool fail = false;
    //Camera maincam;
    public int accessThisIndex = 0; //will use to access whatever index of gameobject from list. So if one before, do accessThisIndex-1
    //Color customColor;



    //public float max = 8f;
    void Start()
    {
        //maincam = GameObject.Find("Main Camera").GetComponent<Camera>();
        minX = transform.position.x;
        minZ = -5.01f;
        blocksList = new List<GameObject>();
        GameObject g = GameObject.Find("SlidingBlock1");//finds the beginning sliding block to add to list
                                                        //to be able to use my code for whatever game object, just accessing it from the list
        /*
        customColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        var gRenderer = g.GetComponent<Renderer>();
        gRenderer.material.SetColor("_Color", customColor);
        */
        //Gizmos.DrawWireCube
        blocksList.Add(g);
        slideblock = false;
        slideSpeed = 3;
        slideCounter = 0;
        handleScore.StartText(); //displays "stacker"
        handleScore.startBeginText(); //displays the begin instruction
        //max = transform.position.x+10;
        
    }

    // Update is called once per frame
    void Update()
    {
        //if the block is allowed to slide
        //then the time will keep running for the pingpong function

        //once S is pressed, and game hasn't ended, allow for you to start game
        if(Input.GetKeyDown("s") && fail !=true)
        {
            handleScore.increaseScore(scoreCount); //set text equal to 0
            handleScore.deleteBegin();
            slideblock = true; //allow block to slide
        }

        

        //make another for the z side, so like if(slideblock==true && side == 1)... add the slide part to this one too
        //multiply the Time.deltaTime * variable of speed to increase the speed gradually.
        if(slideblock==true && side==0)
        {
        //changes the position of the sliding block by using pingpong function, function which goes back and forth from 0 to length.
        //pingpong uses (t, ) which is a variable that changes by itself to move constantly, such as time
        //uses ( , num) to know the length to go
        timer += Time.deltaTime*slideSpeed;
        blocksList[accessThisIndex].transform.position =new Vector3(Mathf.PingPong(timer,12)+minX, BlockY, blocksList[accessThisIndex].transform.position.z);
        
        }

        ///////////////////////////////////////////////
        
        if(slideblock==true && side==1)
        {
        timer += Time.deltaTime*slideSpeed;   
        //changes the position of the sliding block by using pingpong function, function which goes back and forth from 0 to length.
        //pingpong uses (t, ) which is a variable that changes by itself to move constantly, such as time
        //uses ( , num) to know the length to go
        //since the accessThisIndex would have gone up, it would access the other GameObject
        blocksList[accessThisIndex].transform.position =new Vector3(blocksList[accessThisIndex].transform.position.x, blocksList[accessThisIndex].transform.position.y, Mathf.PingPong(timer,12) - 5.01f); //Mathf.PingPong(timer,12)-5.01f
        }
        
        ////////////////////////////////////////////////
        //this will check that game hasn't ended to ask if you want to play again
        if(fail==true)
        {
            //GameObject po1 = GameObject.Find("GoToThis");
            //po1.transform.position = new Vector3(18.46f; transform.position.y, 9.06f);
            moveCamera.panOut();
            handleScore.playAgainText();
            if (Input.GetKeyDown("a"))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        
        }

        //if space is clicked and the game has started, then allow for the space to work
        if (Input.GetKeyDown("space") && slideblock == true)
        {
            //Debug.Log("space key was pressed");
            slideblock=false; //makes it so time stop at where it's at so pingpong stops
            
            if(side==0)
            {
            chopOffOnX();    
            }

            
            else if(side==1)
            {
            chopOffOnZ();
            }
            
            
        }
    }

    public void chopOffOnX() //make sure to replace the BlockY
    {
        //float oldBlockX = 10.22f; //will change these to fit the size of the old block, to start off it's only the size of the base block
        //float oldBlockZ = 0.91f;

        //if the size of the list is greater than one, then get the x of the old block
        //if it's 0, then the old blockX will be 10.22
        if(blocksList.Count != 1)
        oldBlockX = blocksList[accessThisIndex-1].transform.position.x;

        float currentx = blocksList[accessThisIndex].transform.position.x;
        float currenty = blocksList[accessThisIndex].transform.position.y;
        float CurrentBlockY = BlockY + 1.07f;
        //float currentz = transform.position.z;
        float direction = 0;

        //MAKE USE OF OLDBLOCKY AND ADDING VALUE, CUZ RN IT ONLY WORKS WITH THE BEGINNING 
        //hangover will be the part of the block that is hanging over hwen you click the spacebar
        float hangOver = currentx - oldBlockX;
        //adjusts the size of the block on the X axis
        float newXSize;
        newXSize = blocksList[accessThisIndex].transform.localScale.x - Mathf.Abs(hangOver);
        float fallingBlockSizeX = blocksList[accessThisIndex].transform.localScale.x - newXSize; //subtract the size of the block minus the size of the new block to 
                                                                    //determine how big the size of the falling block is.
       


        //this will determine what side the hangover is on the previous block, to know what side to drop the cube on
        //if more of the hangover is towards the further side of the previous block, then it is on the "negative side"
        if(currentx < oldBlockX)
        direction = -1;

        ////if more of the hangover is towards the closer side of the previous block, then it is on the "positive side"
        else
        direction = 1;


       
        //if the calculated newXSize ends up being less than 0, then make the x as 0, meaning that the whole sliding block would fall
        //did this because whenever you make something a negative size, it makes everything positive, so instead of making the block as 0
        //it made it bigger, messing up what I indended it to do
        if(newXSize < 0)
        {
        SpawnMissedBlock(); 
        }
        
        //if the calculated newXSize is not less than 0, then make the newXSize 0
        //so when it is properly working, make the newXSize the original size minus the hangover
        else
        {
        blocksList[accessThisIndex].transform.localScale = new Vector3(newXSize, blocksList[accessThisIndex].transform.localScale.y, blocksList[accessThisIndex].transform.localScale.z);
        
        //positions the block to meet the edge of the previous block
        float newXPosition = oldBlockX + (hangOver / 2);
        blocksList[accessThisIndex].transform.position = new Vector3(newXPosition, BlockY, blocksList[accessThisIndex].transform.position.z);

        //this will be the position on X axis on where to spawn the falling block
        //if it is on the positive side of the previous block, the new spawn falling position will be on the positive side, same with negative
        float CubeEdgeOnX = currentx + (blocksList[accessThisIndex].transform.localScale.x / 2f * direction);

        //testing, will add onto function later

        SpawnFallBlockOnX(fallingBlockSizeX, CubeEdgeOnX);
        
        }
    }



    public void chopOffOnZ()
    {
        //float oldBlockX = 10.22f; //will change these to fit the size of the old block, to start off it's only the size of the base block
        //float oldBlockZ = 0.91f;

        //if the size of the list is greater than one, then get the x of the old block
        //if it's 0, then the old blockZ will be 0.91
        if(blocksList.Count != 1)
        {
        oldBlockZ = blocksList[accessThisIndex-1].transform.position.z; 
        //Debug.Log("Does access the last Z");   
        }
        

        //Debug.Log("THIS RUNS");
        float currentz = blocksList[accessThisIndex].transform.position.z;
        float currenty = blocksList[accessThisIndex].transform.position.y;
        float CurrentBlockY = BlockY + 1.07f; //take this out?? you don't use it
        //float currentz = transform.position.z;
        float direction = 0;

        //MAKE USE OF OLDBLOCKY AND ADDING VALUE, CUZ RN IT ONLY WORKS WITH THE BEGINNING 
        //hangover will be the part of the block that is hanging over hwen you click the spacebar
        float hangOver = currentz - oldBlockZ;
        //adjusts the size of the block on the X axis
        float newZSize;
        newZSize = blocksList[accessThisIndex].transform.localScale.z - Mathf.Abs(hangOver);
        float fallingBlockSizeZ = blocksList[accessThisIndex].transform.localScale.z - newZSize; //subtract the size of the block minus the size of the new block to 
                                                                    //determine how big the size of the falling block is.
       


        //this will determine what side the hangover is on the previous block, to know what side to drop the cube on
        //if more of the hangover is towards the further side of the previous block, then it is on the "negative side"
        if(currentz < oldBlockZ)
        direction = -1;

        ////if more of the hangover is towards the closer side of the previous block, then it is on the "positive side"
        else
        direction = 1;


       
        //if the calculated newXSize ends up being less than 0, then make the x as 0, meaning that the whole sliding block would fall
        //did this because whenever you make something a negative size, it makes everything positive, so instead of making the block as 0
        //it made it bigger, messing up what I indended it to do
        if(newZSize < 0)
        {
        SpawnMissedBlock(); 
        }
        
        //if the calculated newXSize is not less than 0, then make the newXSize 0
        //so when it is properly working, make the newXSize the original size minus the hangover
        else
        {
        blocksList[accessThisIndex].transform.localScale = new Vector3(blocksList[accessThisIndex].transform.localScale.x, blocksList[accessThisIndex].transform.localScale.y, newZSize);
        
        //positions the block to meet the edge of the previous block
        float newZPosition = oldBlockZ + (hangOver / 2);
        blocksList[accessThisIndex].transform.position = new Vector3(blocksList[accessThisIndex].transform.position.x, BlockY, newZPosition);

        //this will be the position on X axis on where to spawn the falling block
        //if it is on the positive side of the previous block, the new spawn falling position will be on the positive side, same with negative
        float CubeEdgeOnZ = currentz + (blocksList[accessThisIndex].transform.localScale.z / 2f * direction);


        SpawnFallBlockOnZ(fallingBlockSizeZ, CubeEdgeOnZ);
        
        }
    }

    
    public void SpawnFallBlockOnX(float fallingBlockSizeX, float CubeEdgeOnX)
    {
        
        var fallingBlock = GameObject.CreatePrimitive(PrimitiveType.Cube); //creates a new gameobject from script
        fallingBlock.transform.localScale = new Vector3(fallingBlockSizeX, blocksList[accessThisIndex].transform.localScale.y, blocksList[accessThisIndex].transform.localScale.z); //shapes the cube
        fallingBlock.transform.position = new Vector3(CubeEdgeOnX, blocksList[accessThisIndex].transform.position.y, blocksList[accessThisIndex].transform.position.z); //positions block
        fallingBlock.AddComponent<Rigidbody>(); //adds physics RigidBody so it falls
        /////////////////////////////////
        //makes the new oldBlock positions the block which was just placed
        oldBlockX = transform.position.x;
        BlockY += 0.52f;
        
        

        //make new GameObject the same size as the previous block to start the sliding block on Z axis
        var newBlockOnZ = GameObject.CreatePrimitive(PrimitiveType.Cube); //creates a new gameobject from script
        newBlockOnZ.transform.position = new Vector3(blocksList[accessThisIndex].transform.position.x, blocksList[accessThisIndex].transform.position.y+0.52f, -5.01f); //positions block -5.01f
        newBlockOnZ.transform.localScale = new Vector3(blocksList[accessThisIndex].transform.localScale.x, blocksList[accessThisIndex].transform.localScale.y, blocksList[accessThisIndex].transform.localScale.z); //shapes the cube      
        blocksList.Add(newBlockOnZ);

        //Camera maincam = GameObject.Find("Main Camera").GetComponent<Camera>();
        //maincam.transform.position = new Vector3(maincam.transform.position.x, maincam.transform.position.y+0.535f, maincam.transform.position.z);
        //maincam.transform.position = new Vector3(maincam.transform.position.x, maincam.transform.position.y+0.535f, maincam.transform.position.z);
        scoreCount+=1;
        slideCounter+=1;
        handleScore.increaseScore(scoreCount);
        moveCamera.updateCam1();
        //Camera.main.transform.Translate(0, -10, 0);

        if(slideCounter == 4)
        {
            //Debug.Log("yes");
            slideSpeed+=0.55f;
            slideCounter=0;
        }

        //make sure to set timer back to 0 so it starts at the beginning
        timer = 0;
        side=1; //if side equals to 1, then it'll start moving the block on the Z axis
        accessThisIndex+=1; //to access the new GameObject
        slideblock = true; //allows the block to slide
        /////////////////////////////////
    }

    public void SpawnFallBlockOnZ(float fallingBlockSizeZ, float CubeEdgeOnZ)
    {
        
        var fallingBlock = GameObject.CreatePrimitive(PrimitiveType.Cube); //creates a new gameobject from script
        fallingBlock.transform.localScale = new Vector3(blocksList[accessThisIndex].transform.localScale.x, blocksList[accessThisIndex].transform.localScale.y, fallingBlockSizeZ); //shapes the cube
        fallingBlock.transform.position = new Vector3(blocksList[accessThisIndex].transform.position.x, blocksList[accessThisIndex].transform.position.y, CubeEdgeOnZ); //positions block
        fallingBlock.AddComponent<Rigidbody>(); //adds physics RigidBody so it falls
        /////////////////////////////////
        //makes the new oldBlock positions the block which was just placed
        oldBlockZ = transform.position.z;
        BlockY += 0.52f;
        
        
        //make new GameObject the same size as the previous block to start the sliding block on Z axis
        var newBlockOnX = GameObject.CreatePrimitive(PrimitiveType.Cube); //creates a new gameobject from script
        newBlockOnX.transform.position = new Vector3(4.3f, blocksList[accessThisIndex].transform.position.y+0.52f, blocksList[accessThisIndex].transform.position.z); //positions block -5.01f
        newBlockOnX.transform.localScale = new Vector3(blocksList[accessThisIndex].transform.localScale.x, blocksList[accessThisIndex].transform.localScale.y, blocksList[accessThisIndex].transform.localScale.z); //shapes the cube      
        blocksList.Add(newBlockOnX);

        //Debug.Log(blocksList[accessThisIndex].transform.position.z);
        scoreCount+=1;
        slideCounter+=1;
        handleScore.increaseScore(scoreCount);
        moveCamera.updateCam1();

        if(slideCounter == 4)
        {
            //Debug.Log("yes");
            slideSpeed+=0.55f;
            slideCounter=0;
        }

        //make sure to set timer back to 0 so it starts at the beginning
        timer = 0;
        side=0; //if side equals to 1, then it'll start moving the block on the Z axis
        accessThisIndex+=1; //to access the new GameObject
        slideblock = true; //allows the block to slide
        /////////////////////////////////
        
    }

    
    public void SpawnMissedBlock()
    {
        Vector3 storeFallingBlockSize = new Vector3(blocksList[accessThisIndex].transform.localScale.x, blocksList[accessThisIndex].transform.localScale.y, blocksList[accessThisIndex].transform.localScale.z); 
        Vector3 storeFallingBlockPosition = new Vector3(blocksList[accessThisIndex].transform.position.x, blocksList[accessThisIndex].transform.position.y, blocksList[accessThisIndex].transform.position.z);
        Destroy(blocksList[accessThisIndex].gameObject);
        
        var fallingBlock = GameObject.CreatePrimitive(PrimitiveType.Cube); //creates a new gameobject from script
        fallingBlock.transform.localScale = storeFallingBlockSize; //shapes the cube
        fallingBlock.transform.position = new Vector3(blocksList[accessThisIndex].transform.position.x, blocksList[accessThisIndex].transform.position.y, blocksList[accessThisIndex].transform.position.z); //positions block
        fallingBlock.AddComponent<Rigidbody>(); //adds physics RigidBody so it falls
        
        fail = true;
    }

    
    
}
