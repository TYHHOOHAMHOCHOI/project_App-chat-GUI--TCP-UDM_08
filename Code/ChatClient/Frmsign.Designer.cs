namespace ChatClient
{
    partial class Frmsign
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            panel1 = new Panel();
            panel3 = new Panel();
            label5 = new Label();
            label4 = new Label();
            reusername = new TextBox();
            recopassword = new TextBox();
            label3 = new Label();
            register = new Button();
            repassword = new TextBox();
            label2 = new Label();
            label1 = new Label();
            panel2 = new Panel();
            pictureBox1 = new PictureBox();
            panel1.SuspendLayout();
            panel3.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.Gainsboro;
            panel1.Controls.Add(panel3);
            panel1.Controls.Add(panel2);
            panel1.Location = new Point(168, 128);
            panel1.Name = "panel1";
            panel1.Size = new Size(971, 504);
            panel1.TabIndex = 0;
            // 
            // panel3
            // 
            panel3.Controls.Add(label5);
            panel3.Controls.Add(label4);
            panel3.Controls.Add(reusername);
            panel3.Controls.Add(recopassword);
            panel3.Controls.Add(label3);
            panel3.Controls.Add(register);
            panel3.Controls.Add(repassword);
            panel3.Controls.Add(label2);
            panel3.Controls.Add(label1);
            panel3.Dock = DockStyle.Right;
            panel3.Location = new Point(468, 0);
            panel3.Name = "panel3";
            panel3.Size = new Size(503, 504);
            panel3.TabIndex = 1;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.BackColor = Color.Gainsboro;
            label5.Font = new Font("Courier New", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.ForeColor = Color.FromArgb(0, 0, 192);
            label5.Location = new Point(314, 446);
            label5.Name = "label5";
            label5.Size = new Size(138, 27);
            label5.TabIndex = 9;
            label5.Text = "Đăng nhập";
            label5.Click += label5_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Courier New", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label4.ForeColor = Color.Black;
            label4.Location = new Point(72, 446);
            label4.Name = "label4";
            label4.Size = new Size(236, 27);
            label4.TabIndex = 8;
            label4.Text = "Đã có tài khoản?";
            label4.Click += label4_Click_1;
            // 
            // reusername
            // 
            reusername.Location = new Point(45, 66);
            reusername.Name = "reusername";
            reusername.Size = new Size(427, 39);
            reusername.TabIndex = 2;
            reusername.TextChanged += textBox1_TextChanged;
            // 
            // recopassword
            // 
            recopassword.Location = new Point(45, 321);
            recopassword.Name = "recopassword";
            recopassword.Size = new Size(427, 39);
            recopassword.TabIndex = 7;
            recopassword.TextChanged += textBox3_TextChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Courier New", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.ForeColor = Color.Blue;
            label3.Location = new Point(45, 264);
            label3.Name = "label3";
            label3.Size = new Size(319, 36);
            label3.TabIndex = 6;
            label3.Text = "Confirm password";
            label3.Click += label3_Click;
            // 
            // register
            // 
            register.BackColor = Color.FromArgb(128, 128, 255);
            register.Font = new Font("Courier New", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            register.ForeColor = SystemColors.ButtonHighlight;
            register.Location = new Point(45, 382);
            register.Name = "register";
            register.Size = new Size(427, 48);
            register.TabIndex = 4;
            register.Text = "Đăng ký";
            register.UseVisualStyleBackColor = false;
            register.Click += button1_Click;
            // 
            // repassword
            // 
            repassword.Location = new Point(45, 189);
            repassword.Name = "repassword";
            repassword.Size = new Size(427, 39);
            repassword.TabIndex = 3;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Courier New", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.ForeColor = Color.Blue;
            label2.Location = new Point(45, 136);
            label2.Name = "label2";
            label2.Size = new Size(167, 36);
            label2.TabIndex = 1;
            label2.Text = "Password";
            label2.Click += label2_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Courier New", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.Blue;
            label1.Location = new Point(45, 10);
            label1.Name = "label1";
            label1.Size = new Size(167, 36);
            label1.TabIndex = 0;
            label1.Text = "Username";
            label1.Click += label1_Click;
            // 
            // panel2
            // 
            panel2.Controls.Add(pictureBox1);
            panel2.Dock = DockStyle.Left;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(486, 504);
            panel2.TabIndex = 0;
            // 
            // pictureBox1
            // 
            pictureBox1.BackgroundImage = Properties.Resources.z7836550866230_417f1a1f391c1f9fe70c8209d71ad6ef;
            pictureBox1.Dock = DockStyle.Fill;
            pictureBox1.Location = new Point(0, 0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(486, 504);
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            pictureBox1.Click += label1_Click;
            // 
            // Frmsign
            // 
            AcceptButton = register;
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(0, 0, 64);
            ClientSize = new Size(1307, 756);
            Controls.Add(panel1);
            Name = "Frmsign";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Sign up";
            Load += Frmsign_Load;
            panel1.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Panel panel3;
        private Label label2;
        private Label label1;
        private Panel panel2;
        private Button register;
        private TextBox repassword;
        private TextBox reusername;
        private TextBox recopassword;
        private Label label3;
        private Label label4;
        private Label label5;
        private PictureBox pictureBox1;
    }
}