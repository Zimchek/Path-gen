using System;
using System.Windows;
using System.Text;
using System.Security.Cryptography;

namespace PasswordGenerator
{
    public partial class MainWindow : Window
    {
        private readonly char[] _numbers = "0123456789".ToCharArray();
        private readonly char[] _specialCharacters = "!@#$%^&*()-_=+[{]};:'\",.<>/?|".ToCharArray();
        private readonly char[] _letters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

        public MainWindow()
        {
            InitializeComponent();
        }

        // Генерация пароля по нажатию кнопки
        private void GeneratePassword_Click(object sender, RoutedEventArgs e)
        {
            var length = Convert.ToInt32(LengthSlider.Value);
            bool includeNumbers = IncludeNumbersCheckbox.IsChecked.GetValueOrDefault(false);
            bool includeSpecialChars = IncludeSpecialCharsCheckbox.IsChecked.GetValueOrDefault(false);

            string password = GenerateRandomPassword(length, includeNumbers, includeSpecialChars);
            GeneratedPasswordTextbox.Text = password;
        }

        // Алгоритм генерации пароля
        private string GenerateRandomPassword(int length, bool useNumbers, bool useSpecialChars)
        {
            StringBuilder password = new StringBuilder();
            using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                byte[] randomBytes = new byte[length];
                rngCryptoServiceProvider.GetBytes(randomBytes);

                foreach (byte b in randomBytes)
                {
                    char c;
                    switch (b % 3)
                    {
                        case 0:
                            c = _letters[b % _letters.Length];
                            break;
                        case 1 when useNumbers:
                            c = _numbers[b % _numbers.Length];
                            break;
                        case 2 when useSpecialChars:
                            c = _specialCharacters[b % _specialCharacters.Length];
                            break;
                        default:
                            c = _letters[b % _letters.Length];
                            break;
                    }

                    password.Append(c);
                }
            }

            return password.ToString();
        }

        // Копирует пароль в буфер обмена
        private void CopyToClipboard_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(GeneratedPasswordTextbox.Text);
            MessageBox.Show("Пароль скопирован в буфер.", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
