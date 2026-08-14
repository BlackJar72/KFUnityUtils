using UnityEngine;
using static kfutils.time.WorldTime;


namespace kfutils.time {

    /// <summary>
    /// A class to make it easy to add a time field to a MonoBehaviour or 
    /// ScriptableObject.  The purpose is to make it easy for designers to 
    /// intuitively input times that are compatible with the WorldTime class. 
    /// 
    /// All times are reletive to the start of the game; e.g., if the game 
    /// starts in April then April will be month and March will be 11.  This 
    /// could be changed, however, if developers choose to set the time to 
    /// something other than zero at the start of the game (most likely to 
    /// make the months more similar to a familiar real world order or to 
    /// fit a fictitious in world calender). 
    /// </summary>
    [System.Serializable]
    public class HumanTime
    {
        [SerializeField][Range (0, SECONDS_IN_MINUTE - 1)] int second;
        [SerializeField][Range (0, MINUTES_IN_HOUR - 1)] int minute;
        [SerializeField][Range (0, HOURS_IN_DAY - 1)] int hour;
        [SerializeField][Range (0, DAYS_IN_MONTH - 1)] int day;
        [SerializeField][Range (0, MONTHS_IN_YEAR - 1)] int month;
        [SerializeField] int year;

        private float fraction;

        public int Second => second;
        public int Minute => minute;
        public int Hour => hour;
        public int Day => day;
        public int Month => month;
        public float Fraction => fraction;


        public HumanTime()
        {
            year = month = day = hour = minute = second = 0;
        }


        public HumanTime(int second, int minute, int hour, int day = 0, int month = 0, int year = 0)
        {
            this.second = second;
            this.minute = minute;
            this.hour = hour;
            this.day = day;
            this.month = month;
            this.year = year;
        }


        public HumanTime(double time)
        {
            Set(time);
        }


        public void Set(int second, int minute, int hour, int day = 0, int month = 0, int year = 0)
        {
            this.second = second;
            this.minute = minute;
            this.hour = hour;
            this.day = day;
            this.month = month;
            this.year = year;
        }


        public void Set(double time)
        {
            month = (int)(time / MONTH);
            time %= MONTH;
            day = (int)(time / DAY);
            time %= DAY;
            hour = (int)(time / HOUR);
            time %= HOUR;
            minute = (int)(time / MINUTE);
            second = (int)time;
            fraction = (float)(time - second);
        }


        public GameTime ToGameTime()
        {
            return new(second + (minute * MINUTE) + (hour * HOUR) + (day * DAY) + (month * MONTH) + (year * YEAR) + fraction);
        }
    }


    /// <summary>
    /// A small struct wrapping a double, mostly allowing easy conversion from 
    /// WorldTime time as a raw double or float to and from something more 
    /// human readable and human comprehensible.
    /// </summary>
    public readonly struct GameTime
    {
        private readonly double seconds;

        public double time => seconds;
        public float ftime => (float)seconds;
        public int Day => Mathf.FloorToInt((float)(seconds / RT_DAY));
        public int Week => Mathf.FloorToInt((float)(seconds / RT_WEEK));
        public int Month => Mathf.FloorToInt((float)(seconds / RT_MONTH));
        public int DayOfWeek => Day % 7;
        public float TimeInDay => (float)(seconds / RT_DAY) - Day;
        public int DayOfMonth => Day % 28;
        public float TimeInMonth => (float)(seconds / RT_MONTH) - Month; 

        public static implicit operator double(GameTime t) => t.seconds;
        public static implicit operator GameTime(double t) => new(t);
        public static explicit operator float(GameTime t) => (float)t.seconds;
        public static implicit operator GameTime(float t) => new(t);
        public static explicit operator HumanTime(GameTime t) => new(t.seconds);
        public static implicit operator GameTime(HumanTime t) => new(t.ToGameTime());

        public GameTime(double seconds) => this.seconds = seconds;
    }

}

