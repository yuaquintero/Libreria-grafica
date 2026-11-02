# Documentación Técnica y Referencia de API: Engine2D

**Engine2D** es una biblioteca de clases desarrollada en **C# / .NET 8** diseñada como un marco de trabajo (*framework*) pedagógico para la enseñanza de la **Programación Orientada a Objetos (POO)**. 

Esta documentación describe la arquitectura interna de la librería, las clases principales y su catálogo de métodos, sirviendo como guía de referencia rápida para los estudiantes durante el desarrollo de sus videojuegos 2D.

---

## 🏛️ Arquitectura General de la Librería

El motor gráfico opera bajo la separación entre el **Mundo Lógico (Escena)** y la **Ventana de Renderizado (Visor)**:

```
┌───────────────────────────────────────────────────────────────┐
│                      GraphicsView (Visor)                     │
│  - Control de WinForms (60 FPS, Doble Búfer)                  │
│  - Captura eventos de Teclado / Mouse                         │
└──────────────────────────────┬────────────────────────────────┘
                               │ Muestra
                               ▼
┌───────────────────────────────────────────────────────────────┐
│                     GraphicsScene (Escena)                    │
│  - Dimensiones lógicas (Width x Height)                       │
│  - Sistema de coordenadas y Cámara (CamaraX)                  │
│  - Fondo (Color o Imagen de Recursos)                         │
└──────────────────────────────┬────────────────────────────────┘
                               │ Administra (Polimorfismo)
                               ▼
┌───────────────────────────────────────────────────────────────┐
│                   GraphicsItem (Clase Base)                   │
│  - Posición (X, Y), Tamaño (Ancho, Alto), Rotación            │
│  - Colisiones AABB, Origen de transformación                  │
└──────────────┬────────────────────────────────┬───────────────┘
               │                                │
               ▼                                ▼
     [ Objetos Estáticos ]             AnimatedItem
   (Muros, Suelo, Monedas)       (Spritesheet 8x8, FPS, Filas)
                                                │
                                                ▼
                                           [ Mago / Jugador ]
```

---

## 📚 Catálogo de Clases y Métodos Principales

---

### 1. `GraphicsItem` (Clase Base Abstracta)
Representa cualquier entidad u objeto dentro de la escena. Define el contrato base para posición, dimensiones, transformaciones geométricas, colisiones y dibujo.

#### 🔹 Propiedades Clave
| Propiedad | Tipo | Descripción |
| :--- | :--- | :--- |
| `X` | `float` | Posición horizontal del objeto en el mundo. |
| `Y` | `float` | Posición vertical del objeto en el mundo. |
| `Ancho` | `float` | Ancho del objeto en píxeles. |
| `Alto` | `float` | Alto del objeto en píxeles. |
| `Rotacion` | `float` | Ángulo de rotación del objeto en grados ($0^\circ - 360^\circ$). |
| `Visible` | `bool` | Indica si el objeto se dibuja en la escena (`true` por defecto). |
| `ZValue` | `int` | Orden de dibujo o capa (valores altos se dibujan sobre valores bajos). |
| `ColorItem` | `Color` | Color de relleno cuando no hay una imagen asignada. |
| `Forma` | `ItemShape` | Define la forma de la figura (`Rectangle`, `Circle`) por defecto es rectagular |

#### 🔹 Métodos Principales

##### `SetPos(float x, float y)`
Establece la posición del objeto en el sistema de coordenadas de la escena.
* **Parámetros:** 
  * `x`: Nueva coordenada X.
  * `y`: Nueva coordenada Y.

##### `SetSize(float ancho, float alto)`
Establece las dimensiones del objeto.
* **Parámetros:**
  * `ancho`: Ancho en píxeles.
  * `alto`: Alto en píxeles.

##### `ColisionaCon(GraphicsItem otro) : bool`
Determina si este objeto colisiona con otro ítem utilizando la técnica de cajas delimitadoras alineadas a los ejes (**AABB - Axis-Aligned Bounding Box**).
* **Parámetros:** `otro` - Instancia de `GraphicsItem` a evaluar.
* **Retorno:** `true` si existe superposición entre las áreas de ambos objetos; `false` en caso contrario.

##### `GetRectangle() : RectangleF`
Retorna el rectángulo delimitador (*bounding box*) del objeto en coordenadas del mundo.

##### `Update(float deltaTime)` *(Virtual)*
Actualiza la lógica interna del objeto. Se invoca automáticamente en cada cuadro del bucle de juego.
* **Parámetros:** `deltaTime` - Tiempo transcurrido desde el último cuadro en segundos (ej. ~0.016s a 60 FPS).

##### `Paint(Graphics g)` *(Virtual)*
Dibuja el aspecto visual del objeto en el contexto gráfico provisto. Puede ser sobrescrito por clases hijas para personalizar el renderizado.

---

### 2. `AnimatedItem` (Subclase de `GraphicsItem`)
Clase especializada para objetos visuales cuya representación gráfica proviene de una hoja de sprites (**spritesheet**). Maneja el recorte dinámico de sub-regiones y el control de animación por fotogramas por segundo (FPS).

