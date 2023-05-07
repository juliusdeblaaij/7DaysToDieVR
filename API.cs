using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static XUiC_ArchetypesWindowGroup;
using HarmonyLib;
using UnityEngine;
using System.Reflection;
using InControl;
using Gizmos = Popcron.Gizmos;
using System.Linq;

namespace _7DaysToDieVR
{
    public class API : IModApi
    {
        public static float VRHeadsetFOV = 90f;

        public void InitMod(Mod mod)
        {
            Log.Out(" Loading Patch: " + GetType().ToString());
            new Harmony(GetType().ToString()).PatchAll(Assembly.GetExecutingAssembly());
        }
    }

    /* FORMAT OF 7D2D arms
    
        prefabEntityPlayerLocal (Clone)
        ↳ Camera
            ↳ femaleArms_fp
                ↳ Origin
                    ↳ RightShoulder
                        ↳ RightArm
                            ↳ Right ForeArm
                                ↳ RightForeArm Roll1
                                    ↳ RightForeArm Roll2
                                        ↳ RightForeArm Roll3
                                            ↳ RightForeArm Roll4
                                                ↳ RightHand
                                                    ↳ RightHandThumb1
                                                        ↳ RightHandThumb2
                                                            ↳ RightHandThumb3
                                                                ↳ RightHandThumb4
         */

    [HarmonyPatch(typeof(EntityPlayerLocal))]
    [HarmonyPatch("Init")]
    public class EntityPlayerLocal_Init
    {
        static public vp_FPPlayerEventHandler eventHandler = new vp_FPPlayerEventHandler(); 

        static void Postfix(EntityPlayerLocal __instance, int _entityClass)
        {
            if (VorpX.VPX_RESULT.VPX_RES_OK != VorpX.vpxInit())
            {
                Log.Out($"VorpX fatal error when trying to initialize");
                return;
            }

            var instanceCamera = __instance.cameraTransform.GetComponent<Camera>();

            if (instanceCamera)
            {
                API.VRHeadsetFOV = VorpX.vpxGetHeadsetFOV();
            }

            instanceCamera.fieldOfView = API.VRHeadsetFOV;

            // Create an instance of the custom InputDevice.
            var leftController = new VRControllerDevice(0);
            var rightController = new VRControllerDevice(1);

            // Attach the custom InputDevice to the InputManager.
            InputManager.AttachDevice(leftController);
            InputManager.AttachDevice(rightController);

            VorpX.vpxForceControllerRendering(VorpX.VPX_BOOL.VPX_TRUE);

        }

        static void PrintTransformHierarchy(Transform transform, string prefix = "")
        {
            Log.Out(prefix + transform.name);

            foreach (Transform child in transform)
            {
                PrintTransformHierarchy(child, prefix + transform.name + "/");
            }
        }
    }

    [HarmonyPatch(typeof(EntityPlayerLocal))]
    [HarmonyPatch("LateUpdate")]
    public class EntityPlayerLocal_LateUpdate
    {

        static bool printedOnce = false;

        static void Postfix(EntityPlayerLocal __instance)
        {
            Transform cameraTransform = __instance.cameraTransform;
            var instanceCamera = cameraTransform.GetComponent<Camera>();

            if (instanceCamera)
            {
                API.VRHeadsetFOV = VorpX.vpxGetHeadsetFOV();
            }
            instanceCamera.fieldOfView = API.VRHeadsetFOV;

            var headsetRotation4f = VorpX.vpxGetHeadsetRotationQuaternion();
            var headsetPosition3f = VorpX.vpxGetHeadsetPosition();

            Quaternion headsetRotation = new Quaternion(-headsetRotation4f.x, -headsetRotation4f.y, headsetRotation4f.z, headsetRotation4f.w);
            Vector3 headsetPosition = new Vector3(headsetPosition3f.x, headsetPosition3f.y, headsetPosition3f.z);

            __instance.playerCamera.transform.rotation = headsetRotation;
            __instance.playerCamera.transform.position = __instance.cameraTransform.position + headsetPosition;

            var rightControllerPositionWorld = __instance.playerCamera.transform.position + VorpX.GetControllerPosition(1);


            //__instance.vp_FPWeapon.WeaponModel.transform.SetPositionAndRotation(rightControllerPositionWorld, VorpX.GetControllerRotationQuaternion(1));
        }
    }
}