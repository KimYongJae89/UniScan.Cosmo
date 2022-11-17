using DynMvp.Base;
using DynMvp.Devices.Dio;
using DynMvp.Devices.MotionController;
using DynMvp.UI.Touch;
using Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AlphaMotionMove
{
    public partial class MainForm : Form
    {
        Motion motion = null;
        ThreadHandler workThreadHandler = null;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                MotionInfo motionInfo = MotionInfoFactory.CreateMotionInfo(MotionType.AlphaMotion302);
                motionInfo.NumAxis = 1;
                motion = MotionFactory.Create(motionInfo);
                bool ok = motion.Initialize(motionInfo);
                if (ok == false)
                {
                    MessageBox.Show("Motion Initialize Fail!!");
                    Close();
                    return;
                }
            }
            catch (DllNotFoundException ex)
            {
                string message = string.Format("{0}\r\n{1}", ex.Message, ex.StackTrace);
                MessageBox.Show(message, "UniScan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }

            comboBoxMovFVel.SelectedIndex = 0;
            comboBoxMovBVel.SelectedIndex = 0;
            comboBoxArrVel.SelectedIndex = 0;
            comboBoxRetVel.SelectedIndex = 0;

            this.timerUiUpdate.Start();
        }

        private void timerUiUpdate_Tick(object sender, EventArgs e)
        {
            MotionStatus motionState = motion.GetMotionStatus(0);
            if (motionState.alarm)
                this.buttonEMG.BackColor = Color.Red;
            else
                this.buttonEMG.BackColor = Control.DefaultBackColor;

            if (motionState.servoOn)
                this.buttonServo.BackColor = Color.LightGreen;
            else
                this.buttonServo.BackColor = Control.DefaultBackColor;

            if(workThreadHandler!=null)
                buttonMove.Text = "Stop";
                else
                buttonMove.Text = "Move";

            uint dioValue = ((IDigitalIo)motion).ReadOutputGroup(0);
            this.labelCurIoVal.Text = string.Format("0x {0}", dioValue.ToString("X4"));
        }

        private void buttonServo_Click(object sender, EventArgs e)
        {
            MotionStatus motionState = motion.GetMotionStatus(0);
            motion.TurnOnServo(!motionState.servoOn);
        }

        private MovingParam GetMovingParam()
        {
            float acc, spd;

            if (float.TryParse(textBoxAcc.Text, out acc) == false)
                return null;

            if (float.TryParse(textBoxSpd.Text, out spd) == false)
                return null;

            MovingParam movingParam = new MovingParam("", 10, acc, acc, spd * 1000, 0);
            return movingParam;
        }

        private HomeParam GetHomingParam()
        {
            MovingParam movingParam = GetMovingParam();
            if (movingParam == null)
                return null;


            HomeParam homeParam = new HomeParam();
            homeParam.HomeDirection = MoveDirection.CW;
            homeParam.HomeMode = HomeMode.HomeSensor;
            homeParam.HighSpeed = new MovingParam("", movingParam.MaxVelocity / 20, movingParam.AccelerationTimeMs, movingParam.DecelerationTimeMs, movingParam.MaxVelocity/2, 0);
            homeParam.MediumSpeed = new MovingParam("", movingParam.MaxVelocity / 40, movingParam.AccelerationTimeMs, movingParam.DecelerationTimeMs, movingParam.MaxVelocity/4, 0);
            homeParam.FineSpeed= new MovingParam("", movingParam.MaxVelocity / 80, movingParam.AccelerationTimeMs, movingParam.DecelerationTimeMs, movingParam.MaxVelocity/8, 0);

            return homeParam;
        }

        private PointF GetStartEndPos()
        {
            float startPos, endPos;
            if (float.TryParse(textBoxStartPos.Text, out startPos) == false)
                return PointF.Empty;

            if (float.TryParse(textBoxEndPos.Text, out endPos) == false)
                return PointF.Empty;

            return new PointF(startPos*1000, endPos*1000);

        }

        private delegate uint GetDioValueDelegate(ComboBox comboBox);
        private uint GetDioValue(ComboBox comboBox)
        {
            if(InvokeRequired)
            {
                return (uint)Invoke(new GetDioValueDelegate(GetDioValue), comboBox);
            }
            return (uint)(comboBox.SelectedIndex == 0 ? 0xffff : 0x0000);
        }

        private void buttonMove_Click(object sender, EventArgs e)
        {
            if (workThreadHandler == null)
            {
                MotionStatus motionState = motion.GetMotionStatus(0);
                if (motionState.homeOk == false)
                {
                    bool ok = Homeing(true);
                    motion.StopMove();
                    if (ok == false)
                        return;
                }

                workThreadHandler = new ThreadHandler("WorkThread", new Thread(WorkThreadProc));
                workThreadHandler.Start();
            }
            else
            {
                workThreadHandler.Stop();
                workThreadHandler = null;
            }
        }

        private void WorkThreadProc()
        {
            MovingParam movingParam = GetMovingParam();
            PointF startEndPos = GetStartEndPos();

            while (workThreadHandler.RequestStop == false)
            {
                if (checkBoxMovF.Checked)
                    ((IDigitalIo)motion).WriteOutputGroup(0, GetDioValue(comboBoxMovFVel));

                motion.Move(0, startEndPos.Y, movingParam);

                if (checkBoxArr.Checked)
                    ((IDigitalIo)motion).WriteOutputGroup(0, GetDioValue(comboBoxArrVel));

                if (checkBoxMovB.Checked)
                    ((IDigitalIo)motion).WriteOutputGroup(0, GetDioValue(comboBoxMovBVel));

                motion.Move(0, startEndPos.X, movingParam);

                if (checkBoxRet.Checked)
                    ((IDigitalIo)motion).WriteOutputGroup(0, GetDioValue(comboBoxRetVel));
            }
        }

        private bool Homeing(bool userQuary)
        {
            bool moveHome = true;
            if (userQuary)
            {
                DialogResult dialogResult = MessageForm.Show(null, "Homeing?", MessageFormType.YesNo);
                moveHome = (dialogResult == DialogResult.Yes);
            }

            if (moveHome)
            {
                HomeParam homeParam = GetHomingParam();
                SimpleProgressForm simpleProgressForm = new SimpleProgressForm();
                CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
                bool result = false;
                simpleProgressForm.Show(() =>
                {
                    Stopwatch sw = new Stopwatch();
                    motion.StartHomeMove(0, homeParam);
                    while (true)
                    {
                        bool isHomeDone = motion.IsHomeDone(0);
                        bool isMoveDone = motion.IsMoveDone(0);
                        if (isHomeDone)
                        {
                            result = true;
                            break;
                        }
                        else if (isMoveDone)
                        {
                            if (sw.IsRunning == false)
                                sw.Restart();
                            else if (sw.ElapsedMilliseconds > 1000)
                            {
                                sw.Stop();
                                result = isHomeDone;
                                break;
                            }
                        }

                        if (cancellationTokenSource.IsCancellationRequested)
                        {
                            result = false;
                            break;
                        }
                        Thread.Sleep(100);
                    }
                }, cancellationTokenSource);
                    motion.StopMove(0);
                return result;
            }
            else
            {
                return false;
            }

        }

        private void buttonEMG_Click(object sender, EventArgs e)
        {
            workThreadHandler.AsyncStop();
            while(workThreadHandler.IsStop()==false)
                motion.StopMove();
            workThreadHandler = null;
        }

        
    }
}
