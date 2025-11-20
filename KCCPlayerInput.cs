using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


namespace Dz.KCC
{
    public class KCCPlayerInput : MonoBehaviour
    {


        private KCCCharacterController _kccCharCtrl;

        public PlayerCamera camMgr;






        #region Variables  


        [Header("Inputs")]

        public bool unlockCursorOnStart = false;
        public bool showCursorOnStart = false;

        [SerializeField]
        private Inputs inputs;

        [System.Serializable]
        public class Inputs
        {
            public Vector2 _moveDirection;
            public Vector2 _lookDirection;


            public InputKeys _walk;
            public InputKeys _sprint;
            public InputKeys _jump;
            public InputKeys _crouch;
            public InputKeys _prone;
            public InputKeys _interact;
            public InputKeys _ladder;

            public InputKeys _slide;


        }






        //public KeyCode toggleWalk = KeyCode.CapsLock;

        [Header("Movement Input")]

        public PlayerInput playerInput;

        //public Dz_PlayerInput inputActions;



        //public InputActionAsset inputActionAsset;

        public InputActionReferences inputActionReferences;

        [System.Serializable]

        public class InputActionReferences
        {
            [Header("Move")]
            /// <summary>
            /// Action reference for moving the player.
            /// </summary>
            [Tooltip("Action reference for moving the player")]
            public InputActionReference moveActionReference;

            public InputActionReference lookActionReference;


            [Header("Actions")]
            public InputActionReference jumpActionReference;
            public InputActionReference toggleWalkActionReference;
            public InputActionReference sprintActionReference;
            public InputActionReference crouchActionReference;
            public InputActionReference proneActionReference;


            public InputActionReference rollActionReference;
            public InputActionReference slideActionReference;
            public InputActionReference diveActionReference;


            [Header("Interaction")]
            public InputActionReference interactActionReference;
            public InputActionReference dropItemActionReference;

            public InputActionReference danceActionReference;
            [Header("Camera")]
            public InputActionReference toggleViewActionReference;
            public InputActionReference switchCameraSideActionReference;
            //public InputActionReference switchViewActionReference;
        }

        public List<InputAction> _activeInputActions = new List<InputAction>();



        private InputAction move;
        private InputAction look;
        private InputAction sprint;
        private InputAction crouch;

        private InputAction prone;

        private InputAction jump;
        private InputAction roll;
        private InputAction slide;

        private InputAction dive;


        private InputAction dance;



        private InputAction toggleWalk;
        private InputAction toggleStrafe;

        private InputAction interact;
        private InputAction dropItem;




        private InputAction toggleTpsFpsCamera;

        private InputAction switchCameraSide;
        private InputAction switchAimSide;

        [HideInInspector] public bool lockInput;


        [SerializeField]
        private CurrentInput m_curInput = CurrentInput.General;

        [SerializeField]
        private bool _offhandSelected;

        public bool OffhandSelection { get { return _offhandSelected; } set { _offhandSelected = value; } }


        private enum CurrentInput
        {
            General,
            Strafe,
            Climb
        }



        #endregion


        #region Events

    public delegate void OnUpdateEvent();
    public event OnUpdateEvent onUpdate;
    public event OnUpdateEvent onLateUpdate;
    public event OnUpdateEvent onFixedUpdate;
    public event OnUpdateEvent onAnimatorMove;

        #endregion

        bool isStrafe;

        public virtual bool IsStrafing { get { return isStrafe; } }

        /// <summary>
        /// Input movement from player input updated each frame.
        /// </summary>
        public Vector3 InputMovement { get; private set; }
        public Quaternion InputRotation { get; private set; }

        public bool InteractionPressed { get { return InteractionPressed; } set {; } }
        public bool InteractionReleased { get { return InteractionReleased; } set {; } }




        public virtual void Awake()
        {
            /*            if (!cRefs)
                            GetComponentReferences();*/

            _kccCharCtrl = GetComponent<KCCCharacterController>();

            //inputActionAsset = new();

            GetInput();


        }


/*        public virtual void GetComponentReferences()
        {
            var c = transform.root.GetComponent<ComponentReferences>();
            if (c != null)
                cRefs = c;
            else
                cRefs = transform.root.GetComponentInChildren<ComponentReferences>();
        }
*/

