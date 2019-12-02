using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;
using UnityEngine.SceneManagement;

public class DialogueManagerTests
{
    DialogueManager dialogueManager;
    Dialogue testDialogue;
    Animator anim;

    [UnitySetUp]
    public IEnumerator Setup()
    {
        SetupTestDialogue();
        yield return LoadScene();
    }

    private IEnumerator LoadScene()
    {
        // Load Test scene
        AsyncOperation sceneLoading = SceneManager.LoadSceneAsync("TestScene", LoadSceneMode.Single);

        float timer = 0;
        while (!sceneLoading.isDone)
        {
            if (timer > 15f)
            {
                break;
            }
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        Assert.IsTrue(sceneLoading.isDone, "Scene load timed out! The scene could not be loaded.");
    }

    private void SetupTestDialogue()
    {
        string name = "Random Character";
        string sentence1 = "Test for sentence 1";
        string sentence2 = "Test for sentence 2";
        testDialogue = new Dialogue()
        {
            sentences = new string[] { sentence1, sentence2 },
            name = name,
        };
    }

    [UnityTest]
    public IEnumerator DialogueManager_StartDialogue_ShouldModifyDialogueComponents()
    {
        GameObject go = MonoBehaviour.Instantiate(TestResourceManager.Instance.GetResource("Dialogue Manager"), Vector3.zero, Quaternion.identity);
        dialogueManager = go.GetComponent<DialogueManager>();

        TextMeshProUGUI name = dialogueManager.dialogueName;
        TextMeshProUGUI text = dialogueManager.dialogueText;

        string initialName = name.text;
        string initialText = text.text;

        dialogueManager.StartDialogue(testDialogue, false);
        Assert.AreNotEqual(initialName, name.text);
        Assert.AreNotEqual(initialText, text.text);

        yield return null;
    }

    [UnityTest]
    public IEnumerator DialogueManager_StartDialogue_ShouldShowDialogueBox()
    {
        GameObject go = MonoBehaviour.Instantiate(TestResourceManager.Instance.GetResource("Dialogue Manager"), Vector3.zero, Quaternion.identity);
        dialogueManager = go.GetComponent<DialogueManager>();
        anim = dialogueManager.anim;
        Assert.AreEqual(anim.GetCurrentAnimatorClipInfo(0)[0].clip.name, "DialogueBoxClose");
        dialogueManager.StartDialogue(testDialogue, false);
        yield return new WaitForSeconds(0.3f);
        Assert.AreEqual(anim.GetCurrentAnimatorClipInfo(0)[0].clip.name, "DialogueBoxOpen");
    }

    [UnityTest]
    public IEnumerator DialogueManager_ShouldCloseDialogueAfterSentencesFinish()
    {
        GameObject go = MonoBehaviour.Instantiate(TestResourceManager.Instance.GetResource("Dialogue Manager"), Vector3.zero, Quaternion.identity);
        dialogueManager = go.GetComponent<DialogueManager>();
        anim = dialogueManager.anim;
        dialogueManager.StartDialogue(testDialogue, false);
        yield return new WaitForSeconds(4f);
        Assert.AreEqual(anim.GetCurrentAnimatorClipInfo(0)[0].clip.name, "DialogueBoxClose");
    }
}
