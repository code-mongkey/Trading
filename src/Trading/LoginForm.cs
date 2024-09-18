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
                MessageBox.Show("Access Key�� Secret Key�� ��� �Է����ּ���.", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Ű ���� �ɼ��� üũ�� ��� ���ÿ� Ű ����
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

            // AccessKey�� SecretKey �Ӽ� ����
            AccessKey = accessKey;
            SecretKey = secretKey;

            // DialogResult�� OK�� �����Ͽ� �α��� ������ �˸�
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
