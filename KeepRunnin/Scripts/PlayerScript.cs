using UnityEngine;
using System;

public class PlayerScript: MonoBehaviour {
    [SerializeField]
    ushort moveSpeed = 33, jumpForce = 22;

    [SerializeField]
    float ropeSpeedBoost = 20;

    [SerializeField]
    float wallJumpX = 10, wallJumpY = 20, wallReleaseX = -2, wallReleaseY = 7;//wallReleaseY = -3;

    [HideInInspector]
    public Rigidbody2D rb;
    Transform charDir;

    LayerMask lm;

    Transform[] feet = new Transform[2];
    float wallJumpRad;
    Vector2 jumpVector;

    Vector2 ropeHandleVec;
    Transform ropeHandler;
    readonly float ropeAutoRelease = 2f;
    CircleCollider2D ropeHandleCollider;

    Animator anim;

    Vector2 wallHoldSnap;
    Vector3 wallBodyOffset;
    float wallRayOffsetY;

    Collider2D[] cols;
    [SerializeField]
    PhysicsMaterial2D[] physicsMats;

    Vector3[] slidePushOffsets;
    Vector3[] rollEdgeOffsets;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();

        charDir = GameObject.Find("/Player/Character").transform;
        feet[0] = GameObject.Find("/Player/FootA").transform;
        feet[1] = GameObject.Find("/Player/FootB").transform;

        jumpVector = new Vector2(0,jumpForce);
        lm = LayerMask.GetMask("Foreground");

        ropeHandleVec = new Vector2(0f,GetComponentInChildren<CircleCollider2D>().radius * 3f);

        ropeHandleCollider = GameObject.Find("/Player/RopeHandle").GetComponent<CircleCollider2D>();
        ropeHandler = ropeHandleCollider.transform;

        anim = GetComponentInChildren<Animator>();

        wallJumpRad = transform.localScale.x / 2 + 0.08f;
        wallHoldSnap = new Vector2(0.06f,0);
        wallBodyOffset = new Vector3(0,transform.localScale.y / 2 - 0.1f,0);
        wallRayOffsetY = transform.localScale.y / 2 + 1f;

        //First col vertical(all other animations), second horizontal(slide), third half size (rolling)
        cols = new Collider2D[3]; cols[0] = GetComponent<PolygonCollider2D>();
        GetComponents<CapsuleCollider2D>().CopyTo(cols, 1);

