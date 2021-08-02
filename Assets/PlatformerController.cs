using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO:
//clean up code and remove unnecessary bits
//crouching (create function to change player/collider size)

public class PlatformerController : MonoBehaviour
{
    [Header("Editor Settings")]
    public Collider2D col;
    public LayerMask groundLayer, oneWayLayer;

    [Header("Basic Character Settings")]
    public float movementSpeed = 7.0f;
    public float maxWalkAngle = 50f;
    public float maxJumpHeight = 3.6f; //in units
    public bool constantXSpeed = true; //maintain max horizontal speed on slopes
    public float gravity = 20.0f;

    // Advanced Character Settings
    float remainGroundedSnap = 0.25f;
    float groundSnap = 0.1f;

    float jumpCooldown = 0.1f;
    float jumpVel = 1.0f;
    float jumpInputPersist = 0.1f;

    int hRays = 9, vRays = 15;
    float edgeInset = 0.98f;
    int movementSubdivisions = 4;

    // Character State
    int moveDir = 0;
    int lastMoveDir = 0;
    bool grounded;
    bool jumping = false;
    Vector2 groundNormal;
    float groundAngle;
    Vector2 velocity = Vector2.zero; //only used for in-air vertical velocity
    
    // Temp Raycast Result Storage
    RaycastHit2D topHit, bottomHit, rightHit, leftHit, moveHit;
    float topDist, bottomDist, rightDist, leftDist, moveDist;
    bool topCollide, bottomCollide, rightCollide, leftCollide, moveCollide;

    // Input
    int xAxis = 0;
    bool jumpInput = false;
    float step;

    [Header("Debug")]
    public float updatePeriod = -0.1f;
    public bool showRays = true;
    public bool updateJumpVel = false;
    float updateCounter = 0.0f;
    public int raycastsPerUpdate = 0;

    //For convenience
    float halfWidth, halfHeight;
    Vector3 center, halfHeightVec, halfWidthVec;

    void Start() {
        //calculate jump velocity based on max jump height
        jumpVel = Mathf.Sqrt(2 * gravity * maxJumpHeight);
    }

    void Update(){
        UpdateInput();

        if(updateJumpVel){ //allow updating jump variables on the fly (debug)
            jumpVel = Mathf.Sqrt(2 * gravity * maxJumpHeight);
        }
    }

    void FixedUpdate() {
        if(updateCounter < updatePeriod){ //debug
            updateCounter += Time.fixedDeltaTime;
            return;
        }
        updateCounter = 0f;
        raycastsPerUpdate = 0;

        UpdateVariables();
        CastHRays(Vector3.zero);
        CastVRays(Vector3.zero);
        GetAccurateGroundDist(Vector3.zero);
        CheckGrounded();

        HandleMove();
    }

