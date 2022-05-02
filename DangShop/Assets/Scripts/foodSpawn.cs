using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class foodSpawn : MonoBehaviour
{
    public GameObject milk;
    public GameObject tp;
    public GameObject[] milkSpawnPoints;
    public GameObject[] tpSpawnPoints;
    public Text taskList;
    public List<string> tasks;
    string display;

    void Start()
    {
        milkSpawn();
        TPSpawn();
        DisplayList();
    }

    private void Update()
    {
        taskList.text = display;        
    }

    public void milkSpawn()
    {
        int spawnDecision;
        spawnDecision = Random.Range(0, 2);
        if (spawnDecision == 0)
        {
            return;
        } 
            tasks.Add("Milk");
            milk.transform.localPosition = new Vector3(0f, 0f, 0f);
            milkSpawnPoints = GameObject.FindGameObjectsWithTag("milk_sp");
            Instantiate(milk, milkSpawnPoints[Random.Range(0, milkSpawnPoints.Length)].transform);
    }

    void TPSpawn()
    {
        int spawnDecision;
        spawnDecision = Random.Range(0, 2);
        if (spawnDecision == 0)
        {
            return;
        }
        else
        {
            tasks.Add("Toilet Paper");
            tp.transform.localPosition = new Vector3(0f, 0.15f, 0f);
            tpSpawnPoints = GameObject.FindGameObjectsWithTag("tp_sp");
            Instantiate(tp, tpSpawnPoints[Random.Range(0, tpSpawnPoints.Length)].transform);
        }
    }

    void DisplayList()
    {
        display = "Your tasks:\n";
        foreach (string task in tasks)
        { 
            display += task + "\n";
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "milk")
        {
            Destroy(collision.gameObject);
            tasks.Remove("Milk");
            DisplayList();
        }
        if (collision.transform.tag == "tp")
        {
            Destroy(collision.gameObject);
            tasks.Remove("Toilet Paper");
            DisplayList();
        }
    }
}
