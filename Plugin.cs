using BepInEx;
using System;
using UnityEngine;
using GorillaNetworking;
using CppPlayerVolume.Tools;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using HarmonyLib;
using BepInEx.Configuration;
using UnityEngine.UIElements;
using GorillaExtensions;
using UnityEngine.Pool;

namespace CppPlayerVolume
{

    [BepInPlugin("com.kylethescientist.gorillatag.cppplayervolume", "CppPlayerVolume", "1.0.0")]
    [BepInDependency("com.kylethescientist.gorillatag.computerplusplus")]
    public class Plugin : BaseUnityPlugin
    {
        float pollInterval = 2f, lastPoll;
        public static List<RigWrapper> rigWrappers = new List<RigWrapper>();

        void Awake()
        {
            Logging.Init();
            MetadataSerializer.Deserialize();
        }

        void FixedUpdate()
        {
            if (Time.time - lastPoll > pollInterval)
            {
                //Wrap();
                lastPoll = Time.time;
            }
        }

        void Wrap()
        {
            foreach (var vrrig in GorillaParent.instance.vrrigs)
            {
                if(!vrrig.gameObject.GetComponent<RigWrapper>())
                    rigWrappers.Add(vrrig.gameObject.AddComponent<RigWrapper>());
            }
        }
    }
}