        public virtual void GetInput()
        {
            //move = inputActionReferences.moveActionReference;
            //look = inputActionReferences.lookActionReference;

/*            move = inputActionReferences.moveActionReference != null ? inputActionReferences.moveActionReference : null;
            look = inputActionReferences.lookActionReference != null ? inputActionReferences.lookActionReference : null;

            jump = inputActionReferences.jumpActionReference != null ? inputActionReferences.jumpActionReference : null;*/

            if (inputActionReferences.moveActionReference) { move = inputActionReferences.moveActionReference; }
            if (inputActionReferences.lookActionReference) { look = inputActionReferences.lookActionReference; }


            if (inputActionReferences.jumpActionReference) { jump = inputActionReferences.jumpActionReference; jump.performed += Jump; jump.canceled += Jump; }

            if (inputActionReferences.sprintActionReference) { sprint = inputActionReferences.sprintActionReference; sprint.performed += Sprint; sprint.canceled += Sprint; }

            if (inputActionReferences.crouchActionReference) { crouch = inputActionReferences.crouchActionReference; crouch.performed += Crouch; crouch.canceled += Crouch; }

            if (inputActionReferences.proneActionReference) { prone = inputActionReferences.proneActionReference; prone.performed += Prone; prone.canceled += Prone; }

            if (inputActionReferences.rollActionReference) { roll = inputActionReferences.rollActionReference; roll.performed += Roll; roll.canceled += Roll; }



            if (inputActionReferences.slideActionReference) { slide = inputActionReferences.slideActionReference; slide.performed += Slide; slide.canceled += Slide; }

            if (inputActionReferences.diveActionReference) { dive = inputActionReferences.diveActionReference; slide.performed += Dive; dive.canceled += Dive; }

            if (inputActionReferences.toggleWalkActionReference) { toggleWalk = inputActionReferences.toggleWalkActionReference; toggleWalk.performed += ToggleWalk; toggleWalk.canceled += ToggleWalk; }

            if (inputActionReferences.danceActionReference) { dance = inputActionReferences.danceActionReference; dance.performed += Dance; dance.canceled += Dance; }

            if (inputActionReferences.interactActionReference) { interact = inputActionReferences.interactActionReference; interact.performed += Interact; interact.canceled += Interact; }

            if (inputActionReferences.dropItemActionReference) { dropItem = inputActionReferences.dropItemActionReference; dropItem.performed += DropItem; dropItem.canceled += DropItem; }





            //cover = inputActions.Interaction.Cover;

            /*            cover.performed += Cover;
                        cover.canceled -= Cover;


                        attack = inputActions.Combat.Attack;
                        attack.performed += PrimaryAttackPressed;
                        attack.canceled += PrimaryAttackReleased;*/



            //aim = inputActions.Combat.Aiming;

            /*aim.performed += SecondaryAttackPressed;
            aim.canceled += SecondaryAttackReleased;

            reloadSelector = inputActions.Firearms.ReloadSelector;
            reloadSelector.performed += ReloadSelectorPressed;
            reloadSelector.canceled += ReloadSelectorReleased;

            primaryReload = inputActions.Firearms.PrimaryReload;
            primaryReload.performed += PrimaryReloadPressed;
            primaryReload.canceled += PrimaryReloadReleased;

            altReload = inputActions.Firearms.AltReload;
            altReload.performed += AltReloadPressed;
            altReload.canceled += AltReloadReleased;

            primaryReloadOptions = inputActions.Firearms.PrimaryReloadOptions;
            primaryReloadOptions.performed += PrimaryReloadOptionsPressed;
            primaryReloadOptions.canceled += PrimaryReloadOptionsReleased;

            altReloadOptions = inputActions.Firearms.AltReloadOptions;
            altReloadOptions.performed += AltReloadOptionsPressed;
            altReloadOptions.canceled += AltReloadOptionsReleased;


            holsterEquipped = inputActions.Firearms.HolsterEquipped;
            holsterEquipped.performed += HolsterEquipped;
            //holsterEquipped.canceled += HolsterRightHip;


            equipLastHolstered = inputActions.Firearms.EquipLastHolstered;
            equipLastHolstered.performed += EquipLastHolstered;
            //equipLastHolstered.canceled += HolsterRightHip;


            offhandSelection = inputActions.Firearms.SelectOffHandWeapons;
            offhandSelection.performed += OffhandSelected;
            offhandSelection.canceled += OffhandSelected;


            primaryWeapon = inputActions.Firearms.SelectPrimaryWeapon;
            primaryWeapon.performed += PrimaryWeaponSelected;
            //holsterRightHip.canceled += HolsterRightHip;

            secondaryWeapon = inputActions.Firearms.SelectSecondaryWeapon;
            secondaryWeapon.performed += SecondaryWeaponSelected;
            //holsterLeftHip.canceled += HolsterLeftHip;

            tertiaryWeapon = inputActions.Firearms.SelectTertiaryWeapon;
            tertiaryWeapon.performed += TertiaryWeaponSelected;
            //holsterRightShoulder.canceled += HolsterRightShoulder;

            quaternaryWeapon = inputActions.Firearms.SelectQuaternaryWeapon;
            quaternaryWeapon.performed += QuaternaryWeaponSelected;
            //holsterLeftShoulder.canceled += HolsterLeftShoulder;




            checkAmmo = inputActions.Firearms.CheckAmmo;
            checkAmmo.performed += CheckAmmo;
            checkAmmo.canceled += CheckAmmo;

            firemodeCycle = inputActions.Firearms.FiremodeCycle;
            //firemodeCycle.performed += FiremodeCycle;
            firemodeCycle.canceled += FiremodeCycle;

            firemodeSelection = inputActions.Firearms.FiremodeSelection;
            firemodeSelection.performed += FiremodeSelection;
            firemodeSelection.canceled += FiremodeSelection;


            mechanismCycle = inputActions.Firearms.MechanismCycle;
            //firemodeCycle.performed += FiremodeCycle;
            mechanismCycle.canceled += MechanismCycle;

            stockToggle = inputActions.Firearms.StockToggle;
            stockToggle.performed += StockToggle;*/

            //toggleTpsFpsCamera = inputActions.Camera.CameraToggle;



            if (inputActionReferences.toggleViewActionReference) { toggleTpsFpsCamera = inputActionReferences.toggleViewActionReference; toggleTpsFpsCamera.performed += ToggleFPSTPSView; toggleTpsFpsCamera.canceled += ToggleFPSTPSView; }


            if (inputActionReferences.switchCameraSideActionReference) { switchCameraSide = inputActionReferences.switchCameraSideActionReference; switchCameraSide.performed += SwitchCameraSide; switchCameraSide.canceled += SwitchCameraSide; }


            /*
                        toggleTpsFpsCamera = inputActionReferences.toggleViewActionReference;

                        toggleTpsFpsCamera.performed += ToggleFPSTPSView;*/


            //switchCameraSide = inputActions.Camera.CameraSwitchSide;




/*            switchCameraSide = inputActionReferences.switchCameraSideActionReference;


            switchCameraSide.performed += SwitchCameraSide;
*/



            //toggleCamera.canceled -= Attack;


        }

