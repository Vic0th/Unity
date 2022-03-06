using UnityEngine;
using System.Collections.Generic;

public class Steps : MonoBehaviour
{
    Vector3 rayOffsetRight;
    Vector3 rayOffsetLeft;

    LayerMask lm;
    Transform tr;
    Rigidbody2D rb;
    float xScale;
    public bool isRight;
    PlayerScript ps;

    void Awake() {
        xScale = GetComponentInParent<Transform>().localScale.x / 2;


        EdgeCollider2D[] ec = GetComponents<EdgeCollider2D>();
        List<Vector2> pointR = new List<Vector2>() {
            new Vector2(xScale + 0.01f,0.1f),
            new Vector2(xScale + 0.01f,-0.1f)
        };

        List<Vector2> pointL = new List<Vector2>() {
            new Vector2(-xScale - 0.01f,0.1f),
            new Vector2(-xScale - 0.01f,-0.1f)
        };

        ec[0].SetPoints(pointR);
        ec[1].SetPoints(pointL);


        rayOffsetRight = new Vector3(xScale + 0.05f,0.3f,0);
        rayOffsetLeft = new Vector3(-xScale - 0.05f,0.3f,0);

        lm = LayerMask.GetMask("Foreground");
        tr = transform.parent.parent;
        rb = GetComponentInParent<Rigidbody2D>();
        ps = GetComponentInParent<PlayerScript>();
    }


    //void OnCollisionEnter2D(Collision2D col) => CollisionHandle(col);
    private void OnCollisionStay2D(Collision2D collision) {}

    //Step up according to player input

    void CollisionHandle_Right() {
        if(ps.moveVar > 0) {

        }
    }
    void CollisionHandle_Left() {

    }

    /*
    void CollisionHandle(Collision2D col) {
        if(col.gameObject.CompareTag("Stairs")  ){

            //Collision On Right
            if(rb.velocity.x > 0) {
                if(!Physics.Raycast(transform.position + rayOffsetRight,Vector3.right,0.1f,lm))

                    rb.position += stepVecRight;
                return;
            }

            //Collision On Left
            else if (rb.velocity.x < 0){
                if(!Physics.Raycast(transform.position + rayOffsetLeft,Vector3.left,0.1f,lm))

                    rb.position += stepVecLeft;
                return;
            }
        }
    }
    */

}
