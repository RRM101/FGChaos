using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FGChaos.Effects
{
    public class FallGuyRain : Effect
    {
        public FallGuyRain()
        {
            Name = "Fall Guy Rain";
            Duration = 30;
            BlockedEffects = new Type[] { typeof(FirstPersonMode) };
        }

        float delay = 0.2f;

        public override void Update()
        {
            if (delay > 0)
            {
                delay -= Time.deltaTime / Time.timeScale;
            }
            else
            {
                delay = 0.2f;

                Quaternion rotation = Quaternion.Euler(0, chaos.fallGuy.transform.eulerAngles.y, chaos.fallGuy.transform.eulerAngles.z);
                Vector3 position = chaos.fallGuy.transform.position + rotation * new Vector3(0, 0, 25);
                Vector3 randompoint = position + (Vector3)(20 * UnityEngine.Random.insideUnitCircle);
                randompoint.y += 40;

                SpawnFallGuy(randompoint);
            }
        }

        void SpawnFallGuy(Vector3 position)
        {
            GameObject gameObject = new GameObject("FallGuyRainDrop");
            BoxCollider collider = gameObject.AddComponent<BoxCollider>();
            collider.material.bounciness = 0.5f;
            collider.material.bouncyness = 0.5f;
            collider.material.bounceCombine = PhysicMaterialCombine.Maximum;
            Rigidbody rigidbody = gameObject.AddComponent<Rigidbody>();
            rigidbody.mass = 10;
            rigidbody.drag = 1;
            gameObject.transform.position = position;
            GameObject model = GameObject.Instantiate(chaos.fallGuy.transform.GetChild(0).gameObject, gameObject.transform);
            model.transform.localPosition = Vector3.zero;
        }
    }
}