        public virtual void OnEnable()
        {
            EnableInputs();
            //SetCharacterReferences();

        }

        public virtual void EnableInputs()
        {

/*

            move.Enable();
            look.Enable();*/

            if (inputActionReferences.moveActionReference) { move.Enable(); }
            if (inputActionReferences.lookActionReference) { look.Enable(); }

            if (inputActionReferences.jumpActionReference) { jump.Enable(); }
            if (inputActionReferences.sprintActionReference) { sprint.Enable(); }
            if (inputActionReferences.crouchActionReference) { crouch.Enable(); }
            if (inputActionReferences.proneActionReference) { prone.Enable(); }

            if (inputActionReferences.diveActionReference) { dive.Enable(); }
            if (inputActionReferences.slideActionReference) { slide.Enable(); }

            if (inputActionReferences.toggleWalkActionReference) { toggleWalk.Enable(); }

            if (inputActionReferences.interactActionReference) { interact.Enable(); }

            if (inputActionReferences.dropItemActionReference) { dropItem.Enable(); }

            if (inputActionReferences.toggleViewActionReference) { toggleTpsFpsCamera.Enable(); }
            if (inputActionReferences.switchCameraSideActionReference) { switchCameraSide.Enable(); }


            //switchAimSide.Enable();



            if (inputActionReferences.danceActionReference) { dance.Enable(); }


            //EnableCombatInputs();
            //EnableFirearmInputs();



        }

