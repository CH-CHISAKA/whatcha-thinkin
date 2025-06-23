using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public int categoryChosen;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        categoryChosen = 0; //Animals
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setCategoryAnimal()
    {
        categoryChosen = 0;
    }
    public void startGame()
    {
        if (categoryChosen == 0)
        {
            //Do something

        }
    }
}
