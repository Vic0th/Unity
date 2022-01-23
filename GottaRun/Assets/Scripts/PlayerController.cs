using UnityEngine;


public class PlayerController : MonoBehaviour
{
    [SerializeField]
    ushort moveSpeed = 25, jumpForce = 25;


    Rigidbody2D rb;
    SpriteRenderer sr;
    Vector2 jumpVector;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        jumpVector = new Vector2(0,jumpForce);
    }



    float moveVar;

    public void onMove(float moveInp){

        moveVar = moveInp * moveSpeed;

        if(moveInp != 0)
            sr.flipX = moveInp < 0;
    }

    

    public void Jump() {
        if(rb.velocity.y == 0)
            rb.velocity += jumpVector;
    }


    public sbyte getDirection() => (sbyte)(sr.flipX ? -1 : 1);


    void Update(){
        rb.velocity += new Vector2(moveVar * Time.deltaTime,0);
    }
}
