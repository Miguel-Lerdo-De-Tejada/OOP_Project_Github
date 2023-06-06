using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;
using UnityEngine.Animations.Rigging;


public class ThirdPersonShooterController : MonoBehaviour
{
    [Header("Virtual cameras configurations:")]
    [SerializeField, Tooltip("Drag Aim virtual camera here")] CinemachineVirtualCamera aimVirtualCamera;
    [SerializeField, Tooltip("Normal camera rotation speed")] private float normalCameraRotationSpeed;
    [SerializeField, Tooltip("Aim camera rotation speed")] private float aimCameraRotationSpeed;
    [SerializeField, Tooltip("Aim collider mask")] private LayerMask aimColliderMask = new LayerMask();
    [SerializeField, Tooltip("Debug ray cast transform component")] Transform debugTransform;
    [SerializeField, Tooltip("Crosshair")] GameObject crosshair;
    [SerializeField, Tooltip("Aiming point")] GameObject aimingPoint;
    [SerializeField, Tooltip("Aim rig")] Rig aimRig;
    float aimRigWeight;
    // float rigSpeed = 10f;
    [Space(6)]

    [Header("Projectile configuration:")]
    [SerializeField, Tooltip("Shooting bullet")] Transform prefabBulletProjectile;
    [SerializeField, Tooltip("Bullet position when it is spawned")] Transform spawnBulletPosition;
    [SerializeField, Tooltip("Time rate for the next shooting"), Range(30,100)] float shootTimeRate=30;
    float nextTimeToFire = 0f;

    StarterAssetsInputs starterAssetsInputs;
    ThirdPersonController thirdPersonController;
    Animator animator;
    float speedAimAnim = 10f;

    float aimRotate = 20.0f;

    private void Start()
    {
        GetComponents();
    }

    [System.Obsolete]
    private void Update()
    {
        Vector3 mouseWorldPosition = ObtainMouseWorldPositiont();
        ZoomAim(mouseWorldPosition);
        ShootAim(prefabBulletProjectile,mouseWorldPosition);

        aimRig.weight = aimRigWeight; // Mathf.Lerp(aimRig.weight, aimRigWeight, Time.deltaTime * rigSpeed);
    }

    private void GetComponents()
    {
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        thirdPersonController = GetComponent<ThirdPersonController>();
        animator = GetComponent<Animator>();
    }

    private Vector3 ObtainMouseWorldPositiont()
    {
        Vector3 mouseWorldPosition = Vector3.zero;
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2.0f, Screen.height / 2.0f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999.0f, aimColliderMask))
        {
            debugTransform.position = raycastHit.point;

            mouseWorldPosition = raycastHit.point;
        }

        return mouseWorldPosition;
    }

    private void ZoomAim(Vector3 mouseWorldPosition)
    {
        if (starterAssetsInputs.aim)
        {
            ActivateAimCamera(true);            
            LookAtAim(mouseWorldPosition);
            aimRigWeight = 1f;
        }
        else
        {
            ActivateAimCamera(false);
            aimRigWeight = 0f;
        }
    }

    [System.Obsolete]
    private void ShootAim(Transform bullet, Vector3 mouseWorldPosition)
    {
        if (starterAssetsInputs.shoot && IsReadyToShoot())
        {            
            ShootBullet(bullet, mouseWorldPosition);
            ShootAnimation(true);
            starterAssetsInputs.shoot = false;
        }
        else
        {
            ShootAnimation(false);
        }

    }

    private void LookAtAim(Vector3 aimTarget)
    {
        Vector3 worldAimTarget = aimTarget;
        worldAimTarget.y = transform.position.y;
        Vector3 aimTargetDirection = (worldAimTarget - transform.position).normalized;

        transform.forward = Vector3.Lerp(transform.forward, aimTargetDirection, Time.deltaTime * aimRotate);
    }

    private void ActivateAimCamera(bool isActived)
    {
        aimVirtualCamera.gameObject.SetActive(isActived);
        thirdPersonController.SetCameraRotationSensitivity(aimCameraRotationSpeed);
        thirdPersonController.SetRotateOnMove(!isActived);
        animator.SetLayerWeight(AnimLayers.aim, LerpWeight(animator.GetLayerWeight(AnimLayers.aim), AimWaight(isActived),speedAimAnim));
        crosshair.SetActive(isActived);
        aimingPoint.SetActive(isActived);        
    }

    [System.Obsolete]
    private void ShootBullet(Transform bullet, Vector3 mouseWorldPosition)
    {
        Vector3 aimDir = (mouseWorldPosition - spawnBulletPosition.position).normalized;
        Instantiate(bullet, spawnBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.back));
    }

    private void ShootAnimation(bool isAnimated)
    {
        animator.SetLayerWeight(AnimLayers.shoot, AimWaight(isAnimated));
    }

    // Weight of the aim layer intended to reproduce it.
    private float AimWaight(bool isActive)
    {
        float weight=0.0f;        
        if (isActive) { weight = 1.0f; }
        return weight;
    }

    private float LerpWeight(float iniPoint,float finalWeight, float speed)
    {
        float currentWeight = Mathf.Lerp(iniPoint, finalWeight, speed * Time.deltaTime);
        return currentWeight;
    }

    private bool IsReadyToShoot()
    {
        bool isReady = Time.time >= nextTimeToFire;
        if (isReady) { nextTimeToFire = Time.time + 1/shootTimeRate; }
        return isReady;
    }
}

public struct AnimLayers
{
    public static int starterAsset = 0;
    public static int aim = 1;
    public static int shoot = 2;
}
