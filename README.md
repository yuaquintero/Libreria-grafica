# 🕹️ Engine2D - Librería Gráfica Educativa para C# (.NET 8)

[![NET 8.0](https://img.shields.io/badge/.NET-8.0-purple.svg)](https://dotnet.microsoft.com/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)
[![Curso](https://img.shields.io/badge/Curso-POO-blue.svg)](#)

**Engine2D** es un marco de trabajo (*framework*) gráfico 2D simplificado, diseñado específicamente con fines pedagógicos para la asignatura de **Programación Orientada a Objetos (POO)** en la Institución Universitaria Pascual Bravo.

Su objetivo principal es abstraer la complejidad del renderizado gráfico en Windows Forms y la manipulación del lienzo a bajo nivel, permitiendo a los estudiantes concentrarse en la **arquitectura de software, el diseño de clases y la aplicación práctica de los principios de la POO**.

---

## 🎯 Propósito Pedagógico y Conceptos de POO

A través de esta librería, los estudiantes ejercitan activamente:

* **Abstracción:** Modelado de entidades del mundo de los videojuegos (personajes, obstáculos, coleccionables) ocultando los detalles de implementación gráfica.
* **Encapsulamiento:** Ocultamiento del estado interno y control de comportamientos mediante interfaces limpias (`SetPos`, `CambiarEstado`, `LanzarHechizo`).
* **Herencia:** Extensión de la clase base abstracta `GraphicsItem` y la clase `AnimatedItem` para la creación de objetos estáticos y animados.
* **Polimorfismo:** Procesamiento y renderizado uniforme de múltiples tipos de objetos contenidos dentro de la escena (`GraphicsScene`).

---

## 📁 Estructura del Repositorio

```text
libreria-grafica/
├── Engine2D.dll              # Binario compilado listo para usar en proyectos de estudiantes
├── docs/
│   └── Guia_Engine2D.pdf     # Guía paso a paso de integración en Visual Studio
└── src/
    ├── Engine2D/             # Código fuente de la Librería de Clases (.NET 8)
    │   ├── GraphicsItem.cs   # Clase base para todos los elementos gráficos
    │   ├── AnimatedItem.cs   # Manejo de hojas de sprites (spritesheets)
    │   ├── GraphicsScene.cs  # Contenedor y cámara del mundo del juego
    │   └── GraphicsView.cs   # Control de renderizado (Visor a 60 FPS)
    │
    └── Demo_GUI/             # Proyecto de demostración (Windows Forms)
        ├── Escenario.cs      # Inicialización del escenario y ciclo de juego
        ├── Pollito.cs        # Ejemplo de ítem estático con recurso gráfico
        ├── Mago.cs           # Ejemplo de personaje animado (hoja de sprites 8x8)
        └── Bloque.cs         # Obstáculos y colisiones AABB
