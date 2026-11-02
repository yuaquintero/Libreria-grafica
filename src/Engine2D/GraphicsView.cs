using System.Diagnostics;
using Timer = System.Windows.Forms.Timer;

namespace Engine2D
{
    /// <summary>
    /// Control de usuario que actúa como el lienzo de dibujo e interconexión para renderizar una GraphicsScene.
    /// </summary>
    public class GraphicsView : UserControl
    {
        private GraphicsScene scene;
        private readonly Timer gameTimer;
        private readonly Stopwatch stopwatch;
        private float lastFrameTime;

        /// <summary> Asigna o obtiene la escena de juego vinculada a este visor. </summary>
        public GraphicsScene Scene
        {
            get => scene;
            set { scene = value; Invalidate(); }
        }

        /// <summary> Inicializa el visor con configuración de doble búfer para evitar parpadeos (flickering). </summary>
        public GraphicsView()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.OptimizedDoubleBuffer, true);

            gameTimer = new Timer { Interval = 16 }; // Aproximadamente 60 FPS
            gameTimer.Tick += GameLoop;

            stopwatch = new Stopwatch();
        }

        /// <summary> Inicia el bucle principal de renderizado y física a 60 FPS. </summary>
        public void Start()
        {
            stopwatch.Start();
            lastFrameTime = (float)stopwatch.Elapsed.TotalSeconds;
            gameTimer.Start();
        }

        /// <summary> Detiene el bucle de ejecución. </summary>
        public void Stop()
        {
            gameTimer.Stop();
            stopwatch.Stop();
        }

        private void GameLoop(object sender, EventArgs e)
        {
            float currentTime = (float)stopwatch.Elapsed.TotalSeconds;
            float deltaTime = currentTime - lastFrameTime;
            lastFrameTime = currentTime;

            if (scene != null)
            {
                scene.Update(deltaTime);
                Invalidate(); // Fuerza el repintado del control
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (scene != null)
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                scene.Render(e.Graphics);
            }
        }

        /// <summary>
        /// Garantiza que el visor capture las flechas de dirección del teclado como entradas válidas de juego.
        /// </summary>
        protected override bool IsInputKey(Keys keyData)
        {
            switch (keyData & Keys.KeyCode)
            {
                case Keys.Up:
                case Keys.Down:
                case Keys.Left:
                case Keys.Right:
                    return true;
            }
            return base.IsInputKey(keyData);
        }
    }
}