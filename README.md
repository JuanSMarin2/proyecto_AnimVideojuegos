# Entrega 3 – Animación para Videojuegos

## Integrantes
* Juan Sebastián Marín
* Esteban Puerta
* Simara Paola Villasmil
* Isaac Pineda



## Descripción general

El proyecto implementa un sistema de combate tipo ARPG basado en combos direccionales. Cada ataque depende tanto del tiempo como de la dirección del joystick, permitiendo ejecutar variantes según el input del jugador.

El sistema integra locomoción, control de estados, encadenamiento mediante ventanas de tiempo y feedback visual al impactar. Se busca mantener una experiencia fluida, evitando inconsistencias entre movimiento y combate.

Criterios de diseño

La direccionalidad de los ataques se obtiene a partir del joystick. Se aplica una deadzone para evitar ruido y se determina la dirección dominante entre los ejes horizontal y vertical. Esto permite clasificar los ataques en variantes direccionales claras.

Para mejorar la respuesta, se implementa un buffer de dirección que conserva la última dirección válida durante una ventana corta. Esto evita la pérdida de inputs cuando el jugador suelta el joystick justo antes de atacar.

El sistema de combos se basa en ventanas temporales definidas desde las animaciones. Durante estas ventanas, se captura la dirección que se utilizará en el siguiente ataque. Si no hay una dirección capturada, el sistema recurre a otras fuentes siguiendo una prioridad definida:

* dirección capturada durante la ventana de combo
* dirección almacenada en buffer
* input actual del jugador

Esta jerarquía evita conflictos y mantiene un comportamiento consistente.

La histéresis se resuelve mediante la combinación de deadzone y buffer, reduciendo cambios bruscos de dirección y estabilizando el input.

## Lista de secuencias

Las secuencias dependen tanto de la dirección como del timing. El sistema permite encadenar ataques distintos según cómo y cuándo se introduce el input.

Algunas combinaciones posibles incluyen:

* neutral seguido de dirección hacia adelante
* dirección lateral cambiando a la opuesta durante el combo
* ataques repetidos en la misma dirección
* combinación de ataque ligero y fuerte dentro de una ventana



## El sistema de locomoción permite controlar al personaje desde el inicio mediante joystick. El personaje puede alternar entre idle y movimiento, y rota de forma coherente según la dirección.

Se tienen las siguientes características:

* movimiento relativo a la cámara
* rotación alineada con la dirección de desplazamiento
* integración con parámetros del Animator

Durante los ataques, el movimiento se bloquea para evitar interferencias. Esto asegura que la locomoción no rompa el sistema de combate ni genere inconsistencias.

## Feedback de impacto

Se implementa un efecto de camera shake al impactar ataques, con variaciones según el tipo de ataque.


* ataques ligeros generan un shake más suave
* ataques fuertes generan un shake más intenso

El efecto busca mejorar la sensación de impacto sin afectar la legibilidad ni el control.

## Limitaciones conocidas

El sistema presenta algunas limitaciones que no afectan su funcionamiento base pero sí su complejidad:

* dependencia directa del Animator para ventanas y transiciones
* falta de mecánicas avanzadas como cancelaciones

* ## Video y flujo del animator
  https://drive.google.com/file/d/19G7mcbSeudHYseEBQNDa8xA1Ru4CIkny/view?usp=sharing
  

