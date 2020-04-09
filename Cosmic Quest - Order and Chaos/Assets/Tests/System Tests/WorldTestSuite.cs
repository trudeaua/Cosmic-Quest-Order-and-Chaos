using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class WorldTestSuite
    {
        // A Test behaves as an ordinary method
        [Test]
        public void Collectable_StartInteract_ObjectDeletedAfterInteract() 
        {
            GameObject gameObject = new GameObject();
            Transform objPos = gameObject.GetComponent<Transform>();
            objPos.position = Vector3.zero; //position (0,0,0)
            Collectable collectable = gameObject.AddComponent<Collectable>();
            collectable.colour = CharacterColour.Red; //set target and 'player' colour to same colour

            //variables for 'player'
            GameObject player = new GameObject();
            Transform playerPos = player.GetComponent<Transform>(); 
            playerPos.position = Vector3.zero; //position (0,0,0)
            EntityStatsController playerControl = player.AddComponent<EntityStatsController>();
            playerControl.characterColour = CharacterColour.Red;//set player colour to Red to start

            Assert.IsNotNull(gameObject);        
            //we will have the player and collectable interact
            collectable.StartInteract(playerPos);
            Assert.IsNull(gameObject); 

            Object.Destroy(gameObject);
            Object.Destroy(player);
        }


        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        /*
        [UnityTest]
        public IEnumerator PlayerCharacterTestSuiteWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }*/
    }
}
