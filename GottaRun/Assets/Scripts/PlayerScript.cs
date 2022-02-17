using UnityEngine;


public class PlayerScript : Character
{
    [SerializeField]
    ushort moveSpeed = 30, jumpForce = 25;

    [SerializeField]
    Vector2 wallJumpR, wallJumpL;

    Rigidbody2D rb;
    SpriteRenderer sr;
    Vector2 jumpVector;
    LayerMask lm;
    Transform feet;
    float jumpRadius;
    float wallJumpRad;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        feet = GameObject.Find("/Player/Feet").transform;

        jumpVector = new Vector2(0,jumpForce);
        lm = LayerMask.GetMask("Foreground");
        jumpRadius = transform.localScale.x / 2 - 0.1f ;
        wallJumpRad = transform.localScale.x / 2 + 0.1f;
    }


    [HideInInspector]
    public float moveVar;

    public void onMove(float moveInp){
        moveVar = moveInp * moveSpeed;

        if(moveInp != 0)
            sr.flipX = moveInp < 0;
    }



    public void Jump() {
        //Normal Jump
        if(Physics2D.OverlapCircleAll(feet.position,jumpRadius,lm).Length > 1)
            rb.velocity += jumpVector;

        //Walljump Right
        else if (Physics2D.RaycastAll(transform.position,Vector2.left,wallJumpRad,lm).Length > 1){
            rb.velocity += wallJumpR;
        }
        //Walljump Left
        else if (Physics2D.RaycastAll(transform.position,Vector2.right,wallJumpRad,lm).Length > 1){
            rb.velocity += wallJumpL;
        }
    }


    public sbyte getDirection() => (sbyte)(sr.flipX ? -1 : 1);



    void Update(){
        rb.velocity += new Vector2(moveVar * Time.deltaTime,0);
    }
}
