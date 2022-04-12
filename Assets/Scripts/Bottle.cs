using UnityEngine;

public class Bottle : MonoBehaviour
{
    private static bool isPicked = false;
    private GameObject player;
    private Rigidbody rb;
    private bool _isInHand = false;
    private float soundSpread = 30f;
    [SerializeField] private AudioClip audioClip;
    [SerializeField] private GameObject breakParticle;
    [SerializeField] private float swayMultiplier;
    [SerializeField] private float smooth;
    private const float minTorque = 1;
    private const float maxTorque = 10;

    private void Start()
    {
        player = GameObject.Find("player");
        rb = gameObject.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Throw();
        BottleSway();
    }

    public void PickedUp()
    {
        if (isPicked) return;
        rb.isKinematic = true;
        gameObject.transform.GetComponent<MeshCollider>().isTrigger = true;
        gameObject.transform.parent = player.transform.GetChild(0);
        if (gameObject.transform.parent.childCount > 1)
        {
            gameObject.SetActive(false);
        }
        gameObject.transform.localPosition = new(0.2f, -0.23f, 0.44f);
        gameObject.transform.localRotation = new(0f, 0f, 0f, 0f);
        isPicked = true;
        _isInHand = true;
    }

    private void Throw()
    {
        if (!Input.GetMouseButtonUp(0) || !_isInHand) return;
        gameObject.transform.parent = null;
        rb.isKinematic = false;
        gameObject.transform.GetComponent<MeshCollider>().isTrigger = false;
        rb.AddTorque(new Vector3(Random.Range(minTorque, maxTorque), 0f, 0f));
        rb.AddForce(gameObject.transform.forward * 10f, ForceMode.Impulse);
        isPicked = false;
        _isInHand = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.relativeVelocity.magnitude);
        try 
        { 
            var zombie = collision.gameObject.GetComponent<zombie>(); 
            if(zombie != null)
            {
                zombie.TakeDamage(1);
            }
        }
        catch(System.NullReferenceException e) { }
        if (collision.relativeVelocity.magnitude > 2)
        {
            Collider[] hitCollider = Physics.OverlapSphere(gameObject.transform.position, soundSpread);
            foreach (Collider obj in hitCollider)
            {
                try { obj.gameObject.GetComponent<zombie>().SoundHeard(gameObject.transform); }
                catch (System.NullReferenceException) { }
            }
            var particle = Instantiate(breakParticle, gameObject.transform.position, gameObject.transform.rotation);
            AudioSource.PlayClipAtPoint(audioClip, gameObject.transform.position);
            Destroy(gameObject);
            Destroy(particle, 1f);
        }
    }

    private void BottleSway()
    {
        if (gameObject.transform.parent != null)
        {
            float mouseX = Input.GetAxisRaw("Mouse X") * swayMultiplier;
            float mouseY = Input.GetAxisRaw("Mouse Y") * swayMultiplier;

            Quaternion rotateX = Quaternion.AngleAxis(-mouseY, Vector3.up);
            Quaternion rotateY = Quaternion.AngleAxis(mouseX, Vector3.forward);

            Quaternion targetRotation = rotateX * rotateY;
            Debug.Log(targetRotation);

            transform.localRotation = Quaternion.Lerp(Quaternion.identity, targetRotation, smooth);
        }
    }
}
