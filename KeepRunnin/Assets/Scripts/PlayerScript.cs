using UnityEngine;


public class PlayerScript: MonoBehaviour {
    [SerializeField]
    ushort moveSpeed = 30, jumpForce = 23;

    [SerializeField]
    float ropeSpeedBoost = 20;

    [SerializeField]
    float wallJumpX = 7, wallJumpY = 20, wallReleaseX = -2, wallReleaseY = -3;

    [HideInInspector]
    public Rigidbody2D rb;
    //SpriteRenderer sr;
    Transform charDir;
    //GameController gc;

    LayerMask lm;

    Transform feet;
    float jumpRadius;
    float wallJumpRad;
    Vector2 jumpVector;

    Vector2 ropeHandleVec;
    Transform ropeHandler;
    float ropeAutoRelease = 2f;
    CircleCollider2D ropeHandleCollider;

    Animator anim;

    Vector2 wallHoldSnap;
    Vector3 wallBodyOffset;
    float wallRayOffsetY;

    CapsuleCollider2D[] cols;

    private void Awake() {
        //gc = GameObject.Find("/MAIN").GetComponent<GameController>();
        rb = GetComponent<Rigidbody2D>();
        //sr = GetComponentInChildren<SpriteRenderer>();

        charDir = GameObject.Find("/Player/Character").transform;
        feet = GameObject.Find("/Player/Feet").transform;

        jumpVector = new Vector2(0,jumpForce);
        lm = LayerMask.GetMask("Foreground");
        jumpRadius = transform.localScale.x / 2 - 0.1f;

        ropeHandleVec = new Vector2(0f,GetComponentInChildren<CircleCollider2D>().radius * 3f);

        ropeHandleCollider = GameObject.Find("/Player/RopeHandle").GetComponent<CircleCollider2D>();
        ropeHandler = ropeHandleCollider.transform;

        anim = GetComponentInChildren<Animator>();

        wallJumpRad = transform.localScale.x / 2 + 0.08f;
        wallHoldSnap = new Vector2(0.06f,0);
        wallBodyOffset = new Vector3(0,transform.localScale.y / 2 - 0.1f,0);
        wallRayOffsetY = transform.localScale.y / 2 + 1f;

        //First col vertical, second horizontal
        cols = GetComponents<CapsuleCollider2D>();
    }


    //void FaceDirection(bool dir) => sr.flipX = !dir;
    void FaceDirection(bool dir) => charDir.localScale = new Vector3(dir ? 1 : -1,1,1);


    [HideInInspector]
    public float moveVar = 0;
    float moveInpVer = 0;
    bool isMovin = false;

    public void onMove(Vector2 moveInp) {
        moveVar = moveInp.x * moveSpeed;
        isMovin = moveVar != 0;
        
        moveInpVer = moveInp.y;
    }


    public void Jump() {
        //Jump off the rope
        if(ropeSlide)
            OffRopeSlide();

        //WallJump
        else if(holdinWall) {
            OffWallHold();
            FaceDirection(isMovin ? !holdingWallRight : holdingWallRight);

            if(moveInpVer < -0.30)
                rb.velocity += new Vector2(holdingWallRight ? wallReleaseX : -wallReleaseX,wallReleaseY);

            else if(!isMovin)
                rb.velocity += new Vector2(holdingWallRight ? wallReleaseX : -wallReleaseX,wallJumpY);

            else
                rb.velocity += new Vector2(holdingWallRight ? -wallJumpX : wallJumpX,wallJumpY);
        }

        //Normal Jump
        else if(onFloor)
            rb.velocity += jumpVector;
    }



    bool ropeSlide = false, ropeDirRight;
    float ropeHandleSnap = 0.08f;
    //float ropeTime = 0;

    public void OnRopeSlide(bool isBelow,bool isFromLeft,bool isFromRight,bool DirectionToRight) {
        //if(Time.time < ropeTime + 0.3f)
            //return;

        ropeSlide = true;
        canMove = false;
        holdinWall = false;
        ropeDirRight = DirectionToRight;
        //ropeTime = Time.time;

        if(isBelow)
            rb.position += ropeHandleVec;

        if(isFromLeft)
            rb.position += new Vector2(ropeHandleSnap,0f);

        else if(isFromRight)
            rb.position += new Vector2(-ropeHandleSnap,0f);


        FaceDirection(DirectionToRight);


        if(DirectionToRight) {
            if(rb.velocity.x < 0)
                rb.velocity = new Vector2(0f,rb.velocity.y);
        }

        else {
            if(rb.velocity.x > 0)
                rb.velocity = new Vector2(0f,rb.velocity.y);
        }
    }



