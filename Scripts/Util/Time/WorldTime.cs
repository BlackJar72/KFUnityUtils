using UnityEngine;


namespace kfutils.time {

    /// <summary>
    /// Persistent world time, intended to be edited it needed to (for example) time scale 
    /// this class should be edited accordingly.  This is meant to be a singleton, and no 
    /// more than one should ever exist in the game at once (reguardless of how many scenes 
    /// are loaded). This uses a double to allow for long running and open-ended games, and 
    /// the seconds should be saved in save files, set from the save file on load, and reset 
    /// to 0 for a new game.
    /// 
    /// If run in the editor it will also track the number of frames since its creation and 
    /// should useful information in the inspector.  This is not done in a stand alone 
    /// application.  
    /// 
    /// This is designed to measure a time using simplified, stylized time, where all months 
    /// are the same length.  It does not track days and years in a ways consistent with 
    /// time in the real world.
    /// </summary>
    public class WorldTime : MonoBehaviour
    {
        // How many times faster time runs in game; 60 gives a 24 minute day.
        // Edit this to change rate time passes in the game; 1.0f is real time.
        public const float TIME_SCALE = 60f; 
        
        // Relative lengths of time units to the next larger unit, edit if you 
        // want to change these (e.g., the number of months in a year).
        public const int SECONDS_IN_MINUTE = 60;
        public const int MINUTES_IN_HOUR = 60;
        public const int HOURS_IN_DAY = 24;
        public const int DAYS_IN_WEEK = 7;
        public const int WEEKS_IN_MONTH = 4;
        public const int MONTHS_IN_YEAR = 12;

        // Derived relations between time units more not directly adjacent in size; 
        // these should not usually be directly edited.
        public const int DAYS_IN_MONTH = DAYS_IN_WEEK * WEEKS_IN_MONTH;
        public const int DAYS_IN_YEAR = DAYS_IN_MONTH * MONTHS_IN_YEAR;

        // Time Units in In-Game World Time
        public const double MINUTE = SECONDS_IN_MINUTE;
        public const double HOUR = MINUTE * MINUTES_IN_HOUR;
        public const double DAY = HOUR * HOURS_IN_DAY;
        public const double WEEK = DAY * DAYS_IN_WEEK;
        public const double MONTH = WEEK * WEEKS_IN_MONTH;
        public const double YEAR = MONTH * MONTHS_IN_YEAR;

        // In-Game Time Units in Real Time
        public const double RT_MINUTE = MINUTE / TIME_SCALE;
        public const double RT_HOUR = HOUR / TIME_SCALE;
        public const double RT_DAY = DAY / TIME_SCALE;
        public const double RT_WEEK = WEEK / TIME_SCALE;
        public const double RT_MONTH = MONTH / TIME_SCALE;
        public const double RT_YEAR = YEAR / TIME_SCALE;


        private static WorldTime instance;
        private static double seconds;


        public static double time => seconds;
        public static float ftime => (float)seconds;

        public static int Minute => Mathf.FloorToInt((float)(seconds / RT_MINUTE));
        public static int Hour => Mathf.FloorToInt((float)(seconds / RT_HOUR)); 
        public static int Day => Mathf.FloorToInt((float)(seconds / RT_DAY));
        public static int Week => Mathf.FloorToInt((float)(seconds / RT_WEEK));
        public static int Month => Mathf.FloorToInt((float)(seconds / RT_MONTH));
        public static int Year => Mathf.FloorToInt((float)(seconds / RT_YEAR));
        public static int DayOfWeek => Day % DAYS_IN_WEEK;
        public static float TimeInDay => (float)(seconds / RT_DAY) - Day;
        public static float SecondOfMinute => Hour % SECONDS_IN_MINUTE;
        public static float MinuteOfHour => Hour % MINUTES_IN_HOUR;
        public static int HourOfDay => Hour % HOURS_IN_DAY; 
        public static int DayOfMonth => Day % DAYS_IN_MONTH;
        public static int WeekOfMonth => Week % WEEKS_IN_MONTH;
        public static int MonthOfYear => Month % MONTHS_IN_YEAR;
        public static int DayOfYear => Day % DAYS_IN_YEAR;
        public static float TimeInMonth => (float)(seconds / RT_MONTH) - Month;


#if UNITY_EDITOR
        // This exists to be visible in the inspector
        [SerializeField] double worldTime; 
        private static long frame = 0;
        public static long Frame => frame;

        [SerializeField] float unityTime;
        [SerializeField] float minute;
        [SerializeField] float hour;
        [SerializeField] float day;
        [SerializeField] float timeInDay;
#endif


        void Awake()
        {
            // Even though member are their own static values, this must be a singleton to avoid multiple calls to Update(), 
            // which would speed up time by a factor of the number of instances.  I'm going hardcore and deleting extra 
            // gameobjects.
            if ((instance != null) && (instance != this))
            {
                Debug.LogError("ERROR: Two WorldTime object created at once; this is not allowed.");
                Destroy(instance.gameObject);
                #if UNITY_EDITOR || DEBUG
                throw new System.Exception("Two WorldTime object created at once; this is not allowed.");
                #endif
            }
            instance = this;
            #if UNITY_EDITOR
            Debug.Log("Minute = " + MINUTE + " => " + RT_MINUTE);
            Debug.Log("Hour = " + HOUR + " => " + RT_HOUR);
            Debug.Log("Day = " + DAY + " => " + RT_DAY);
            Debug.Log("Week = " + WEEK + " => " + RT_WEEK);
            #endif
        }


        void OnDestroy()
        {
            instance = null;
        }


        void Update()
        {
            seconds += Time.deltaTime;
#if UNITY_EDITOR
            worldTime = seconds;
            frame++;

            unityTime = Time.time;
            minute = (float)(seconds / RT_MINUTE);
            hour = (float)(seconds / RT_HOUR);
            day = (float)(seconds / RT_DAY);
            timeInDay = TimeInDay;
#endif
        }


        public static void SetTime(double t)
        {
            seconds = t;
        }


        public static void NewGame()
        {
            seconds = 0.0;
        }




    }


}