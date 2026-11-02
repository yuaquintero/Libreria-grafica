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
* **Encapsulamiento:** Ocultamiento del estado interno y control de comportamientos mediante interfaces limpias (`SetPos`, `CambiarEstado`).
* **Herencia:** Extensión de la clase base abstracta `GraphicsItem` y la clase `AnimatedItem` para la creación de objetos estáticos y animados.
* **Polimorfismo:** Procesamiento y renderizado uniforme de múltiples tipos de objetos contenidos dentro de la escena (`GraphicsScene`).

---

## 📁 Estructura del Repositorio

```text
libreria-grafica/
├── Engine2D.dll                                 # Binario compilado listo para usar en proyectos de estudiantes
├── doc/
│    ├── Guia_Engine2D.pdf                       # Guía paso a paso de integración en Visual Studio
│    └──documentacion_tecnica_de_engine2d.md     # Descripción de las clases que componen la libreria y sus principales métodos
├── src/Engine2D                                 # Código fuente de la ibrería 
│    └── Guia_Engine2D.pdf 
└── Demo_GUI/                                     # Proyecto de demostración (Windows Forms)
        ├── Escenario.cs                          # Inicialización del escenario y ciclo de juego
        ├── Pollito.cs                            # Ejemplo de creación de un ítem con características gráficas a partir de un recurso gráfico
        └── Bloque.cs                             # Obstáculo estático con forma rectagular y color sólido. Permite simular las colisiones 
