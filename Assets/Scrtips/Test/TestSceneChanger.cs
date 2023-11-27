using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneChangeEvent
{

}

public class TestSceneChanger : MonoBehaviour
{

    public string currentSceneName;

    // Start is called before the first frame update
    void Start()
    {
        currentSceneName = "A";
        SceneManager.LoadSceneAsync(currentSceneName, LoadSceneMode.Additive);
      


    }

    public void ChangeScene()
    {

        SceneManager.UnloadSceneAsync(currentSceneName);
        if (currentSceneName == "A")
        {
            currentSceneName = "B";
        }
        else
        {
            currentSceneName = "A";
        }
        SceneManager.LoadSceneAsync(currentSceneName, LoadSceneMode.Additive).completed += TestSceneChanger_completed; ;




    }

    private void TestSceneChanger_completed(AsyncOperation obj)
    {

        EventBus.Publish(new SceneChangeEvent());
    }


    // Update is called once per frame
    void Update()
    {
        

        
    }
}