    public void OffRopeSlide() {
        if(!ropeSlide)
            return;

        rb.position -= ropeHandleVec;

        ropeSlide = false;
        canMove = true;

        //StartCoroutine(RopeHandleToogle());
    }


    public System.Collections.IEnumerator RopeHandleToogle() {
        ropeHandleCollider.enabled = false;
        yield return new WaitForSeconds(0.3f);
        ropeHandleCollider.enabled = true;
    }



    bool holdingWallRight;
    float wallTime = 0;

    void OnWallHold(bool toRight) {
        if(toRight == holdingWallRight && Time.time < wallTime + 0.4)
            return;

        if(ropeSlide)
            OffRopeSlide();

        holdinWall = true;
        canMove = false;
        holdingWallRight = toRight;
        FaceDirection(toRight);

        rb.position += toRight ? wallHoldSnap : -wallHoldSnap;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }



    void OffWallHold() {
        holdinWall = false;
        canMove = true;
        wallTime = Time.time;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }



    void isHoldingWall() {
        if(!holdinWall) {
            if(onFloor || Physics2D.RaycastAll(transform.position,Vector2.down,wallRayOffsetY,lm).Length > 1)
                return;

            Vector3 raycastPosUp = transform.position + wallBodyOffset;
            Vector3 raycastPosDown = transform.position - wallBodyOffset;


            if(Physics2D.RaycastAll(raycastPosUp,Vector2.right,wallJumpRad,lm).Length > 1 &&
                Physics2D.RaycastAll(raycastPosDown,Vector2.right,wallJumpRad,lm).Length > 1)
                OnWallHold(true);
            else if(Physics2D.RaycastAll(raycastPosUp,Vector2.left,wallJumpRad,lm).Length > 1 &&
                Physics2D.RaycastAll(raycastPosDown,Vector2.left,wallJumpRad,lm).Length > 1)
                OnWallHold(false);
        }

        else if(onFloor)
            OffWallHold();
    }



    void OnSlide() {
        canMove = false;
        isSlidin = true;

        cols[0].enabled = false;
        cols[1].enabled = true;
    }



    void OffSlide() {
        canMove = true;
        isSlidin = false;

        cols[0].enabled = true;
        cols[1].enabled = false;
    }



    void OnRoll() {
        canMove = false;
        isRollin = true;
    }



    void OffRoll() {
        canMove = true;
        isRollin = false;
    }



    void SetAnimationValues() {
        if(isMovin && !ropeSlide && !holdinWall)
            FaceDirection(moveVar > 0);

        anim.SetBool("isMovin", isMovin);
        anim.SetFloat("velocityX",rb.velocity.x);
        anim.SetFloat("velocityY",rb.velocity.y);
        anim.SetBool("onFloor",onFloor);
        anim.SetBool("isRopeSliding",ropeSlide);
        anim.SetBool("holdinWall",holdinWall);
        anim.SetBool("isSlidin",isSlidin);
        anim.SetBool("isRollin",isRollin);
    }



    bool canMove = true, holdinWall = false, isSlidin = false, isRollin = false;
    [HideInInspector]
    public bool onFloor = false;

    void FixedUpdate(){
        onFloor = Physics2D.OverlapCircleAll(feet.position,jumpRadius,lm).Length > 1;
        isHoldingWall();

        if(canMove)
            rb.velocity += new Vector2(moveVar * Time.fixedDeltaTime,0);

        else if(ropeSlide) {
            rb.velocity += new Vector2((ropeDirRight ? ropeSpeedBoost : -ropeSpeedBoost)*Time.fixedDeltaTime,0);

            RaycastHit2D rch = Physics2D.Raycast(ropeHandler.position,
                ropeDirRight ? Vector2.right : Vector2.left,ropeAutoRelease,lm); 

            if(rch != false && rch.collider.IsTouchingLayers(lm) || moveInpVer < -0.3)
                OffRopeSlide();
        }
    }


    void Update() => SetAnimationValues();

    //public void DealDamage(uint dmg = 1) => gc.DamagePlayer(dmg);
}
