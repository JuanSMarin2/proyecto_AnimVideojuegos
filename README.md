# Entrega 2 – Animación para Videojuegos

## Integrantes
* Juan Sebastián Marín
* Esteban Puerta
* Simara Paola Villasmil
* Isaac Pineda



## Controles

* **WASD / Flechas** → Movimiento del personaje
* **Shift** → Agacharse
* **E** → Realizar emote
* **Click derecho** → Apuntar
* **Click izquierdo** *(mientras se apunta)* → Disparar
* **R** *(mientras se apunta)* → Recargar
* **Botón de scroll del mouse** → Activar / desactivar lock-on al enemigo

## Ejecutable y video

**[[Enlace aquí]](https://drive.google.com/drive/folders/1BwPA_n3NXnRCb0h3rottGgR-wL409rxe?usp=sharing)**



## Posibles *edge cases* encontrados

Durante las pruebas se identificaron algunos comportamientos visuales o técnicos menores:

* Mientras se apunta, el arma puede **clipear ligeramente con la mano** del segundo personaje.
* El **sistema de lock-on** puede comportarse de forma extraña al **acercarse demasiado al enemigo**.
* Existe **foot sliding** leve en la animación de **caminar hacia atrás mientras el personaje está agachado**.
* Los **pies del personaje atraviesan ligeramente el suelo** al entrar en la pose de agachado.
* En el ejecutable no se ven los gizmos por lo que es mas dificil saber donde estan impactando las balas.

## Profundización realizada
Lock On automatico, se puede activar y desactivar con el botón en la parte superior izquierda de la pantalla

## Personajes

### Personaje 1: XBot

Personaje equilibrado con **velocidad de movimiento y disparo estándar**.
Al apuntar, la cámara se ubica directamente detrás del personaje, posicionándolo en el centro de la pantalla.

### Personaje 2: Jef

Personaje pesado con **velocidad de movimiento y disparo reducida**.
Presenta una animación de recoil más fuerte al disparar.
Al apuntar, la **cámara se posiciona detrás del personaje pero desplazada hacia la derecha**, dejando al personaje ubicado en la parte izquierda de la pantalla.

### Personaje 3: Rob

Personaje ligero con **velocidad de movimiento y disparo rápida**.
La cámara se posiciona de forma similar a la de XBot, pero ligeramente más elevada, permitiendo que el personaje ocupe menos espacio en pantalla.

