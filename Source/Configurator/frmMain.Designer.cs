namespace Configurator
{
    partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.gbGenSettings = new System.Windows.Forms.GroupBox();
            this.cbTrueScale = new System.Windows.Forms.CheckBox();
            this.lblResolution = new System.Windows.Forms.Label();
            this.cbSimMode = new System.Windows.Forms.CheckBox();
            this.lblRequirementsMsg = new System.Windows.Forms.Label();
            this.cbFullScreen = new System.Windows.Forms.CheckBox();
            this.cbAA = new System.Windows.Forms.CheckBox();
            this.cbVsync = new System.Windows.Forms.CheckBox();
            this.cbResolution = new System.Windows.Forms.ComboBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
            this.pbLogo = new System.Windows.Forms.PictureBox();
            this.btnStartGame = new System.Windows.Forms.Button();
            this.gbSoundSettings = new System.Windows.Forms.GroupBox();
            this.lblSFXVol = new System.Windows.Forms.Label();
            this.lblMusicVol = new System.Windows.Forms.Label();
            this.lblMasterVol = new System.Windows.Forms.Label();
            this.numSFXVol = new System.Windows.Forms.NumericUpDown();
            this.numMusicVol = new System.Windows.Forms.NumericUpDown();
            this.numMasterVol = new System.Windows.Forms.NumericUpDown();
            this.tabcMain = new System.Windows.Forms.TabControl();
            this.tabpGeneral = new System.Windows.Forms.TabPage();
            this.tabpSound = new System.Windows.Forms.TabPage();
            this.tabpOculus = new System.Windows.Forms.TabPage();
            this.gbOculusSettings = new System.Windows.Forms.GroupBox();
            this.cbOculus = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabpSim = new System.Windows.Forms.TabPage();
            this.gbSimulationSettings = new System.Windows.Forms.GroupBox();
            this.lblSimPlanet = new System.Windows.Forms.Label();
            this.lblSimSpeed = new System.Windows.Forms.Label();
            this.txtSimPlanet = new System.Windows.Forms.TextBox();
            this.numSimSpeed = new System.Windows.Forms.NumericUpDown();
            this.pbRequirements = new System.Windows.Forms.ProgressBar();
            this.lblReqLoading = new System.Windows.Forms.Label();
            this.gbGenSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).BeginInit();
            this.gbSoundSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSFXVol)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMusicVol)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMasterVol)).BeginInit();
            this.tabcMain.SuspendLayout();
            this.tabpGeneral.SuspendLayout();
            this.tabpSound.SuspendLayout();
            this.tabpOculus.SuspendLayout();
            this.gbOculusSettings.SuspendLayout();
            this.tabpSim.SuspendLayout();
            this.gbSimulationSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSimSpeed)).BeginInit();
            this.SuspendLayout();
            // 
            // gbGenSettings
            // 
            this.gbGenSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbGenSettings.Controls.Add(this.lblReqLoading);
            this.gbGenSettings.Controls.Add(this.pbRequirements);
            this.gbGenSettings.Controls.Add(this.cbTrueScale);
            this.gbGenSettings.Controls.Add(this.lblResolution);
            this.gbGenSettings.Controls.Add(this.cbSimMode);
            this.gbGenSettings.Controls.Add(this.lblRequirementsMsg);
            this.gbGenSettings.Controls.Add(this.cbFullScreen);
            this.gbGenSettings.Controls.Add(this.cbAA);
            this.gbGenSettings.Controls.Add(this.cbVsync);
            this.gbGenSettings.Controls.Add(this.cbResolution);
            this.gbGenSettings.Location = new System.Drawing.Point(6, 6);
            this.gbGenSettings.Name = "gbGenSettings";
            this.gbGenSettings.Size = new System.Drawing.Size(506, 103);
            this.gbGenSettings.TabIndex = 0;
            this.gbGenSettings.TabStop = false;
            this.gbGenSettings.Text = "General Settings";
            // 
            // cbTrueScale
            // 
            this.cbTrueScale.AutoSize = true;
            this.cbTrueScale.Location = new System.Drawing.Point(160, 46);
            this.cbTrueScale.Name = "cbTrueScale";
            this.cbTrueScale.Size = new System.Drawing.Size(108, 17);
            this.cbTrueScale.TabIndex = 10;
            this.cbTrueScale.Text = "True Scale (EXP)";
            this.cbTrueScale.UseVisualStyleBackColor = true;
            // 
            // lblResolution
            // 
            this.lblResolution.AutoSize = true;
            this.lblResolution.Location = new System.Drawing.Point(6, 22);
            this.lblResolution.Name = "lblResolution";
            this.lblResolution.Size = new System.Drawing.Size(60, 13);
            this.lblResolution.TabIndex = 9;
            this.lblResolution.Text = "Resolution:";
            // 
            // cbSimMode
            // 
            this.cbSimMode.AutoSize = true;
            this.cbSimMode.Location = new System.Drawing.Point(50, 46);
            this.cbSimMode.Name = "cbSimMode";
            this.cbSimMode.Size = new System.Drawing.Size(104, 17);
            this.cbSimMode.TabIndex = 8;
            this.cbSimMode.Text = "Simulation Mode";
            this.cbSimMode.UseVisualStyleBackColor = true;
            // 
            // lblRequirementsMsg
            // 
            this.lblRequirementsMsg.AutoSize = true;
            this.lblRequirementsMsg.Location = new System.Drawing.Point(6, 73);
            this.lblRequirementsMsg.Name = "lblRequirementsMsg";
            this.lblRequirementsMsg.Size = new System.Drawing.Size(115, 13);
            this.lblRequirementsMsg.TabIndex = 6;
            this.lblRequirementsMsg.Text = "RequirementsMessage";
            // 
            // cbFullScreen
            // 
            this.cbFullScreen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbFullScreen.AutoSize = true;
            this.cbFullScreen.Location = new System.Drawing.Point(274, 46);
            this.cbFullScreen.Name = "cbFullScreen";
            this.cbFullScreen.Size = new System.Drawing.Size(74, 17);
            this.cbFullScreen.TabIndex = 5;
            this.cbFullScreen.Text = "Fullscreen";
            this.cbFullScreen.UseVisualStyleBackColor = true;
            // 
            // cbAA
            // 
            this.cbAA.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbAA.AutoSize = true;
            this.cbAA.Location = new System.Drawing.Point(417, 46);
            this.cbAA.Name = "cbAA";
            this.cbAA.Size = new System.Drawing.Size(83, 17);
            this.cbAA.TabIndex = 4;
            this.cbAA.Text = "Anti-Aliasing";
            this.cbAA.UseVisualStyleBackColor = true;
            // 
            // cbVsync
            // 
            this.cbVsync.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbVsync.AutoSize = true;
            this.cbVsync.Location = new System.Drawing.Point(354, 46);
            this.cbVsync.Name = "cbVsync";
            this.cbVsync.Size = new System.Drawing.Size(57, 17);
            this.cbVsync.TabIndex = 3;
            this.cbVsync.Text = "VSync";
            this.cbVsync.UseVisualStyleBackColor = true;
            // 
            // cbResolution
            // 
            this.cbResolution.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbResolution.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbResolution.FormattingEnabled = true;
            this.cbResolution.Location = new System.Drawing.Point(72, 19);
            this.cbResolution.Name = "cbResolution";
            this.cbResolution.Size = new System.Drawing.Size(428, 21);
            this.cbResolution.TabIndex = 1;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(463, 300);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnApply
            // 
            this.btnApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnApply.Location = new System.Drawing.Point(382, 300);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(75, 23);
            this.btnApply.TabIndex = 3;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // pbLogo
            // 
            this.pbLogo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbLogo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbLogo.Image = global::Configurator.Properties.Resources.logo;
            this.pbLogo.Location = new System.Drawing.Point(12, 12);
            this.pbLogo.Name = "pbLogo";
            this.pbLogo.Size = new System.Drawing.Size(526, 135);
            this.pbLogo.TabIndex = 4;
            this.pbLogo.TabStop = false;
            // 
            // btnStartGame
            // 
            this.btnStartGame.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnStartGame.Location = new System.Drawing.Point(12, 300);
            this.btnStartGame.Name = "btnStartGame";
            this.btnStartGame.Size = new System.Drawing.Size(116, 23);
            this.btnStartGame.TabIndex = 5;
            this.btnStartGame.Text = "Start Game";
            this.btnStartGame.UseVisualStyleBackColor = true;
            this.btnStartGame.Click += new System.EventHandler(this.btnStartGame_Click);
            // 
            // gbSoundSettings
            // 
            this.gbSoundSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbSoundSettings.Controls.Add(this.lblSFXVol);
            this.gbSoundSettings.Controls.Add(this.lblMusicVol);
            this.gbSoundSettings.Controls.Add(this.lblMasterVol);
            this.gbSoundSettings.Controls.Add(this.numSFXVol);
            this.gbSoundSettings.Controls.Add(this.numMusicVol);
            this.gbSoundSettings.Controls.Add(this.numMasterVol);
            this.gbSoundSettings.Location = new System.Drawing.Point(6, 6);
            this.gbSoundSettings.Name = "gbSoundSettings";
            this.gbSoundSettings.Size = new System.Drawing.Size(506, 103);
            this.gbSoundSettings.TabIndex = 6;
            this.gbSoundSettings.TabStop = false;
            this.gbSoundSettings.Text = "Sound Settings";
            // 
            // lblSFXVol
            // 
            this.lblSFXVol.AutoSize = true;
            this.lblSFXVol.Location = new System.Drawing.Point(6, 73);
            this.lblSFXVol.Name = "lblSFXVol";
            this.lblSFXVol.Size = new System.Drawing.Size(68, 13);
            this.lblSFXVol.TabIndex = 5;
            this.lblSFXVol.Text = "SFX Volume:";
            // 
            // lblMusicVol
            // 
            this.lblMusicVol.AutoSize = true;
            this.lblMusicVol.Location = new System.Drawing.Point(6, 47);
            this.lblMusicVol.Name = "lblMusicVol";
            this.lblMusicVol.Size = new System.Drawing.Size(76, 13);
            this.lblMusicVol.TabIndex = 4;
            this.lblMusicVol.Text = "Music Volume:";
            // 
            // lblMasterVol
            // 
            this.lblMasterVol.AutoSize = true;
            this.lblMasterVol.Location = new System.Drawing.Point(6, 21);
            this.lblMasterVol.Name = "lblMasterVol";
            this.lblMasterVol.Size = new System.Drawing.Size(80, 13);
            this.lblMasterVol.TabIndex = 3;
            this.lblMasterVol.Text = "Master Volume:";
            // 
            // numSFXVol
            // 
            this.numSFXVol.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numSFXVol.Location = new System.Drawing.Point(420, 71);
            this.numSFXVol.Name = "numSFXVol";
            this.numSFXVol.Size = new System.Drawing.Size(80, 20);
            this.numSFXVol.TabIndex = 2;
            // 
            // numMusicVol
            // 
            this.numMusicVol.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numMusicVol.Location = new System.Drawing.Point(420, 45);
            this.numMusicVol.Name = "numMusicVol";
            this.numMusicVol.Size = new System.Drawing.Size(80, 20);
            this.numMusicVol.TabIndex = 1;
            // 
            // numMasterVol
            // 
            this.numMasterVol.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numMasterVol.Location = new System.Drawing.Point(420, 19);
            this.numMasterVol.Name = "numMasterVol";
            this.numMasterVol.Size = new System.Drawing.Size(80, 20);
            this.numMasterVol.TabIndex = 0;
            // 
            // tabcMain
            // 
            this.tabcMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabcMain.Controls.Add(this.tabpGeneral);
            this.tabcMain.Controls.Add(this.tabpSound);
            this.tabcMain.Controls.Add(this.tabpOculus);
            this.tabcMain.Controls.Add(this.tabpSim);
            this.tabcMain.Location = new System.Drawing.Point(12, 153);
            this.tabcMain.Name = "tabcMain";
            this.tabcMain.SelectedIndex = 0;
            this.tabcMain.Size = new System.Drawing.Size(526, 141);
            this.tabcMain.TabIndex = 7;
            // 
            // tabpGeneral
            // 
            this.tabpGeneral.Controls.Add(this.gbGenSettings);
            this.tabpGeneral.Location = new System.Drawing.Point(4, 22);
            this.tabpGeneral.Name = "tabpGeneral";
            this.tabpGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tabpGeneral.Size = new System.Drawing.Size(518, 115);
            this.tabpGeneral.TabIndex = 0;
            this.tabpGeneral.Text = "General Settings";
            this.tabpGeneral.UseVisualStyleBackColor = true;
            // 
            // tabpSound
            // 
            this.tabpSound.Controls.Add(this.gbSoundSettings);
            this.tabpSound.Location = new System.Drawing.Point(4, 22);
            this.tabpSound.Name = "tabpSound";
            this.tabpSound.Padding = new System.Windows.Forms.Padding(3);
            this.tabpSound.Size = new System.Drawing.Size(518, 115);
            this.tabpSound.TabIndex = 1;
            this.tabpSound.Text = "Sound Settings";
            this.tabpSound.UseVisualStyleBackColor = true;
            // 
            // tabpOculus
            // 
            this.tabpOculus.Controls.Add(this.gbOculusSettings);
            this.tabpOculus.Location = new System.Drawing.Point(4, 22);
            this.tabpOculus.Name = "tabpOculus";
            this.tabpOculus.Padding = new System.Windows.Forms.Padding(3);
            this.tabpOculus.Size = new System.Drawing.Size(518, 115);
            this.tabpOculus.TabIndex = 2;
            this.tabpOculus.Text = "Oculus VR Settings";
            this.tabpOculus.UseVisualStyleBackColor = true;
            // 
            // gbOculusSettings
            // 
            this.gbOculusSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbOculusSettings.Controls.Add(this.cbOculus);
            this.gbOculusSettings.Controls.Add(this.label1);
            this.gbOculusSettings.Location = new System.Drawing.Point(6, 6);
            this.gbOculusSettings.Name = "gbOculusSettings";
            this.gbOculusSettings.Size = new System.Drawing.Size(506, 103);
            this.gbOculusSettings.TabIndex = 1;
            this.gbOculusSettings.TabStop = false;
            this.gbOculusSettings.Text = "Oculus VR Settings";
            // 
            // cbOculus
            // 
            this.cbOculus.AutoSize = true;
            this.cbOculus.Location = new System.Drawing.Point(6, 19);
            this.cbOculus.Name = "cbOculus";
            this.cbOculus.Size = new System.Drawing.Size(144, 17);
            this.cbOculus.TabIndex = 8;
            this.cbOculus.Text = "Oculus VR Mode (BETA)";
            this.cbOculus.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(156, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Oculus Settings Coming Soon...";
            // 
            // tabpSim
            // 
            this.tabpSim.Controls.Add(this.gbSimulationSettings);
            this.tabpSim.Location = new System.Drawing.Point(4, 22);
            this.tabpSim.Name = "tabpSim";
            this.tabpSim.Padding = new System.Windows.Forms.Padding(3);
            this.tabpSim.Size = new System.Drawing.Size(518, 115);
            this.tabpSim.TabIndex = 3;
            this.tabpSim.Text = "Simulation Settings";
            this.tabpSim.UseVisualStyleBackColor = true;
            // 
            // gbSimulationSettings
            // 
            this.gbSimulationSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbSimulationSettings.Controls.Add(this.lblSimPlanet);
            this.gbSimulationSettings.Controls.Add(this.lblSimSpeed);
            this.gbSimulationSettings.Controls.Add(this.txtSimPlanet);
            this.gbSimulationSettings.Controls.Add(this.numSimSpeed);
            this.gbSimulationSettings.Location = new System.Drawing.Point(6, 6);
            this.gbSimulationSettings.Name = "gbSimulationSettings";
            this.gbSimulationSettings.Size = new System.Drawing.Size(506, 103);
            this.gbSimulationSettings.TabIndex = 4;
            this.gbSimulationSettings.TabStop = false;
            this.gbSimulationSettings.Text = "Simulation Settings";
            // 
            // lblSimPlanet
            // 
            this.lblSimPlanet.AutoSize = true;
            this.lblSimPlanet.Location = new System.Drawing.Point(6, 22);
            this.lblSimPlanet.Name = "lblSimPlanet";
            this.lblSimPlanet.Size = new System.Drawing.Size(89, 13);
            this.lblSimPlanet.TabIndex = 2;
            this.lblSimPlanet.Text = "Simulated Planet:";
            // 
            // lblSimSpeed
            // 
            this.lblSimSpeed.AutoSize = true;
            this.lblSimSpeed.Location = new System.Drawing.Point(6, 47);
            this.lblSimSpeed.Name = "lblSimSpeed";
            this.lblSimSpeed.Size = new System.Drawing.Size(92, 13);
            this.lblSimSpeed.TabIndex = 3;
            this.lblSimSpeed.Text = "Simulation Speed:";
            // 
            // txtSimPlanet
            // 
            this.txtSimPlanet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSimPlanet.Location = new System.Drawing.Point(253, 19);
            this.txtSimPlanet.Name = "txtSimPlanet";
            this.txtSimPlanet.Size = new System.Drawing.Size(247, 20);
            this.txtSimPlanet.TabIndex = 0;
            // 
            // numSimSpeed
            // 
            this.numSimSpeed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numSimSpeed.Location = new System.Drawing.Point(253, 45);
            this.numSimSpeed.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numSimSpeed.Name = "numSimSpeed";
            this.numSimSpeed.Size = new System.Drawing.Size(247, 20);
            this.numSimSpeed.TabIndex = 1;
            // 
            // pbRequirements
            // 
            this.pbRequirements.Location = new System.Drawing.Point(6, 69);
            this.pbRequirements.Name = "pbRequirements";
            this.pbRequirements.Size = new System.Drawing.Size(494, 23);
            this.pbRequirements.TabIndex = 11;
            // 
            // lblReqLoading
            // 
            this.lblReqLoading.AutoSize = true;
            this.lblReqLoading.Location = new System.Drawing.Point(212, 73);
            this.lblReqLoading.Name = "lblReqLoading";
            this.lblReqLoading.Size = new System.Drawing.Size(91, 13);
            this.lblReqLoading.TabIndex = 12;
            this.lblReqLoading.Text = "Testing your PC...";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(550, 335);
            this.Controls.Add(this.tabcMain);
            this.Controls.Add(this.btnStartGame);
            this.Controls.Add(this.pbLogo);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.Text = "Planetary Orbit - Configuration";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.gbGenSettings.ResumeLayout(false);
            this.gbGenSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).EndInit();
            this.gbSoundSettings.ResumeLayout(false);
            this.gbSoundSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSFXVol)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMusicVol)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMasterVol)).EndInit();
            this.tabcMain.ResumeLayout(false);
            this.tabpGeneral.ResumeLayout(false);
            this.tabpSound.ResumeLayout(false);
            this.tabpOculus.ResumeLayout(false);
            this.gbOculusSettings.ResumeLayout(false);
            this.gbOculusSettings.PerformLayout();
            this.tabpSim.ResumeLayout(false);
            this.gbSimulationSettings.ResumeLayout(false);
            this.gbSimulationSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSimSpeed)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbGenSettings;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.PictureBox pbLogo;
        private System.Windows.Forms.Button btnStartGame;
        private System.Windows.Forms.ComboBox cbResolution;
        private System.Windows.Forms.CheckBox cbAA;
        private System.Windows.Forms.CheckBox cbVsync;
        private System.Windows.Forms.GroupBox gbSoundSettings;
        private System.Windows.Forms.Label lblSFXVol;
        private System.Windows.Forms.Label lblMusicVol;
        private System.Windows.Forms.Label lblMasterVol;
        private System.Windows.Forms.NumericUpDown numSFXVol;
        private System.Windows.Forms.NumericUpDown numMusicVol;
        private System.Windows.Forms.NumericUpDown numMasterVol;
        private System.Windows.Forms.CheckBox cbFullScreen;
        private System.Windows.Forms.Label lblRequirementsMsg;
        private System.Windows.Forms.TabControl tabcMain;
        private System.Windows.Forms.TabPage tabpGeneral;
        private System.Windows.Forms.TabPage tabpSound;
        private System.Windows.Forms.TabPage tabpOculus;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox cbSimMode;
        private System.Windows.Forms.TabPage tabpSim;
        private System.Windows.Forms.Label lblSimPlanet;
        private System.Windows.Forms.NumericUpDown numSimSpeed;
        private System.Windows.Forms.TextBox txtSimPlanet;
        private System.Windows.Forms.Label lblSimSpeed;
        private System.Windows.Forms.GroupBox gbOculusSettings;
        private System.Windows.Forms.GroupBox gbSimulationSettings;
        private System.Windows.Forms.Label lblResolution;
        private System.Windows.Forms.CheckBox cbTrueScale;
        private System.Windows.Forms.CheckBox cbOculus;
        private System.Windows.Forms.ProgressBar pbRequirements;
        private System.Windows.Forms.Label lblReqLoading;
    }
}

