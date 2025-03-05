using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

namespace kfutils {
public class ClockTurner : MonoBehaviour
{
        [SerializeField] GameObject hourHand;
        [SerializeField] GameObject minuteHand;

        [SerializeField] float timeScale = 60;
        [SerializeField] float startOffsetHours = 0;
        [SerializeField] ATimeForClock timeProvider;
        [SerializeField] bool preProcessedHours = true;


        private float currentTime;
        private float currentAngle;
        private float startOffset;

        private const float realSecondsPerRotation = 60 * 60 * 24;
        private float secondsPerRotation;
        private float secondsPerDegree;

        private delegate void MoveHands();
        private MoveHands moveHands;


        void Awake()
        {
            if(timeScale == 0)
            {
                timeScale = 1;
            }
            secondsPerRotation = realSecondsPerRotation / timeScale;
            secondsPerDegree = secondsPerRotation / 360;
            startOffset = (startOffsetHours * secondsPerRotation) / 12;
            currentAngle = 0;
        }


        // Start is called before the first frame update
        void Start()
        {
            if(timeProvider == null) {
                moveHands = MoveHandsByUnityTime;        
            } else if(preProcessedHours) {
                moveHands = MoveHandsByScaledTime;
            } else {
                moveHands = MoveHandsByRawTime;
            }
        }


        // Update is called once per frame
        void Update()
        {
            moveHands();
        }


        private void MoveHandsByUnityTime() {
            float hourHandAngle = (Time.time + startOffset) / secondsPerDegree;
            float minuteHandAngle = hourHandAngle * 12;
            hourHand.transform.localRotation = Quaternion.Euler(0, 0, hourHandAngle);
            minuteHand.transform.localRotation = Quaternion.Euler(0, 0, minuteHandAngle);
        }


        private void MoveHandsByRawTime() {
            float hourHandAngle = (timeProvider.GetTime() + startOffset) / secondsPerDegree;
            float minuteHandAngle = hourHandAngle * 12;
            hourHand.transform.localRotation = Quaternion.Euler(0, 0, hourHandAngle);
            minuteHand.transform.localRotation = Quaternion.Euler(0, 0, minuteHandAngle);
        }


        private void MoveHandsByScaledTime() {
            float hourHandAngle = timeProvider.GetTime() / 30;
            float minuteHandAngle = hourHandAngle * 12;
            hourHand.transform.localRotation = Quaternion.Euler(0, 0, hourHandAngle);
            minuteHand.transform.localRotation = Quaternion.Euler(0, 0, minuteHandAngle);
        }


        /// <summary>
        /// A base class for providing time to the clock, derived from scriptable object.
        /// 
        /// For use with raw time, override GetTime() to return the time in seconds. This 
        /// should be the time since the game began (from the very beginning, saved in in 
        /// save files, etc).  Time off-set and scaling are not included.
        /// 
        /// For use with pre-processed time, override GetTime() to return the time in hours. 
        /// This needs to include any time off-set, as none is provided; it is assumed your 
        /// are feeding the clock the actual in-game hour.
        /// </summary>
        public abstract class ATimeForClock : ScriptableObject {
            /// <summary>
            /// Should return either time in seconds since the beginning of the game for raw 
            //  time, or current time of day for pre-processed time.
            /// </summary>
            /// <returns></returns>
            public abstract float GetTime();
        }



    }

}
