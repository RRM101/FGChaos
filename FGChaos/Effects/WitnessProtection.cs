﻿using SRF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FGChaos.Effects
{
    public class WitnessProtection : Effect
    {
        public override string Name
        {
            get { return "Witness Protection"; }
            set { }
        }

        public override int Duration
        {
            get { return 30; }
            set { }
        }

        float rotation;
        Transform witnessProtection;

        public override void Run()
        {
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
            base.Run();
        }

        public override void Update()
        {
            float wait = 0;
            if (wait < 30)
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
            GameObject.Destroy(witnessProtection.gameObject);
            base.End();
        }
    }
}
