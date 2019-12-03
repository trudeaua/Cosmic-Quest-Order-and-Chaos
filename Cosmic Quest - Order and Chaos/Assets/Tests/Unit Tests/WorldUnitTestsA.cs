using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace WorldTests //list of tests i should do 
{ 
    public class WorldUnitTestScript
    {
        // A Test behaves as an ordinary method
        [Test]
        public void Interactable_CanInteract_TrueIfInRangeAndColourMatches()
        {//set up testing variables for 'target'
            GameObject gameObject = new GameObject();
            Transform objPos = gameObject.GetComponent<Transform>();
            objPos.position = Vector3.one; //set both this and the 'player' to the same position
            Interactable interactable = gameObject.AddComponent<Interactable>();
            interactable.colour = CharacterColour.Red; //set target and 'player' colour to red
            
            //target.GetComponent<EntityStatsController>().characterColour

            //variables for 'player'
            GameObject player = new GameObject();
            Transform playerPos = player.GetComponent<Transform>(); 
            playerPos.position = Vector3.one;
            EntityStatsController playerControl = player.AddComponent<EntityStatsController>();
            playerControl.characterColour = CharacterColour.Red;//set player colour to red as well

            //assert that we can pick up an object in the same position as us, which matches colour
            Assert.True(interactable.CanInteract(playerPos));

            //assert for each additional colour
            interactable.colour = CharacterColour.Green;
            playerControl.characterColour = CharacterColour.Green;
            Assert.True(interactable.CanInteract(playerPos));

            interactable.colour = CharacterColour.Yellow;
            playerControl.characterColour = CharacterColour.Yellow;
            Assert.True(interactable.CanInteract(playerPos));

            interactable.colour = CharacterColour.Purple;
            playerControl.characterColour = CharacterColour.Purple;
            Assert.True(interactable.CanInteract(playerPos));

            //assert for COLOURLESS object, with each player colour

            //at this point, player colour is still purple, so we don't need to set it again
            interactable.colour = CharacterColour.None;
            Assert.True(interactable.CanInteract(playerPos));
            //then check for each other colour with the interactable being NONE
            playerControl.characterColour = CharacterColour.Red;
            Assert.True(interactable.CanInteract(playerPos));

            playerControl.characterColour = CharacterColour.Yellow;
            Assert.True(interactable.CanInteract(playerPos));

            playerControl.characterColour = CharacterColour.Green;
            Assert.True(interactable.CanInteract(playerPos));

            Object.DestroyImmediate(gameObject);
            Object.DestroyImmediate(player);
            

        }



        [Test]
        public void Interactable_CanInteract_FalseIfInRangeAndWrongColour()
        {//set up testing variables for 'target'
            GameObject gameObject = new GameObject();
            Transform objPos = gameObject.GetComponent<Transform>();
            objPos.position = Vector3.one; //set both this and the 'player' to the same position
            Interactable interactable = gameObject.AddComponent<Interactable>();
            interactable.radius = 10f; //arbitrarily large, the target and player are in same spot anyways for testing
            interactable.colour = CharacterColour.Red; //set target and 'player' colour to NOT MATCH
            
            //target.GetComponent<EntityStatsController>().characterColour

            //variables for 'player'
            GameObject player = new GameObject();
            Transform playerPos = player.GetComponent<Transform>(); 
            playerPos.position = Vector3.one;
            EntityStatsController playerControl = player.AddComponent<EntityStatsController>();
            playerControl.characterColour = CharacterColour.Green;//set player colour to Green to start

            //assert that we cannot pick up an object in the same position as us, if it doesn't match colour
            //Red object, Green Player
            Assert.False(interactable.CanInteract(playerPos));

            //Purple object, Green Player
            interactable.colour = CharacterColour.Purple; 
            Assert.False(interactable.CanInteract(playerPos));

            //Yellow object, Green Player
            interactable.colour = CharacterColour.Yellow; 
            Assert.False(interactable.CanInteract(playerPos));


            playerControl.characterColour = CharacterColour.Red;//set player colour to red now
            //Yellow object, Red player
            Assert.False(interactable.CanInteract(playerPos));

            //Purple object, Red Player
            interactable.colour = CharacterColour.Purple; 
            Assert.False(interactable.CanInteract(playerPos));

            //Green object, Red Player
            interactable.colour = CharacterColour.Green; 
            Assert.False(interactable.CanInteract(playerPos));


            playerControl.characterColour = CharacterColour.Purple;//set player colour to Purple now
            
            //Green object, Purple Player
            Assert.False(interactable.CanInteract(playerPos));

            //Yellow object, Purple Player
            interactable.colour = CharacterColour.Yellow; 
            Assert.False(interactable.CanInteract(playerPos));

            //Red object, Purple Player
            interactable.colour = CharacterColour.Red; 
            Assert.False(interactable.CanInteract(playerPos));


            playerControl.characterColour = CharacterColour.Yellow;//Finally set player colour to Yellow
            
            //Red object, Yellow Player
            Assert.False(interactable.CanInteract(playerPos));

            //Purple object, Yellow Player
            interactable.colour = CharacterColour.Purple; 
            Assert.False(interactable.CanInteract(playerPos));

            //Green object, Yellow Player
            interactable.colour = CharacterColour.Green; 
            Assert.False(interactable.CanInteract(playerPos));

            Object.DestroyImmediate(gameObject);
            Object.DestroyImmediate(player);

        }



        [Test]
        public void Interactable_CanInteract_FalseIfOutOfRange()
        {
            GameObject gameObject = new GameObject();
            Transform objPos = gameObject.GetComponent<Transform>();
            objPos.position = Vector3.up; //position (0,1,0)
            Interactable interactable = gameObject.AddComponent<Interactable>();
            interactable.radius = 0f; //test range of 0 first, then range of 0.5, range of 1, and range of arbitrarily large.
            interactable.colour = CharacterColour.Red; //set target and 'player' colour to RED

            //variables for 'player'
            GameObject player = new GameObject();
            Transform playerPos = player.GetComponent<Transform>(); 
            playerPos.position = Vector3.zero; //position (0,0,0)
            EntityStatsController playerControl = player.AddComponent<EntityStatsController>();
            playerControl.characterColour = CharacterColour.Red;//set player colour to Red to start

            Assert.False(interactable.CanInteract(playerPos));

            interactable.radius = 0.5f;
            Assert.False(interactable.CanInteract(playerPos));

            interactable.radius = 0.9999f;
            Assert.False(interactable.CanInteract(playerPos));


            interactable.radius = 1.0f; // this should be interactible
            Assert.True(interactable.CanInteract(playerPos));

            objPos.position = new Vector3(0,2,0); //position (0,2,0)
            //interactible radius is still 1, test it is now out of range
            Assert.False(interactable.CanInteract(playerPos));

            //arbitrarily large number 235463

            interactable.radius = 235463.0f;
            objPos.position = new Vector3(235463,235463,235463); //arbitrarily far
            //interactible radius is still 1, test it is now out of range
            Assert.False(interactable.CanInteract(playerPos));

            Object.DestroyImmediate(gameObject);
            //Object.Destroy(player);
        }

        [Test]
        public void Interactable_CanInteract_FalseIfOutOfRangeAndWrongColour() //test a few to make sure double negative does not result in positive
        {
            GameObject gameObject = new GameObject();
            Transform objPos = gameObject.GetComponent<Transform>();
            objPos.position = Vector3.up; //position (0,1,0)
            Interactable interactable = gameObject.AddComponent<Interactable>();
            interactable.radius = 0f; //will not test as many for this
            interactable.colour = CharacterColour.Red; //set target and 'player' colour to different colours

            //variables for 'player'
            GameObject player = new GameObject();
            Transform playerPos = player.GetComponent<Transform>(); 
            playerPos.position = Vector3.zero; //position (0,0,0)
            EntityStatsController playerControl = player.AddComponent<EntityStatsController>();
            playerControl.characterColour = CharacterColour.Green;//set player colour to Green to start

            Assert.False(interactable.CanInteract(playerPos));

            interactable.radius = 0.5f;
            Assert.False(interactable.CanInteract(playerPos));

            interactable.radius = 0.9999f;
            Assert.False(interactable.CanInteract(playerPos));



            objPos.position = new Vector3(0,2,0); //position (0,2,0)
            //interactible radius is still 1, test it is out of range
            Assert.False(interactable.CanInteract(playerPos));

            //arbitrarily large number 235463

            interactable.radius = 732363.0f;
            objPos.position = new Vector3(732363,732363,732363); //arbitrarily far
            //interactible radius is still 0.9999, test it is now out of range
            Assert.False(interactable.CanInteract(playerPos));
        

            Object.DestroyImmediate(gameObject);
            Object.DestroyImmediate(player);
        }
        //I will not test interactable's functions STARTINTERACT or STOPINTERACT themselves since these functions are meant to be overwritten, not used directly.




        /*// A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator ExampleUnitTestScriptWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }*/
    } 
}
