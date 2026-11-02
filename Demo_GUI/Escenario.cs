using Engine2D;
using static System.Net.Mime.MediaTypeNames;

namespace Demo_GUI
{
    public partial class Escenario : Form
    {
        private GraphicsScene scene;
        private Pollito pollito;
        private List<Bloque> bloques = new List<Bloque>();

        public Escenario()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            scene = new GraphicsScene();
            scene.Origin = CoordinateOrigin.TopLeft;
            scene.BackgroundColor = Color.FromArgb(30, 30, 30); // Fondo oscuro estilo editor
            scene.SetBackgroundImage("fondo estrellas");
            visor.Scene = scene;
            visor.Size = new Size(800, 600);
            pollito = new Pollito(80, 80, 5);
            scene.AddItem(pollito);
            int grosor = 20;
            CrearBloque(0, 0, 800, grosor);                  // Muro Superior
            CrearBloque(0, 600 - grosor, 800, grosor); // Muro Inferior (ajustado al alto del borde)
            CrearBloque(0, 0, grosor, 600);                  // Muro Izquierdo
            CrearBloque(800 - grosor, 0, grosor, 600);  // Muro Derecho
            this.Shown += (s, e) => visor.Start();
        }



        void CrearBloque(float x, float y, float ancho, float alto)
        {
            Bloque pared = new Bloque(x, y, ancho, alto);
            bloques.Add(pared); // Guardamos la referencia para colisiones
            scene.AddItem(pared);   // Agregamos a la escena para renderizar
        }

        private void visor_KeyPress(object sender, KeyPressEventArgs e)
        {
            float velocidad = 10f;
            char letra = char.ToUpper(e.KeyChar);

            float xAnterior = pollito.X;
            float yAnterior = pollito.Y;
            switch (letra)
            {
                case 'A':
                    pollito.X -= velocidad;
                    break;
                case 'D':
                    pollito.X += velocidad;
                    break;
                case 'W':
                    pollito.Y -= velocidad;
                    break;

                case 'S':
                    pollito.Y += velocidad;
                    break;

                case 'Q': // Rotar a la izquierda
                    pollito.Rotacion -= 5f;
                    break;

                case 'E': // Rotar a la derecha
                    pollito.Rotacion += 5f;
                    break;
            }
            if (pollito.ColisionaCon(bloques))
            {
                pollito.SetPos(xAnterior, yAnterior);

            }

        }
    }
}
