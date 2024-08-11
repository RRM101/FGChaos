using FG.Common.Character;
using FG.Common.Character.MotorSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FGChaos.MonoBehaviours
{
    public class ReplayRecorder : MonoBehaviour
    {
        public bool recording = true;
        bool stopped;
        List<Vector3> positions = new();
        List<Quaternion> rotations = new();
        int replayFrameIndex;

        Rigidbody rb;
        FallGuysCharacterController fallGuysCharacter;
        MotorAgent motorAgent;
        MotorFunctionMovement movement;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
            fallGuysCharacter = GetComponent<FallGuysCharacterController>();
            motorAgent = GetComponent<MotorAgent>();
            movement = motorAgent.GetMotorFunction<MotorFunctionMovement>();
        }

        void FixedUpdate()
        {
            if (!stopped)
            {
                if (recording)
                {
                    positions.Add(transform.position);
                    rotations.Add(transform.rotation);
                }
                else
                {
                    rb.velocity = Vector3.zero;
                    transform.position = positions[replayFrameIndex];
                    transform.rotation = rotations[replayFrameIndex];
                    fallGuysCharacter.SetDesiredRotation(rotations[replayFrameIndex]);
                    motorAgent.Animator.SetBool(new HashedAnimatorString("Moving"), true);
                    movement.SetDesiredLean(1);
                    replayFrameIndex++;
                }
            }
        }

        public void StopRecording()
        {
            stopped = true;
            positions.Clear();
        }
    }
}
