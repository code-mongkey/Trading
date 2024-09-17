namespace Trading
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // LoginForm�� ��� ���̾�α׷� ǥ��
            using (var loginForm = new LoginForm())
            {
                var result = loginForm.ShowDialog();

                if (result == DialogResult.OK)
                {
                    // �α��� ���� �� MainForm ����
                    Application.Run(new MainForm(loginForm.AccessKey, loginForm.SecretKey));
                }
                else
                {
                    // �α��� ��� �Ǵ� ���� �� ���ø����̼� ����
                    Application.Exit();
                }
            }
        }
    }
}