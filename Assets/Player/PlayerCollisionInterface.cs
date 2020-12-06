using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerCollision {
    void OnJumpObstacleCollision(bool on_exit);
    void OnSlideObstacleCollision(bool on_exit);
    void OnFullObstacleCollision(bool on_exit);
    void OnJumpOrSlideObstacleCollision(bool on_exit);
    void OnCollectibleCollision();


}
