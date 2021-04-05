using System;
using System.Collections.Generic;
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

namespace TaskManagement.WPF.Views {
	/// <summary>
	/// Логика взаимодействия для RegisterView.xaml
	/// </summary>
	public partial class RegisterView : UserControl {
		public static readonly DependencyProperty RegisterCommandProperty =
			DependencyProperty.Register("RegisterCommand", typeof(ICommand), typeof(RegisterView), new PropertyMetadata(null));

		public ICommand RegisterCommand {
			get { return (ICommand)GetValue(RegisterCommandProperty); }
			set { SetValue(RegisterCommandProperty, value); }
		}

		public static readonly DependencyProperty ErrorMessageProperty =
			DependencyProperty.Register("ErrorMessage", typeof(string), typeof(RegisterView), new PropertyMetadata(null));

		public string ErrorMessage {
			get { return (string)GetValue(ErrorMessageProperty); }
			set { SetValue(ErrorMessageProperty, value); }
		}
		public RegisterView() {
			InitializeComponent();
		}

		private void Register_Click(object sender, RoutedEventArgs e) {
			if (RegisterCommand != null) {
				ErrorMessage = string.Empty;
				string password = pbPassword.Password;
				string confirmPassword = pbConfirmPassword.Password;

				if (password != confirmPassword) {
					ResourceDictionary oldDict = (from d in Application.Current.Resources.MergedDictionaries
												  where d.Source != null && d.Source.OriginalString.StartsWith("Resources/Lang.")
												  select d).First();
					ErrorMessage = oldDict["Errors_PasswordMissmatch"].ToString();
					return;
				}

				RegisterCommand.Execute(password);
			}
		}
	}
}