        public virtual void OnDisable()
        {
            //move.Disable();
            //look.Disable();

            if (inputActionReferences.moveActionReference) { move.Disable(); }
            if (inputActionReferences.lookActionReference) { look.Disable(); }

            if (inputActionReferences.jumpActionReference) { jump.Disable(); }
            if (inputActionReferences.sprintActionReference) { sprint.Disable(); }
            if (inputActionReferences.crouchActionReference) { crouch.Disable(); }
            if (inputActionReferences.proneActionReference) { prone.Disable(); }

            if (inputActionReferences.diveActionReference) { dive.Disable(); }
            if (inputActionReferences.slideActionReference) { slide.Disable(); }

            if (inputActionReferences.toggleWalkActionReference) { toggleWalk.Disable(); }

            if (inputActionReferences.interactActionReference) { interact.Disable(); }

            if (inputActionReferences.dropItemActionReference) { dropItem.Disable(); }

            if (inputActionReferences.toggleViewActionReference) { toggleTpsFpsCamera.Disable(); }
            if (inputActionReferences.switchCameraSideActionReference) { switchCameraSide.Disable(); }



        }




        // Update is called once per frame
        void Update()
        {
            switch (m_curInput)
            {
                case CurrentInput.General:
                    { UpdateGeneralInput(); }
                    break;
                case CurrentInput.Strafe:
                    { UpdateStrafeInput(); }
                    break;
                case CurrentInput.Climb:
                    { UpdateClimbInput(); }
                    break;
            }
            if (onFixedUpdate != null)
            {
                onFixedUpdate.Invoke();
            }

            HandleCharacterInput();
        }



        private void LateUpdate()
        {
            //cRefs.camManager.HandleCamera();

            //HandleCameraInput();
        }

        #region General Input

        private void Crouch(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                if (inputs._crouch._inputType == InputKeys.Type.Hold)
                {
                    _kccCharCtrl.StartCrouch();
                }
                else
                {
                    _kccCharCtrl.ToggleCrouch();
                }



                //StartCoroutine(CrouchKeyPressed());

                StartCoroutine(OnKeyPressed(inputs._crouch));
            }
            if (context.canceled)
            {
                if (inputs._crouch._inputType == InputKeys.Type.Hold)
                {
                    _kccCharCtrl.StopCrouch();
                }

                //StartCoroutine(OnKeyReleased(inputs._crouch));

                StartCoroutine(CrouchKeyReleased());

/*                if (!inputs._prone._held)
                {
                    cRefs._kccCharCtrl.ToggleCrouch();

                }*/
                //Debug.Log("Crouch Button Pressed");
            }
        }


        public IEnumerator CrouchKeyPressed()
        {
            inputs._crouch._pressed = true;
            inputs._crouch._held = true;

            yield return new WaitForEndOfFrame();
            inputs._crouch._pressed = false;
            Debug.Log("Crouch Key Pressed"); // this log fires exactly once
        }
        public IEnumerator CrouchKeyReleased()
        {
            inputs._crouch._released = true;

            yield return new WaitForEndOfFrame();
            inputs._crouch._held = false;
            inputs._crouch._released = false;

            Debug.Log("Crouch Key Released"); // this log fires exactly once
        }


