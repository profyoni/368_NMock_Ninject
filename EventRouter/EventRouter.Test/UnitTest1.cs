using System;
using System.Reflection;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using Ninject.Modules;
using NMock;
using NMock.Actions;

// http://www.ninject.org/
// http://blog.agilistic.nl/a-step-by-step-guide-to-using-ninject-for-dependancy-injection-in-c-sharp/

namespace EventRouter.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void IsAlarmRinging()
        {
            var alarm = new Alarm(new SystemTimer());
            alarm.SetAlarm(DateTime.UtcNow);
            alarm.IsAlarmRinging.Should().BeTrue();
        }


        [TestMethod]
        public void IsAlarmRinging2()
        {
            var mf = new MockFactory();
            var mockTimer = mf.CreateMock<ITimer>();
            DateTime time1 = new DateTime(2016, 2, 2, 0, 0, 0);
            mockTimer.Expects.One.Method(t => t.GetTime()).WillReturn(time1);
            var alarm = new Alarm(mockTimer.MockObject);
            alarm.SetAlarm(time1.AddSeconds(-1));
            alarm.IsAlarmRinging.Should().BeFalse();
        }

        [TestMethod]
        public void IsAlarmRingingThrowsException()
        {
            var mf = new MockFactory();
            var mockTimer = mf.CreateMock<ITimer>();
            DateTime time1 = new DateTime(2016, 2, 2, 0, 0, 0);
            mockTimer.Expects.One.Method(t => t.GetTime()).Will(new ThrowAction(new InvalidTimeZoneException()));
            var alarm = new Alarm(mockTimer.MockObject);
            
            alarm.IsAlarmRinging.Should().BeFalse();
        }



        [TestMethod]
        public void CheckTime()
        {
            var mf = new MockFactory();
            var mockTimer = mf.CreateMock<ITimer>();
            DateTime time1 = new DateTime(2016, 2, 2, 0, 0, 0);
            mockTimer.Expects.One.Method(t => t.GetTime()).Will(new ThrowAction(new InvalidTimeZoneException()));
            var alarm = new Alarm(mockTimer.MockObject);

            alarm.CheckTime();
        }
        public class Bindings : NinjectModule
        {
            public override void Load()
            {
                var mf = new MockFactory();
                var mockTimer = mf.CreateMock<ITimer>();
                Bind<ITimer>().To<SystemTimer>();
            }
        }


        [TestMethod]
        public void Ninject()
        {
            var kernel = new StandardKernel();
            kernel.Load(Assembly.GetExecutingAssembly());
            var alarm = kernel.Get<Alarm>();

            alarm.CheckTime();
        }
    }
}
