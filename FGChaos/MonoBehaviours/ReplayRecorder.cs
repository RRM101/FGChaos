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

        void Start()
        {
            rb = GetComponent<Rigidbody>();
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
