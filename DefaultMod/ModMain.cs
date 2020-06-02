using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Media;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using GooseShared;
using SamEngine;
using DefaultMod;
using System.Threading.Tasks;

namespace OnePunchGoose
{
    public class ModEntryPoint : IMod
    {


        [DllImport("user32.dll")]
        public static extern short GetAsyncKeyState(Keys vKey);
        ParticleEngine TEST = new ParticleEngine(new Vector2(400, 240));
        void IMod.Init()
        {

            InjectionPoints.PostTickEvent += PostTick;
            InjectionPoints.PreRenderEvent += preRenderEvent;
        }

        public void preRenderEvent(GooseEntity goose, Graphics g)
        {
            if (goose.currentTask == API.TaskDatabase.getTaskIndexByID("CustomMouseNab"))
            {
                FollowMouseLowAccelerationTask.ChangeColorTaskData data = (FollowMouseLowAccelerationTask.ChangeColorTaskData)goose.currentTaskData;
                if (data.ONEPUNCH)
                {
                    TEST.Size = 8;
                    TEST.Velocity = 4f;
                    TEST.EmitterLocation = goose.rig.head2EndPoint;
                    TEST.Update();
                    TEST.Draw(g);
                }

            }
            else
            {
                TEST.Velocity = 80f;
                TEST.EmitterLocation = goose.rig.head2EndPoint;
                TEST.Stop();
                TEST.Draw(g);
            }
        }

        public void PostTick(GooseEntity g)
        {

            if (g.currentTask == API.TaskDatabase.getTaskIndexByID("NabMouse"))
            {

                API.Goose.setCurrentTaskByID(g,"CustomMouseNab", false);
            }
        }
    }
}
