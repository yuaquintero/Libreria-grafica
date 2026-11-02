namespace Engine2D
{
    /// <summary>
    /// Representa un elemento gráfico en la escena cuya apariencia visual 
    /// se anima a partir de una hoja de sprites (spritesheet).
    /// </summary>
    public class AnimatedItem : GraphicsItem
    {
        private readonly int columnas;
        private readonly int filas;
        private readonly int totalFrames;

        private int frameInicio;
        private int frameFin;
        private int frameActual;

        private float tiempoPorFrame;
        private float tiempoAcumulado;

        private int anchoFrame;
        private int altoFrame;

        /// <summary>
        /// Determina si la secuencia de animación actual se repite en bucle.
        /// </summary>
        public bool Loop { get; set; } = true;

        /// <summary>
        /// Estado de reproducción de la animación.
        /// </summary>
        public bool IsPlaying { get; set; } = true;

        public AnimatedItem(float x, float y, int columnas, int filas, float fps, string nombreRecurso)
            : base(x, y, 0, 0, nombreRecurso)
        {
            this.columnas = columnas;
            this.filas = filas;
            this.totalFrames = columnas * filas;

            this.frameInicio = 0;
            this.frameFin = totalFrames - 1;
            this.frameActual = 0;

            SetFPS(fps);
            CalcularDimensionesFrame();
        }

        private void CalcularDimensionesFrame()
        {
            if (Imagen != null && columnas > 0 && filas > 0)
            {
                this.anchoFrame = Imagen.Width / columnas;
                this.altoFrame = Imagen.Height / filas;

                // El tamaño del objeto en la escena corresponde al tamaño de un solo cuadro
                this.ancho = anchoFrame;
                this.alto = altoFrame;
            }
        }

        public void SetFPS(float fps)
        {
            if (fps > 0)
                this.tiempoPorFrame = 1.0f / fps;
        }

        /// <summary>
        /// Selecciona y reproduce una fila específica de la hoja de sprites (ej. dirección de movimiento).
        /// </summary>
        /// <param name="indiceFila">Índice de la fila basada en 0.</param>
        /// <param name="loop">Indica si debe repetirse continuamente.</param>
        public void SeleccionarFila(int indiceFila, bool loop = true)
        {
            if (indiceFila < 0 || indiceFila >= filas) return;

            int inicio = indiceFila * columnas;
            int fin = inicio + columnas - 1;

            EstablecerRangoFrames(inicio, fin, loop);
        }

        public void EstablecerRangoFrames(int inicio, int fin, bool loop = true)
        {
            if (inicio < 0 || fin >= totalFrames || inicio > fin) return;

            if (this.frameInicio != inicio || this.frameFin != fin)
            {
                this.frameInicio = inicio;
                this.frameFin = fin;
                this.frameActual = inicio;
                this.tiempoAcumulado = 0f;
            }

            this.Loop = loop;
            this.IsPlaying = true;
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            if (!IsPlaying || Imagen == null || totalFrames <= 1) return;

            tiempoAcumulado += deltaTime;

            if (tiempoAcumulado >= tiempoPorFrame)
            {
                tiempoAcumulado -= tiempoPorFrame;
                frameActual++;

                if (frameActual > frameFin)
                {
                    if (Loop)
                    {
                        frameActual = frameInicio;
                    }
                    else
                    {
                        frameActual = frameFin;
                        IsPlaying = false;
                    }
                }
            }
        }

        public override void Paint(Graphics g)
        {
            if (Imagen != null && anchoFrame > 0 && altoFrame > 0)
            {
                int col = frameActual % columnas;
                int fila = frameActual / columnas;

                Rectangle srcRect = new Rectangle(
                    col * anchoFrame,
                    fila * altoFrame,
                    anchoFrame,
                    altoFrame
                );

                RectangleF destRect = new RectangleF(0, 0, ancho, alto);

                g.DrawImage(Imagen, destRect, srcRect, GraphicsUnit.Pixel);
            }
            else
            {
                base.Paint(g);
            }
        }
    }
}
