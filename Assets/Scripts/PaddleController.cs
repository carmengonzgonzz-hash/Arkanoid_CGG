using UnityEngine;
using UnityEngine.InputSystem;
public class PaddleController : MonoBehaviour
{
    [SerializeField] float speed = 10f;
    [SerializeField] float limitX = 7f;
    [SerializeField] GameManager gameManager;

    void Update()
    {
        //el return hace que todo lo que hay debajo no funcione si no esta en el modo playing. es decir, si ese condicional no se cumple no se hace lo que hay debajo en el código es como un EXIT
            if (gameManager.CurrentState != GameState.Playing)return;


        float input = Input.GetAxis("Horizontal");
        Vector3 movement = Vector3.right * input * speed * Time.deltaTime;
        transform.Translate(movement);
        
        float clampedX = Mathf.Clamp(transform.position.x, -limitX, limitX);
        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);

    }
}