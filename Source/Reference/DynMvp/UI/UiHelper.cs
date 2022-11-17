using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;

namespace DynMvp.UI
{
    public class STADialog
    {
        private CommonDialog commonDialog;

        private DialogResult result;
        public DialogResult Result
        {
            get { return result; }
        }

        public STADialog(CommonDialog commonDialog)
        {
            this.commonDialog = commonDialog;
        }

        public void ThreadProcShowDialog()
        {
            result = commonDialog.ShowDialog();
        }
    }

    public class UiHelper
    {
        [STAThreadAttribute]
        public static DialogResult ShowSTADialog(CommonDialog commonDialog)
        {
            STADialog sTAFileDialog = new STADialog(commonDialog);
            System.Threading.Thread t = new System.Threading.Thread(sTAFileDialog.ThreadProcShowDialog);
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();
            return sTAFileDialog.Result;
        }

        [DllImport("user32.dll", EntryPoint = "SendMessageA", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        private const int WM_SETREDRAW = 0xB;

        public static void SuspendDrawing(Control target)
        {
            if (target.IsDisposed == false)
                SendMessage(target.Handle, WM_SETREDRAW, 0, 0);
        }

        public static void ResumeDrawing(Control target) { ResumeDrawing(target, true); }
        public static void ResumeDrawing(Control target, bool redraw)
        {
            SendMessage(target.Handle, WM_SETREDRAW, 1, 0);

            if (redraw)
            {
                target.Refresh();
            }
        }

        public static void AutoFontSize(Control control)
        {
            Font font;
            Graphics gp;
            Single factor, factorX, factorY;
            gp = control.CreateGraphics();
            SizeF size = gp.MeasureString(control.Text, control.Font);
            gp.Dispose();

            factorX = (control.Width) / size.Width;
            factorY = (control.Height) / size.Height;
            if (factorX > factorY)
                factor = factorY;
            else
                factor = factorX;
            font = control.Font;

            if (factor < 1)
                factor = 1;

            control.Font= new Font(font.Name, font.SizeInPoints * (factor) - 1);
        }

        public static Font AutoFontSize(Label label, String text)
        {
            Graphics gp = label.CreateGraphics();
            SizeF size = gp.MeasureString(text, label.Font);
            gp.Dispose();

            float factorX = (label.Width - label.Margin.Horizontal) / size.Width;
            float factorY = (label.Height - label.Margin.Vertical) / size.Height;

            float factor = 0;
            if (factorX > factorY)
                factor = factorY;
            else
                factor = factorX;

            return new Font(label.Font.Name, label.Font.SizeInPoints * factor, label.Font.Style);
        }

        public static void ExportCsv(DataTable dataTable)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "CSV File (.csv)|*.csv|All Files (*.*)|*.*";
            if (dialog.ShowDialog() == DialogResult.Cancel)
                return;

            try
            {
                StreamWriter csvFileWriter = new StreamWriter(dialog.FileName, false, Encoding.Default);

                string oneLine = "";
                foreach (DataColumn column in dataTable.Columns)
                {
                    if (oneLine != "")
                        oneLine += ",";
                    oneLine += column.ColumnName;
                }
                csvFileWriter.WriteLine(oneLine);

                foreach (DataRow row in dataTable.Rows)
                {
                    oneLine = "";
                    foreach (object obj in row.ItemArray)
                    {
                        if (oneLine != "")
                            oneLine += ",";
                        oneLine += "\"" + obj.ToString() + "\"";
                    }
                    csvFileWriter.WriteLine(oneLine);
                }

                csvFileWriter.Flush();
                csvFileWriter.Close();
            }
            catch (Exception exceptionObject)
            {
                MessageBox.Show(exceptionObject.ToString());
            }   
        }

        public static void ExportCsv(DataGridView dataGridView)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "CSV File (.csv)|*.csv|All Files (*.*)|*.*";
            if (dialog.ShowDialog() == DialogResult.Cancel)
                return;

            StreamWriter csvFileWriter = null;

            try
            {
                csvFileWriter = new StreamWriter(dialog.FileName, false);

                string oneLine = "";
                foreach (DataGridViewColumn column in dataGridView.Columns)
                {
                    if (oneLine != "")
                        oneLine += ",";
                    oneLine += column.HeaderText;
                }
                csvFileWriter.WriteLine(oneLine);

                foreach (DataGridViewRow row in dataGridView.Rows)
                {
                    oneLine = "";
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (oneLine != "")
                            oneLine += ",";
                        oneLine += cell.Value.ToString();
                    }
                    csvFileWriter.WriteLine(oneLine);
                }

                csvFileWriter.Flush();
                csvFileWriter.Close();
            }
            catch (Exception exceptionObject)
            {
                MessageBox.Show(exceptionObject.ToString());

                if (csvFileWriter != null)
                {
                    csvFileWriter.Flush();
                    csvFileWriter.Close();
                }
            }
        }

        public static void ExportCsv(DataGridView dataGridView, string filePath)
        {
            StreamWriter csvFileWriter = null;

            try
            {
                csvFileWriter = new StreamWriter(filePath, false);

                string oneLine = "";
                foreach (DataGridViewColumn column in dataGridView.Columns)
                {
                    if (oneLine != "")
                        oneLine += ",";
                    oneLine += column.HeaderText;
                }
                csvFileWriter.WriteLine(oneLine);

                foreach (DataGridViewRow row in dataGridView.Rows)
                {
                    oneLine = "";
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (oneLine != "")
                            oneLine += ",";
                        oneLine += cell.Value.ToString();
                    }
                    csvFileWriter.WriteLine(oneLine);
                }

                csvFileWriter.Flush();
                csvFileWriter.Close();
            }
            catch (Exception exceptionObject)
            {
                MessageBox.Show(exceptionObject.ToString());

                if (csvFileWriter != null)
                {
                    csvFileWriter.Flush();
                    csvFileWriter.Close();
                }
            }
        }

        public static void MoveUp(DataGridView dataGridView)
        {
            if (dataGridView.SelectedRows.Count == 0)
                return;

            int rowIndex = dataGridView.SelectedRows[0].Index;
            if (rowIndex <= 0)
                return;

            DataGridViewRow selectedRow = dataGridView.Rows[rowIndex];
            dataGridView.Rows.Remove(selectedRow);
            dataGridView.Rows.Insert(rowIndex - 1, selectedRow);
            dataGridView.ClearSelection();
            dataGridView.Rows[rowIndex - 1].Selected = true;
        }

        public static void MoveDown(DataGridView dataGridView)
        {
            if (dataGridView.SelectedRows.Count == 0)
                return;

            int rowIndex = dataGridView.SelectedRows[0].Index;
            if (rowIndex >= (dataGridView.Rows.Count - 1))
                return;

            DataGridViewRow selectedRow = dataGridView.Rows[rowIndex];
            dataGridView.Rows.Remove(selectedRow);
            dataGridView.Rows.Insert(rowIndex + 1, selectedRow);
            dataGridView.ClearSelection();
            dataGridView.Rows[rowIndex + 1].Selected = true;
        }
    }
}
