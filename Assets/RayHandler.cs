using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO:
// Allow dynamic resizing of player dimensions
// have width and height parameters to enter which will:
//  -   adjust number of rays to go along with dimensions
//  -   resize collider and adjust offset
//  -   resize sprite (only temp)

public class RayHandler : MonoBehaviour
{
    [Header("Editor Settings")]
    public Collider2D col;
    public BoxCollider2D boxCol;
    public LayerMask groundLayer, oneWayLayer;

    [Header("Basic Character Settings")]
    public float movementSpeed = 7.0f;
    public float maxWalkAngle = 50f;
    public float maxJumpHeight = 3.6f; //in units
    public int maxAirJumps = 2;
    public float gravity = 20.0f;
    public bool canJump = true;
    public bool canCrouch = false;
    public float crouchHeightMod = 0.5f;

    // Advanced Character Settings
    float remainGroundedSnap = 0.25f;
    float groundSnap = 0.05f;
    float snapVelocityLimit = 4.0f;

    float standingHeight = 1.0f;
    float crouchHeight = 1.0f;

    float jumpCooldown = 0.1f;
    float jumpVel = 1.0f;
    float jumpInputPersist = 0.1f;

    public float slideVel = 10.0f;
    float knockbackVel = 1.0f;
    float knockbackVelH = 3.0f;
    float knockbackHeight = 1.0f;
    public float iFrameDuration = 1.0f;
    bool invincible = false;

    public int hRays = 5, vRays = 9;
    public float rayDist = 1.0f;
    float edgeInset = 0.98f;
    public int movementSubdivisions = 4;

    // Character State
    int moveDir = 0;
    public int lastMoveDir = 0;
    public bool grounded;
    Vector2 groundNormal;
    float groundAngle;
    public bool knockback = false;
    bool simEnabled = true;
    public bool isCrouched = false;

    public Vector2 velocity = Vector2.zero; //only used for in-air vertical velocity
    bool jumping = false;
    public int airJumpsPerformed = 0;
    
    // Temp Raycast Result Storage
    RaycastHit2D topHit, bottomHit, rightHit, leftHit, moveHit;
    public float topDist, bottomDist, rightDist, leftDist, moveDist;
    public bool topCollide, bottomCollide, rightCollide, leftCollide, moveCollide;

    // Input
    int xAxis = 0;
    bool jumpInput = false;
    bool crouchInput = false;
    bool jumpDampenFlag = false;
    float step;

    [Header("Debug")]
    public float updatePeriod = -0.1f;
    public bool showRays = true;
    public int raycastsPerUpdate = 0;

    //For convenience
    float halfWidth, halfHeight;
    Vector3 center, halfHeightVec, halfWidthVec;

    void Start() {
        //calculate jump velocity based on max jump height
        jumpVel = Mathf.Sqrt(2 * gravity * maxJumpHeight);
        knockbackVel = Mathf.Sqrt(2 * gravity * knockbackHeight);

        standingHeight = col.bounds.size.y;
        crouchHeight = standingHeight * crouchHeightMod;
    }
    
    void FixedUpdate() {
        CastRays();
        AdvanceMovement();
    }

    public void ProvideInput(int x, bool jumpStart, bool jumpRelease, bool crouch){
        if(knockback || !simEnabled){
            //override inputs if stunned
            return;
        }
        xAxis = x;
        crouchInput = crouch;

        if(canJump && jumpStart){
            jumpInput = true;
            jumpDampenFlag = false;
            StartCoroutine(JumpInputPersist());
        }
        if(canJump && jumpRelease){
            jumpDampenFlag = true;
        }
    }

    public void StartKnockback(GameObject other){
        if(invincible || !simEnabled){
            return;
        }
        StartCoroutine(StartIFrame());
        knockback = true;
        crouchInput = false;

        if (center.x  >= other.transform.position.x){
            velocity.x = knockbackVelH;
        }else{
            velocity.x = -knockbackVelH;
        }

        if (center.y >= other.transform.position.y || true){
            grounded = false;
            jumping = true;
            velocity.y = knockbackVel;
            StartCoroutine(JumpStartup());
        }
    }

    public void SetSimState(bool enabled){
        simEnabled = enabled;
        xAxis = 0;
        jumpInput = false;
        crouchInput = false;
    }

    public bool SimActive(){
        return simEnabled;
    }

    public bool IsInvincible(){
        return invincible;
    }

    public void CastRays() {
        if(simEnabled){
            raycastsPerUpdate = 0;
            UpdateVariables();
            CastHRays(Vector3.zero);
            CastVRays(Vector3.zero);
            GetAccurateGroundDist(Vector3.zero);
            CheckGrounded();
        }
    }

    public void AdvanceMovement(){
        if(simEnabled){
            HandleMove();
        }
    }

    public bool SetCharHeight(float height){
        float oldHeight = boxCol.size.y;

        if(topCollide && topDist < height - oldHeight){
            Debug.Log("can't change height!");
            return false;
        }
        
        boxCol.size = new Vector2(boxCol.size.x, height);
        Vector3 temp = boxCol.transform.localPosition;
        temp.y = height * 0.5f;
        boxCol.transform.localPosition = temp;

        return true;
    }

