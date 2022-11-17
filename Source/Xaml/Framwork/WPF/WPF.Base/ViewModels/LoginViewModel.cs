using DynMvp.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WPF.Base.Helpers;
using WPF.Base.Views;

namespace WPF.Base.ViewModels
{
    public class LoginViewModel : Observable
    {
        #region 변수

        private string userAccount = "";
        public string UserAccount
        {
            get => userAccount;
            set => Set(ref userAccount, value);
        }

        private string userPassword = "";
        public string UserPassword
        {
            get => userPassword;
            set => Set(ref userPassword, value);
        }

        #endregion

        #region Command

        private ICommand loginButtonClick;
        public ICommand LoginButtonClick
        {
            get => loginButtonClick ?? (loginButtonClick = new RelayCommand<Window>(LoginButtonClickAction));
        }

        private void LoginButtonClickAction(Window wnd)
        {
            var loginUser = UserHandler.Instance().GetUser(UserAccount, UserPassword);
            if (loginUser != null)
            {
                UserHandler.Instance().CurrentUser = loginUser;
                wnd.DialogResult = true;
            }
            else
            {
                MessageBox.Show("Warning", "사용자 정보가 일치하지 않습니다.");
            }
        }

        
        private ICommand optionsButtonClick;
        public ICommand OptionsButtonClick
        {
            get => optionsButtonClick ?? (optionsButtonClick = new RelayCommand(OptionsButtonClickAction));
        }

        private void OptionsButtonClickAction()
        {
            ConfigWindow configWindow = new ConfigWindow();
            configWindow.ShowDialog();
        }

        private ICommand cancelButtonClick;
        public ICommand CancelButtonClick
        {
            get => cancelButtonClick ?? (cancelButtonClick = new RelayCommand<Window>(CancelButtonClickAction));
        }

        private void CancelButtonClickAction(Window wnd)
        {
            wnd.DialogResult = false;
        }

        #endregion

        public LoginViewModel()
        {
#if DEBUG
            UserAccount = "developer";
            UserPassword = "masterkey";
#endif
        }
    }
}
