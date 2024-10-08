﻿using FGClient.Rendering.XRay;
using SRF;
using UnityEngine;

namespace FGChaos.Effects
{
    public class WitnessProtection : Effect
    {
        public WitnessProtection()
        {
            Name = "Witness Protection";
            Duration = 30;
        }

        float rotation;
        Transform witnessProtection;
        float wait = 0;

        public override void Run()
        {
            XRayUtils.EnableXRayMeshRenderer(chaos.fallGuy, false);

            witnessProtection = chaos.fallGuy.transform.CreateChild("WitnessProtection").transform;

            rotation = 0;
            witnessProtection.transform.localPosition = Vector3.zero;
            witnessProtection.transform.eulerAngles = new Vector3(0, rotation, 0);
            Transform characterTransform = chaos.fallGuy.transform.GetChild(0);

            for (int i = 0; i < 18; i++)
            {
                Transform witness = witnessProtection.CreateChild("Witness").transform;
                witness.transform.localPosition = Vector3.zero;
                witness.transform.eulerAngles = new Vector3(0, i * 20, 0);
                GameObject witnessGameObject = GameObject.Instantiate(characterTransform.gameObject, witness);
                witnessGameObject.SetActive(true);
                witnessGameObject.transform.localPosition = new Vector3(0, 0, 5);
                witnessGameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
            }
            RunWithoutEnd();
        }

        public override void Update()
        {
            if (wait < Duration)
            {
                rotation += 200 * Time.deltaTime;
                witnessProtection.transform.eulerAngles = new Vector3(0, rotation, 0);
                wait += Time.deltaTime;
            }
            else
            {
                End();
            }
        }

        public override void End()
        {
            if (witnessProtection != null)
            {
                GameObject.Destroy(witnessProtection.gameObject);
            }

            if (chaos != null)
            {
                XRayUtils.EnableXRayMeshRenderer(chaos.fallGuy, true);
            }
            base.End();
        }
    }
}
