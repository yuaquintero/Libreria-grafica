using System.Drawing.Drawing2D;
using System.Reflection;
using System.Resources;

namespace Engine2D
{
    /// <summary>
    /// Enum para definir el punto de origen del sistema de coordenadas de la escena.
    /// </summary>
    public enum CoordinateOrigin
    {
        TopLeft,
        BottomLeft,
        Center
    }

    /// <summary>
    /// Representa la escena lógica o lienzo del juego en 2D.
    /// Contiene y administra todos los ítems gráficos, el color/imagen de fondo 
    /// y las transformaciones del sistema de coordenadas.
    /// </summary>
    public class GraphicsScene
    {
        private readonly List<GraphicsItem> items = new List<GraphicsItem>();

        /// <summary> Ancho lógico de la escena en píxeles. </summary>
        public float Width { get; set; } = 800f;

        /// <summary> Alto lógico de la escena en píxeles. </summary>
        public float Height { get; set; } = 600f;

        /// <summary> Color de fondo uniforme de la escena. </summary>
        public Color BackgroundColor { get; set; } = Color.DarkGray;

        /// <summary> Imagen de fondo de la escena (si es null, se utiliza BackgroundColor). </summary>
        public Image BackgroundImage { get; set; }

        /// <summary> Origen del sistema de coordenadas (TopLeft, BottomLeft o Center). </summary>
        public CoordinateOrigin Origin { get; set; } = CoordinateOrigin.TopLeft;

        /// <summary>
        /// Posición X de la cámara en el mundo. 
        /// Permite crear desplazamientos de pantalla (scroll horizontal).
        /// </summary>
        public float CamaraX { get; set; } = 0f;

        public GraphicsScene()
        {
        }

        /// <summary>
        /// Agrega un ítem gráfico a la escena.
        /// </summary>
        public void AddItem(GraphicsItem item)
        {
            if (item != null && !items.Contains(item))
            {
                items.Add(item);
            }
        }

        /// <summary>
        /// Remueve un ítem gráfico de la escena.
        /// </summary>
        public void RemoveItem(GraphicsItem item)
        {
            if (item != null)
            {
                items.Remove(item);
            }
        }

        /// <summary>
        /// Elimina todos los ítems de la escena.
        /// </summary>
        public void Clear()
        {
            items.Clear();
        }

        /// <summary>
        /// Retorna una copia de la lista de ítems presentes en la escena.
        /// </summary>
        public List<GraphicsItem> ObtenerItems()
        {
            return new List<GraphicsItem>(items);
        }

        /// <summary>
        /// Carga una imagen de fondo desde los recursos incrustados (.resx) del ejecutable principal.
        /// </summary>
        /// <param name="nombreRecurso">Nombre exacto de la imagen en los recursos del proyecto.</param>
        public void SetBackgroundImage(string nombreRecurso)
        {
            try
            {
                Assembly ejecutable = Assembly.GetEntryAssembly();
                if (ejecutable != null && !string.IsNullOrEmpty(nombreRecurso))
                {
                    string nombreEnsamblado = ejecutable.GetName().Name;
                    ResourceManager manager = new ResourceManager($"{nombreEnsamblado}.Properties.Resources", ejecutable);

                    object recurso = manager.GetObject(nombreRecurso);
                    if (recurso is Image img)
                    {
                        BackgroundImage = img;
                    }
                }
            }
            catch
            {
                BackgroundImage = null;
            }
        }

        /// <summary>
        /// Actualiza la lógica de todos los ítems de la escena.
        /// Se invoca en cada cuadro (frame) del bucle de juego.
        /// </summary>
        /// <param name="deltaTime">Tiempo transcurrido desde el último cuadro en segundos.</param>
        public void Update(float deltaTime)
        {
            for (int i = items.Count - 1; i >= 0; i--)
            {
                if (i < items.Count)
                {
                    items[i].Update(deltaTime);
                }
            }
        }

        /// <summary>
        /// Dibuja el fondo y todos los ítems visibles en el lienzo gráfico.
        /// </summary>
        /// <param name="g">Objeto Graphics proporcionado por el visor.</param>


        public void Render(Graphics g)
        {
            // 1. Renderizar Fondo (Imagen estirada a las dimensiones de la escena o Color Sólido)
            if (BackgroundImage != null)
            {
                g.DrawImage(BackgroundImage, 0, 0, Width, Height);
            }
            else
            {
                g.Clear(BackgroundColor);
            }

            var sceneState = g.Save();

            // 2. Aplicar Desplazamiento de Cámara (Scroll Horizontal)
            if (CamaraX != 0f)
            {
                g.TranslateTransform(-CamaraX, 0);
            }

            // 3. Aplicar Transformaciones según el Origen de Coordenadas
            if (Origin == CoordinateOrigin.BottomLeft)
            {
                g.TranslateTransform(0, Height);
                g.ScaleTransform(1, -1);
            }
            else if (Origin == CoordinateOrigin.Center)
            {
                g.TranslateTransform(Width / 2f, Height / 2f);
                g.ScaleTransform(1, -1);
            }

            // 4. Frustum Culling (Renderizar solo los ítems visibles dentro del visor)
            float margen = 100f;
            float limiteIzquierdo = CamaraX - margen;
            float limiteDerecho = CamaraX + Width + margen;

            var itemsVisibles = items
                .Where(i => i.Visible && (i.X + i.Ancho) >= limiteIzquierdo && i.X <= limiteDerecho)
                .OrderBy(i => i.ZValue);

            // 5. Dibujar cada ítem gráfico
            foreach (var item in itemsVisibles)
            {
                var itemState = g.Save();

                using (var transform = item.GetTransform())
                {
                    g.MultiplyTransform(transform, MatrixOrder.Prepend);
                }

                if (Origin == CoordinateOrigin.BottomLeft || Origin == CoordinateOrigin.Center)
                {
                    g.TranslateTransform(0, item.Alto);
                    g.ScaleTransform(1, -1);
                }

                item.Paint(g);

                g.Restore(itemState);
            }

            g.Restore(sceneState);
        }
    }
}