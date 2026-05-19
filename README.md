# Entrega 4 – Animación para Videojuegos

## Integrantes
* Juan Sebastián Marín
* Esteban Puerta
* Simara Paola Villasmil
* Isaac Pineda


# README

## Controles

- **WASD** → Movimiento del personaje.
- **Mover el mouse** → Control de cámara.
- **Click izquierdo** → Ataque ligero.
  - Ataques rápidos.
  - Ideales para realizar combos.
- **Click derecho** → Ataque pesado.
  - Más lentos.
  - Infligen el doble de daño que un ataque normal.
- **TAB** → Cambiar de personaje en cualquier momento.

---

## Link del ejecutable: https://drive.google.com/file/d/1S_cVBYmlOGcmqAJKShEWnOym1ObK_RhE/view?usp=sharing
<img width="1575" height="881" alt="image" src="https://github.com/user-attachments/assets/a45d460f-917c-4cdc-be76-dd65a57c9042" />


---

# ¿Cómo funciona el juego?

El juego es un **Hack and Slash de oleadas infinitas**, donde el jugador debe sobrevivir derrotando enemigos cada vez más fuertes. El combate se centra en la diferencia de estilos entre los personajes jugables y los distintos comportamientos enemigos.

## Personajes Jugables

### Gru
Personaje enfocado en velocidad y movilidad.

- Ataques rápidos.
- Poco alcance.
- Bajo daño por golpe.
- Ideal para combos rápidos y juego agresivo.

<img width="426" height="463" alt="image" src="https://github.com/user-attachments/assets/1474d792-2f48-4ae0-be26-6235185de003" />


### Vector
Personaje pesado enfocado en daño y control.

- Ataques lentos.
- Gran alcance.
- Alto daño.
- Ideal para eliminar enemigos resistentes.

<img width="517" height="627" alt="image" src="https://github.com/user-attachments/assets/5787559b-1ddf-46f9-8bea-c530779a93ba" />

---

# Tipos de enemigos

## Brawler
Enemigo base construido en clase.

### Comportamiento
1. Inicia en estado **Patrol**.
2. Cuando detecta al jugador cambia a **Chase State**.
3. Cuando alcanza al jugador entra en **Attack State**.
4. Ejecuta combos cuerpo a cuerpo usando espada.

---

## Mage
Enemigo a distancia.

### Comportamiento
1. Inicia en **Patrol**.
2. No utiliza Chase State.
3. Dispara una bola de fuego en dirección al jugador.

---

## MageBoss
Versión avanzada del Mage.

### Características
- Más vida.
- Más daño.
- Dispara 3 bolas de fuego.
- Los proyectiles tienen una separación de 25° entre sí.

---

## Mixed
Combinación entre Brawler y Mage.

### Características
- Tiene Chase State y Attack State.
- Persigue al jugador.
- Dispara 3 bolas de fuego al atacar.

---

## FinalBoss
Enemigo más poderoso del juego.

### Características
- Mayor vida y daño que todos los enemigos anteriores.
- Funciona como una versión avanzada del Mixed.
- Dispara 10 bolas de fuego:
  - 5 hacia el frente.
  - 5 hacia atrás.
- Los proyectiles tienen una separación de 10° entre sí.

---

<img width="587" height="157" alt="image" src="https://github.com/user-attachments/assets/6f7a11f6-5bdc-458c-87f8-35bc1fface4b" />



# Sistema de oleadas

Los enemigos aparecen organizados en oleadas.

## Oleadas iniciales
Las primeras 9 oleadas están diseñadas manualmente:
- Siempre aparecen los mismos enemigos.
- La dificultad aumenta progresivamente.

## Oleadas infinitas
Después de la oleada 9:
- El sistema genera oleadas infinitas automáticamente.
- Cada oleada genera entre 5 y 10 enemigos.
- Cada enemigo tiene distintas probabilidades de aparición según su fuerza.
- La vida de todos los enemigos aumenta en 10 puntos por oleada.

La siguiente oleada comienza automáticamente cuando el jugador derrota al último enemigo de la oleada actual.

---

# Decorators implementados

## BerserkDecorator

Power-up de daño.

### Características
- Multiplica `DamageMultiplier`.
- Activa emisión visual en los renderers mientras está activo.
- Revierte los cambios al terminar.

---

## ShieldDecorator

Power-up defensivo.

### Características
- Activa `IsInvulnerable = true`.
- Habilita emisión visual mientras dura.
- Revierte el efecto al finalizar.

---

## SpeedDecorator

Power-up de velocidad.

### Características
- Multiplica `MoveSpeedMultiplier`.
- Incrementa la velocidad de movimiento temporalmente.

---

# Funcionamiento de los power-ups
Los enemigos tienen un 50% de probabilidades de dropear algo al ser eliminados.
Tienen un 70% de probabilidades de dropear un Healer que cura el 50% de la vida total.
El 30% restante se reparte en los power ups:

Todos los power-ups heredan de `PowerUpDecorator`.

Esta clase se encarga de:
- Gestionar la duración.
- Controlar el tiempo restante.
- Eliminar automáticamente el power-up cuando expira o el objeto se deshabilita.

---

# Sistema de pickups

Cuando el jugador recoge un pickup:
1. El power-up se agrega al jugador activo.
2. Se inicia automáticamente su rutina de duración.
3. Si el mismo power-up ya está activo:
   - No se duplica.
   - Solo se reinicia su duración mediante `Refresh()`.

---

# Power-ups encadenables (stackeables)

El sistema utiliza `PlayerStatsContext` para encadenar decorators.

## Funcionamiento
- Cada decorador envuelve al anterior usando:
  - `SetInner(CurrentStats)`
- El último decorador agregado pasa a ser `CurrentStats`.

Esto permite combinar distintos efectos simultáneamente.

### Ejemplo
Un jugador puede tener aumento de velocidad y daño al mismo tiempo.

Los multiplicadores se acumulan:
- `SpeedDecorator` multiplica la velocidad actual.
- `BerserkDecorator` multiplica el daño actual.

No existe stack del mismo power-up:
- Recoger otro igual únicamente reinicia su duración.

---

# Limitaciones conocidas y decisiones de diseño relevantes

- Las primeras 9 oleadas están diseñadas manualmente para garantizar una progresión controlada.
- A partir de la oleada 10 el sistema pasa a generación procedural infinita.
- Los enemigos aumentan su vida progresivamente para mantener la dificultad.
- El sistema de power-ups permite combinar distintos efectos, pero no acumular varias veces el mismo tipo de power-up.
- El cambio de personaje es instantáneo y puede hacerse en cualquier momento de la partida.
- La cámara atraviesa paredes y los power-ups aparecen atravesando el suelo

<img width="625" height="384" alt="image" src="https://github.com/user-attachments/assets/4ac4ae34-0b1e-490c-a0a8-f01573b1e09f" />
<img width="704" height="387" alt="image" src="https://github.com/user-attachments/assets/30d46b0f-1286-4cf2-b4cb-5136dc92ed62" />


