using System.Security.Cryptography;
using System.Text;

namespace Trading
{
    public partial class LoginForm : Form
    {
        private const string KeyFilePath = "keys.dat";

        public string AccessKey { get; private set; }
        public string SecretKey { get; private set; }

        public LoginForm()
        {
            InitializeComponent();
            LoadKeys();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            var accessKey = txtAccessKey.Text.Trim();
            var secretKey = txtSecretKey.Text.Trim();

            if (string.IsNullOrEmpty(accessKey) || string.IsNullOrEmpty(secretKey))
            {
                MessageBox.Show("Access Key와 Secret Key를 모두 입력해주세요.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 키 저장 옵션이 체크된 경우 로컬에 키 저장
            if (chkSaveKeys.Checked)
            {
                SaveKeys(accessKey, secretKey);
            }
            else
            {
                DeleteKeys();
            }

            // AccessKey와 SecretKey 속성 설정
            AccessKey = accessKey;
            SecretKey = secretKey;

            // DialogResult를 OK로 설정하여 로그인 성공을 알림
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void SaveKeys(string accessKey, string secretKey)
        {
            var keys = $"{accessKey}\n{secretKey}";
            var encryptedData = ProtectedData.Protect(Encoding.UTF8.GetBytes(keys), null, DataProtectionScope.CurrentUser);
            File.WriteAllBytes(KeyFilePath, encryptedData);
        }

        private void LoadKeys()
        {
            if (File.Exists(KeyFilePath))
            {
                var encryptedData = File.ReadAllBytes(KeyFilePath);
                var decryptedData = ProtectedData.Unprotect(encryptedData, null, DataProtectionScope.CurrentUser);
                var keys = Encoding.UTF8.GetString(decryptedData).Split('\n');
                if (keys.Length >= 2)
                {
                    txtAccessKey.Text = keys[0];
                    txtSecretKey.Text = keys[1];
                    chkSaveKeys.Checked = true;
                }
            }
        }

        private void DeleteKeys()
        {
            if (File.Exists(KeyFilePath))
            {
                File.Delete(KeyFilePath);
            }
        }
    }
}
