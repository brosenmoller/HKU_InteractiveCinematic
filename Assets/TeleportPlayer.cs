using UnityEngine;

public class TeleportPlayer : MonoBehaviour
{
    [SerializeField] private PlayerMovement player;
    [SerializeField] private Vector3 teleportTo;

    private void Awake()
    {
        player.WarpPlayer(teleportTo);
    }
}