    void CastHRays(Vector3 offset){
        //reset vars
        rightCollide = false;
        leftCollide = false;

        //Handle horizontal first
        float rayDistH = halfWidth + rayDist;

        for(int i = 0; i< hRays; i++){

            //cast right
            Vector3 start = center - halfHeightVec*edgeInset + offset;
            Vector3 end = center + halfHeightVec*edgeInset + offset;
            Vector3 inc = (end - start) / (Mathf.Max(hRays - 1, 1));

            Vector3 rayPos = start + (inc * i);
            RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.right, rayDistH, groundLayer);

            if(hit.collider != null){
                if(showRays) Debug.DrawLine(rayPos, hit.point, Color.green, Time.fixedDeltaTime/updatePeriod);

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
            }else{
                if(showRays) Debug.DrawLine(rayPos, rayPos + Vector3.right*rayDistH, Color.red, Time.fixedDeltaTime/updatePeriod);
            }
            raycastsPerUpdate++;

            //cast left
            start = center - halfHeightVec*edgeInset + offset;
            end = center + halfHeightVec*edgeInset + offset;
            inc = (end - start) / (Mathf.Max(hRays - 1, 1));

            rayPos = start + (inc * i);
            hit = Physics2D.Raycast(rayPos, Vector2.left, rayDistH, groundLayer);

            if(hit.collider != null){
                if(showRays) Debug.DrawLine(rayPos, hit.point, Color.green, Time.fixedDeltaTime/updatePeriod);

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
            }else{
                if(showRays) Debug.DrawLine(rayPos, rayPos + Vector3.left*rayDistH, Color.red, Time.fixedDeltaTime/updatePeriod);
            }
            raycastsPerUpdate++;
        }
    }
    void CastVRays(Vector3 offset){
        //reset vars
        topCollide = false;
        bottomCollide = false;

        //handle vertical
        float rayDistV = halfHeight + rayDist;

        for(int i = 0; i< vRays; i++){

            //cast up
            Vector3 start = center - halfWidthVec*edgeInset + offset;
            Vector3 end = center + halfWidthVec*edgeInset + offset;
            Vector3 inc = (end - start) / (Mathf.Max(vRays - 1, 1));

            Vector3 rayPos = start + (inc * i);
            RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.up, rayDistV, groundLayer);

            if(hit.collider != null){
                float dist = hit.point.y - (center.y + halfHeight + offset.y);
                if(!topCollide || dist < topDist){
                    topCollide = true;
                    topDist = dist;
                    topHit = hit;
                }
                if(showRays) Debug.DrawLine(rayPos, hit.point, Color.green, Time.fixedDeltaTime/updatePeriod);
            }else{
                if(showRays) Debug.DrawLine(rayPos, rayPos + Vector3.up*rayDistV, Color.red, Time.fixedDeltaTime/updatePeriod);
            }
            raycastsPerUpdate++;

            //cast down
            start = center - halfWidthVec*edgeInset + offset - (halfHeightVec * edgeInset - (Vector3.up * groundSnap));
            end = center + halfWidthVec*edgeInset + offset - (halfHeightVec * edgeInset - (Vector3.up * groundSnap));
            inc = (end - start) / (Mathf.Max(vRays - 1, 1));

            rayPos = start + (inc * i);
            hit = Physics2D.Raycast(rayPos, Vector2.down, rayDistV, groundLayer | oneWayLayer);

            if(hit.collider != null){
                float dist = hit.point.y - (center.y - halfHeight + offset.y);

                if(oneWayLayer == (oneWayLayer | (1 << hit.collider.gameObject.layer))){
                    if(!crouchInput && dist <= 0.005f && velocity.y <= 0){
                        //valid one way platform
                    }else{
                        continue;
                    }
                }
                if(!bottomCollide || dist > bottomDist){
                    bottomCollide = true;
                    bottomDist = dist;
                    bottomHit = hit;
                }
                if(showRays) Debug.DrawLine(rayPos, hit.point, Color.green, Time.fixedDeltaTime/updatePeriod);
            }else{
                if(showRays) Debug.DrawLine(rayPos, rayPos + Vector3.down*rayDistV, Color.red, Time.fixedDeltaTime/updatePeriod);
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

        float rayDistV = halfHeight + rayDist;

        foreach(Vector3 pos in rayPosList){
            Vector3 usePos = pos;

            if(pos.x > center.x + halfWidth + offset.x){
                usePos.x = center.x + halfWidth + offset.x;
            }
            if(pos.x < center.x - halfWidth + offset.x){
                usePos.x = center.x - halfWidth + offset.x;
            }

            RaycastHit2D hit = Physics2D.Raycast(usePos, Vector2.down, rayDistV, groundLayer | oneWayLayer);

            if(hit.collider != null){
                float dist = hit.point.y - (center.y - halfHeight + offset.y);
                if(oneWayLayer == (oneWayLayer | (1 << hit.collider.gameObject.layer))){
                    if(!crouchInput && dist <= 0.005f && velocity.y <= 0){
                        //valid one way platform
                    }else{
                        continue;
                    }
                }
                if(!bottomCollide || dist > bottomDist){
                    bottomCollide = true;
                    bottomDist = dist;
                    bottomHit = hit;
                }
                if(showRays) Debug.DrawLine(usePos, hit.point, Color.blue, Time.fixedDeltaTime/updatePeriod);
            }else{
                if(showRays) Debug.DrawLine(usePos, usePos + Vector3.down*rayDistV, Color.red, Time.fixedDeltaTime/updatePeriod);
            }
            raycastsPerUpdate++;
        }
    }

    void CheckGrounded(){
        if(bottomCollide && !jumping && (velocity.y <= snapVelocityLimit || grounded) &&
            ((grounded && bottomDist >= -remainGroundedSnap) || (!grounded && bottomDist >= -groundSnap))){
            
            grounded = true;
            groundNormal = bottomHit.normal;
            velocity.y = 0;
            groundAngle = GetHitAngle(bottomHit, false);
            airJumpsPerformed = 0;
            knockback = false;
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
        float movementInc = step / (float)movementSubdivisions;
        float stepInc = 1.0f / (float) movementSubdivisions;

        if(velocity.x > 0){
            velocity.x = Mathf.Max(velocity.x - 0.1f * step, 0.0f);
        }
        else if(velocity.x < 0){
            velocity.x = Mathf.Min(velocity.x + 0.1f * step, 0.0f);
        }
        
        
        UpdateVariables();
        for(int i = 0; i < movementSubdivisions; i++){
            //update info
            CastHRays(moveVec);
            CastVRays(moveVec);
            //GetAccurateGroundDist(moveVec);
            CheckGrounded();

            Vector3 moveVecInc = Vector2.Perpendicular(groundNormal)*-1f * velocity.x * movementInc;

            // Handle jumping
            if(((airJumpsPerformed < maxAirJumps) || grounded) && !jumping && jumpInput && (!topCollide || topDist > 0.05f)){
                if(!grounded){
                    airJumpsPerformed++;
                }
                if(isCrouched && false){
                    velocity.x = slideVel * (float)lastMoveDir; //WIP sliding
                    //could use arrow keys to slide as well. Can only slide when x velocity is below certain value
                    //when sliding, use custom decelleration for velocity, or tweak existing one
                }else{
                    jumpInput = false;
                    jumping = true;
                    velocity.y = jumpVel;
                    grounded = false;
                    
                    StopCoroutine(JumpInputPersist());
                    StartCoroutine(JumpStartup());
                }
                
            }

            // Handle crouching
            if(canCrouch && grounded && !isCrouched && crouchInput){
                isCrouched = SetCharHeight(crouchHeight);
            }else if (isCrouched && (!grounded || !crouchInput)){
                isCrouched = !SetCharHeight(standingHeight);
            }

            if(!grounded){
                moveVecInc += Vector3.up * velocity.y *step * stepInc;
            }

            if(moveDir==-1 && leftCollide && leftDist > moveVecInc.x){ //if moving left into wall
                moveVecInc.x = leftDist;
                velocity.x = 0;
            }
            if(moveDir==1 && rightCollide && rightDist < moveVecInc.x){ //if moving right into wall
                moveVecInc.x = rightDist;
                velocity.x = 0;
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
                if(jumpDampenFlag && velocity.y > 0){
                    velocity.y = velocity.y / 2.0f;
                    jumpDampenFlag = false;
                }
                velocity.y -= (gravity * step) * stepInc;
            }
        }

        if(bottomCollide && grounded){
            moveVec += Vector3.up * bottomDist;
        }

        transform.position += moveVec;
    }

    void UpdateVariables(){
        moveDir = xAxis;
        if(moveDir != 0){
            lastMoveDir = moveDir;
        }
        if(!knockback){
            velocity.x = moveDir * movementSpeed;
        }
        step = Time.fixedDeltaTime;

        halfWidth = col.bounds.extents.x;
        halfHeight = col.bounds.extents.y;
        center = col.bounds.center;

        halfWidthVec = Vector3.right * halfWidth;
        halfHeightVec = Vector3.up * halfHeight;

        jumpVel = Mathf.Sqrt(2 * gravity * maxJumpHeight);
        knockbackVel = Mathf.Sqrt(2 * gravity * knockbackHeight);
    }

    IEnumerator JumpStartup(){
        yield return new WaitForSeconds(jumpCooldown);
        jumping = false;
    }

    IEnumerator JumpInputPersist(){
        yield return new WaitForSeconds(jumpInputPersist);
        jumpInput = false;
    }

    IEnumerator StartIFrame(){
        invincible = true;
        yield return new WaitForSeconds(iFrameDuration);
        invincible = false;
        HitDetector hd = GetComponentInChildren<HitDetector>();
        if(hd != null){
            hd.DetectHit();
        }
    }
}
