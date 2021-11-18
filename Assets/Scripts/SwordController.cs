using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour
{

    [SerializeField] private bool rotate; // do you want it to rotate?
    [SerializeField] private float rotationSpeed;
    [SerializeField] private AudioClip collectSound;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.World);

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Collect();
        }
    }

	public void Collect()
	{
		if (collectSound) AudioSource.PlayClipAtPoint(collectSound, transform.position);
        
        GameManager.instance.AddItem(Instantiate(gameObject));

        Destroy(gameObject);
	}
}
