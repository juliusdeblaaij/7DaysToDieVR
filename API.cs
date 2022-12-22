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

    [HarmonyPatch(typeof(EntityPlayerLocal))]
    [HarmonyPatch("Init")]
    public class EntityPlayerLocal_Init
    {
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
        }
    }

    [HarmonyPatch(typeof(EntityPlayerLocal))]
    [HarmonyPatch("LateUpdate")]
    public class EntityPlayerLocal_LateUpdate
    {
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


            var controllerState = VorpX.vpxGetControllerState(0);

            __instance.movementInput.rotation = new Vector3(headsetRotation.x, headsetRotation.y, headsetRotation.z);
            __instance.movementInput.moveForward = controllerState.StickY;
            __instance.movementInput.moveStrafe = controllerState.StickX;

            __instance.playerCamera.transform.position = __instance.cameraTransform.position + headsetPosition;
        }
    }
}