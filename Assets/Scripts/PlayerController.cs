using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float velocidadPlayer = 1000f;
    [SerializeField] private float fuerzaSalto = 500f;
    [SerializeField] private float velocidadGiro = 10f;
    [SerializeField] private Animator animaPlayer;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] private AudioClip walkSound;


    [SerializeField] private int lifePlayer;
    [SerializeField] private int shieldPlayer;
    [SerializeField] private int attackPlayer;


    private AudioSource audioPlayer;

    private bool isGrounded = true;
    private bool isRotate = false;

    private float giroPlayer = 0f;

    //[SerializeField] private Animator animaPlayer = new Animator();


    private Rigidbody rbPlayer;

    // Start is called before the first frame update
    void Start()
    {
        audioPlayer = GetComponent<AudioSource>();
        rbPlayer = GetComponent<Rigidbody>();

        animaPlayer.SetBool("IsIdle", true);
        animaPlayer.SetBool("IsRun", false);
        animaPlayer.SetBool("IsBack", false);
        animaPlayer.SetBool("IsJump", false);

    }


    // Update is called once per frame
    void Update()
    {
        IsGrounded();
    }

    private void FixedUpdate()
    {
        Mover();
        ControlAnimacion();
    }

    private void Mover()
    {
        float ejeVertical = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.A) && isGrounded)
        {
            giroPlayer -= Time.deltaTime * velocidadGiro * 10;
            transform.rotation = Quaternion.Euler(0, giroPlayer, 0);
            isRotate = true;
        }
        else if (Input.GetKey(KeyCode.D) && isGrounded)
        {
            giroPlayer += Time.deltaTime * velocidadGiro * 10;
            transform.rotation = Quaternion.Euler(0, giroPlayer, 0);
            isRotate = true;
        }
        else
        {
            isRotate = false;
        }

        if (ejeVertical != 0 && isGrounded)
        {
            rbPlayer.AddRelativeForce(Vector3.forward * velocidadPlayer * ejeVertical, ForceMode.Force);
            if (!audioPlayer.isPlaying)
            {
                audioPlayer.PlayOneShot(walkSound, 0.5f);
            }
        }




        if (Input.GetKey(KeyCode.Space) && isGrounded)
        {
            rbPlayer.AddRelativeForce(Vector3.up * fuerzaSalto, ForceMode.Impulse);
        }
    }

    private void ControlAnimacion()
    {

        float ejeVertical = Input.GetAxis("Vertical");

        animaPlayer.SetBool("IsRun", ejeVertical > 0 && isGrounded);
        animaPlayer.SetBool("IsBack", ejeVertical < 0 || isRotate);
        animaPlayer.SetBool("IsJump", !isGrounded);


        animaPlayer.SetBool("IsIdle", ejeVertical == 0 && isGrounded && !isRotate);


        Debug.Log($"EjeVertical {ejeVertical}; IsIdle {animaPlayer.GetBool("IsIdle")} ; IsJump {animaPlayer.GetBool("IsJump")}; IsRun {animaPlayer.GetBool("IsRun")} ; IsBack {animaPlayer.GetBool("IsBack")}; isRotate {isRotate}; isGround {isGrounded}");
    }


    private void IsGrounded()
    {
        if (Physics.Raycast(transform.position, Vector3.down, 0.05f, groundLayer))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }


    }

    public void AddShield(int _shield)
    {
        shieldPlayer += _shield;
    }
    public void AddAttack(int _attack)
    {
        attackPlayer += _attack;
    }
    public void AddLife(int _life)
    {
        lifePlayer += _life;
    }

    public int Shield { get { return shieldPlayer; } set { shieldPlayer = value; } }
    public int Attack { get { return attackPlayer; } set { attackPlayer = value; } }
    public int Life { get { return lifePlayer; } set { lifePlayer = value; } }



    private void OnDrawGizmos()
    {

        Gizmos.color = Color.blue;
        Vector3 direction = gameObject.transform.TransformDirection(Vector3.down) * 0.05f;
        Gizmos.DrawRay(gameObject.transform.position, direction);

    }
}
