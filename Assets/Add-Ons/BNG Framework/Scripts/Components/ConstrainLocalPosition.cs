using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BNG {

    /// <summary>
    /// Constrain a Transform's LocalPosition to a given value.
    /// </summary>
    public class ConstrainLocalPosition : MonoBehaviour {

        public bool ConstrainLocalX = false;
        public float LocalXMin = -1f;
        public float LocalXMax = 1f;

        public bool ConstrainLocalY = false;
        public float LocalYMin = -1f;
        public float LocalYMax = 1f;

        public bool ConstrainLocalZ = false;
        public float LocalZMin = -1f;
        public float LocalZMax = 1f;

        void Update() {

            // Save a lookup
            if(!ConstrainLocalX && !ConstrainLocalY && !ConstrainLocalZ) {
                return;
            }

            Vector3 currentPos = transform.localPosition;
            float newX = ConstrainLocalX ? Mathf.Clamp(currentPos.x, LocalXMin, LocalXMax) : currentPos.x;
            float newY = ConstrainLocalX ? Mathf.Clamp(currentPos.x, LocalXMin, LocalXMax) : currentPos.x;
            float newZ = ConstrainLocalX ? Mathf.Clamp(currentPos.x, LocalXMin, LocalXMax) : currentPos.x;

            transform.localPosition = new Vector3(newX, newY, newZ);
        }
    }
}