        private void Prone(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                //inputs._prone._held = true;

                //StartCoroutine(OnKeyUp(inputs._prone._pressed));
                if (inputs._prone._inputType == InputKeys.Type.Hold)
                {
                    _kccCharCtrl.StartProne();
                }
                else
                {
                    _kccCharCtrl.ToggleProne();
                }

            }
            if (context.canceled)
            {
                if (inputs._prone._inputType == InputKeys.Type.Hold)
                {
                    _kccCharCtrl.StopProne();
                }
                //inputs._prone._held = false;

                //StartCoroutine(OnKeyUp(inputs._prone._released));

            }
        }
        private void Jump(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                //JumpPressed();
                inputs._jump._held = true;
                StartCoroutine(JumpKeyPressed());
                //Debug.Log("Character Jumped");
            }
            if (context.canceled)
            {
                //JumpReleased();
                inputs._jump._held = false;

                StartCoroutine(JumpKeyReleased());
                //Debug.Log("Character Stopped Sprinting");
            }

        }

        public IEnumerator JumpKeyPressed()
        {
            inputs._jump._pressed = true;
            //Debug.Log("Jump Key Pressed"); // this log fires exactly once
            yield return new WaitForEndOfFrame();
            inputs._jump._pressed = false;

        }
        public IEnumerator JumpKeyReleased()
        {
            inputs._jump._released = true;
            //Debug.Log("Jump Key Released"); // this log fires exactly once
            yield return new WaitForEndOfFrame();
            inputs._jump._released = false;
        }


        private void Sprint(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                inputs._sprint._held = true;

                StartCoroutine(SprintKeyPressed());
                if (inputs._sprint._inputType == InputKeys.Type.Hold)
                {
                    _kccCharCtrl.StartSprint();
                }
                else
                {
                    _kccCharCtrl.ToggleSprint();
                }



                //SprintBegin();
                //Debug.Log("Character Started Sprinting");
            }


            if (context.canceled)
            {
                inputs._sprint._held = false;
                if (inputs._sprint._inputType == InputKeys.Type.Hold)
                {
                    _kccCharCtrl.StopSprint();
                }

                    StartCoroutine(SprintKeyReleased());


                //SprintEnd();
                //Debug.Log("Character Stopped Sprinting");
            }
        }

        public IEnumerator SprintKeyPressed()
        {
            inputs._sprint._pressed = true;
            //Debug.Log("Sprint Key Pressed"); // this log fires exactly once
            yield return new WaitForEndOfFrame();
            inputs._sprint._pressed = false;

        }
        public IEnumerator SprintKeyReleased()
        {
            inputs._sprint._released = true;
            //Debug.Log("Sprint Key Released"); // this log fires exactly once
            yield return new WaitForEndOfFrame();
            inputs._sprint._released = false;
        }



        private void Roll(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                RollBegin();

            }
            //Debug.Log("Character Rolled");
        }
        private void Slide(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _kccCharCtrl.StartSlide();
                //Debug.Log("Character Toggled Walk");
                //StartCoroutine(SprintKeyPressed());
            }
            if (context.canceled)
            {

                if (inputs._slide._inputType == InputKeys.Type.Hold)
                {
                    _kccCharCtrl.FinishSlide();
                }

                //StartCoroutine(SlideKeyReleased());


                //SprintEnd();
                //Debug.Log("Character Stopped Sprinting");
            }
            Debug.Log("Character Sliding");
        }

        private void Dive(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                ShootDiveBegin();
                //Debug.Log("Character Toggled Walk");
            }
            Debug.Log("Character Diving");
        }

        private void ToggleWalk(InputAction.CallbackContext context)
        {
            if (context.performed)
            {

                _kccCharCtrl.ToggleWalk();

                inputs._walk._held = true;
                StartCoroutine(WalkKeyPressed());
                //Debug.Log("Character Toggled Walk");
            }
            if (context.canceled)
            {
                //InteractionReleased();
                inputs._walk._held = false;

                if (inputs._walk._inputType == InputKeys.Type.Hold)
                {
                    _kccCharCtrl.ToggleWalk();
                }

                //Debug.Log("Character Toggled Strafe");

                StartCoroutine(WalkKeyReleased());
            }

        }

        public IEnumerator WalkKeyPressed()
        {
            inputs._walk._pressed = true;
            Debug.Log("Walk Key Pressed"); // this log fires exactly once
            yield return new WaitForEndOfFrame();
            inputs._walk._pressed = false;

        }
        public IEnumerator WalkKeyReleased()
        {
            inputs._walk._released = true;
            Debug.Log("Walk Key Released"); // this log fires exactly once
            yield return new WaitForEndOfFrame();
            inputs._walk._released = false;
        }

        private void ToggleStrafe(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                ToggleStrafe();
                //Debug.Log("Character Toggled Strafe");
            }


        }
        private void Interact(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                inputs._interact._held = true;

                StartCoroutine(InteractKeyPressed());



                _kccCharCtrl.Interact();

                //StartCoroutine(OnKeyPressed(inputs._interact));
            }
            if (context.canceled)
            {
                inputs._interact._held = false;


                //StartCoroutine(OnKeyReleased(inputs._interact));

                StartCoroutine(InteractKeyReleased());


                //Debug.Log("Crouch Button Pressed");
            }
            

        }

        public IEnumerator InteractKeyPressed()
        {
            inputs._interact._pressed = true;
            //inputs._interact._held = true;

            yield return new WaitForEndOfFrame();
            inputs._interact._pressed = false;
            Debug.Log("Interact Key Pressed"); // this log fires exactly once
        }
        public IEnumerator InteractKeyReleased()
        {
            inputs._interact._released = true;
            //inputs._interact._held = false;
            yield return new WaitForEndOfFrame();

            inputs._interact._released = false;

            Debug.Log("Interact Key Released"); // this log fires exactly once
        }


        private void DropItem(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                if (!OffhandSelection)
                {
                    //_kccCharCtrl.DropItem(ECharAttachments.Hand_Equip_R);
                }
                else
                {
                    //_kccCharCtrl.DropItem(ECharAttachments.Hand_Equip_L);
                }
            }

            Debug.Log("Pressed Drop Item Key");
        }



        private void ToggleFPSTPSView(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                camMgr.ToggleCharacterView();
                //ToggleView();
                //cRefs.decoupledTpc.ToggleCharacterView();

            }

        }

