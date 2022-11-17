﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfControlLibrary.Helper;

namespace WpfControlLibrary.UI
{
    /// <summary>
    /// MessageBox.xaml에 대한 상호 작용 논리
    /// </summary>
    /// 

    public partial class CustomInputForm : Window, IMultiLanguageSupport
    {
        public CustomInputForm()
        {
            InitializeComponent();

            LocalizeHelper.AddListener(this);
        }

        public void UpdateLanguage()
        {
            LocalizeHelper.UpdateString(this);
        }

        static CustomInputForm inputForm;
        static MessageBoxResult _result = MessageBoxResult.No;
        static string inputText = null;
        public static Tuple<MessageBoxResult, string> Show (string caption, string msg, MessageBoxImage icon, string prevMsg = "")
        {
            inputForm = new CustomInputForm { txtInput = { Text = prevMsg }, txtMsg = { Text = caption }, MessageTitle = { Text = msg } };
            inputForm.SetImage(icon.ToString() + ".png");
            inputForm.txtInput.SelectAll();
            inputForm.txtInput.Focus();
            inputForm.ShowDialog();

            return new Tuple<MessageBoxResult, string> (_result, inputText);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender == btnOk)
                _result = MessageBoxResult.OK;
            else if (sender == btnCancel)
                _result = MessageBoxResult.Cancel;
            else
                _result = MessageBoxResult.None;

            inputText = txtInput.Text;

            inputForm.Close();
            inputForm = null;
        }
        private void SetImage(string imageName)
        {
            string uri = string.Format("pack://siteoforigin:,,,/Resources/{0}", imageName);
            var uriSource = new Uri(uri, UriKind.Absolute);
            img.Source = new BitmapImage(uriSource);
        }

        private void TxtInput_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                Button_Click(btnOk, null);
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            LocalizeHelper.RemoveListener(this);
        }
    }
}
