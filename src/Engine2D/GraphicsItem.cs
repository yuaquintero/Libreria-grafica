using System.Drawing.Drawing2D;
using System.Reflection;
using System.Resources;

namespace Engine2D
{
    /// <summary>
    /// Especifica la forma geométrica de respaldo para un ítem cuando no tiene una imagen asignada.
    /// </summary>
    public enum ItemShape
    {
        /// <summary> Dibuja un rectángulo o cuadrado sólido. </summary>
        Rectangle,
        /// <summary> Dibuja un círculo o elipse sólida. </summary>
        Circle
    }

    /// <summary>
    /// Clase base abstracta para todos los elementos gráficos manipulables dentro de la escena.
    /// </summary>
    public abstract class GraphicsItem
    {
        protected float x;
        protected float y;
        protected float ancho;
        protected float alto;

        /// <summary> Obtiene o establece la imagen sprite cargada para este ítem. </summary>
        public Image Imagen { get; set; }

        /// <summary> Obtiene o establece el color de relleno por defecto si no hay imagen (por defecto Magenta). </summary>
        public Color ColorItem { get; set; } = Color.Magenta;

        /// <summary> Obtiene o establece la forma geométrica de renderizado por defecto si no hay imagen. </summary>
        public ItemShape Forma { get; set; } = ItemShape.Rectangle;

        /// <summary> Coordenada horizontal (X) del ítem en la escena. </summary>
        public float X { get => x; set => x = value; }

        /// <summary> Coordenada vertical (Y) del ítem en la escena. </summary>
        public float Y { get => y; set => y = value; }

        /// <summary> Ancho en píxeles del ítem. </summary>
        public float Ancho { get => ancho; set => ancho = value; }

        /// <summary> Alto en píxeles del ítem. </summary>
        public float Alto { get => alto; set => alto = value; }

        /// <summary> Ángulo de rotación en grados sobre el centro del ítem. </summary>
        public float Rotacion { get; set; } = 0f;

        /// <summary> Orden de profundidad en la escena (capas). A mayor valor, más al frente se dibuja. </summary>
        public int ZValue { get; set; } = 0;

        /// <summary> Determina si el ítem es visible y se renderiza en pantalla. </summary>
        public bool Visible { get; set; } = true;

        /// <summary> Determina si el ítem está activo para procesamiento de colisiones y actualizaciones. </summary>
        public bool Enabled { get; set; } = true;

        /// <summary> Inicializa un ítem gráfico por defecto de 32x32 píxeles en la posición (0,0). </summary>
        public GraphicsItem() : this(0, 0, 32, 32) { }

        /// <summary> Inicializa un ítem gráfico definiendo posición y dimensiones. </summary>
        public GraphicsItem(float x, float y, float ancho, float alto)
        {
            this.x = x;
            this.y = y;
            this.ancho = ancho;
            this.alto = alto;
        }

        /// <summary> Inicializa un ítem gráfico asignando color sólido y forma geométrica. </summary>
        public GraphicsItem(float x, float y, float ancho, float alto, Color color, ItemShape forma = ItemShape.Rectangle)
            : this(x, y, ancho, alto)
        {
            this.ColorItem = color;
            this.Forma = forma;
        }

        /// <summary> Inicializa un ítem gráfico intentando cargar una imagen desde los Recursos incrustados (.resx). </summary>
        public GraphicsItem(float x, float y, float ancho, float alto, string nombreRecurso)
            : this(x, y, ancho, alto)
        {
            CargarRecurso(nombreRecurso);
        }

        /// <summary>
        /// Intenta cargar una imagen desde los recursos incrustados (Properties.Resources) del proyecto principal.
        /// </summary>
        /// <param name="nombreRecurso">Nombre exacto del recurso de imagen.</param>
        public void CargarRecurso(string nombreRecurso)
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
                        Imagen = img;
                        if (ancho == 0 || alto == 0)
                        {
                            ancho = Imagen.Width;
                            alto = Imagen.Height;
                        }
                    }
                }
            }
            catch
            {
                Imagen = null;
            }
        }

        /// <summary> Establece simultáneamente la posición X e Y del ítem. </summary>
        public void SetPos(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary> Devuelve el rectángulo delimitador (Axis-Aligned Bounding Box) del ítem en el espacio de la escena. </summary>
        public virtual RectangleF GetRectangle()
        {
            return new RectangleF(x, y, ancho, alto);
        }

        /// <summary> Calcula la matriz de transformación local (traslación y rotación). </summary>
        public Matrix GetTransform()
        {
            Matrix transform = new Matrix();
            transform.Translate(x, y);

            if (Rotacion != 0f)
            {
                transform.Translate(ancho / 2f, alto / 2f);
                transform.Rotate(Rotacion);
                transform.Translate(-ancho / 2f, -alto / 2f);
            }

            return transform;
        }

        /// <summary> Método de actualización lógica ejecutado en cada fotograma del bucle principal. </summary>
        /// <param name="deltaTime">Tiempo transcurrido en segundos desde el último fotograma.</param>
        public virtual void Update(float deltaTime) { }

        /// <summary> Renderiza la imagen o la representación geométrica del ítem en el lienzo. </summary>
        public virtual void Paint(Graphics g)
        {
            if (Imagen != null)
            {
                g.DrawImage(Imagen, 0, 0, ancho, alto);
            }
            else
            {
                using (Brush pincel = new SolidBrush(ColorItem))
                {
                    if (Forma == ItemShape.Circle)
                        g.FillEllipse(pincel, 0, 0, ancho, alto);
                    else
                        g.FillRectangle(pincel, 0, 0, ancho, alto);
                }
            }
        }

        #region Métodos de Colisión

        /// <summary> Comprueba si este ítem colisiona con otro ítem gráfico en la escena (AABB). </summary>
        /// <param name="otro">El otro ítem a evaluar.</param>
        /// <returns>True si existe superposición de cajas delimitadoras; de lo contrario, False.</returns>
        public virtual bool ColisionaCon(GraphicsItem otro)
        {
            if (otro == null || otro == this || !this.Enabled || !otro.Enabled || !this.Visible || !otro.Visible)
                return false;

            return this.GetRectangle().IntersectsWith(otro.GetRectangle());
        }

        /// <summary> Comprueba si este ítem colisiona con al menos un elemento de la lista proporcionada. </summary>
        /// <param name="otros">Colección de ítems a evaluar.</param>
        public virtual bool ColisionaCon(IEnumerable<GraphicsItem> otros)
        {
            if (otros == null) return false;

            foreach (var otro in otros)
            {
                if (ColisionaCon(otro)) return true;
            }

            return false;
        }

        /// <summary> Devuelve el primer ítem con el que se detecta una colisión dentro de una colección. </summary>
        /// <param name="otros">Colección de ítems a verificar.</param>
        /// <returns>El objeto colisionado o null si no hay colisión.</returns>
        public virtual GraphicsItem ObtenerColision(IEnumerable<GraphicsItem> otros)
        {
            if (otros == null) return null;

            foreach (var otro in otros)
            {
                if (ColisionaCon(otro)) return otro;
            }

            return null;
        }

        #endregion
    }
}