/*        public virtual void ToggleView()
        {
            camMgr.ToggleCharacterView();
        }*/

        private void SwitchCameraSide(InputAction.CallbackContext context)
        {
            if (context.performed)
            {

                //Debug.Log("Character Toggled Walk");
            }
            //Debug.Log("Character Attacking");
        }

        private void SwitchAimSide(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                //cRefs.decoupledTpc.AimingSwitchSide();
                //Debug.Log("Character Toggled Walk");
            }
            //Debug.Log("Character Attacking");
        }
        private void UpdateGeneralInput()
        {
            //bool denyMovement = PlayerInputUtils.playerMovementState == PlayerInputState.Deny;
            //Vector2 _moveDirection = denyMovement ? Vector2.zero : move?.ReadValue<Vector2>() ?? Vector2.zero;

            //Vector2 _lookDirection = denyMovement ? Vector2.zero : look?.ReadValue<Vector2>() ?? Vector2.zero;

            inputs._moveDirection = move.ReadValue<Vector2>();
            inputs._lookDirection = look.ReadValue<Vector2>();

            InputMovement = new Vector3(inputs._moveDirection.x, 0, inputs._moveDirection.y);

            InputRotation = new Quaternion(inputs._lookDirection.x, inputs._lookDirection.y, 0, 0);



            Vector3 lookInputVector = new Vector3(inputs._lookDirection.x, inputs._lookDirection.y, 0f);

            //_moveDirection = move.ReadValue<Vector2>();
            //_lookDirection = look.ReadValue<Vector2>();

            /*            cRefs._kccCharCtrl.MoveInputVector = _moveDirection;
                        cRefs._kccCharCtrl.LookInputVector = _lookDirection;*/

            /*            if (sprintInput.GetButton())
                        {
                            SprintBegin();
                        }
                        else if (sprintInput.GetButtonUp())
                        {
                            SprintEnd();
                        }

                        if (strafeInput.GetButton())
                        {
                            if (!isStrafe)
                            {
                                StrafeBegin();
                            }
                            else
                            {
                                StrafeEnd();
                            }
                        }

                        if (slideInput.GetButton())
                        {
                            SlideBegin();
                        }
                        else if (jumpInput.GetButton())
                        {
                            JumpBegin();
                        }*/
        }



        private void SprintBegin()
        {
            //_kccCharCtrl.StartSprint();
            //cRefs.mxm._trajectoryGenerator.InputProfile = sprintLocomotion;
        }
        private void SprintEnd()
        {
            //_kccCharCtrl.StopSprint();
            //mxm_TrajectoryGenerator.InputProfile = generalLocomotion;
        }

        private void StrafeBegin()
        {
            //charController.StartStrafe();
            //mxm_TrajectoryGenerator.InputProfile = strafeLocomotion;
            isStrafe = true;
            m_curInput = CurrentInput.Strafe;
        }
        private void StrafeEnd()
        {
            //charController.StopStrafe();
            //mxm_TrajectoryGenerator.InputProfile = generalLocomotion;
            isStrafe = false;
            m_curInput = CurrentInput.General;
        }
        private void SlideBegin()
        {

            //charController.StartSlide();
        }
        private void SlideEnd()
        {
            _kccCharCtrl.FinishSlide();
            //charController.StartSlide();
        }

        private void RollBegin()
        {
            _kccCharCtrl.StartRoll();
            //charController.StartRoll();
        }
        private void RollEnd()
        {

        }

        private void JumpPressed()
        {
            inputs._jump._held = true;

            //charController.StartJump();
        }

        private void JumpReleased()
        {
            inputs._jump._held = false;

        }


        private void ShootDiveBegin()
        {

            //charController.StartDive();
        }
        private void ShootDiveEnd()
        {

        }


        private void ToggleWalk()
        {

        }

        #endregion

        #region Strafe Input


        void UpdateStrafeInput()
        {

        }


        private void ToggleStrafe()
        {
            //charController.ToggleStrafe();
        }

        #endregion

        #region Climb Input

        private void UpdateClimbInput()
        {

        }

        #endregion

        #region Interact
        private void StartInteraction()
        {

            //charController.Interacting = true;
        }
        private void EndInteraction()
        {
            //charController.Interacting = false;

        }



        #endregion


        #region Emotes

        private void Dance(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _kccCharCtrl.ToggleDance();
                //cRefs.decoupledTpc.ToggleDance();
            }


        }

        #endregion

        #region Devices

