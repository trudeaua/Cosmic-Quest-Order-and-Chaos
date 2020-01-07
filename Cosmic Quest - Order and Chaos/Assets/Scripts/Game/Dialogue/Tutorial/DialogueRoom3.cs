using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogueRoom3 : MonoBehaviour
{
    public Animator Anim;

    // Start is called before the first frame update
    void Start()
    {
        Anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            // Light the Leol floor print
            Anim.SetTrigger("EnterRoom3");
        }
        StartCoroutine(BackToMenu());
    }

    IEnumerator BackToMenu() {
        yield return new WaitForSeconds(3);
        StopAllCoroutines();
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("MenuStaging");

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}

