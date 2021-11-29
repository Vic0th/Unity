    using UnityEngine;


public class PlayerController : MonoBehaviour
{
    [SerializeField]
    ushort moveSpeed = 25;


    Rigidbody2D rb;
    Transform tr;
    Camera cam;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        tr = GetComponentInChildren<SpriteRenderer>().transform;
        cam = GetComponentInChildren<Camera>();
    }



    Vector2 moveVector;

    public void onMove(Vector2 movVec)
    {
        this.moveVector = movVec * moveSpeed;
    }


    Quaternion qu;
    Vector3 mousePos;

    public void aimTo(Vector2 pos) {
        mousePos = cam.ScreenToWorldPoint(pos) + new Vector3(0,0,10);

        //Debug.Log("Quaternion: " + qu);
        //Debug.Log("Quaternion Euler: " + qu.eulerAngles);
    }

    

    void Update()
    {
        rb.velocity += moveVector * Time.deltaTime;

        tr.rotation = Quaternion.FromToRotation(Vector3.up,mousePos - transform.position);
        //cam.transform.rotation = Quaternion.FromToRotation(mousePos,transform.position);


        //mousePos -= transform.position;
        //rb.rotation = Mathf.Atan2(mousePos.y,mousePos.x) * Mathf.Rad2Deg;
        //transform.rotation = Quaternion.FromToRotation(mousePos,transform.position);
        //transform.rotation.SetFromToRotation(Vector3.up,mousePos - transform.position);
        //transform.rotation.SetFromToRotation(mousePos, transform.position);
        

    }
}
