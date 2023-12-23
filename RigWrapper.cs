using CppPlayerVolume.Tools;
using HarmonyLib;
using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace CppPlayerVolume
{
    public class RigWrapper : MonoBehaviour
    {

        public PhotonView photonView;
        public VRRig rig;
        public AudioSource voiceAudio;
        public string userId;
        public int volume;
        Traverse rigTraverse;
        public bool invalid;

        float refreshInterval = 5f, lastRefresh;

        void Awake() => Refresh();

        void FixedUpdate()
        {
            if (Time.time - lastRefresh > refreshInterval)
            {
                Refresh();
                lastRefresh = Time.time;
            }
        }

        public void Refresh()
        {
            try
            {
                rig = GetComponent<VRRig>();
                rigTraverse = Traverse.Create(rig);
                photonView = rigTraverse.Field("photonView").GetValue<PhotonView>();
                voiceAudio = rigTraverse.Field("voiceAudio").GetValue<AudioSource>();
                userId = photonView.Owner.UserId;
                if (MetadataSerializer.volumes.ContainsKey(userId))
                    volume = MetadataSerializer.volumes[userId];
                else
                    volume = 9;
                voiceAudio.volume = volume / 9f;
                invalid = false;
            }catch (Exception e)
            {
                invalid = true;
                //Logging.Exception(e);
            }
        }
    }
}
