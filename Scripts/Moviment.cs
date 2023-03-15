using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moviment : MonoBehaviour
{
    Rigidbody2D rb;
    Animator ani;
    public LayerMask mask;


    public float speed = 6f;
    public int jumpForce = 2000;
    public int cuentaSaltos = 2;    //Este contador servirá por si en un futuro se añade el doble salto al jugador como power-up
    bool facingRight = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();   //Importante para ahorrar codigo
        //DontDestroyOnLoad(gameObject);
        ani = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        moviment();


        if (Input.GetKeyDown(KeyCode.UpArrow) && canJump == true)
        {
            canJump = false;
            cuentaSaltos = cuentaSaltos - 1;
            rb.velocity = new Vector2(0, 0);        //Para poder rebotar en las paredes sin problema es necesario reiniciar la velocidad en cada salto
            rb.AddForce(new Vector2(0, jumpForce));
            ani.SetBool("jump", true);
        }

        atacNormal();

       
    }
    bool canJump;
   // bool stayWall;    //Será necesario si se quiere modificar la mecánica de rebote en la pared

    private void moviment()
    {
        float h = Input.GetAxisRaw("Horizontal");   //De nuevo ahorramos codigo
        Input.GetKeyDown(KeyCode.UpArrow);

        Vector2 pos = rb.position;
        pos.x = rb.position.x + h * speed * Time.fixedDeltaTime; //La posicion del personaje cambiará frame a frame de forma estable

        ani.SetFloat("vel", Mathf.Abs(h));  //Con esto se ejecuta la animacion de caminar al moverse
        if (facingRight && h < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            facingRight = false;
        }
        if (!facingRight && h > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            facingRight = true;
        }
        rb.position = pos;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Wall")
        {
            //stayWall = false;
            ani.SetBool("paret", true);
            canJump = true;
            cuentaSaltos = cuentaSaltos + 1;
        }

        if (collision.transform.tag == "Ground")
        {
            canJump = true;
           // stayWall = false;
            cuentaSaltos = 2;
            ani.SetBool("jump", false);

        }

    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.tag == "Wall")
        {
            ani.SetBool("paret", false);
        }
    }

    private void atacNormal()   //Codi optimitzat per l'atac normal
    {
        if (Input.GetKeyDown("x"))
        {
            ani.SetBool("atacNormal", true);
        }
        else ani.SetBool("atacNormal", false);
    }
}
 
