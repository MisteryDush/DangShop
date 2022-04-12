using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class playerStats : MonoBehaviour
{
    public float hp = 100f;
    public lookAround lookAround;
    public playerMovement playerMovement;
    int amountOfFAK = 1;
    public Text displayFAK;
    public Text displayHP;


    private void Start()
    {
        displayHP.text = (int)hp + " HP";
        displayFAK.text = amountOfFAK + "";
    }

    private void Update()
    {
        if (hp <= 0)
        {
            lookAround.enabled = false;
            playerMovement.enabled = false;
            //StartCoroutine(Wait());
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponentInParent<zombie>() != null && collision.gameObject.GetComponentInParent<zombie>().dead == false)
        {
            var damage = Random.Range(30f, 50f);
            if(damage > hp)
            {
                hp = 0;
                displayHP.text = (int)hp + " HP";
                return;
            }
            hp -= damage;
            Debug.Log(hp);
            displayHP.text = (int)hp + " HP";
        }
        if(collision.gameObject.transform.tag == "fak")
        {
            amountOfFAK += 1;
            displayFAK.text = amountOfFAK + "";
        }
    }



    public IEnumerator Heal()
    {
        if (amountOfFAK > 0)
        {
            amountOfFAK -= 1;
            Debug.Log("Healing!");
            yield return new WaitForSeconds(3f); // length of the future animation
            hp += 50;
            if(hp > 100f)
            {
                hp = 100f;
            }
            displayFAK.text = amountOfFAK + "";
            displayHP.text = (int)hp + " HP";
        }
    }


    IEnumerator Wait()
    {
        yield return new WaitForSeconds(3f);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Dead");
    }
}
