using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FGChaos.MonoBehaviours
{
    public class BouncyPlayer : MonoBehaviour
    {
        Rigidbody rb;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        void OnCollisionEnter(Collision collision)
        {
            Vector3 normal = collision.contacts[0].normal;
            Vector3 incomingVelocity = rb.velocity;
            Vector3 reflectedVelocity = Vector3.Reflect(incomingVelocity, normal);

            rb.velocity = reflectedVelocity * 10;
        }
    }
}
