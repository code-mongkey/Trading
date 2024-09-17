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

            // LoginForm을 모달 다이얼로그로 표시
            using (var loginForm = new LoginForm())
            {
                var result = loginForm.ShowDialog();

                if (result == DialogResult.OK)
                {
                    // 로그인 성공 시 MainForm 실행
                    Application.Run(new MainForm(loginForm.AccessKey, loginForm.SecretKey));
                }
                else
                {
                    // 로그인 취소 또는 실패 시 애플리케이션 종료
                    Application.Exit();
                }
            }
        }
    }
}