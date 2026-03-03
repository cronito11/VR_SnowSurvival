using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private Transform target;

    public void LateUpdate()
    {
        if (target != null)
        {
            Vector3 postion = target.position;
            postion.y = transform.position.y;
            transform.position = postion;
            
            Vector3 rotation = transform.eulerAngles;
            rotation.y = target.eulerAngles.y;
            transform.eulerAngles = rotation;
        }
    }
}
