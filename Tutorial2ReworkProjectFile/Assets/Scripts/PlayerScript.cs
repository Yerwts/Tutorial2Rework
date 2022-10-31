using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rd2d;
    public float speed;
    public float jump;
    
    private bool isOnGround;
    public Transform groundcheck;
    public float checkRadius;
    public LayerMask allGround;
    private bool facingRight = true;

    public TextMeshProUGUI score;
    public TextMeshProUGUI livesText;
    public GameObject winTextObject;
    public GameObject loseTextObject;

    public AudioClip musicClipOne;
    public AudioClip musicClipTwo;
    public AudioSource musicSource;

    private int scoreValue = 0;
    private int livesValue = 3;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();
        score.text = "Score: " + scoreValue.ToString();
        livesText.text = "Lives: " + livesValue.ToString();
        winTextObject.SetActive(false);
        loseTextObject.SetActive(false);
        anim = GetComponent<Animator>();
        musicSource.clip = musicClipTwo;
        musicSource.Play();
        SetCountText();
    }

void Flip()
   {
     facingRight = !facingRight;
     Vector2 Scaler = transform.localScale;
     Scaler.x = Scaler.x * -1;
     transform.localScale = Scaler;
   }

    // Update is called once per frame
    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float vertMovement = Input.GetAxis("Vertical");
        rd2d.AddForce(new Vector2(hozMovement * speed, vertMovement * speed));
        isOnGround = Physics2D.OverlapCircle(groundcheck.position, checkRadius, allGround);
if (facingRight == false && hozMovement > 0)
   {
     Flip();
   }
else if (facingRight == true && hozMovement < 0)
   {
     Flip();
   }
   if(hozMovement > 0 || hozMovement < 0){
    anim.SetInteger("State", 1);
   }
   else if(hozMovement == 0){
    anim.SetInteger("State", 0);
   }
    if(vertMovement > 0 || isOnGround == false){
        anim.SetInteger("State", 2);
    }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
       if (collision.collider.tag == "Coin")
        {
           scoreValue += 1;
           SetCountText();
           Destroy(collision.collider.gameObject);
            
        }


        if (collision.collider.tag == "Enemy")
        {
            livesValue -= 1;
            SetCountText();
            Destroy(collision.collider.gameObject);
            
        }
    }

    void SetCountText(){
           // scoreValue += 1;
            score.text = "Score: "+ scoreValue.ToString();
            //Destroy(collision.collider.gameObject);
            if(scoreValue == 4){
                transform.position = new Vector2(58.45f, -0.5f);
                livesValue = 3;
            }
            if(scoreValue >= 8){
                winTextObject.SetActive(true);
                musicSource.clip = musicClipTwo;
                musicSource.Stop();
                musicSource.clip = musicClipOne;
                musicSource.Play();
                musicSource.loop = false;
                speed = 0;
              
        }
            livesText.text = "Lives: " + livesValue.ToString();
            if(livesValue == 0){
                loseTextObject.SetActive(true);
                speed = 0;
             
            }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground" && isOnGround)
        {
            if (Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0f, jump), ForceMode2D.Impulse); //the 3 in this line of code is the player's "jumpforce," and you change that number to get different jump behaviors.  You can also create a public variable for it and then edit it in the inspector.
             
            }
        }
    }
}