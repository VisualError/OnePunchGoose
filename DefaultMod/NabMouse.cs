using System.Media;
using System.Threading;
using System.Windows.Forms;
using GooseShared;
using OnePunchGoose;
using OnePunchGoose.Properties;
using SamEngine;

namespace DefaultMod
{

    class FollowMouseLowAccelerationTask : GooseTaskInfo
    {

        public FollowMouseLowAccelerationTask()
        {

            canBePickedRandomly = false;


            shortName = "Custom Nab";
            description = "Custom Mouse Nab for the one punch mod.";



            taskID = "CustomMouseNab";

        }



        public class ChangeColorTaskData : GooseTaskData
        {
            public float timeStarted;
            public float originalAcceleration;
            public bool ONEPUNCH;
            public SoundPlayer s1;
            public float dir;
        }


        public override GooseTaskData GetNewTaskData(GooseEntity goose)
        {
            ChangeColorTaskData taskData = new ChangeColorTaskData();
            taskData.timeStarted = Time.time;
            taskData.originalAcceleration = goose.currentAcceleration;
            taskData.ONEPUNCH = false;
            taskData.s1 = new SoundPlayer(Resources.ONE_PUNCH);
            taskData.s1.Load();
            taskData.dir = goose.direction;
            return taskData;
        }


        public override void RunTask(GooseEntity goose)
        {

            ChangeColorTaskData data = (ChangeColorTaskData)goose.currentTaskData;
            Vector2 vector = new Vector2(Cursor.Position.X, Cursor.Position.Y);

            goose.currentAcceleration = 30000;
            goose.targetPos = vector - (goose.rig.head2EndPoint - goose.position);
            if (Time.time - data.timeStarted > 7.354f)
            {
                goose.currentAcceleration = data.originalAcceleration;
                data.ONEPUNCH = false;

                API.Goose.setTaskRoaming(goose);
            }
            if (Vector2.Distance(goose.rig.head2EndPoint, vector) < 15f)
            {
                if (!data.ONEPUNCH)
                {
                    new Thread(() =>
                    {
                        if (Config.settings.EnablePunchFX)
                        {
                            data.s1.PlaySync();
                        }
                    }).Start();
                    data.ONEPUNCH = true;
                    data.dir = goose.direction;
                }
            }
        }
    }
}