#### 🔹 Propiedades Clave
| Propiedad | Tipo | Descripción |
| :--- | :--- | :--- |
| `IsPlaying` | `bool` | Permite pausar o reanudar la animación. |
| `Loop` | `bool` | Determina si la secuencia actual se repite indefinidamente. |

#### 🔹 Métodos Principales

##### `AnimatedItem(float x, float y, int columnas, int filas, float fps, string nombreRecurso)`
Constructor. Inicializa el ítem animado vinculándolo a un recurso gráfico.
* **Parámetros:**
  * `columnas`: Cantidad de columnas horizontales de la matriz de la imagen.
  * `filas`: Cantidad de filas verticales de la matriz de la imagen.
  * `fps`: Cuadros por segundo deseados para la animación.
  * `nombreRecurso`: Nombre exacto del archivo gráfico en `Properties.Resources`.

##### `SeleccionarFila(int indiceFila, bool loop = true)`
Configura la animación para reproducir únicamente los fotogramas de una fila específica de la hoja de sprites (ideal para cambiar entre estados como *Caminar a la derecha*, *Saltar*, *Atacar*).
* **Parámetros:**
  * `indiceFila`: Índice basado en cero de la fila deseada.
  * `loop`: Indica si la fila debe repetirse en bucle.

##### `EstablecerRangoFrames(int inicio, int fin, bool loop = true)`
Permite reproducir un rango arbitrario de fotogramas secuenciales dentro de la hoja de sprites.

##### `SetFPS(float fps)`
Modifica la velocidad de reproducción de la animación en tiempo de ejecución.

---

### 3. `GraphicsScene` (El Mundo Lógico)
Administra todos los objetos del juego, la cámara, el fondo y el sistema de coordenadas.

#### 🔹 Propiedades Clave
| Propiedad | Tipo | Descripción |
| :--- | :--- | :--- |
| `Width` | `float` | Ancho total del mundo lógico (ej. 800px para pantallas fijas, 4000px para niveles extensos). |
| `Height` | `float` | Alto total del mundo lógico (ej. 600px). |
| `CamaraX` | `float` | Posición X de la cámara. Desplaza el punto de vista para lograr scroll horizontal. |
| `BackgroundColor` | `Color` | Color de fondo uniforme de la escena. |
| `BackgroundImage` | `Image` | Imagen de mapa de bits utilizada como fondo de pantalla. |
| `Origin` | `CoordinateOrigin` | Define el origen de coordenadas (`TopLeft`, `BottomLeft`, `Center`). |

#### 🔹 Métodos Principales

##### `AddItem(GraphicsItem item)`
Agrega un ítem a la lista de administración de la escena. Acepta cualquier clase que herede de `GraphicsItem` (**Polimorfismo**).

##### `RemoveItem(GraphicsItem item)`
Remueve un objeto específico de la escena.

##### `Clear()`
Elimina todos los objetos actualmente registrados en la escena.

##### `SetBackgroundImage(string nombreRecurso)`
Carga y asigna una imagen de fondo desde el archivo de recursos del proyecto (`Resources.resx`).
* **Parámetros:** `nombreRecurso` - Nombre del recurso de imagen.

##### `ObtenerItems() : List<GraphicsItem>`
Retorna una copia de la lista de todos los objetos registrados en la escena.

##### `Update(float deltaTime)`
Bucle de actualización lógica. Recorre e invoca el método `Update` de todos los ítems registrados.

##### `Render(Graphics g)`
Dibuja el fondo de la escena, aplica el desplazamiento de la cámara (`CamaraX`), realiza la optimización de descarte de objetos fuera de pantalla (**Frustum Culling**) y dibuja cada objeto visible.

---

### 4. `GraphicsView` (El Visor Visual)
Control personalizado de Windows Forms que sirve como la "pantalla" o "cámara física" del juego.

#### 🔹 Propiedades Clave
| Propiedad | Tipo | Descripción |
| :--- | :--- | :--- |
| `Scene` | `GraphicsScene` | Referencia a la escena lógica que este visor se encarga de renderizar. |
| `FPS` | `int` | Fotogramas por segundo objetivo del bucle de juego (60 FPS por defecto). |

#### 🔹 Métodos Principales

##### `Start()`
Inicia el temporizador de alta precisión (*Game Loop*), comenzando la actualización lógica a 60 FPS y el redibujado continuo.

##### `Stop()`
Pausa la ejecución del bucle de juego.

---

## 🛠️ Resumen de Uso en Código (Ejemplo Integrado)

```csharp
// 1. Instanciar Escena Lógica de 4000px de ancho (Scroll tipo Mario)
GraphicsScene escena = new GraphicsScene 
{ 
    Width = 4000, 
    Height = 600 
};
escena.SetBackgroundImage("fondo_estrellas");

// 2. Crear objetos y agregarlos (Polimorfismo)
GraphicsItem bloque = new Bloque(300, 400, 150, 30);
AnimatedItem mago = new Mago(100, 200);

escena.AddItem(bloque);
escena.AddItem(mago);

// 3. Vincular al visor y arrancar el motor
visor.Scene = escena;
visor.Start();

// 4. Mover la cámara para seguir al personaje en el bucle del formulario
escena.CamaraX = Math.Max(0, mago.X - (visor.Width / 2f));
```