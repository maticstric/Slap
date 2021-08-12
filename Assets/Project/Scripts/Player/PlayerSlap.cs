using UnityEngine;
using Mirror;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

public class PlayerSlap : NetworkBehaviour {
    [Header("Objects")]
    [SerializeField] private MeshFilter slapMeshFilter;
    [SerializeField] private TrailRenderer slapTrail;
    public ParticleSystem SlapParticles;

    [Header("Layer Masks")]
    [SerializeField] private LayerMask objectsLayerMask;
    [SerializeField] private LayerMask playersLayerMask;

    [Header("Stats")]
    [SerializeField] private float initialSlapRadius;
    [SerializeField] private float maxSlapRadius;
    [SerializeField] private float timeToMaxSlapRadius;
    [SerializeField] private float slapAngleSize;
    [SerializeField] private float slapForce;
    [SerializeField] private int slapUINumOfRays;
    [SerializeField] private float slapTrailDuration;
    [SerializeField] private float slapCooldown;
    [SerializeField] private float slapMovementCooldown;

    private Vector3 _slapDirection;
    private Vector3 _previousSlapDirection;

    private Mesh _slapMesh;
    private Player _player;
    private float _currentSlapRadius;

    private LevelManager _levelManager;

    [SyncVar(hook = "OnIsSlapTrailEmittingChanged")]
    public bool IsSlapTrailEmitting = false;

    private void Awake() {
        _player = GetComponent<Player>();

        _currentSlapRadius = initialSlapRadius;

        _slapMesh = new Mesh();
        slapMeshFilter.mesh = _slapMesh;
    }

    private void Start() {
        _levelManager = FindObjectOfType<LevelManager>();

        if (isLocalPlayer) {
            _levelManager.SlapJoystick.onPointerUpEvent += Slap;
        }
    }

    private void Update() {
        if (isLocalPlayer) {
            UpdateSlapDirection();

            // Only draw slap UI when moving joystick
            if (_slapDirection != Vector3.zero) {
                if (_previousSlapDirection == Vector3.zero) {
                    StartCoroutine("ChargeCurrentSlapRadius");

                    _player.Animator.SetBool("IsCharging", true);
                }

                DrawSlapUI();
            } else {
                if (_previousSlapDirection != Vector3.zero) {
                    StopCoroutine("ChargeCurrentSlapRadius");

                    _player.Animator.SetBool("IsCharging", false);
                }

                _slapMesh.Clear();
            }

            _previousSlapDirection = _slapDirection;
        }
    }

    [Command]
    private void CmdSetIsSlapTrailEmitting(bool isEmitting) {
        IsSlapTrailEmitting = isEmitting;
    }

    private IEnumerator ChargeCurrentSlapRadius() {
        while (true) {
            float time = 0;
            _currentSlapRadius = initialSlapRadius;

            while (time < timeToMaxSlapRadius) {
                float percent = time / timeToMaxSlapRadius;

                _currentSlapRadius = Mathf.SmoothStep(initialSlapRadius, maxSlapRadius, percent);

                time += Time.deltaTime;

                yield return null;
            }

            time = 0;

            while (time < timeToMaxSlapRadius) {
                float percent = time / timeToMaxSlapRadius;

                _currentSlapRadius = Mathf.SmoothStep(maxSlapRadius, initialSlapRadius, percent);

                time += Time.deltaTime;

                yield return null;
            }
        }
    }

    private IEnumerator ActivateSlapTrail() {
        CmdSetIsSlapTrailEmitting(true);

        yield return new WaitForSeconds(slapTrailDuration);

        CmdSetIsSlapTrailEmitting(false);
    }

    private IEnumerator DisableSlapJoystick() {
        _levelManager.SlapJoystick.SetEnabled(false);

        yield return new WaitForSeconds(slapCooldown);

        _levelManager.SlapJoystick.SetEnabled(true);
    }

    private void Slap() {
        if (_slapDirection != Vector3.zero) {
            StartCoroutine("ActivateSlapTrail");
            StartCoroutine("DisableSlapJoystick");

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

                Vector3 slapForceDirection = (playerObject.transform.position - transform.position).normalized;
                slapForceDirection *= slapForce;

                CmdSlap(playerObject, slapForceDirection);
            }
        }
    }

    [Command]
    private void CmdSlap(GameObject target, Vector3 slapForceDirection) {
        NetworkIdentity targetIdentity = target.GetComponent<NetworkIdentity>();

        TargetSlap(targetIdentity.connectionToClient, slapForceDirection);
        RpcPlaySlapParticles(target);
    }

    [ClientRpc]
    private void RpcPlaySlapParticles(GameObject target) {
        target.GetComponent<PlayerSlap>().SlapParticles.Play();
    }

    [TargetRpc]
    private void TargetSlap(NetworkConnection target, Vector3 slapForceDirection) {
        PlayerMovement playerMovement = target.identity.GetComponent<PlayerMovement>();
        CameraShake cameraShake = target.identity.GetComponent<Player>().CameraShake;

        StartCoroutine(cameraShake.Shake());

        StopCoroutine("ActivateMovementCooldown");
        StartCoroutine(ActivateMovementCooldown(playerMovement));

        Rigidbody targetRigidbody = target.identity.GetComponent<Rigidbody>();
        targetRigidbody.AddForce(slapForceDirection * 10, ForceMode.Impulse);
    }

    private IEnumerator ActivateMovementCooldown(PlayerMovement playerMovement) {
        playerMovement.MovementEnabled = false;

        yield return new WaitForSeconds(slapMovementCooldown);

        playerMovement.MovementEnabled = true;
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
            if (!Physics.Raycast(transform.position, direction, out hit, _currentSlapRadius, layerMask)) {
                hit.point = transform.position + direction * _currentSlapRadius;
            }

            hits.Add(hit);
        }

        return hits;
    }

    private void UpdateSlapDirection() {
        _slapDirection = new Vector3(_levelManager.SlapJoystick.Horizontal, 0, _levelManager.SlapJoystick.Vertical).normalized;
        _slapDirection = Quaternion.Euler(0, _player.InitialRotationForward * Mathf.Rad2Deg, 0) * _slapDirection;
    }

    private void OnIsSlapTrailEmittingChanged(bool oldValue, bool newValue) {
        slapTrail.emitting = IsSlapTrailEmitting;
    }
}