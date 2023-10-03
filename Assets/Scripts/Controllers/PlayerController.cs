using UnityEngine.EventSystems;
using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public LayerMask movementMask;
    Camera cam;
    PlayerMotor playerMotor;
    public Interactable focus;
    void Start()
    {
        cam = Camera.main;
        playerMotor = GetComponent<PlayerMotor>();
    }

    // Update is called once per frame
    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100, movementMask))
            {
                //Debug.Log("We hit " + hit.collider.name + "," + hit.point);
                playerMotor.MoveToPoint(hit.point);
                RemoveFocus();
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                //Debug.Log("We hit " + hit.collider.name + "," + hit.point);
                Interactable interactable= hit.collider.GetComponent<Interactable>();
                if(interactable!=null)
                {
                    SetFocus(interactable); 
                }
            }
        }
    }

    void RemoveFocus()
    {
        if(focus!=null)
            focus.OnDefocused();
        focus=null;
        playerMotor.StopFollowTarget();
        
    }

    void SetFocus(Interactable newFocus)
    {
        if(newFocus!=focus)
        {
            if(focus!=null)
                focus.OnDefocused();
            focus = newFocus;
            playerMotor.FollowTarget(newFocus);
        }
        newFocus.OnFocused(transform);
    }
}
