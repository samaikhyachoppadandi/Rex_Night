using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public delegate void DeadEventHandler();

public class player : Character
{ 
    private static player instance;

    private static int remainingLives = 3;

    public static int RemainingLives
    {
        get { return remainingLives; }
    }

    public event DeadEventHandler Dead;

    public static player Instance
    {
        get
        {
            if(instance == null)
            {
                instance = GameObject.FindObjectOfType<player>();
            }
            return instance;
        }
    }

   [SerializeField]
    private Transform[] groundPoints;

    [SerializeField]
    private float groundRadius;

    [SerializeField]
    private LayerMask whatIsGround;

    [SerializeField]
    private bool airControl;

    [SerializeField]
    private float jumpForce;

    private bool immortal = false;

    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private float immortalTime;

    private AudioManager audioManager;

    private float direction;
    private bool move;
    private float btnHorizontal;
    public Rigidbody2D MyRigidbody { get; set; }

    
    public bool Slide { get; set; }
    public bool Jump { get; set; }
    public bool OnGround { get; set; }

    public override bool IsDead
    {
        get
        {
            if (health <= 0)
            {
                OnDead();
            }
            return health <= 0;
        }
    }

    private Vector2 startPos;

   

    public override void Start()    
	{ 
        base.Start();
          startPos = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
        MyRigidbody = GetComponent<Rigidbody2D>();
       

        audioManager = AudioManager.instance;
        if(audioManager == null)
        { Debug.LogError("none"); }
	}

	void Update()
	{
        if (!TakingDamage && !IsDead)
        {
            if (transform.position.y <= -16f)
            {

                Death();
            }
            HandleInput();
        }
	}

	
	void FixedUpdate ()
	{
        if (!TakingDamage && !IsDead)
        {
            float horizontal = Input.GetAxis("Horizontal");

            OnGround = IsGrounded();

            if (move)
            {
                this.btnHorizontal = Mathf.Lerp(btnHorizontal, direction, Time.deltaTime * 2);
                HandleMovement(btnHorizontal);
                Flip(direction);
            }
            else
            {
                HandleMovement(horizontal);
                Flip(horizontal);
            }
            HandleLayers();
        }

	}

    public void OnDead()
    {
        if(Dead!=null)
        {
            Dead();
        }
    }
    private void HandleMovement(float horizontal)
        {
        if (MyRigidbody.velocity.y < 0)
        {
            MyAnimator.SetBool("land", true);
        }
        if (!Attack && !Slide && (OnGround || airControl))
        {
            MyRigidbody.velocity = new Vector2(horizontal * movementSpeed, MyRigidbody.velocity.y);
        }
        if(Jump && MyRigidbody.velocity.y==0)
        {
            MyRigidbody.AddForce(new Vector2(0, jumpForce));
        }

        MyAnimator.SetFloat("speed", Mathf.Abs(horizontal));
        }

	private void HandleInput()
	{
        if (Input.GetKeyDown(KeyCode.Space))
        {

            MyAnimator.SetTrigger("jump");

		}
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            MyAnimator.SetTrigger("attack");
        }
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            MyAnimator.SetTrigger("slide");
        }
        if(Input.GetKeyDown(KeyCode.V))
        {
            MyAnimator.SetTrigger("throw");
           
        }
	}
	private void Flip(float horizontal)
	{
        if (horizontal > 0 && !facingRight || horizontal < 0 && facingRight)
        {
            ChangeDirection();

        }
	}

	

    private bool IsGrounded()
    {
        if (MyRigidbody.velocity.y <=0)
        {
            foreach (Transform point in groundPoints)
            {
                UnityEngine.Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, groundRadius, whatIsGround);

                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].gameObject != gameObject)
                    {
                      
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private void HandleLayers()
    {
        if(!OnGround)
        {
            MyAnimator.SetLayerWeight(1, 1);
        }
        else
        {
            MyAnimator.SetLayerWeight(1, 0);
        }
    }

    public override void ThrowKnife(int value)
    {
        if (!OnGround && value == 1 || OnGround && value == 0)
        {
            base.ThrowKnife(value);
        }
    }

    public IEnumerator IndicateImmortal()
    {
        while(immortal)
        {
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(.1f);
            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(.1f);

        }
    }

    public void EndGame()
    {
        Debug.Log("game over");
        SceneManager.LoadScene("gameover");
    }

    public override IEnumerator TakeDamage()
    {
        if (!immortal)
        {
            health -= 10;
            if (!IsDead)
            {
                MyAnimator.SetTrigger("damage");

                immortal = true;
                StartCoroutine(IndicateImmortal());
                yield return new WaitForSeconds(immortalTime);
                immortal = false;
            }
            else
            {
                MyAnimator.SetLayerWeight(1, 0);
                MyAnimator.SetTrigger("die");
            }
            
        }
    }

    public override void Death()
    {
        remainingLives -= 1;
        if (remainingLives <= 0)
        {
            EndGame();
        }
        else
        {
            MyRigidbody.velocity = Vector2.zero;
            MyAnimator.SetTrigger("idle");
            health = 30;
            transform.position = startPos;
        }
    }

    public void BtnJump()
    {
        MyAnimator.SetTrigger("jump");
        Jump = true; 
    
    }
    public void BtnAttack()
    {
        MyAnimator.SetTrigger("attack");

    }
    public void BtnSlide()
    {
        MyAnimator.SetTrigger("slide");

    }
    public void BtnMove( float direction)
    {

        this.direction = direction;
        this.move = true;
    }
    public void BtnThrow()
    {
        MyAnimator.SetTrigger("throw");
    }
   
    public void BtnStopMove()
    {
        this.direction = 0;
        this.btnHorizontal = 0;
        move = false;
    }

    private void OnCollisionEnter2D(Collision2D other)

    {
        if(other.gameObject.tag =="Coin")
        {
            GameManager.Instance.CollectedCoins++;
            Destroy(other.gameObject);

        }
    }
}


