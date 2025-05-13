using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AlignedGames
{
    // Manages weapon selection, firing, ammunition, and animations
    public class WeaponManager : MonoBehaviour
    {
        // Weapon GameObjects
        public GameObject Shotgun;
        public GameObject MachineGun;
        public GameObject Pistol;

        // Weapon selection states
        public bool ShotgunSelected;
        public bool MachineGunSelected;
        public bool PistolSelected;

        // Fire state
        public bool HasFired;

        // Muzzle flash effects
        public GameObject ShotgunMuzzleflash;
        public GameObject MachinegunMuzzleflash;
        public GameObject PistolMuzzleflash;

        // Explosion effects
        public GameObject MachineGunExplosion;

        // Ammunition variables
        public float CurrentAmmunition;
        public float ShotgunAmmunition;
        public float MachineGunAmmunition;
        public float PistolAmmunition;

        // Weapon fire audio clips
        public AudioClip MachineGunFireAudioClip;
        public AudioClip ShotgunFireAudioClip;
        public AudioClip PistolFireAudioClip;
        public AudioClip MelleeAudioClip;

        // Muzzle points
        public GameObject ShotgunMuzzlePoint1;
        public GameObject MachineGunMuzzlePoint1;
        public GameObject PistolMuzzlePoint1;

        // UI elements
        public GameObject AmmunitionText;
        public GameObject MagsLeftAmmunitionText;
        public GameObject WeaponImage;
        public GameObject Camera;

        // Weapon damage values
        public float Damage;
        public float PistolDamage;
        public float MachineGunDamage;
        public float ShotgunDamage;

        // Hit effects
        public GameObject HitExplosionUntagged;
        public GameObject HitExplosionEnemy;

        // Raycasting for hit detection
        public RaycastHit Hit;
        public float HitRange;
        public float GunHitRange;
        public float MelleeHitRange;
        public LayerMask IgnoreMe;
        public Ray Ray;

        // Weapon animations
        public Animation PistolArmsAnimation;
        public Animation MachineGunArmsAnimation;
        public Animation ShotgunArmsAnimation;
        public Animation WeaponManagerAnimation;

        // Glow stick mechanics
        public GameObject FakeGlowStick;
        public GameObject GlowStick;
        public GameObject ThrowPoint;
        public float GlowsticksAmount;
        public GameObject GlowSticksText;
        public bool IsThrowing;
        public float ThrowTimer;
        public float Timer;
        public bool HasPlayedGlowstickBreak;
        public AudioClip GlowstickBreak;

        // Weapon aim positions
        public Vector3 PistolArmsAimPosition;
        public Vector3 PistolArmsNormalPosition;
        public Vector3 MachineGunArmsAimPosition;
        public Vector3 MachineGunArmsNormalPosition;
        public Vector3 ShotgunArmsAimPosition;
        public Vector3 ShotgunArmsNormalPosition;

        // UI elements for damage feedback
        public GameObject CrossHair;
        public GameObject DamageImage;
        public Sprite LimbShotSprite;
        public Sprite BodyShotSprite;
        public Sprite HeadShotSprite;
        public Sprite NoDamageShotSprite;
        public Sprite NoSprite;

        // Shooting helper text
        public GameObject ShootingHelperText;
        public float ResetDamageImageTime;
        public float DamageImageTimer;
        public bool ResetDamageImage;
        public bool ResetShootingHelperText;
        public float ShootingHelperTextTime;
        public float ShootingHelperTextTimer;

        // Weapon switching
        private int currentWeaponIndex = 0;
        private GameObject[] weapons;
        public float switchDelay;

        // Reloading state
        public bool IsReloading;

        // Magazine capacities and remaining mags
        public float PistolMagSize;
        public float PistolMagsLeft;
        public float MachineGunMagSize;
        public float MachineGunMagsLeft;
        public float ShotgunMagSize;
        public float ShotgunMagsLeft;

        // Aiming transition properties
        public float aimTransitionSpeed = 5f; // Speed of transition to aim
        private Vector3 targetPosition;
        private Vector3 initialPosition;
        private WeaponAnimatorBehaviour weaponAnimator;

        // Initialization functions
        public void Awake()
        {
            // Find and assign UI elements
            WeaponImage = GameObject.Find("WeaponImage");
            CrossHair = GameObject.Find("CrossHair");

            // Store weapon objects in an array
            weapons = new GameObject[] { Pistol, MachineGun, Shotgun };

            // Initialize weapon selection
            SwitchWeapon(currentWeaponIndex);

            // Find and initialize damage feedback UI
            DamageImage = GameObject.Find("DamageImage");
            DamageImage.GetComponent<Image>().sprite = NoSprite;

            // Find and initialize shooting helper text UI
            ShootingHelperText = GameObject.Find("ShootingHelperText");
            ShootingHelperText.GetComponent<Text>().text = "";
        }

        void Start()
        {
            // Store initial weapon position
            initialPosition = transform.localPosition;
            targetPosition = initialPosition;

            // Get reference to weapon animation script
            weaponAnimator = GetComponent<WeaponAnimatorBehaviour>();
        }

        public void Update()
        {
            // Finding the UI elements for weapon image and crosshair
            WeaponImage = GameObject.Find("WeaponImage");
            CrossHair = GameObject.Find("CrossHair");

            // Handling player input such as switching weapons, shooting, etc.
            HandleInput();

            // Creating a ray from the camera based on mouse position
            Ray = Camera.GetComponentInParent<Camera>().ScreenPointToRay(Input.mousePosition);

            // Finding the UI elements for displaying ammunition and magazine count
            AmmunitionText = GameObject.Find("AmmunitionText");
            MagsLeftAmmunitionText = GameObject.Find("AmmunitionText");

            // Updating the ammunition display based on the selected weapon
            if (PistolSelected)
            {
                // Showing pistol ammunition and magazines left in the UI
                AmmunitionText.GetComponent<Text>().text = CurrentAmmunition.ToString("0") + " / " + PistolMagsLeft.ToString("0");
            }

            if (MachineGunSelected)
            {
                // Showing machine gun ammunition and magazines left in the UI
                AmmunitionText.GetComponent<Text>().text = CurrentAmmunition.ToString("0") + " / " + MachineGunMagsLeft.ToString("0");
            }

            if (ShotgunSelected)
            {
                // Showing shotgun ammunition and magazines left in the UI
                AmmunitionText.GetComponent<Text>().text = CurrentAmmunition.ToString("0") + " / " + ShotgunMagsLeft.ToString("0");
            }

            // Updating the glowstick count on the UI
            GlowSticksText = GameObject.Find("GlowSticksText");
            GlowSticksText.GetComponent<Text>().text = GlowsticksAmount.ToString("0");

            // Resetting damage image if necessary
            if (ResetDamageImage)
            {
                // Timer for damage image reset
                DamageImageTimer += Time.deltaTime;

                // If the damage image timer exceeds the reset time, reset the image
                if (DamageImageTimer > ResetDamageImageTime)
                {
                    // Resetting the damage image sprite to no sprite
                    DamageImage.GetComponent<Image>().sprite = NoSprite;
                    ResetDamageImage = false;

                    // Resetting the damage timer
                    DamageImageTimer = 0;
                }
            }

            // Resetting the shooting helper text if necessary
            if (ResetShootingHelperText)
            {
                // Timer for resetting the shooting helper text
                ShootingHelperTextTimer += Time.deltaTime;

                // If the timer exceeds the allotted time, reset the shooting helper text
                if (ShootingHelperTextTimer > ShootingHelperTextTime)
                {
                    // Clearing the shooting helper text on the UI
                    ShootingHelperText.GetComponent<Text>().text = "";
                    ResetShootingHelperText = false;

                    // Resetting the helper text timer
                    ShootingHelperTextTimer = 0;
                }
            }

            // Checking if the reload button is pressed (R key)
            if (Input.GetKeyDown(KeyCode.R))
            {
                // Ensuring the player is not reloading or throwing
                if (!IsReloading)
                {
                    if (!IsThrowing)
                    {
                        if (!HasFired)
                        {
                            // Play reload animation
                            PlayReloadAnimation();
                        }
                    }
                }
            }

            // Checking if the throw button is pressed (default mouse button or custom button)
            if (Input.GetButtonDown("Throw"))
            {
                // Ensuring the player is not currently throwing
                if (!IsThrowing)
                {
                    if (GlowsticksAmount > 0)
                    {
                        // Set IsThrowing to true to begin the throwing animation
                        IsThrowing = true;

                        // Playing the appropriate throwing animation based on the selected weapon
                        if (PistolSelected)
                        {
                            // Pistol throw animation
                            PistolArmsAnimation.Stop();
                            PistolArmsAnimation.GetComponent<Animation>()["PistolArmsThrow"].layer = 1;
                            PistolArmsAnimation.Play("PistolArmsThrow");
                        }

                        if (MachineGunSelected)
                        {
                            // Machine gun throw animation
                            MachineGunArmsAnimation.Stop();
                            MachineGunArmsAnimation.GetComponent<Animation>()["MachineGunArmsThrow"].layer = 1;
                            MachineGunArmsAnimation.Play("MachineGunArmsThrow");
                        }

                        if (ShotgunSelected)
                        {
                            // Shotgun throw animation
                            ShotgunArmsAnimation.Stop();
                            ShotgunArmsAnimation.GetComponent<Animation>()["ShotgunArmsThrow"].layer = 1;
                            ShotgunArmsAnimation.Play("ShotgunArmsThrow");
                        }
                    }
                }
            }

            // Checking if the player is holding the right mouse button (aiming)
            if (Input.GetKey(KeyCode.Mouse1))
            {
                // Disabling the crosshair and setting the aiming flag to true
                CrossHair.GetComponent<UnityEngine.UI.Image>().enabled = false;
                GetComponent<WeaponAnimatorBehaviour>().isAiming = true;

                // Setting the target position for aiming based on the selected weapon
                if (PistolSelected)
                {
                    targetPosition = PistolArmsAimPosition;
                }
                else if (MachineGunSelected)
                {
                    targetPosition = MachineGunArmsAimPosition;
                }
                else if (ShotgunSelected)
                {
                    targetPosition = ShotgunArmsAimPosition;
                }
            }
            else
            {
                // Re-enabling the crosshair and resetting the aim position when released
                CrossHair.GetComponent<UnityEngine.UI.Image>().enabled = true;
                targetPosition = initialPosition;

                // Setting the aiming flag to false
                GetComponent<WeaponAnimatorBehaviour>().isAiming = false;
            }

            // Smoothly transitioning the weapon's position to the target aiming position
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, Time.deltaTime * aimTransitionSpeed);

            // Passing the weapon's current position to the animator for further handling
            weaponAnimator.SetCurrentPosition(transform.localPosition);

            // If the player is throwing a glowstick, manage the throw process
            if (IsThrowing)
            {
                Timer += Time.deltaTime;
                if (Timer > ThrowTimer / 2)
                {
                    // Show a fake glowstick effect after half the throw time
                    FakeGlowStick.SetActive(true);
                    if (!HasPlayedGlowstickBreak)
                    {
                        HasPlayedGlowstickBreak = true;
                        if (!GetComponent<AudioSource>().isPlaying)
                        {
                            // Play the glowstick break sound
                            GetComponent<AudioSource>().PlayOneShot(GlowstickBreak);
                        }
                    }
                }
                if (Timer > ThrowTimer)
                {
                    // Once the timer exceeds the throw time, instantiate the glowstick and reset states
                    IsThrowing = false;
                    Timer = 0;
                    Instantiate(GlowStick, ThrowPoint.transform.position, ThrowPoint.transform.rotation);
                    GlowsticksAmount -= 1;
                    HasPlayedGlowstickBreak = false;
                    FakeGlowStick.SetActive(false);
                }
            }

            // Handling firing and ammunition for the Pistol weapon
            if (PistolSelected)
            {
                Damage = PistolDamage;
                CurrentAmmunition = PistolAmmunition;
                if (CurrentAmmunition >= 1)
                {
                    if (!IsReloading)
                    {
                        if (Input.GetButtonDown("Fire1"))
                        {
                            if (!PistolArmsAnimation.IsPlaying("PistolArmsShoot"))
                            {
                                if (!HasFired)
                                {
                                    // Triggering screen shake for the pistol shot
                                    GetComponentInParent<ScreenShakeManager>().TriggerShake(0.2f, 0.1f);

                                    // Playing pistol shooting animation and reducing ammunition
                                    PistolArmsAnimation.Play("PistolArmsShoot");
                                    PistolAmmunition -= 1;
                                    GetComponentInChildren<AudioSource>().PlayOneShot(PistolFireAudioClip);
                                    ShootNormalRaycast();
                                    HitRange = GunHitRange;
                                    Instantiate(PistolMuzzleflash, PistolMuzzlePoint1.transform.position, PistolMuzzlePoint1.transform.rotation, PistolMuzzlePoint1.transform);
                                    HasFired = true;
                                }
                            }
                            if (HasFired)
                            {
                                HasFired = false;
                            }
                        }
                    }
                }
            }

            // Handling firing and ammunition for the Machine Gun weapon
            if (MachineGunSelected)
            {
                Damage = MachineGunDamage;
                CurrentAmmunition = MachineGunAmmunition;
                if (CurrentAmmunition >= 1)
                {
                    if (!IsReloading)
                    {
                        if (Input.GetButton("Fire1"))
                        {
                            if (!MachineGunArmsAnimation.IsPlaying("MachineGunArmsShoot"))
                            {
                                if (!HasFired)
                                {
                                    // Triggering screen shake for the machine gun shot
                                    GetComponentInParent<ScreenShakeManager>().TriggerShake(0.2f, 0.1f);

                                    // Playing machine gun shooting animation and reducing ammunition
                                    MachineGunArmsAnimation.Play("MachineGunArmsShoot");
                                    MachineGunAmmunition -= 1;
                                    GetComponentInChildren<AudioSource>().PlayOneShot(MachineGunFireAudioClip);
                                    ShootNormalRaycast();
                                    HitRange = GunHitRange;
                                    Instantiate(PistolMuzzleflash, MachineGunMuzzlePoint1.transform.position, MachineGunMuzzlePoint1.transform.rotation, MachineGunMuzzlePoint1.transform);
                                    Instantiate(MachineGunExplosion, Hit.point, transform.rotation);
                                    HasFired = true;
                                }
                            }
                            if (HasFired)
                            {
                                HasFired = false;
                            }
                        }
                    }
                }
            }

            // Handling firing and ammunition for the Shotgun weapon
            if (ShotgunSelected)
            {
                Damage = ShotgunDamage;
                CurrentAmmunition = ShotgunAmmunition;
                if (CurrentAmmunition >= 1)
                {
                    if (!IsReloading)
                    {
                        if (Input.GetButtonDown("Fire1"))
                        {
                            if (!ShotgunArmsAnimation.IsPlaying("ShotgunArmsShoot"))
                            {
                                if (!HasFired)
                                {
                                    // Triggering screen shake for the shotgun shot
                                    GetComponentInParent<ScreenShakeManager>().TriggerShake(0.2f, 0.3f);

                                    // Playing shotgun shooting animation and reducing ammunition
                                    ShotgunArmsAnimation.Play("ShotgunArmsShoot");
                                    ShotgunAmmunition -= 1;
                                    GetComponentInChildren<AudioSource>().PlayOneShot(ShotgunFireAudioClip);
                                    ShootNormalRaycast();
                                    HitRange = GunHitRange;
                                    Instantiate(ShotgunMuzzleflash, ShotgunMuzzlePoint1.transform.position, ShotgunMuzzlePoint1.transform.rotation, ShotgunMuzzlePoint1.transform);
                                    HasFired = true;
                                }
                            }
                            if (HasFired)
                            {
                                HasFired = false;
                            }
                        }
                    }
                }

            }

        }

        public void PlayReloadAnimation()
        {
            // Check if the Pistol is selected
            if (PistolSelected)
            {
                // Ensure there is at least one magazine left
                if (PistolMagsLeft >= 1)
                {
                    WeaponManagerAnimation.Stop(); // Stop the current animation
                                                   // Check if the reload animation is not already playing
                    if (!WeaponManagerAnimation.IsPlaying("WeaponReload"))
                    {
                        WeaponManagerAnimation.Play("WeaponReload"); // Play reload animation
                        IsReloading = true; // Set reloading flag
                        Invoke("Reload", 2f); // Reload after 2 seconds
                    }
                }
            }

            // Check if the Shotgun is selected
            if (ShotgunSelected)
            {
                // Ensure there is at least one magazine left
                if (ShotgunMagsLeft >= 1)
                {
                    WeaponManagerAnimation.Stop(); // Stop the current animation
                                                   // Check if the reload animation is not already playing
                    if (!WeaponManagerAnimation.IsPlaying("WeaponReload"))
                    {
                        WeaponManagerAnimation.Play("WeaponReload"); // Play reload animation
                        IsReloading = true; // Set reloading flag
                        Invoke("Reload", 2f); // Reload after 2 seconds
                    }
                }
            }

            // Check if the Machine Gun is selected
            if (MachineGunSelected)
            {
                // Ensure there is at least one magazine left
                if (MachineGunMagsLeft >= 1)
                {
                    WeaponManagerAnimation.Stop(); // Stop the current animation
                                                   // Check if the reload animation is not already playing
                    if (!WeaponManagerAnimation.IsPlaying("WeaponReload"))
                    {
                        WeaponManagerAnimation.Play("WeaponReload"); // Play reload animation
                        IsReloading = true; // Set reloading flag
                        Invoke("Reload", 2f); // Reload after 2 seconds
                    }
                }
            }
        }

        public void Reload()
        {
            // Reload logic for Pistol
            if (PistolSelected)
            {
                if (IsReloading)
                {
                    PistolAmmunition = 0f; // Reset ammo
                    CurrentAmmunition = 0f; // Reset current ammo
                    PistolAmmunition += PistolMagSize; // Set new ammo amount
                    CurrentAmmunition += PistolMagSize; // Set new current ammo
                    PistolMagsLeft -= 1; // Decrease magazine count
                    IsReloading = false; // Set reloading flag to false
                }
            }

            // Reload logic for Machine Gun
            if (MachineGunSelected)
            {
                if (IsReloading)
                {
                    MachineGunAmmunition = 0f; // Reset ammo
                    CurrentAmmunition = 0f; // Reset current ammo
                    MachineGunAmmunition += MachineGunMagSize; // Set new ammo amount
                    CurrentAmmunition += PistolMagSize; // Set new current ammo
                    MachineGunMagsLeft -= 1; // Decrease magazine count
                    IsReloading = false; // Set reloading flag to false
                }
            }

            // Reload logic for Shotgun
            if (ShotgunSelected)
            {
                if (IsReloading)
                {
                    ShotgunAmmunition = 0f; // Reset ammo
                    CurrentAmmunition = 0f; // Reset current ammo
                    ShotgunAmmunition += ShotgunMagSize; // Set new ammo amount
                    CurrentAmmunition += PistolMagSize; // Set new current ammo
                    ShotgunMagsLeft -= 1; // Decrease magazine count
                    IsReloading = false; // Set reloading flag to false
                }
            }
        }

        private void HandleInput()
        {
            // Check if the player is not currently reloading
            if (!IsReloading)
            {
                // Check for scrolling up to switch to the next weapon
                if (Input.GetAxis("Mouse ScrollWheel") > 0)
                {
                    // Switch to the next weapon
                    currentWeaponIndex = (currentWeaponIndex + 1) % weapons.Length;
                    StartCoroutine(SwitchWeaponWithAnimation(currentWeaponIndex));
                }
                // Check for scrolling down to switch to the previous weapon
                else if (Input.GetAxis("Mouse ScrollWheel") < 0)
                {
                    currentWeaponIndex--; // Decrease the weapon index
                    if (currentWeaponIndex < 0) currentWeaponIndex = weapons.Length - 1; // Wrap around
                    StartCoroutine(SwitchWeaponWithAnimation(currentWeaponIndex));
                }
                // Check for specific key inputs to switch weapons (Alpha1, Alpha2, Alpha3)
                else if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    StartCoroutine(SwitchWeaponWithAnimation(0));
                }
                else if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    StartCoroutine(SwitchWeaponWithAnimation(1));
                }
                else if (Input.GetKeyDown(KeyCode.Alpha3))
                {
                    StartCoroutine(SwitchWeaponWithAnimation(2));
                }
            }
        }

        private IEnumerator SwitchWeaponWithAnimation(int index)
        {
            // Play the switch animation
            SwitchAnimation();

            // Wait for the switch animation to complete
            yield return new WaitForSeconds(switchDelay);

            // Switch the weapon
            SwitchWeapon(index);
        }

        public void SwitchWeapon(int index)
        {
            currentWeaponIndex = index; // Set the current weapon index

            // Set the active weapon based on the index
            for (int i = 0; i < weapons.Length; i++)
            {
                weapons[i].SetActive(i == currentWeaponIndex);
            }

            // Set weapon selection flags based on the index
            PistolSelected = currentWeaponIndex == 0;
            MachineGunSelected = currentWeaponIndex == 1;
            ShotgunSelected = currentWeaponIndex == 2;
        }

        private void SwitchAnimation()
        {
            // Check if the weapon switch animation is not already playing
            if (!WeaponManagerAnimation.IsPlaying("WeaponSwitch"))
            {
                WeaponManagerAnimation.Play("WeaponSwitch"); // Play the weapon switch animation
            }
        }

        public void ShootNormalRaycast()
        {
            // Perform a raycast and check if it hits anything
            if (Physics.Raycast(Ray, out Hit, HitRange, ~IgnoreMe))
            {
                // Check if the hit object is untagged
                if (Hit.transform.gameObject.tag == "Untagged")
                {
                    Instantiate(HitExplosionUntagged, Hit.point, Hit.transform.rotation); // Instantiate explosion effect
                }

                // Check if the hit object is an enemy
                if (Hit.transform.gameObject.tag == "Enemy")
                {
                    if (Hit.transform.gameObject.GetComponent<EnemyHealthManager>())
                    {
                        if (Hit.transform.gameObject.GetComponent<EnemyAnimationManager>())
                        {
                            Hit.transform.gameObject.GetComponent<EnemyAnimationManager>().PlayFlinchAnimation(); // Play flinch animation
                        }

                        // Check if the enemy can take damage
                        if (!Hit.transform.gameObject.GetComponent<EnemyHealthManager>().CanTakeDamage)
                        {
                            print("Hit enemy is out of light and cant take damage");
                            DamageImage.GetComponent<Image>().sprite = NoDamageShotSprite; // Set no damage sprite
                            ResetDamageImage = true;
                        }
                        else if (Hit.transform.gameObject.GetComponent<EnemyHealthManager>().CanTakeDamage)
                        {
                            // Check for limb, body, or head shot and apply damage accordingly
                            if (Hit.collider.transform.GetComponent<HitDamageManager>().IsLimb)
                            {
                                Hit.transform.gameObject.GetComponentInParent<EnemyHealthManager>().ApplyDamage(Damage / 2); // Apply limb shot damage
                                print("LimbShot");
                                DamageImage.GetComponent<Image>().sprite = LimbShotSprite;
                                ResetDamageImage = true;
                            }

                            if (Hit.collider.transform.GetComponent<HitDamageManager>().IsBody)
                            {
                                Hit.transform.gameObject.GetComponentInParent<EnemyHealthManager>().ApplyDamage(Damage); // Apply body shot damage
                                print("BodyShot");
                                DamageImage.GetComponent<Image>().sprite = BodyShotSprite;
                                ResetDamageImage = true;
                            }

                            if (Hit.collider.transform.GetComponent<HitDamageManager>().IsHead)
                            {
                                Hit.transform.gameObject.GetComponentInParent<EnemyHealthManager>().ApplyDamage(Random.Range(Damage * 2, Damage * 10)); // Apply head shot damage
                                print("HeadShot");
                                DamageImage.GetComponent<Image>().sprite = HeadShotSprite;
                                ResetDamageImage = true;
                            }
                        }

                        Instantiate(HitExplosionEnemy, Hit.point, Hit.transform.rotation); // Instantiate enemy hit explosion effect

                        // Make enemy chase the player
                        if (Hit.transform.gameObject.GetComponent<EnemyAIManager>())
                        {
                            Hit.transform.gameObject.GetComponent<EnemyAIManager>().sightRange = Mathf.Infinity;
                            Hit.transform.gameObject.GetComponent<EnemyAIManager>().playerInSightRange = true;
                            Hit.transform.gameObject.GetComponent<EnemyAIManager>().ChasePlayer();
                        }
                    }
                    else
                    {
                        DamageImage.GetComponent<Image>().sprite = NoDamageShotSprite; // Set no damage sprite for non-enemy objects
                        ResetDamageImage = true;
                        ResetShootingHelperText = true;
                    }
                }

                // Check if the hit object is an object (e.g., environmental object)
                if (Hit.transform.gameObject.tag == "Object")
                {
                    if (Hit.transform.gameObject.gameObject.GetComponent<ObjectHealthManager>())
                    {
                        Hit.transform.gameObject.GetComponent<ObjectHealthManager>().ApplyDamage(Damage); // Apply damage to the object
                        Instantiate(HitExplosionUntagged, Hit.point, Hit.transform.rotation); // Instantiate explosion effect
                    }
                }

                // Check if the hit object is a trigger (e.g., door switch)
                if (Hit.transform.gameObject.tag == "Trigger")
                {
                    if (Hit.transform.gameObject.GetComponent<TriggerBehaviour>().IsDoorSwitch)
                    {
                        Hit.transform.gameObject.GetComponent<TriggerBehaviour>().SwitchDoor(); // Switch the door state
                        Instantiate(HitExplosionUntagged, Hit.point, Hit.transform.rotation); // Instantiate explosion effect
                    }
                }
            }
        }

    }

}