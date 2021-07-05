using UnityEngine;
using Mirror;
using System.Collections.Generic;
using System.Linq;

public class PlayerSlap : NetworkBehaviour {
    [Header("Objects")]
    [SerializeField] private MeshFilter slapMeshFilter;

    [Header("Layer Masks")]
    [SerializeField] private LayerMask objectsLayerMask;
    [SerializeField] private LayerMask playersLayerMask;

    [Header("Stats")]
    [SerializeField] private float slapRadius;
    [SerializeField] private float slapAngleSize;
    [SerializeField] private float slapForce;
    [SerializeField] private float slapUpForce;
    [SerializeField] private int slapUINumOfRays;

    private Vector3 _slapDirection;
    private Mesh _slapMesh;
    private Player _player;

    private void Awake() {
        _player = GetComponent<Player>();

        _slapMesh = new Mesh();
        slapMeshFilter.mesh = _slapMesh;
    }

    public override void OnStartClient() {
        if (isLocalPlayer) {
            _player.SlapJoystick.OnPointerUpEvent.AddListener(Slap);
        }

        base.OnStartClient();
    }

    private void Update() {
        if (isLocalPlayer) {
            UpdateSlapDirection();

            // Only draw slap UI when moving joystick
            if (_slapDirection != Vector3.zero) {
                _player.Animator.SetBool("IsCharging", true);
                
                DrawSlapUI();
            } else {
                _player.Animator.SetBool("IsCharging", false);

                _slapMesh.Clear();
            }
        }
    }

    private void Slap() {
        _player.Animator.Play(_player.SlapAnimation.name);

        List<RaycastHit> slapHits = GetSlapHits(playersLayerMask);
        List<GameObject> playerHits = new List<GameObject>();

        foreach (RaycastHit hit in slapHits) {
            if (hit.collider) {
                playerHits.Add(hit.collider.gameObject);
            }
        }

        playerHits = playerHits.Distinct().ToList();

        foreach (GameObject playerHit in playerHits) {
            // The collider is on the model, which is a child of the actual parent object with NetworkIdentity
            GameObject playerObject = playerHit.transform.parent.gameObject;
            Vector3 slapForceDirection = playerObject.transform.position - transform.position;
            slapForceDirection += new Vector3(0, slapUpForce, 0);
            slapForceDirection = slapForceDirection.normalized * slapForce;

            CmdSlap(playerObject, slapForceDirection);
        }
    }

    [Command]
    private void CmdSlap(GameObject target, Vector3 slapForceDirection) {
        NetworkIdentity targetIdentity = target.GetComponent<NetworkIdentity>();

        TargetSlap(targetIdentity.connectionToClient, slapForceDirection);
    }

    [TargetRpc]
    private void TargetSlap(NetworkConnection target, Vector3 slapForceDirection) {
        Rigidbody targetRigidbody = target.identity.GetComponent<Rigidbody>();

        targetRigidbody.AddForce(slapForceDirection * 10, ForceMode.Impulse);
    }

    private void DrawSlapUI() {
        List<RaycastHit> slapHits = GetSlapHits(objectsLayerMask);
        List<Vector3> hitPoints = new List<Vector3>();

        foreach (RaycastHit hit in slapHits) {
            hitPoints.Add(hit.point);
        }

        DrawSlapMesh(hitPoints);
    }

    private void DrawSlapMesh(List<Vector3> hitPoints) {
        int vertexCount = hitPoints.Count + 1; // + 1 for origin vertex at player position
        Vector3[] verticies = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        verticies[0] = Vector3.zero; // All positions are local so zero means on player

        for (int i = 0; i < vertexCount - 1; i++) {
            verticies[i + 1] = transform.InverseTransformPoint(hitPoints[i]);

            if (i < vertexCount - 2) {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }

        _slapMesh.Clear();
        _slapMesh.vertices = verticies;
        _slapMesh.triangles = triangles;
        _slapMesh.RecalculateNormals();
    }

    private List<RaycastHit> GetSlapHits(LayerMask layerMask) {
        float stepAngleSize = slapAngleSize / (slapUINumOfRays + 1);
        List<RaycastHit> hits = new List<RaycastHit>();

        for (int i = 0; i < slapUINumOfRays + 2; i++) { // + 2 because two rays are assumed on each side of the slapAngleSize
            float slapAngle = Mathf.Atan2(_slapDirection.x, _slapDirection.z) * Mathf.Rad2Deg;
            float angle = (slapAngle - slapAngleSize / 2 + stepAngleSize * i) * Mathf.Deg2Rad;

            Vector3 direction = new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle));

            RaycastHit hit;

            // If ray doesn't hit anything, just set the point of the hit to the max. Makes slap visualization easier
            if (!Physics.Raycast(transform.position, direction, out hit, slapRadius, layerMask)) {
                hit.point = transform.position + direction * slapRadius;
            }

            hits.Add(hit);
        }

        return hits;
    }

    private void UpdateSlapDirection() {
        _slapDirection = new Vector3(_player.SlapJoystick.Horizontal, 0, _player.SlapJoystick.Vertical).normalized;
        _slapDirection = Quaternion.Euler(0, _player.InitialRotationForward * Mathf.Rad2Deg, 0) * _slapDirection;
    }
}