        slidePushOffsets = new Vector3[] { new Vector3(-0.88f, 0.1f, 0), new Vector3(0.88f, -0.6f, 0) };
        rollEdgeOffsets = new Vector3[] { new Vector3(0.05f,1.8f), new Vector3(),
        new Vector3(), new Vector3()};
    }


    void FaceDirection(bool dir) => charDir.localScale = new Vector3(dir ? 1 : -1,1,1);
    bool GetFaceDirection() => charDir.localScale.x > 0;


    [HideInInspector]
    public float moveInpHor = 0;
    float moveInpVer = 0;
    bool isMovin = false;

    public void OnMove(Vector2 moveInp) {
        moveInpHor = moveInp.x * moveSpeed;
        isMovin = moveInpHor != 0;
        
        moveInpVer = moveInp.y;
    }


    public void Jump() {
        //Jump off the rope
        if(ropeSlide) {
            if(canReleaseRope)
                OffRopeSlide(); StartCoroutine(RopeHandleDelayer());
        }
        
        //WallJump
        else if(holdinWall) {
            OffWallHold();

            //Releasing wall and a tiny jump
            if(moveInpVer < -0.3f) {
                FaceDirection(holdingWallRight);
                rb.velocity += new Vector2(holdingWallRight ? wallReleaseX : -wallReleaseX, wallReleaseY);
            }
                
            //Full wall jump
            else {
                FaceDirection(!holdingWallRight);
                rb.velocity += new Vector2(holdingWallRight ? -wallJumpX : wallJumpX, wallJumpY);
            }

            /*
            if(moveInpVer < -0.30)
                rb.velocity += new Vector2(holdingWallRight ? wallReleaseX : -wallReleaseX, wallReleaseY);
            
            else if(!isMovin)
                rb.velocity += new Vector2(holdingWallRight ? wallReleaseX : -wallReleaseX, wallJumpY);
            */
        }

        //Normal Jump
        else if (onFloor){
            rb.velocity += jumpVector;
            if(isRollin)
                OffRoll();
        }
            
    }


    bool ropeSlide = false, ropeDirRight;
    readonly float ropeHandleSnap = 0.08f;

    public void OnRopeSlide(bool isBelow,bool isFromLeft,bool isFromRight,bool DirectionToRight) {
        ropeSlide = true;
        canMove = false;
        holdinWall = false;
        ropeDirRight = DirectionToRight;

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
        StartCoroutine(RopeHandleDelayer());
    }


    public void OffRopeSlide() {
        if(!ropeSlide)
            return;

        rb.position -= ropeHandleVec;

        ropeSlide = false;
        canMove = true;
    }


    bool canReleaseRope = true;
    public System.Collections.IEnumerator RopeHandleDelayer() {
        canReleaseRope = false;
        yield return new WaitForSeconds(0.3f);
        canReleaseRope = true;
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
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.position += GetFaceDirection() ? -wallHoldSnap : wallHoldSnap;
        holdinWall = false;
        canMove = true;
        wallTime = Time.time;
    }


    void IsHoldingWall() {
        if(!holdinWall){
            if(onFloor || Physics2D.RaycastAll(transform.position, Vector2.down, wallRayOffsetY, lm).Length > 1)
                return;
            

            Vector3 raycastPosUp = transform.position + wallBodyOffset;
            Vector3 raycastPosDown = transform.position - wallBodyOffset;

            if(Physics2D.RaycastAll(raycastPosUp, Vector2.right, wallJumpRad, lm).Length > 1 &&
                Physics2D.RaycastAll(raycastPosDown, Vector2.right, wallJumpRad, lm).Length > 1)
                OnWallHold(true);

            else if(Physics2D.RaycastAll(raycastPosUp, Vector2.left, wallJumpRad, lm).Length > 1 &&
                Physics2D.RaycastAll(raycastPosDown, Vector2.left, wallJumpRad, lm).Length > 1)
                OnWallHold(false);
        }

        else if(Physics2D.RaycastAll(transform.position, Vector2.down, wallRayOffsetY, lm).Length > 1)
            OffWallHold();
        /*
        if(!holdinWall) {
            if(onFloor || Physics2D.RaycastAll(transform.position,Vector2.down,wallRayOffsetY,lm).Length > 1)
                if(Physics2D.RaycastAll(transform.position, Vector2.down, wallRayOffsetY, lm).Length > 1)
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

        else if(Physics2D.RaycastAll(transform.position, Vector2.down, wallRayOffsetY, lm).Length > 1)
            OffWallHold();
        */
    }


    [SerializeField]
    float slideSpeed = 1, slideReq = 8, slidePushThreshold = 2.5f;
    [SerializeField]
    Vector2 slidePushForce;
    float slideCD = 0; bool slideAboveBlock = false, isSlidePushing = false;


    void ShouldSlide() {
        if(!isSlidin) {
            if(onFloor && canMove && moveInpVer < -0.65 && Math.Abs(rb.velocity.x) > slideReq
                && Time.time > slideCD)
                OnSlide(rb.velocity.x > 0);
        }

        else {
            slideAboveBlock = Physics2D.OverlapAreaAll(transform.position + slidePushOffsets[0],
                transform.position + slidePushOffsets[1], lm).Length > 1;

            if(!onFloor || (moveInpVer > -0.65 && (!slideAboveBlock)))
                OffSlide();

            else if(!slideAboveBlock && !isSlidePushing)
                return;

            else if(isSlidePushing) {
                if(!slideAboveBlock)
                    OffSlide();
                else
                    rb.velocity += (GetFaceDirection() ? slidePushForce : -slidePushForce) * Time.fixedDeltaTime;
            }

            else if(Math.Abs(rb.velocity.x) < slidePushThreshold)
                isSlidePushing = true;
        }
    }


    void OnSlide(bool dirRight) {
        canMove = false;
        isSlidin = true;


        rb.velocity += new Vector2(dirRight ? slideSpeed : -slideSpeed,0);
        FaceDirection(dirRight);

        cols[0].enabled = false;
        cols[1].enabled = true;
        rb.sharedMaterial = physicsMats[1];
    }


    void OffSlide() {
        slideCD = Time.time + 0.7f;

        cols[0].enabled = true;
        cols[1].enabled = false;
        rb.sharedMaterial = physicsMats[0];

        canMove = true;
        isSlidin = false;
        isSlidePushing = false;
    }


    [SerializeField]
    float rollReqVelocity = -16.3f;
    [SerializeField]
    ushort rollSpeedMin = 35;
    float rollSpeed, lastVelocityY;
    readonly float rollLength = 0.416f;
    bool rollDir;

    void ShouldRoll() {
        if(!isRollin) {
            //Debug.Log($"Velocity Y: {lastVelocityY}, MoveInputY{moveInpVer}");
            //if(onFloor && moveInpVer < -0.6 && lastVelocityY < rollReqVelocity)
            if(onFloor && lastVelocityY < rollReqVelocity)
                OnRoll();

        }

        else if(Time.time > rollTime)
            OffRoll();

        /*
        else if(Physics2D.OverlapAreaAll(transform.position + rollEdgeOffsets[0], transform.position + 
            (GetFaceDirection() ? rollEdgeOffsets[1] : rollEdgeOffsets[2]),lm).Length < 2) {
            rb.velocity = new Vector2();
        }
        */

        lastVelocityY = rb.velocity.y;
    }


    float rollTime = 0;

    void OnRoll() {
        canMove = false;
        isRollin = true;

        rollDir = GetFaceDirection();
        rollTime = Time.time + rollLength;
        //Debug.Log($"Before Position X: {transform.position.x}");
        rollSpeed = rollSpeedMin + (rb.velocity.x / 2.5f) - (lastVelocityY / 4f);
        rb.velocity = new Vector2(rollDir ? rollSpeed : -rollSpeed,rb.velocity.y);
        //Debug.Log($"Roll Speed: {rollSpeed}");
        //Debug.Log("Roll Start");

        anim.Play("Roll");
    }


    void OffRoll() {
        canMove = true;
        isRollin = false;

        slideCD = Time.time + 0.4f;
        anim.Play("RollEnd");
        //Debug.Log($"After Position X: {transform.position.x}");
    }


    void SetAnimationValues() {
        if(isMovin && !ropeSlide && !holdinWall && !isSlidin && !isRollin)
            FaceDirection(moveInpHor > 0);

        if(isRollin)
            anim.Play("Roll");
        
        anim.SetBool("isRollin", isRollin);
        anim.SetBool("isMovin", isMovin);
        anim.SetFloat("velocityX", rb.velocity.x);
        anim.SetFloat("velocityY", rb.velocity.y);
        anim.SetFloat("lastVelocityY", lastVelocityY);
        anim.SetBool("onFloor", onFloor);
        anim.SetBool("isRopeSliding", ropeSlide);
        anim.SetBool("holdinWall", holdinWall);
        anim.SetBool("isSlidin", isSlidin);
        anim.SetBool("isSlidePushin", isSlidePushing);
    }


    bool canMove = true, holdinWall = false, isSlidin = false, isRollin = false;
    [HideInInspector]
    public bool onFloor = false;
    void FixedUpdate(){
        onFloor = Physics2D.OverlapAreaAll(feet[0].position, feet[1].position, lm).Length > 1;

        IsHoldingWall();
        ShouldRoll();
        ShouldSlide();

        if(canMove)
            rb.velocity += new Vector2(moveInpHor * Time.fixedDeltaTime,0);

        else if(ropeSlide) {
            rb.velocity += new Vector2((ropeDirRight ? ropeSpeedBoost : -ropeSpeedBoost)*Time.fixedDeltaTime,0);

            RaycastHit2D rch = Physics2D.Raycast(ropeHandler.position,
                ropeDirRight ? Vector2.right : Vector2.left,ropeAutoRelease,lm); 

            if(rch != false && rch.collider.IsTouchingLayers(lm))
                OffRopeSlide();
        }

        //if(rb.velocity.y < -9f)
            //Debug.Log("Now!");
    }


    void Update() => SetAnimationValues();
}
