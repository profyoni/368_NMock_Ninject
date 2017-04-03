using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace EventRouter
{
    public class SystemTimer : ITimer
    {
        public DateTime GetTime()
        {
            return DateTime.UtcNow;
        }
    }
    public interface ITimer
    {
        DateTime GetTime();
    }
    public interface IReaction
    {
        void React();
    }
    public class Alarm
    {
        private readonly ITimer _timer;
        private DateTime _alarmTime;
        private IReaction _reaction
           ;

        public Alarm(ITimer timer)
        {
            if (timer == null)
                throw new ArgumentNullException(nameof(timer));
            this._timer = timer;
        }

        public void SetAlarm(DateTime t)
        {
            this._alarmTime = t;
        }

        public void SetReaction(IReaction reaction)
        {
            this._reaction = reaction;
        }

        public bool IsAlarmRinging
        {
            get
            {
                var now = _timer.GetTime();
                var diff = (_alarmTime - now).Seconds;
                return diff >= 0 && diff < 60;
            }
        }

        public void CheckTime()
        {
            if (IsAlarmRinging)
                _reaction.React();
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
        }
    }
}
