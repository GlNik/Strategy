using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraMove : MonoBehaviour
{
    [SerializeField] float _speed = 20f;
    [SerializeField] float _rotationSpeed = 1f;
    [SerializeField] float _scrollSpeed = 1200f;
    //[SerializeField] float _borderThickness = 10f;
    //[SerializeField] Vector2 _limit;

    [SerializeField] float _minY = 8f;
    [SerializeField] float _maxY = 60f;

    Vector2 _positionOne;
    Vector2 _positionTwo;

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            _speed = 0.06f;
            _scrollSpeed = 20;
        }
        else
        {
            _speed = 0.005f;
            _scrollSpeed = 10f;
        }

        float inputX = transform.position.y * _speed * Input.GetAxis("Horizontal");
        float inputY = transform.position.y * _speed * Input.GetAxis("Vertical");
        float scroll = Mathf.Log(transform.position.y) * -_scrollSpeed * Input.GetAxis("Mouse ScrollWheel");

        if ((transform.position.y >= _maxY) && (scroll > 0))
        {
            scroll = 0;
        }
        else if ((transform.position.y <= _minY) && (scroll < 0))
        {
            scroll = 0;
        }

        if ((transform.position.y + scroll) > _maxY)
        {
            scroll = _maxY - transform.position.y;
        }
        else if ((transform.position.y + scroll) < _minY)
        {
            scroll = _minY - transform.position.y;
        }

        Vector3 verticalMove = new Vector3(0, scroll, 0);
        Vector3 lateralMove = inputX * transform.right;
        Vector3 forwardMove = transform.forward;
        forwardMove.y = 0f;
        forwardMove.Normalize();
        forwardMove *= inputY;

        Vector3 move = verticalMove + lateralMove + forwardMove;
      
        transform.position += move; /** Time.deltaTime;*/
        CameraRotation();
    }

    //способ если камера не вращается

    /* private void Update()
     {
         Vector3 position = transform.position;
         if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || Input.mousePosition.y >= Screen.height - _borderThickness)
         {
             position.z += _speed * Time.deltaTime;
         }
         if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) || Input.mousePosition.y <= _borderThickness)
         {
             position.z -= _speed * Time.deltaTime;
         }
         if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) || Input.mousePosition.x <= _borderThickness)
         {
             position.x -= _speed * Time.deltaTime;
         }
         if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) || Input.mousePosition.x >= Screen.width - _borderThickness)
         {
             position.x += _speed * Time.deltaTime;
         }

         float scroll = Input.GetAxis("Mouse ScrollWheel");
         position.y -= scroll * _scrollSpeed * 100f * Time.deltaTime;

         position.x = Mathf.Clamp(position.x, -_limit.x, _limit.x);
         position.y = Mathf.Clamp(position.y, _minY, _maxY);
         position.z = Mathf.Clamp(position.z, -_limit.y, _limit.y);

         transform.position = position;        
     }*/

    void CameraRotation()
    {
        if (Input.GetMouseButtonDown(2))
        {
            _positionOne = Input.mousePosition;
        }
        if (Input.GetMouseButton(2))
        {
            _positionTwo = Input.mousePosition;

            float deltaX = (_positionTwo - _positionOne).x * _rotationSpeed * 100f * Time.deltaTime;
            float deltaY = (_positionTwo - _positionOne).y * _rotationSpeed * 100f * Time.deltaTime;

            transform.rotation *= Quaternion.Euler(new Vector3(0, deltaX, 0));
            transform.GetChild(0).transform.rotation *= Quaternion.Euler(new Vector3(-deltaY, 0, 0));

            _positionOne = _positionTwo;
        }
    }

}
