using System;
using System.Windows.Forms;

namespace HollowKeyauht
{
    public partial class Form1 : Form
    {
        //https://hollowauth.onrender.com/

        public HollowAuth.api HollowApp = new HollowAuth.api(
            "",
            "",
            "",
            version: "1.0",
            apiUrl: "https://hollowauth.onrender.com/api/client"
        );

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            lblStatus.Text = "Conectando con el servidor...";
            lblStatus.ForeColor = System.Drawing.Color.Orange;

            HollowApp.init();

            if (HollowApp.response.success)
            {
                lblStatus.Text = "✓ Conexión exitosa | Listo para usar";
                lblStatus.ForeColor = System.Drawing.Color.Green;
            }
            else
            {
                lblStatus.Text = "✗ Error: " + HollowApp.response.message;
                lblStatus.ForeColor = System.Drawing.Color.Red;
                MessageBox.Show(
                    "No se pudo conectar con el servidor de autenticación.\n\n" +
                    HollowApp.response.message,
                    "Error de Conexión",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUser.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show(
                    "Por favor, ingresa tu usuario y contraseña.",
                    "Campos Incompletos",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            lblStatus.Text = "Verificando credenciales...";
            lblStatus.ForeColor = System.Drawing.Color.Orange;
            this.Cursor = Cursors.WaitCursor;

            HollowApp.login(username, password);

            if (HollowApp.response.success)
            {
                lblStatus.Text = "✓ Login exitoso! Bienvenido " + HollowApp.user_data.username;
                lblStatus.ForeColor = System.Drawing.Color.Green;

                MessageBox.Show(
                    $"✅ ¡Bienvenido {HollowApp.user_data.username}!\n\n" +
                    $"📋 Plan: {HollowApp.user_data.subscription}\n" +
                    $"📅 Expira: {HollowApp.user_data.expires}\n\n" +
                    $"¡Disfruta de la aplicación!",
                    "Acceso Concedido",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                Form2 mainForm = new Form2();
                mainForm.Show();
                this.Hide();
            }
            else
            {
                lblStatus.Text = "✗ Error de autenticación";
                lblStatus.ForeColor = System.Drawing.Color.Red;

                MessageBox.Show(
                    $"❌ Error al iniciar sesión:\n\n{HollowApp.response.message}",
                    "Acceso Denegado",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }

            this.Cursor = Cursors.Default;
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            string username = txtUser.Text.Trim();
            string password = txtPassword.Text.Trim();
            string license = txtLicense.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show(
                    "Por favor, ingresa usuario y contraseña para registrarte.\n\n" +
                    "La licencia es opcional pero recomendada.",
                    "Campos Incompletos",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            DialogResult result = MessageBox.Show(
                $"¿Deseas registrar el usuario '{username}'?\n\n" +
                "Asegúrate de tener una licencia válida.",
                "Confirmar Registro",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result != DialogResult.Yes)
                return;

            lblStatus.Text = "Registrando usuario...";
            lblStatus.ForeColor = System.Drawing.Color.Orange;
            this.Cursor = Cursors.WaitCursor;

            HollowApp.register(username, password, license);

            if (HollowApp.response.success)
            {
                lblStatus.Text = "✓ Registro exitoso! Ahora puedes iniciar sesión";
                lblStatus.ForeColor = System.Drawing.Color.Green;

                MessageBox.Show(
                    "✅ ¡Registro completado con éxito!\n\n" +
                    "Ahora puedes iniciar sesión con tu usuario y contraseña.",
                    "Registro Exitoso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                txtLicense.Clear();
                txtUser.Clear();
                txtPassword.Clear();
                txtUser.Focus();
            }
            else
            {
                lblStatus.Text = "✗ Error en el registro";
                lblStatus.ForeColor = System.Drawing.Color.Red;

                MessageBox.Show(
                    $"❌ Error al registrar:\n\n{HollowApp.response.message}",
                    "Registro Fallido",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }

            this.Cursor = Cursors.Default;
        }

        private void btnLicenseLogin_Click(object sender, EventArgs e)
        {
            string license = txtLicense.Text.Trim();

            if (string.IsNullOrEmpty(license))
            {
                MessageBox.Show(
                    "Por favor, ingresa tu clave de licencia en el campo correspondiente.",
                    "Licencia Requerida",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            lblStatus.Text = "Verificando licencia...";
            lblStatus.ForeColor = System.Drawing.Color.Orange;
            this.Cursor = Cursors.WaitCursor;

            HollowApp.license(license);

            if (HollowApp.response.success)
            {
                lblStatus.Text = "✓ Licencia válida! Acceso concedido";
                lblStatus.ForeColor = System.Drawing.Color.Green;

                MessageBox.Show(
                    $"✅ ¡Licencia válida!\n\n" +
                    $"📋 Plan: {HollowApp.user_data.subscription}\n" +
                    $"📅 Expira: {HollowApp.user_data.expires}\n\n" +
                    $"¡Bienvenido!",
                    "Acceso Concedido",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                Form2 mainForm = new Form2();
                mainForm.Show();
                this.Hide();
            }
            else
            {
                lblStatus.Text = "✗ Licencia inválida";
                lblStatus.ForeColor = System.Drawing.Color.Red;

                MessageBox.Show(
                    $"❌ Licencia inválida o expirada:\n\n{HollowApp.response.message}",
                    "Acceso Denegado",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }

            this.Cursor = Cursors.Default;
        }

        private void txtUser_TextChanged(object sender, EventArgs e)
        {
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
        }

        private void txtLicense_TextChanged(object sender, EventArgs e)
        {
        }
    }
}