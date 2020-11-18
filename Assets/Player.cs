using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Animator anim_ref;

    private bool isInsideTrigger = false;
    private bool isCrafting = false;

    [SerializeField]
    private GameObject body;

    public CharacterController controller;

    public float speed = 10f;
    // Start is called before the first frame update
    void Start()
    {

    }


    // Update is called once per frame
    void Update()
    {
        //turn off context when inside crafting
        //turn off movement when inside crafting
        //start caemra move

        /*if (isCrafting)
            UI_ref.showContextAlert = false;
        else
            UI_ref.showContextAlert = isInsideTrigger;
        UI_ref.showCrafting = isCrafting;*/

        //cant move if crafting
        if (!isCrafting)
        {
            Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            Vector3 direction = new Vector3(move.x, 0, move.z) + body.transform.position;
            body.transform.LookAt(direction);
            controller.Move(move * speed * CraftingModeManager.getTimeDialation() * Time.deltaTime);

            anim_ref.SetFloat("walking", move.magnitude);
        }

        //toggle crafting
        if (isInsideTrigger && Input.GetKeyDown("space"))
        {
            if (isCrafting)
            {
                isCrafting = false;
                CraftingModeManager.pauseSet(false);
            }
            else
            {
                isCrafting = true;
                CraftingModeManager.pauseSet(true);
            }
        }

    }
}
