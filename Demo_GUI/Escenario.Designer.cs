namespace Demo_GUI
{
    partial class Escenario
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            visor = new Engine2D.GraphicsView();
            SuspendLayout();
            // 
            // visor
            // 
            visor.BackColor = SystemColors.ActiveCaptionText;
            visor.BackgroundImage = Properties.Resources.fondo_estrellas;
            visor.Location = new Point(0, 0);
            visor.Name = "visor";
            visor.Scene = null;
            visor.Size = new Size(631, 345);
            visor.TabIndex = 0;
            visor.KeyPress += visor_KeyPress;
            // 
            // Escenario
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(862, 653);
            Controls.Add(visor);
            Name = "Escenario";
            Text = "Demo juego 2D";
            ResumeLayout(false);
        }

        #endregion

        private Engine2D.GraphicsView visor;
    }
}
