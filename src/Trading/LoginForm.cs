using System.Security.Cryptography;
using System.Text;
using Trading.Utiliy;

namespace Trading
{
    public partial class LoginForm : Form
    {
        public string AccessKey { get; private set; }
        public string SecretKey { get; private set; }

        private Security _security;

        public LoginForm()
        {
            InitializeComponent();
            _security = new Security();
            LoadKeys();
        }

        private void LoadKeys()
        {
            var keys = _security.LoadKeys();
            if (keys.TryGetValue("AccessKey", out var accessKey) &&
                keys.TryGetValue("SecretKey", out var secretKey) &&
                keys.TryGetValue("SaveKey", out var saveKey))
            {
                txtAccessKey.Text = accessKey;
                txtSecretKey.Text = secretKey;
                chkSaveKeys.Checked = saveKey == "true" ? true : false;
            }
            else
            {
            }
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
                _security.SaveKey("AccessKey", accessKey);
                _security.SaveKey("SecretKey", secretKey);
                _security.SaveKey("SaveKey", "true");
            }
            else
            {
                _security.DeleteKey("AccessKey");
                _security.DeleteKey("SecretKey");
                _security.DeleteKey("SaveKey");
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
    }
}