/*        public bool IsCurrentDeviceMouse
        {
            get
            {
                //#if ENABLE_INPUT_SYSTEM
                return playerInput.currentControlScheme == "KeyboardMouse";
                //#else
                return false;
                //#endif
            }
        }*/

        #endregion

        private void HandleCharacterInput()
        {
            PlayerInputs characterInputs = new PlayerInputs();

            camMgr._lookDeltaRef.x = inputs._lookDirection.x;
            camMgr._lookDeltaRef.y = inputs._lookDirection.y;

            //cRefs.mxMAnimatorExtension.InputVector = InputMovement;


            // Build the CharacterInputs struct
            characterInputs.MoveAxisVertical = InputMovement.z;
            //characterInputs.MoveAxisVertical = inputs._moveDirection.y;
            //characterInputs.MoveAxisVertical = moveY;

            characterInputs.MoveAxisHorizontal = InputMovement.x;
            //characterInputs.MoveAxisHorizontal = inputs._moveDirection.x;
            //characterInputs.MoveAxisHorizontal = moveX;

            //characterInputs.CameraRotation = new Quaternion(inputs._lookDirection.x, inputs._lookDirection.y, 0, 0);
            characterInputs.CameraRotation = camMgr.playerCamera.transform.rotation;

            characterInputs.Jump = inputs._jump._pressed;
            characterInputs.JumpHeld = inputs._jump._held;

            characterInputs.Interact = inputs._interact._released;

            characterInputs.Ladder = inputs._interact._released;
            //characterInputs.Ladder = Input.GetKeyUp(KeyCode.E);
            //characterInputs.Ladder = Input.GetKeyDown(KeyCode.E);
            //characterInputs.Walk = Input.GetKeyDown(KeyCode.Space);
            //characterInputs.Sprint = Input.GetKeyDown(KeyCode.Space);

            //characterInputs.Crouch = Input.GetKeyDown(KeyCode.C);
            //characterInputs.Prone = Input.GetKeyUp(KeyCode.C);

            //characterInputs.Crouch = inputs._crouch._inputType == InputKeys.Type.Toggle ? inputs._crouch._pressed : inputs._crouch._held;
            characterInputs.Crouch = inputs._crouch._held;
            //characterInputs.CrouchHeld = inputs._crouch._held;

            // Apply inputs to character
            _kccCharCtrl.SetInputs(ref characterInputs);
        }

        public IEnumerator OnKeyPressed(InputKeys key)
        {
            key._pressed = true;
            key._held = true;
            yield return new WaitForEndOfFrame();
            key._pressed = false;
            //Debug.Log("On Key Down" + key.ToString()); // this log fires exactly once
        }
        public IEnumerator OnKeyReleased(InputKeys key)
        {
            key._released = true;
            yield return new WaitForEndOfFrame();
            key._held = false;
            key._released = false;

            //Debug.Log("On Key Up" + key.ToString()); // this log fires exactly once
        }



    }
    public struct PlayerInputs
    {
        public float MoveAxisVertical;
        public float MoveAxisHorizontal;
        public Quaternion CameraRotation;
        public bool Walk;
        public bool Sprint;
        public bool Jump;
        public bool JumpHeld;
        public bool Crouch;
        public bool CrouchHeld;
        public bool Prone;
        public bool Interact;

        public bool Ladder;

    }

    [System.Serializable]
    public class InputKeys
    {
        public Type _inputType;
        public bool _pressed;
        public bool _held;
        public bool _released;

        public enum Type
        {
            Toggle,
            Hold
        }
    }

}