    void CastHRays(Vector3 offset){
        //reset vars
        rightCollide = false;
        leftCollide = false;

        //Handle horizontal first
        float rayDist = halfWidth + 2.0f;

        for(int i = 0; i< hRays; i++){

            //cast right
            Vector3 start = center - halfHeightVec*edgeInset + offset;
            Vector3 end = center + halfHeightVec*edgeInset + offset;
            Vector3 inc = (end - start) / (Mathf.Max(hRays - 1, 1));

            Vector3 rayPos = start + (inc * i);
            RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.right, rayDist, groundLayer);

            if(hit.collider != null){
                float angle = GetHitAngle(hit, false);
                if(angle < maxWalkAngle){
                    continue;
                }
                float dist = hit.point.x - (center.x + halfWidth + offset.x);
                if(!rightCollide || dist < rightDist){
                    rightCollide = true;
                    rightDist = dist;
                    rightHit = hit;
                }
                if(showRays) Debug.DrawLine(rayPos, hit.point, Color.green, Time.fixedDeltaTime/updatePeriod);
            }else{
                if(showRays) Debug.DrawLine(rayPos, rayPos + Vector3.right*rayDist, Color.red, Time.fixedDeltaTime/updatePeriod);
            }
            raycastsPerUpdate++;

            //cast left
            start = center - halfHeightVec*edgeInset + offset;
            end = center + halfHeightVec*edgeInset + offset;
            inc = (end - start) / (Mathf.Max(hRays - 1, 1));

            rayPos = start + (inc * i);
            hit = Physics2D.Raycast(rayPos, Vector2.left, rayDist, groundLayer);

            if(hit.collider != null){
                float angle = GetHitAngle(hit, false);
                if(angle < maxWalkAngle){
                    continue;
                }
                float dist = hit.point.x - (center.x - halfWidth + offset.x);
                if(!leftCollide || dist > leftDist){
                    leftCollide = true;
                    leftDist = dist;
                    leftHit = hit;
                }
                if(showRays) Debug.DrawLine(rayPos, hit.point, Color.green, Time.fixedDeltaTime/updatePeriod);
            }else{
                if(showRays) Debug.DrawLine(rayPos, rayPos + Vector3.left*rayDist, Color.red, Time.fixedDeltaTime/updatePeriod);
            }
            raycastsPerUpdate++;
        }
    }
    void CastVRays(Vector3 offset){
        //reset vars
        topCollide = false;
        bottomCollide = false;

        //handle vertical
        float rayDist = halfHeight + 2.0f;

        for(int i = 0; i< vRays; i++){

            //cast up
            Vector3 start = center - halfWidthVec*edgeInset + offset;
            Vector3 end = center + halfWidthVec*edgeInset + offset;
            Vector3 inc = (end - start) / (Mathf.Max(vRays - 1, 1));

            Vector3 rayPos = start + (inc * i);
            RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.up, rayDist, groundLayer);

            if(hit.collider != null){
                float dist = hit.point.y - (center.y + halfHeight + offset.y);
                if(!topCollide || dist < topDist){
                    topCollide = true;
                    topDist = dist;
                    topHit = hit;
                }
                if(showRays) Debug.DrawLine(rayPos, hit.point, Color.green, Time.fixedDeltaTime/updatePeriod);
            }else{
                if(showRays) Debug.DrawLine(rayPos, rayPos + Vector3.up*rayDist, Color.red, Time.fixedDeltaTime/updatePeriod);
            }
            raycastsPerUpdate++;

            //cast down
            start = center - halfWidthVec*edgeInset + offset;
            end = center + halfWidthVec*edgeInset + offset;
            inc = (end - start) / (Mathf.Max(vRays - 1, 1));

            rayPos = start + (inc * i);
            hit = Physics2D.Raycast(rayPos, Vector2.down, rayDist, groundLayer);

            if(hit.collider != null){
                float dist = hit.point.y - (center.y - halfHeight + offset.y);
                if(!bottomCollide || dist > bottomDist){
                    bottomCollide = true;
                    bottomDist = dist;
                    bottomHit = hit;
                }
                if(showRays) Debug.DrawLine(rayPos, hit.point, Color.green, Time.fixedDeltaTime/updatePeriod);
            }else{
                if(showRays) Debug.DrawLine(rayPos, rayPos + Vector3.down*rayDist, Color.red, Time.fixedDeltaTime/updatePeriod);
            }
            raycastsPerUpdate++;
        }
    }

    void GetAccurateGroundDist(Vector3 offset){
        if(!grounded){
            return;
        }
        List<Vector3> rayPosList = new List<Vector3>();

        rayPosList.Add(center - halfWidthVec + offset);
        rayPosList.Add(center + offset);
        rayPosList.Add(center + halfWidthVec + offset);
        rayPosList.Add(new Vector3(bottomHit.point.x + (1f-edgeInset)*0.5f, center.y + offset.y, 0));
        rayPosList.Add(new Vector3(bottomHit.point.x + (1f-edgeInset)*0.25f, center.y + offset.y, 0));
        rayPosList.Add(new Vector3(bottomHit.point.x - (1f-edgeInset)*0.5f, center.y + offset.y, 0));
        rayPosList.Add(new Vector3(bottomHit.point.x - (1f-edgeInset)*0.25f, center.y + offset.y, 0));

        float rayDist = halfHeight + 2.0f;

        foreach(Vector3 pos in rayPosList){
            Vector3 usePos = pos;

            if(pos.x > center.x + halfWidth + offset.x){
                usePos.x = center.x + halfWidth + offset.x;
            }
            if(pos.x < center.x - halfWidth + offset.x){
                usePos.x = center.x - halfWidth + offset.x;
            }

            RaycastHit2D hit = Physics2D.Raycast(usePos, Vector2.down, rayDist, groundLayer);

            if(hit.collider != null){
                float dist = hit.point.y - (center.y - halfHeight + offset.y);
                if(!bottomCollide || dist > bottomDist){
                    bottomCollide = true;
                    bottomDist = dist;
                    bottomHit = hit;
                }
                if(showRays) Debug.DrawLine(usePos, hit.point, Color.blue, Time.fixedDeltaTime/updatePeriod);
            }else{
                if(showRays) Debug.DrawLine(usePos, usePos + Vector3.down*rayDist, Color.red, Time.fixedDeltaTime/updatePeriod);
            }
            raycastsPerUpdate++;
        }
    }

    void CheckGrounded(){
        if(bottomCollide && !jumping &&
            ((grounded && bottomDist >= -remainGroundedSnap) || (!grounded && bottomDist >= -groundSnap))){
            
            grounded = true;
            groundNormal = bottomHit.normal;
            velocity.y = 0;
            groundAngle = GetHitAngle(bottomHit, false);
        }else{
            grounded = false;
            groundAngle = 0;
            groundNormal = Vector2.up;
        }
    }

    float GetHitAngle(RaycastHit2D hit, bool signed = true){
        if(signed){
            return Vector2.SignedAngle(Vector2.up, hit.normal);
        }else{
            return Vector2.Angle(Vector2.up, hit.normal);
        }
    }
    
    void HandleMove(){
        Vector3 moveVec = Vector3.zero;
        float movementInc = (movementSpeed * step) / (float)movementSubdivisions;
        float stepInc = 1.0f / (float) movementSubdivisions;
        
        UpdateVariables();
        for(int i = 0; i < movementSubdivisions; i++){
            //update info
            CastHRays(moveVec);
            CastVRays(moveVec);
            //GetAccurateGroundDist(moveVec);
            CheckGrounded();

            Vector3 moveVecInc = Vector2.Perpendicular(groundNormal)*-1f * moveDir * movementInc;

            if(constantXSpeed && moveDir != 0){
                float a = (moveDir * movementInc) / moveVecInc.x;
                moveVecInc *= a;
            }

            if(grounded && jumpInput && (!topCollide || topDist > 0.05f)){
                jumpInput = false;
                jumping = true;
                velocity.y = jumpVel;
                grounded = false;
                StopCoroutine(JumpInputPersist());
                StartCoroutine(JumpStartup());
            }

            if(!grounded){
                moveVecInc += Vector3.up * velocity.y *step * stepInc;
            }

            if(moveDir==-1 && leftCollide && leftDist > moveVecInc.x){ //if moving left into wall
                moveVecInc.x = leftDist;
            }
            if(moveDir==1 && rightCollide && rightDist < moveVecInc.x){ //if moving right into wall
                moveVecInc.x = rightDist;
            }
            if(topCollide && topDist < moveVecInc.y){ //if moving up into ceiling
                moveVecInc.y = topDist;
                velocity.y = 0;
            }
            if(bottomCollide && bottomDist > moveVecInc.y){ //if moving down into floor
                moveVecInc.y = bottomDist;
                velocity.y = 0;
            }

            if(bottomCollide && grounded && moveVecInc.y <= 0){
                moveVecInc += Vector3.up * (bottomDist - moveVecInc.y);
            }

            moveVec += moveVecInc;

            CastVRays(moveVec);
            //GetAccurateGroundDist(moveVec);
            CheckGrounded();
            
            if(!grounded){
                velocity.y -= (gravity * step) * stepInc;
            }
        }

        if(bottomCollide && grounded){
            moveVec += Vector3.up * bottomDist;
        }

        transform.position += moveVec;
    }

    void UpdateInput(){
        xAxis = 0;

        if(Input.GetKey(KeyCode.LeftArrow)){
            xAxis -= 1;
        }
        if(Input.GetKey(KeyCode.RightArrow)){
            xAxis += 1;
        }
        if(Input.GetKeyDown(KeyCode.UpArrow)){
            jumpInput = true;
            StartCoroutine(JumpInputPersist());
        }
    }

    void UpdateVariables(){
        moveDir = xAxis;
        if(moveDir != 0){
            lastMoveDir = moveDir;
        }
        step = Time.fixedDeltaTime;

        halfWidth = col.bounds.extents.x;
        halfHeight = col.bounds.extents.y;
        center = col.bounds.center;

        halfWidthVec = Vector3.right * halfWidth;
        halfHeightVec = Vector3.up * halfHeight;
    }

    IEnumerator JumpStartup(){
        yield return new WaitForSeconds(jumpCooldown);
        jumping = false;
    }

    IEnumerator JumpInputPersist(){
        yield return new WaitForSeconds(jumpInputPersist);
        jumpInput = false;
    }
}
