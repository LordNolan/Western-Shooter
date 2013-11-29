using UnityEngine;
using System.Collections;

// TODO: will need to show back of enemy if between player and camera
public class EnemyFaceCamera : MonoBehaviour
{
    void Update()
    {
        GameObject player = (GameObject) GameObject.FindGameObjectWithTag("Player");
        if (player != null && !GlobalParams.InNonPlayingState()) {
            Vector3 playerPos = player.transform.position;
            transform.LookAt(new Vector3(playerPos.x, transform.position.y, playerPos.z));
        }
    }
}
